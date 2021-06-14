using Data.Common.Hash;
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
    public class FilmBLL : BusinessLogicLayer
    {
        private DataAccessLevel dataAccessLevel;
        private bool disposed;

        public FilmBLL(DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL();
            this.dataAccessLevel = dataAccessLevel;
            disposed = false;
        }

        public FilmBLL(BusinessLogicLayer bll, DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL(bll.db);
            this.dataAccessLevel = dataAccessLevel;
            disposed = false;
        }

        private FilmInfo ToFilmInfo(Film film)
        {
            if (film == null)
                return null;

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
                views = 0,
                downvote = 0,
                upvote = 0,
                createAt = DateTime.Now,
                updateAt = DateTime.Now,
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

        public async Task<PagedList<FilmInfo>> GetFilmsAsync(int pageIndex, int pageSize)
        {
            SqlPagedList<Film> pagedList = null;
            Expression<Func<Film, object>> orderBy = f => new { f.ID };
            if (dataAccessLevel == DataAccessLevel.Admin)
                pagedList = await db.Films.ToPagedListAsync(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = await db.Films.ToPagedListAsync(
                    f => new {
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
                    },orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<FilmInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(f => ToFilmInfo(f)).ToList()
            };
        }

        public PagedList<FilmInfo> GetFilms(int pageIndex, int pageSize)
        {
            SqlPagedList<Film> pagedList = null;
            Expression<Func<Film, object>> orderBy = f => new { f.ID };
            if (dataAccessLevel == DataAccessLevel.Admin)
                pagedList = db.Films.ToPagedList(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = db.Films.ToPagedList(
                    f => new {
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
                    },orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<FilmInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(f => ToFilmInfo(f)).ToList()
            };
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

        public async Task<StateOfCreation> CreateFilmAsync(FilmCreation filmCreation)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            Film film = ToFilm(filmCreation);
            if (string.IsNullOrEmpty(film.name) || film.languageId <= 0 
                || film.countryId <= 0 || string.IsNullOrEmpty(film.thumbnail)
            )
            {
                throw new Exception("");
            }

            long checkExists = await db.Films.CountAsync(
                f => f.name == film.name
                    && f.languageId == film.languageId
                    && f.countryId == film.countryId
                    && f.thumbnail == film.thumbnail
            );
            if (checkExists != 0)
                return StateOfCreation.AlreadyExists;

            MD5_Hash md5 = new MD5_Hash();
            film.ID = md5.Hash(string.Format("name:{0}//random:{1}", film.name, new Random().NextString(25)));
            int affected;
            if(
                film.description == null || film.duration == null
                || film.productionCompany == null
            ){
                if(
                    film.description == null && film.duration == null
                    && film.productionCompany == null
                )
                {
                    affected = await db.Films.InsertAsync(film, new List<string> { "description", "productionCompany", "duration" });
                }
                else if(film.description == null)
                {
                    affected = await db.Films.InsertAsync(film, new List<string> { "description" });
                }else if(film.duration == null)
                {
                    affected = await db.Films.InsertAsync(film, new List<string> { "duration" });
                }
                else
                {
                    affected = await db.Films.InsertAsync(film, new List<string> { "productionCompany" });
                }
            }
            else
            {
                affected = await db.Films.InsertAsync(film);
            }

            return (affected == 0) ? StateOfCreation.Failed : StateOfCreation.Success;
        }

        public async Task<StateOfCreation> UpdateFilmAsync(FilmUpdate filmUpdate)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            Film film = ToFilm(filmUpdate);
            if (string.IsNullOrEmpty(film.name) || film.languageId <= 0
                || film.countryId <= 0 || string.IsNullOrEmpty(film.thumbnail)
            )
            {
                throw new Exception("");
            }

            int affected;
            if (
                film.description == null || film.duration == null
                || film.productionCompany == null
            )
            {
                if (
                    film.description == null && film.duration == null
                    && film.productionCompany == null
                )
                {
                    affected = await db.Films.InsertAsync(film, new List<string> { "description", "productionCompany", "duration" });
                    affected = await db.Films.UpdateAsync(
                    film,
                    f => new
                    {
                        f.name,
                        f.countryId,
                        f.languageId,
                        f.releaseDate,
                        f.thumbnail
                    },
                    f => f.ID == film.ID
                );
                }
                else if (film.description == null)
                {
                    affected = await db.Films.UpdateAsync(
                    film,
                    f => new
                    {
                        f.name,
                        f.duration,
                        f.countryId,
                        f.languageId,
                        f.productionCompany,
                        f.releaseDate,
                        f.thumbnail
                    },
                    f => f.ID == film.ID
                );
                }
                else if (film.duration == null)
                {
                    affected = await db.Films.UpdateAsync(
                    film,
                    f => new
                    {
                        f.name,
                        f.description,
                        f.countryId,
                        f.languageId,
                        f.productionCompany,
                        f.releaseDate,
                        f.thumbnail
                    },
                    f => f.ID == film.ID
                );
                }
                else
                {
                    affected = await db.Films.UpdateAsync(
                    film,
                    f => new
                    {
                        f.name,
                        f.description,
                        f.duration,
                        f.countryId,
                        f.languageId,
                        f.releaseDate,
                        f.thumbnail
                    },
                    f => f.ID == film.ID
                );
                }
            }
            else
            {
                affected = await db.Films.UpdateAsync(
                    film,
                    f => new
                    {
                        f.name,
                        f.description,
                        f.duration,
                        f.countryId,
                        f.languageId,
                        f.productionCompany,
                        f.releaseDate,
                        f.thumbnail
                    },
                    f => f.ID == film.ID
                );
            }

            return (affected == 0) ? StateOfCreation.Failed : StateOfCreation.Success;
        }

        public async Task<StateOfDeletion> DeleteFilmAsync(string filmId)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");

            int affected = await db.Films.DeleteAsync(f => f.ID == filmId);
            return (affected == 0) ? StateOfDeletion.Failed : StateOfDeletion.Success;
        }


        public async Task<long> CountAllAsync()
        {
            return await db.Films.CountAsync();
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
