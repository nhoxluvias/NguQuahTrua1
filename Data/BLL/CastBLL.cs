using Data.DAL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class CastBLL : BusinessLogicLayer
    {
        private DataAccessLevel dataAccessLevel;
        private bool disposed;
        public CastBLL(DataAccessLevel dataAccessLevel) 
            : base()
        {
            InitDAL();
            this.dataAccessLevel = dataAccessLevel;
            disposed = false;
        }

        public CastBLL(BusinessLogicLayer bll, DataAccessLevel dataAccessLevel) 
            : base()
        {
            InitDAL(bll.db);
            this.dataAccessLevel = dataAccessLevel;
            disposed = false;
        }

        private CastInfo ToCastInfo(Cast cast)
        {
            if (cast == null)
                return null;
            return new CastInfo
            {
                ID = cast.ID,
                name = cast.name,
                description = cast.description,
                createAt = cast.createAt,
                updateAt = cast.updateAt,
            };
        }

        private Cast ToCast(CastCreation castCreation)
        {
            if (castCreation == null)
                throw new Exception("@'castCreation' must be not null");
            return new Cast
            {
                name = castCreation.name,
                description = castCreation.description,
                createAt = DateTime.Now,
                updateAt = DateTime.Now,
            };
        }

        private Cast ToCast(CastUpdate castUpdate)
        {
            if (castUpdate == null)
                throw new Exception("");
            return new Cast
            {
                ID = castUpdate.ID,
                name = castUpdate.name,
                description = castUpdate.description,
                createAt = DateTime.Now,
                updateAt = DateTime.Now,
            };
        }

        public async Task<List<CastInfo>> GetCastsAsync() // Lấy nhiều diễn viên nên đặt tên có s để phân biệt
        {
            List<CastInfo> casts = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                casts = (await db.Casts.ToListAsync())
                    .Select(c => ToCastInfo(c)).ToList();
            else
                casts = (await db.Casts.ToListAsync(c => new { c.ID, c.name, c.description }))
                     .Select(c => ToCastInfo(c)).ToList();
            return casts;
        }

        //public List<CastInfo> GetCasts()
        //{
        //    List<CastInfo> casts = null;
        //    if (dataAccessLevel == DataAccessLevel.Admin)
        //        casts = (await db.Casts.ToListAsync())
        //            .Select(c => ToCastInfo(c)).ToList();
        //    else
        //        casts = (await db.Casts.ToListAsync(c => new { c.ID, c.name, c.description }))
        //             .Select(c => ToCastInfo(c)).ToList();
        //    return casts;
        //}
    }
}