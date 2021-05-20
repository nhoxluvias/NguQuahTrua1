using Data.DAL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class DirectorBLL : BusinessLogicLayer
    {
        private DataAccessLevel dataAccessLevel;
        public DirectorBLL(DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL();
            this.dataAccessLevel = dataAccessLevel;
        }

        public DirectorBLL(BusinessLogicLayer bll, DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL(bll.db);
            this.dataAccessLevel = dataAccessLevel;
        }

        private DirectorInfo ToDirectorInfo(Director director)
        {
            if (director == null)
                throw new Exception("");
            return new DirectorInfo
            {
                ID = director.ID,
                name = director.name,
                description = director.description,
                createAt = director.createAt,
                updateAt = director.updateAt,
            };
        }

        private Director ToDirector(DirectorCreation directorCreation)
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

        private Director ToDirector(DirectorUpdate directorUpdate)
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

        public async Task<List<DirectorInfo>> GetDirectorsAsync()
        {
            List<DirectorInfo> directors = null;
            if(dataAccessLevel == DataAccessLevel.Admin)
                directors = (await db.Directors.ToListAsync())
                    .Select(d => ToDirectorInfo(d)).ToList();
            else
                directors = (await db.Directors.ToListAsync(c => new { c.ID, c.name, c.description }))
                    .Select(d => ToDirectorInfo(d)).ToList();
            return directors;
        }

        public List<DirectorInfo> GetDirectors()
        {
            List<DirectorInfo> directors = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                directors = db.Directors.ToList()
                    .Select(d => ToDirectorInfo(d)).ToList();
            else
                directors = db.Directors.ToList(c => new { c.ID, c.name, c.description })
                    .Select(d => ToDirectorInfo(d)).ToList();
            return directors;
        }

        public async Task<DirectorInfo> GetDirectorAsync(long directorId)
        {
            if (directorId <= 0)
                throw new Exception("");
            Director director = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                director = await db.Directors.SingleOrDefaultAsync(d => d.ID == directorId);
            else
                director = await db.Directors
                    .SingleOrDefaultAsync(d => new { d.ID, d.name, d.description }, d => d.ID == directorId);
            return ToDirectorInfo(director);
        }

        public DirectorInfo GetDirector(long directorId)
        {
            if (directorId <= 0)
                throw new Exception("");
            Director director = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                director = db.Directors.SingleOrDefault(d => d.ID == directorId);
            else
                director = db.Directors
                    .SingleOrDefault(d => new { d.ID, d.name, d.description }, d => d.ID == directorId);
            return ToDirectorInfo(director);
        }

        public async Task<List<DirectorInfo>> GetDirectorsByFilmIdAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            if(dataAccessLevel == DataAccessLevel.Admin)
                sqlCommand.CommandText = @"Select [Director].* from [Director], [DirectorOfFilm] 
                            where [Director].[ID] = [DirectorOfFilm].[directorId]
                                and [Director].[filmId] = @filmId";
            else
                sqlCommand.CommandText = @"Select [Director].[ID], [Director].[name], [Director].[description] 
                            from [Director], [DirectorOfFilm] 
                            where [Director].[ID] = [DirectorOfFilm].[directorId]
                                and [Director].[filmId] = @filmId";

            sqlCommand.Parameters.Add(new SqlParameter("@filmId", filmId));
            return await db.ExecuteReaderAsync<List<DirectorInfo>>(sqlCommand);
        }

        public List<DirectorInfo> GetDirectorsByFilmId(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            if (dataAccessLevel == DataAccessLevel.Admin)
                sqlCommand.CommandText = @"Select [Director].* from [Director], [DirectorOfFilm] 
                            where [Director].[ID] = [DirectorOfFilm].[directorId]
                                and [Director].[filmId] = @filmId";
            else
                sqlCommand.CommandText = @"Select [Director].[ID], [Director].[name], [Director].[description] 
                            from [Director], [DirectorOfFilm] 
                            where [Director].[ID] = [DirectorOfFilm].[directorId]
                                and [Director].[filmId] = @filmId";

            sqlCommand.Parameters.Add(new SqlParameter("@filmId", filmId));
            return db.ExecuteReader<List<DirectorInfo>>(sqlCommand);
        }

        public async Task<StateOfCreation> CreateDirectorAsync(DirectorCreation directorCreation)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            Director director = ToDirector(directorCreation);
            if (director.name == null)
                throw new Exception("");

            long checkExists = await db.Directors.CountAsync(c => c.name == director.name);
            if (checkExists != 0)
                return StateOfCreation.AlreadyExists;

            int affected;
            if (director.description == null)
                affected = await db.Directors.InsertAsync(director, new List<string> { "ID", "description" });
            else
                affected = await db.Directors.InsertAsync(director, new List<string> { "ID" });

            return (affected == 0) ? StateOfCreation.Failed : StateOfCreation.Success;
        }

        public async Task<bool> UpdateDirectorAsync(DirectorUpdate directorUpdate)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            Director director = ToDirector(directorUpdate);
            if (director.name == null)
                throw new Exception("");

            int affected;
            if (director.description == null)
                affected = await db.Directors.UpdateAsync(
                    director,
                    d => new { d.name, d.updateAt },
                    d => d.ID == director.ID
                );
            else
                affected = await db.Directors.UpdateAsync(
                    director,
                    d => new { d.name, d.description, d.updateAt },
                    d => d.ID == director.ID
                );

            return (affected != 0);
        }

        public async Task<bool> DeleteDirectorAsync(long directorId)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            if (directorId <= 0)
                throw new Exception("");

            long directorOfFilmNumber = await db.DirectorOfFilms.CountAsync(df => df.directorId == directorId);
            if (directorOfFilmNumber > 0)
                return false;

            int affected = await db.Directors.DeleteAsync(d => d.ID == directorId);
            return (affected != 0);
        }

        public async Task<long> CountAllAsync()
        {
            return await db.Directors.CountAsync();
        }
    }
}
