

using Data.DAL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class CountryBLLForUser : CountryBLL
    {
        public CountryBLLForUser()
            : base()
        {

        }

        public CountryBLLForUser(BusinessLogicLayer bll)
            : base(bll)
        {

        }

        public async Task<List<CountryInfoForUser>> GetCountriesAsync()
        {
            return (await db.Countries.ToListAsync(c => new { c.ID, c.name, c.description }))
                .Select(c => ToCountryInfoForUser(c)).ToList();
        }

        public async Task<CountryInfoForUser> GetCountryAsync(int countryId)
        {
            if (countryId <= 0)
                throw new Exception("");
            Country country = (await db.Countries.SingleOrDefaultAsync(c => c.ID == countryId));
            return ToCountryInfoForUser(country);
        }
    }
}
