using Data.DAL;
using Data.DTO;
using MSSQL_Lite.Access;
using MSSQL_Lite.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
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
                updateAt = DateTime.Now,
            };
        }

        public async Task<List<CastInfo>> GetCastsAsync() 
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

        public List<CastInfo> GetCasts()
        {
            List<CastInfo> casts = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                casts = db.Casts.ToList()
                    .Select(c => ToCastInfo(c)).ToList();
            else
                casts = db.Casts.ToList(c => new { c.ID, c.name, c.description })
                     .Select(c => ToCastInfo(c)).ToList();
            return casts;
        }

        public async Task<PagedList<CastInfo>> GetCastsAsync(int pageIndex, int pageSize)
        {
            SqlPagedList<Cast> pagedList = null;
            Expression<Func<Cast, object>> orderBy = c => new { c.ID };
            if (dataAccessLevel == DataAccessLevel.Admin)
                pagedList = await db.Casts.ToPagedListAsync(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = await db.Casts.ToPagedListAsync(
                    c => new { c.ID, c.name, c.description },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<CastInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToCastInfo(c)).ToList()
            };
        }

        public PagedList<CastInfo> GetCasts(int pageIndex, int pageSize)
        {
            SqlPagedList<Cast> pagedList = null;
            Expression<Func<Cast, object>> orderBy = c => new { c.ID };
            if (dataAccessLevel == DataAccessLevel.Admin)
                pagedList = db.Casts.ToPagedList(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = db.Casts.ToPagedList(
                    c => new { c.ID, c.name, c.description },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<CastInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToCastInfo(c)).ToList()
            };
        }

        public async Task<CastInfo> GetCastAsync(int castId)
        {
            if (castId <= 0)
                throw new Exception("");
            Cast cast = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                cast = await db.Casts
                     .SingleOrDefaultAsync(c => c.ID == castId);
            else
                cast = await db.Casts
                    .SingleOrDefaultAsync(c => new { c.ID, c.name, c.description }, c => c.ID == castId);

            return ToCastInfo(cast);
        }

        public CastInfo GetCast(int castId)
        {
            if (castId <= 0)
                throw new Exception("");
            Cast cast = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                cast = db.Casts
                     .SingleOrDefault(c => c.ID == castId);
            else
                cast = db.Casts
                    .SingleOrDefault(c => new { c.ID, c.name, c.description }, c => c.ID == castId);

            return ToCastInfo(cast);
        }

        public async Task<List<CastInfo>> GetCastsByFilmIdAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            if (dataAccessLevel == DataAccessLevel.Admin)
                sqlCommand.CommandText = @"Select [Cast].* from [CastOfFilm], [Cast]
                                where [CastOfFilm].[castID] = [Cast].[ID]
                                    and [CastOfFilm].[filmId] = @filmId";
            else
                sqlCommand.CommandText = @"Select [Cast].[ID], [Cast].[name], [Cast].[description] 
                                from [CastOfFilm], [Cast]
                                where [CastOfFilm].[categoryID] = [Cast].[ID]
                                    and [CastOfFilm].[filmId] = @filmId";

            sqlCommand.Parameters.Add(new SqlParameter("@filmId", filmId));
            return await db.ExecuteReaderAsync<List<CastInfo>>(sqlCommand);
        }

        public List<CastInfo> GetCastsByFilmId(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            if (dataAccessLevel == DataAccessLevel.Admin)
                sqlCommand.CommandText = @"Select [Cast].* from [CastOfFilm], [Cast]
                                where [CastOfFilm].[castID] = [Cast].[ID]
                                    and [CastOfFilm].[filmId] = @filmId";
            else
                sqlCommand.CommandText = @"Select [Cast].[ID], [Cast].[name], [Cast].[description] 
                                from [CastOfFilm], [Cast]
                                where [CastOfFilm].[categoryID] = [Cast].[ID]
                                    and [CastOfFilm].[filmId] = @filmId";

            sqlCommand.Parameters.Add(new SqlParameter("@filmId", filmId));
            return db.ExecuteReader<List<CastInfo>>(sqlCommand);
        }

        public async Task<StateOfCreation> CreateCastAsync(CastCreation castCreation)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            Cast cast = ToCast(castCreation);
            if (cast.name == null)
                throw new Exception("");

            int checkExists = (int)await db.Casts.CountAsync(c => c.name == cast.name);
            if (checkExists != 0)
                return StateOfCreation.AlreadyExists;

            int affected;
            if (cast.description == null)
                affected = await db.Casts.InsertAsync(cast, new List<string> { "ID", "description" });
            else
                affected = await db.Casts.InsertAsync(cast, new List<string> { "ID" });

            return (affected == 0) ? StateOfCreation.Failed : StateOfCreation.Success;
        }

        public async Task<StateOfUpdate> UpdateCastAsync(CastUpdate castUpdate)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            Cast cast = ToCast(castUpdate);
            if (cast.name == null)
                throw new Exception("");

            int affected;
            if (cast.description == null)
                affected = await db.Casts.UpdateAsync(
                    cast,
                    c => new { c.name, c.updateAt },
                    c => c.ID == cast.ID
                );
            else
                affected = await db.Casts.UpdateAsync(
                    cast,
                    c => new { c.name, c.description, c.updateAt },
                    c => c.ID == cast.ID
                );

            return (affected == 0) ? StateOfUpdate.Failed : StateOfUpdate.Success;
        }

        public async Task<StateOfDeletion> DeleteCastAsync(int castId)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            if (castId <= 0)
                throw new Exception("");

            long castOfFilmNumber = await db.CastOfFilm
                .CountAsync(cf => cf.castId == castId);
            if (castOfFilmNumber > 0)
                return StateOfDeletion.ConstraintExists;

            int affected = await db.Casts.DeleteAsync(c => c.ID == castId);
            return (affected == 0) ? StateOfDeletion.Failed : StateOfDeletion.Success;
        }

        public async Task<int> CountAllAsync()
        {
            return (int)await db.Casts.CountAsync();
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