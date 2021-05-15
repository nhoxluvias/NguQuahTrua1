using Data.DAL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class CountryBLL : BusinessLogicLayer
    {
        protected CountryBLL()
            : base()
        {
            InitDAL();
        }

        protected CountryBLL(BusinessLogicLayer bll)
            : base()
        {
            InitDAL(bll.db);
        }

        internal CountryInfoForAdmin ToCountryInfoForAdmin(Country country)
        {
            if (country == null)
                throw new Exception("");
            return new CountryInfoForAdmin
            {
                ID = country.ID,
                name = country.name,
                description = country.description,
                createAt = country.createAt,
                updateAt = country.updateAt
            };
        }

        internal CountryInfoForUser ToCountryInfoForUser(Country country)
        {
            if (country == null)
                throw new Exception("");
            return new CountryInfoForUser
            {
                ID = country.ID,
                name = country.name,
                description = country.description
            };
        }

        internal Country ToCountry(CountryCreation countryCreation)
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

        internal Country ToCountry(CountryUpdate countryUpdate)
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

        public async Task<int> CountAllAsync()
        {
            return (int)await db.Countries.CountAsync();
        }
    }
}
