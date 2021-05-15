using Data.DAL;
using Data.DTO;
using System;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class DirectorBLL : BusinessLogicLayer
    {
        protected DirectorBLL()
            : base()
        {
            InitDAL();
        }

        protected DirectorBLL(BusinessLogicLayer bll)
            : base()
        {
            InitDAL(bll.db);
        }

        internal DirectorInfoForAdmin ToDirectorInfoForAdmin(Director director)
        {
            if (director == null)
                throw new Exception("");
            return new DirectorInfoForAdmin
            {
                ID = director.ID,
                name = director.name,
                description = director.description,
                createAt = director.createAt,
                updateAt = director.updateAt,
            };
        }

        internal DirectorInfoForUser ToDirectorInfoForUser(Director director)
        {
            if (director == null)
                throw new Exception("");
            return new DirectorInfoForUser
            {
                ID = director.ID,
                name = director.name,
                description = director.description
            };
        }

        internal Director ToDirector(DirectorCreation directorCreation)
        {
            if (directorCreation == null)
                throw new Exception("");
            return new Director
            {
                name = directorCreation.name,
                description = directorCreation.description,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };
        }

        internal Director ToDirector(DirectorUpdate directorUpdate)
        {
            if (directorUpdate == null)
                throw new Exception("");
            return new Director
            {
                name = directorUpdate.name,
                description = directorUpdate.description,
                updateAt = DateTime.Now
            };
        }

        public async Task<long> CountAllAsync()
        {
            return await db.Directors.CountAsync();
        }
    }
}
