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
    public class LanguageBLL : BusinessLogicLayer
    {
        private DataAccessLevel dataAccessLevel;
        private bool disposed;

        public LanguageBLL(DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL();
            this.dataAccessLevel = dataAccessLevel;
            disposed = false;
        }

        public LanguageBLL(BusinessLogicLayer bll, DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL(bll.db);
            this.dataAccessLevel = dataAccessLevel;
            disposed = false;
        }

        private LanguageInfo ToLanguageInfo(Language language)
        {
            if (language == null)
                return null;
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
            if (dataAccessLevel == DataAccessLevel.Admin)
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

        public async Task<PagedList<LanguageInfo>> GetLanguagesAsync(int pageIndex, int pageSize)
        {
            SqlPagedList<Language> pagedList = null;
            Expression<Func<Language, object>> orderBy = c => new { c.ID };
            if (dataAccessLevel == DataAccessLevel.Admin)
                pagedList = await db.Languages.ToPagedListAsync(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = await db.Languages.ToPagedListAsync(
                    c => new { c.ID, c.name, c.description },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<LanguageInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToLanguageInfo(c)).ToList()
            };
        }

        public PagedList<LanguageInfo> GetLanguages(int pageIndex, int pageSize)
        {
            SqlPagedList<Language> pagedList = null;
            Expression<Func<Language, object>> orderBy = c => new { c.ID };
            if (dataAccessLevel == DataAccessLevel.Admin)
                pagedList = db.Languages.ToPagedList(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = db.Languages.ToPagedList(
                    c => new { c.ID, c.name, c.description },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<LanguageInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToLanguageInfo(c)).ToList()
            };
        }

        public async Task<LanguageInfo> GetLanguageAsync(int languageId)
        {
            if (languageId <= 0)
                throw new Exception("");
            Language language = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
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

        public async Task<StateOfUpdate> UpdateLanguageAsync(LanguageUpdate languageUpdate)
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
                    l => new { l.name, l.updateAt },
                    l => l.ID == language.ID
                );
            else
                affected = await db.Languages.UpdateAsync(
                    language,
                    l => new { l.name, l.description, l.updateAt },
                    l => l.ID == language.ID
                );

            return (affected == 0) ? StateOfUpdate.Failed : StateOfUpdate.Success;
        }

        public async Task<StateOfDeletion> DeleteLanguageAsync(int languageId)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            if (languageId <= 0)
                throw new Exception("");
            long filmNumberOfLanguageId = await db.Films.CountAsync(f => f.languageId == languageId);
            if (filmNumberOfLanguageId > 0)
                return StateOfDeletion.ConstraintExists;

            int affected = await db.Languages.DeleteAsync(l => l.ID == languageId);
            return (affected == 0) ? StateOfDeletion.Failed : StateOfDeletion.Success;
        }

        public async Task<int> CountAllAsync()
        {
            return (int)await db.Languages.CountAsync();
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
