using Data.DAL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class LanguageBLLForUser : LanguageBLL
    {
        public LanguageBLLForUser()
            : base()
        {

        }

        public LanguageBLLForUser(BusinessLogicLayer bll)
            : base(bll)
        {

        }

        public async Task<List<LanguageInfoForUser>> GetLanguagesAsync()
        {
            return (await db.Languages.ToListAsync(l => new { l.ID, l.name, l.description }))
                .Select(l => ToLanguageInfoForUser(l)).ToList();
        }

        public async Task<LanguageInfoForUser> GetLanguageAsync(int languageId)
        {
            if (languageId <= 0)
                throw new Exception("");
            Language language = (await db.Languages.SingleOrDefaultAsync(l => l.ID == languageId));
            return ToLanguageInfoForUser(language);
        }
    }
}
