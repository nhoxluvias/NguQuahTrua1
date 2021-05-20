using Data.DAL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class FilmBLL : BusinessLogicLayer
    {
        private DataAccessLevel dataAccessLevel;

        public FilmBLL(DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL();
            this.dataAccessLevel = dataAccessLevel;
        }

        public FilmBLL(BusinessLogicLayer bll, DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL(bll.db);
            this.dataAccessLevel = dataAccessLevel;
        }

        private FilmInfo ToFilmInfo(Film film)
        {
            if (film == null)
                throw new Exception("");

            return new FilmInfo
            {
                ID = film.ID,
                name = film.name,
                description = film.description,
                duration = film.duration,
                country = ((film.countryId != 0) 
                    ? new CountryBLL(this, dataAccessLevel).GetCountry(film.countryId) : null),
                productionCompany = film.productionCompany,
                thumbnail = film.thumbnail,
                language = ((film.languageId != 0)
                    ? new LanguageBLL(this, dataAccessLevel).GetLanguage(film.languageId) : null),
                releaseDate = film.releaseDate,
                upvote = film.upvote,
                downvote = film.downvote,
                views = film.views,
                createAt = film.createAt,
                updateAt = film.updateAt,
                Categories = new CategoryBLL(this, dataAccessLevel).GetCategoriesByFilmId(film.ID)
            };
        }

        private Film ToFilm(FilmCreation filmCreation)
        {
            if (filmCreation == null)
                throw new Exception("");
            return new Film
            {
                ID = null,
                name = filmCreation.name,
                description = filmCreation.description,
                duration = filmCreation.duration,
                countryId = filmCreation.countryId,
                productionCompany = filmCreation.productionCompany,
                thumbnail = filmCreation.thumbnail,
                languageId = filmCreation.languageId,
                releaseDate = filmCreation.releaseDate,
                views = filmCreation.views
            };
        }

        private Film ToFilm(FilmUpdate filmUpdate)
        {
            if (filmUpdate == null)
                throw new Exception("");
            return new Film
            {
                ID = filmUpdate.ID,
                name = filmUpdate.name,
                description = filmUpdate.description,
                duration = filmUpdate.duration,
                countryId = filmUpdate.countryId,
                productionCompany = filmUpdate.productionCompany,
                thumbnail = filmUpdate.thumbnail,
                languageId = filmUpdate.languageId,
                releaseDate = filmUpdate.releaseDate,
                views = filmUpdate.views
            };
        }

        public async Task<List<FilmInfo>> GetLatestFilmAsync()
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = @"select top 12 
                                [Film].[ID], [Film].[name], [Film].[thumbnail], [Film].[countryId]
                            from [Film] order by [createAt] desc";
            List<Film> t = await db.ExecuteReaderAsync<List<Film>>(sqlCommand);
            return (await db.ExecuteReaderAsync<List<Film>>(sqlCommand))
                .Select(f => ToFilmInfo(f)).ToList();
        }

        public async Task<List<FilmInfo>> GetFilmsAsync()
        {
            List<FilmInfo> films = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                films = (await db.Films.ToListAsync())
                    .Select(f => ToFilmInfo(f)).ToList();
            else
                films = (await db.Films.ToListAsync(
                            f => new
                            {
                                f.ID,
                                f.name,
                                f.description,
                                f.languageId,
                                f.countryId,
                                f.productionCompany,
                                f.releaseDate,
                                f.duration,
                                f.thumbnail,
                                f.upvote,
                                f.downvote,
                                f.views
                            })
                    ).Select(f => ToFilmInfo(f)).ToList();

            return films;
        }

        public async Task<FilmInfo> GetFilmAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");

            Film film = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                film = await db.Films.SingleOrDefaultAsync(f => f.ID == filmId);
            else
                film = await db.Films.SingleOrDefaultAsync(f => new
                {
                    f.ID,
                    f.name,
                    f.description,
                    f.languageId,
                    f.countryId,
                    f.productionCompany,
                    f.releaseDate,
                    f.duration,
                    f.thumbnail,
                    f.upvote,
                    f.downvote,
                    f.views
                }, f => f.ID == filmId);

            return ToFilmInfo(film);
        }

        public FilmInfo GetFilm(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");

            Film film = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                film = db.Films.SingleOrDefault(f => f.ID == filmId);
            else
                film = db.Films.SingleOrDefault(f => new
                {
                    f.ID,
                    f.name,
                    f.description,
                    f.languageId,
                    f.countryId,
                    f.productionCompany,
                    f.releaseDate,
                    f.duration,
                    f.thumbnail,
                    f.upvote,
                    f.downvote,
                    f.views
                }, f => f.ID == filmId);

            return ToFilmInfo(film);
        }

        public async Task<List<FilmInfo>> GetFilmsByCategoryIdAsync(int categoryId)
        {
            if (categoryId <= 0)
                throw new Exception("");

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            if(dataAccessLevel == DataAccessLevel.Admin)
                sqlCommand.CommandText = @"Select [Film].* from [Film], [CategoryDistribution]
                            where [Film].[ID] = [CategoryDistribution].[filmId]
                                and [CategoryDistribution].[categoryId] = @categoryId";
            else
                sqlCommand.CommandText = @"Select [Film].[ID],[Film].[name],[Film].[description],
                                [Film].[languageId],[Film].[countryId],[Film].[productionCompany],
                                [Film].[releaseDate],[Film].[duration],[Film].[thumbnail],
                                [Film].[upvote],[Film].[downvote],[Film].[views]
                            from [Film], [CategoryDistribution]
                            where [Film].[ID] = [CategoryDistribution].[filmId]
                                and [CategoryDistribution].[categoryId] = @categoryId";

            sqlCommand.Parameters.Add(new SqlParameter("@categoryId", categoryId));
            return await db.ExecuteReaderAsync<List<FilmInfo>>(sqlCommand);
        }

        public async Task<long> CountAllAsync()
        {
            return await db.Films.CountAsync();
        }
    }
}
