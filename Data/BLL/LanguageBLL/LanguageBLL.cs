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
        protected LanguageBLL() 
            : base()
        {
            InitDAL();
        }

        protected LanguageBLL(BusinessLogicLayer bll)
            : base()
        {
            InitDAL(bll.db);
        }

        internal LanguageInfoForAdmin ToLanguageInfoForAdmin(Language language)
        {
            if (language == null)
                throw new Exception("");
            return new LanguageInfoForAdmin
            {
                ID = language.ID,
                name = language.name,
                description = language.description,
                createAt = language.createAt,
                updateAt = language.updateAt,
            };
        }

        internal LanguageInfoForUser ToLanguageInfoForUser(Language language)
        {
            if (language == null)
                throw new Exception("");
            return new LanguageInfoForUser
            {
                ID = language.ID,
                name = language.name,
                description = language.description,
            };
        }

        internal Language ToLanguage(LanguageCreation languageCreation)
        {
            if (languageCreation == null)
                throw new Exception("");
            return new DAL.Language
            {
                name = languageCreation.name,
                description = languageCreation.description,
                createAt = DateTime.Now,
                updateAt = DateTime.Now,
            };
        }

        internal DAL.Language ToLanguage(LanguageUpdate languageUpdate)
        {
            if (languageUpdate == null)
                throw new Exception("");
            return new DAL.Language
            {
                ID = languageUpdate.ID,
                name = languageUpdate.name,
                description = languageUpdate.description,
                updateAt = DateTime.Now
            };
        }

        public async Task<int> CountAllAsync()
        {
            return (int)await db.Languages.CountAsync();
        }
    }
}
