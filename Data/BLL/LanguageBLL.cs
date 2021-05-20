using Data.DAL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class LanguageBLL : BusinessLogicLayer
    {
        private DataAccessLevel dataAccessLevel;
        public LanguageBLL(DataAccessLevel dataAccessLevel) 
            : base()
        {
            InitDAL();
            this.dataAccessLevel = dataAccessLevel;
        }

        public LanguageBLL(BusinessLogicLayer bll, DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL(bll.db);
            this.dataAccessLevel = dataAccessLevel;
        }

        private LanguageInfo ToLanguageInfo(Language language)
        {
            if (language == null)
                throw new Exception("");
            return new LanguageInfo
            {
                ID = language.ID,
                name = language.name,
                description = language.description,
                createAt = language.createAt,
                updateAt = language.updateAt,
            };
        }

        private Language ToLanguage(LanguageCreation languageCreation)
        {
            if (languageCreation == null)
                throw new Exception("");
            return new Language
            {
                name = languageCreation.name,
                description = languageCreation.description,
                createAt = DateTime.Now,
                updateAt = DateTime.Now,
            };
        }

        private Language ToLanguage(LanguageUpdate languageUpdate)
        {
            if (languageUpdate == null)
                throw new Exception("");
            return new Language
            {
                ID = languageUpdate.ID,
                name = languageUpdate.name,
                description = languageUpdate.description,
                updateAt = DateTime.Now
            };
        }

        public async Task<List<LanguageInfo>> GetLanguagesAsync()
        {
            List<LanguageInfo> languages = null;
            if(dataAccessLevel == DataAccessLevel.Admin)
                languages = (await db.Languages.ToListAsync())
                    .Select(l => ToLanguageInfo(l)).ToList();
            else
                languages = (await db.Languages.ToListAsync(c => new { c.ID, c.name, c.description }))
                    .Select(l => ToLanguageInfo(l)).ToList();
            return languages;
        }

        public List<LanguageInfo> GetLanguages()
        {
            List<LanguageInfo> languages = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                languages = db.Languages.ToList()
                    .Select(l => ToLanguageInfo(l)).ToList();
            else
                languages = db.Languages.ToList(c => new { c.ID, c.name, c.description })
                    .Select(l => ToLanguageInfo(l)).ToList();
            return languages;
        }

        public async Task<LanguageInfo> GetLanguageAsync(int languageId)
        {
            if (languageId <= 0)
                throw new Exception("");
            Language language = null;
            if(dataAccessLevel == DataAccessLevel.Admin)
                language = (await db.Languages.SingleOrDefaultAsync(l => l.ID == languageId));
            else
                language = (await db.Languages
                    .SingleOrDefaultAsync(l => new { l.ID, l.name, l.description }, l => l.ID == languageId));
            
            return ToLanguageInfo(language);
        }

        public LanguageInfo GetLanguage(int languageId)
        {
            if (languageId <= 0)
                throw new Exception("");
            Language language = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                language = db.Languages.SingleOrDefault(l => l.ID == languageId);
            else
                language = db.Languages
                    .SingleOrDefault(l => new { l.ID, l.name, l.description }, l => l.ID == languageId);

            return ToLanguageInfo(language);
        }

        public async Task<StateOfCreation> CreateLanguageAsync(LanguageCreation languageCreation)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            Language language = ToLanguage(languageCreation);
            if (language.name == null)
                throw new Exception("");

            int checkExists = (int)await db.Languages.CountAsync(l => l.name == language.name);
            if (checkExists != 0)
                return StateOfCreation.AlreadyExists;

            int affected;
            if (language.description == null)
                affected = await db.Languages.InsertAsync(language, new List<string> { "ID", "description" });
            else
                affected = await db.Languages.InsertAsync(language, new List<string> { "ID" });

            return (affected == 0) ? StateOfCreation.Failed : StateOfCreation.Success;
        }

        public async Task<bool> UpdateLanguageAsync(LanguageUpdate languageUpdate)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
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

            return (affected != 0);
        }

        public async Task<bool> DeleteAsync(int languageId)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            if (languageId <= 0)
                throw new Exception("");
            long filmNumberOfLanguageId = await db.Films.CountAsync(f => f.languageId == languageId);
            if (filmNumberOfLanguageId > 0)
                return false;

            int affected = await db.Languages.DeleteAsync(l => l.ID == languageId);
            return (affected != 0);
        }

        public async Task<int> CountAllAsync()
        {
            return (int)await db.Languages.CountAsync();
        }
    }
}
