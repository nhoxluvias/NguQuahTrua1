using Data.DAL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class CountryBLLForAdmin : CountryBLL
    {
        public CountryBLLForAdmin()
            : base()
        {

        }

        public CountryBLLForAdmin(BusinessLogicLayer bll)
            : base(bll)
        {

        }

        public async Task<List<CountryInfoForAdmin>> GetCountriesAsync()
        {
            return (await db.Countries.ToListAsync())
                .Select(c => ToCountryInfoForAdmin(c)).ToList();
        }

        public async Task<CountryInfoForAdmin> GetCountryAsync(int countryId)
        {
            if (countryId <= 0)
                throw new Exception("");
            Country country = (await db.Countries.SingleOrDefaultAsync(c => c.ID == countryId));
            return ToCountryInfoForAdmin(country);
        }

        public async Task<bool> CreateCountryAsync(CountryCreation countryCreation)
        {
            Country country = ToCountry(countryCreation);
            if (country.name == null)
                throw new Exception("");

            int affected;
            if (country.description == null)
                affected = await db.Countries.InsertAsync(country, new List<string> { "ID", "description" });
            else
                affected = await db.Countries.InsertAsync(country, new List<string> { "ID" });

            if (affected == 0)
                return false;
            return true;
        }

        public async Task<bool> UpdateCountryAsync(CountryUpdate countryUpdate)
        {
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

            if (affected == 0)
                return false;
            return true;
        }

        public async Task<bool> DeleteAsync(int countryId)
        {
            if (countryId <= 0)
                throw new Exception("");
            long filmNumberOfCountryId = await db.Films.CountAsync(f => f.countryId == countryId);
            if (filmNumberOfCountryId > 0)
                return false;
            int affected = await db.Languages.DeleteAsync(l => l.ID == countryId);
            if (affected == 0)
                return false;
            return true;
        }
    }
}
