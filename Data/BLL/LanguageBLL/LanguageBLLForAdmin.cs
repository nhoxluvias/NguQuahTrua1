using Data.DAL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class LanguageBLLForAdmin : LanguageBLL
    {
        public LanguageBLLForAdmin()
            : base()
        {

        }

        public LanguageBLLForAdmin(BusinessLogicLayer bll)
            : base()
        {

        }

        public async Task<List<LanguageInfoForAdmin>> GetLanguagesAsync()
        {
            return (await db.Languages.ToListAsync())
                .Select(l => ToLanguageInfoForAdmin(l)).ToList();
        }

        public async Task<LanguageInfoForAdmin> GetLanguageAsync(int languageId)
        {
            if (languageId <= 0)
                throw new Exception("");
            Language language = (await db.Languages.SingleOrDefaultAsync(l => l.ID == languageId));
            return ToLanguageInfoForAdmin(language);
        }

        public async Task<bool> CreateLanguageAsync(LanguageCreation languageCreation)
        {
            Language language = ToLanguage(languageCreation);
            if (language.name == null)
                throw new Exception("");

            int affected;
            if (language.description == null)
                affected = await db.Languages.InsertAsync(language, new List<string> { "ID", "description" });
            else
                affected = await db.Languages.InsertAsync(language, new List<string> { "ID" });

            if (affected == 0)
                return false;
            return true;
        }

        public async Task<bool> UpdateLanguageAsync(LanguageUpdate languageUpdate)
        {
            Language language = ToLanguage(languageUpdate);
            if (language.name == null)
                throw new Exception("");

            int affected;
            if (language.description == null)
                affected = await db.Languages.UpdateAsync(
                    language,
                    l => new { l.name, l.createAt },
                    l => l.ID == language.ID
                );
            else
                affected = await db.Languages.UpdateAsync(
                    language,
                    l => new { l.name, l.createAt },
                    l => l.ID == language.ID
                );

            if (affected == 0)
                return false;
            return true;
        }

        public async Task<bool> DeleteAsync(int languageId)
        {
            if (languageId <= 0)
                throw new Exception("");
            long filmNumberOfLanguageId = await db.Films.CountAsync(f => f.langugeId == languageId);
            if (filmNumberOfLanguageId > 0)
                return false;
            int affected = await db.Languages.DeleteAsync(l => l.ID == languageId);
            if (affected == 0)
                return false;
            return true;
        }
    }
}
