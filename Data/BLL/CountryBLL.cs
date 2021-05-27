using Data.DAL;
using Data.DTO;
using MSSQL_Lite.Access;
using MSSQL_Lite.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class CountryBLL : BusinessLogicLayer
    {
        private DataAccessLevel dataAccessLevel;
        private bool disposed;
        public CountryBLL(DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL();
            this.dataAccessLevel = dataAccessLevel;
            disposed = false;
        }

        public CountryBLL(BusinessLogicLayer bll, DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL(bll.db);
            this.dataAccessLevel = dataAccessLevel;
            disposed = false;
        }

        private CountryInfo ToCountryInfo(Country country)
        {
            if (country == null)
                throw new Exception("");
            return new CountryInfo
            {
                ID = country.ID,
                name = country.name,
                description = country.description,
                createAt = country.createAt,
                updateAt = country.updateAt
            };
        }

        private Country ToCountry(CountryCreation countryCreation)
        {
            if (countryCreation == null)
                throw new Exception("");
            return new Country
            {
                name = countryCreation.name,
                description = countryCreation.description,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };
        }

        private Country ToCountry(CountryUpdate countryUpdate)
        {
            if (countryUpdate == null)
                throw new Exception("");
            return new Country
            {
                ID = countryUpdate.ID,
                name = countryUpdate.name,
                description = countryUpdate.description,
                updateAt = DateTime.Now
            };
        }

        public async Task<List<CountryInfo>> GetCountriesAsync()
        {
            List<CountryInfo> countries = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                countries = (await db.Countries.ToListAsync())
                    .Select(c => ToCountryInfo(c)).ToList();
            else
                countries = (await db.Countries.ToListAsync(c => new { c.ID, c.name, c.description }))
                    .Select(c => ToCountryInfo(c)).ToList();
            return countries;
        }

        public List<CountryInfo> GetCountries()
        {
            List<CountryInfo> countries = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                countries = db.Countries.ToList()
                    .Select(c => ToCountryInfo(c)).ToList();
            else
                countries = db.Countries.ToList(c => new { c.ID, c.name, c.description })
                    .Select(c => ToCountryInfo(c)).ToList();
            return countries;
        }

        public async Task<PagedList<CountryInfo>> GetCountriesAsync(int pageIndex, int pageSize)
        {
            SqlPagedList<Country> pagedList = null;
            Expression<Func<Country, object>> orderBy = c => new { c.ID };
            if (dataAccessLevel == DataAccessLevel.Admin)
                pagedList = await db.Countries.ToPagedListAsync(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = await db.Countries.ToPagedListAsync(
                    c => new { c.ID, c.name, c.description },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<CountryInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToCountryInfo(c)).ToList()
            };
        }

        public PagedList<CountryInfo> GetCountries(int pageIndex, int pageSize)
        {
            SqlPagedList<Country> pagedList = null;
            Expression<Func<Country, object>> orderBy = c => new { c.ID };
            if (dataAccessLevel == DataAccessLevel.Admin)
                pagedList = db.Countries.ToPagedList(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = db.Countries.ToPagedList(
                    c => new { c.ID, c.name, c.description },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<CountryInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToCountryInfo(c)).ToList()
            };
        }

        public async Task<CountryInfo> GetCountryAsync(int countryId)
        {
            if (countryId <= 0)
                throw new Exception("");
            Country country = null;
            if(dataAccessLevel == DataAccessLevel.Admin)
                country = (await db.Countries.SingleOrDefaultAsync(c => c.ID == countryId));
            else
                country = (await db.Countries
                    .SingleOrDefaultAsync(c => new { c.ID, c.name, c.description }, c => c.ID == countryId));

            return ToCountryInfo(country);
        }

        public CountryInfo GetCountry(int countryId)
        {
            if (countryId <= 0)
                throw new Exception("");
            Country country = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                country = db.Countries.SingleOrDefault(c => c.ID == countryId);
            else
                country = db.Countries
                    .SingleOrDefault(c => new { c.ID, c.name, c.description }, c => c.ID == countryId);

            return ToCountryInfo(country);
        }

        public async Task<StateOfCreation> CreateCountryAsync(CountryCreation countryCreation)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            Country country = ToCountry(countryCreation);
            if (country.name == null)
                throw new Exception("");

            int checkExists = (int)await db.Countries.CountAsync(c => c.name == country.name);
            if (checkExists != 0)
                return StateOfCreation.AlreadyExists;

            int affected;
            if (country.description == null)
                affected = await db.Countries.InsertAsync(country, new List<string> { "ID", "description" });
            else
                affected = await db.Countries.InsertAsync(country, new List<string> { "ID" });

            return (affected == 0) ? StateOfCreation.Failed : StateOfCreation.Success;
        }

        public async Task<StateOfUpdate> UpdateCountryAsync(CountryUpdate countryUpdate)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            Country country = ToCountry(countryUpdate);
            if (country.name == null)
                throw new Exception("");

            int affected;
            if (country.description == null)
                affected = await db.Countries.UpdateAsync(
                    country,
                    c => new { c.name, c.updateAt },
                    c => c.ID == country.ID
                );
            else
                affected = await db.Countries.UpdateAsync(
                    country,
                    c => new { c.name, c.description, c.updateAt },
                    c => c.ID == country.ID
                );

            return (affected == 0) ? StateOfUpdate.Failed : StateOfUpdate.Success;
        }

        public async Task<StateOfDeletion> DeleteCountryAsync(int countryId)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            if (countryId <= 0)
                throw new Exception("");

            long filmNumberOfCountryId = await db.Films.CountAsync(f => f.countryId == countryId);
            if (filmNumberOfCountryId > 0)
                return StateOfDeletion.ConstraintExists;

            int affected = await db.Languages.DeleteAsync(l => l.ID == countryId);
            return (affected == 0) ? StateOfDeletion.Failed : StateOfDeletion.Success;
        }

        public async Task<int> CountAllAsync()
        {
            return (int)await db.Countries.CountAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                try
                {
                    if (disposing)
                    {

                    }
                    this.disposed = true;
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
    }
}
