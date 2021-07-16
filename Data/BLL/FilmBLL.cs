using Common.Hash;
using Common.Web;
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
using System.Threading.Tasks;

namespace Data.BLL
{
    public class FilmBLL : BusinessLogicLayer
    {
        private bool disposed;
        private bool includeCategory;
        private bool includeTag;
        private bool includeLanguage;
        private bool includeCountry;
        private bool includeCast;
        private bool includeDirector;

        public bool IncludeCategory { set { includeCategory = value; } }
        public bool IncludeTag { set { includeTag = value; } }
        public bool IncludeLanguage { set { includeLanguage = value; } }
        public bool IncludeCountry { set { includeCountry = value; } }
        public bool IncludeCast { set { includeCast = value; } }
        public bool IncludeDirector { set { includeDirector = value; } }

        public FilmBLL()
            : base()
        {
            InitDAL();
            disposed = false;
        }

        public FilmBLL(BusinessLogicLayer bll)
            : base()
        {
            InitDAL(bll.db);
            disposed = false;
        }

        public override void SetDefault()
        {
            base.SetDefault();
            includeCategory = false;
            includeTag = false;
            includeCountry = false;
            includeLanguage = false;
            includeCast = false;
            includeDirector = false;
        }

        private FilmInfo ToFilmInfo(Film film)
        {
            if (film == null)
                return null;

            FilmInfo filmInfo = new FilmInfo()
            {
                ID = film.ID,
                name = film.name,
                description = film.description,
                productionCompany = film.productionCompany,
                thumbnail = film.thumbnail,
                releaseDate = film.releaseDate,
                upvote = film.upvote,
                downvote = film.downvote,
                views = film.views,
                duration = film.duration,
                source = film.source
            };

            if (includeCategory)
                filmInfo.Categories = new CategoryBLL(this).GetCategoriesByFilmId(film.ID);

            if (includeTag)
                filmInfo.Tags = new TagBLL(this).GetTagsByFilmId(film.ID);

            if (includeDirector)
                filmInfo.Directors = new DirectorBLL(this).GetDirectorsByFilmId(film.ID);

            if (includeCast)
                filmInfo.Casts = new CastBLL(this).GetCastsByFilmId(film.ID);

            if (includeLanguage)
                filmInfo.Language = ((film.languageId != 0)
                    ? new LanguageBLL(this).GetLanguage(film.languageId) : null);

            if (includeCountry)
                filmInfo.Country = ((film.countryId != 0)
                    ? new CountryBLL(this).GetCountry(film.countryId) : null);

            if (includeTimestamp)
            {
                filmInfo.createAt = film.createAt;
                filmInfo.updateAt = film.updateAt;
            }

            return filmInfo;
        }

        private Film ToFilm(FilmCreation filmCreation)
        {
            if (filmCreation == null)
                throw new Exception("");

            HashFunction hash = new HashFunction();
            string filmId = hash.MD5_Hash(string
                .Format("name:{0}//random:{1}", filmCreation.name, new Random().NextString(25)));

            return new Film
            {
                ID = filmId,
                name = filmCreation.name,
                description = filmCreation.description,
                countryId = filmCreation.countryId,
                productionCompany = filmCreation.productionCompany,
                thumbnail = filmCreation.thumbnail,
                languageId = filmCreation.languageId,
                releaseDate = filmCreation.releaseDate,
                duration = filmCreation.duration,
                source = filmCreation.source,
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
                countryId = filmUpdate.countryId,
                productionCompany = filmUpdate.productionCompany,
                thumbnail = filmUpdate.thumbnail,
                languageId = filmUpdate.languageId,
                releaseDate = filmUpdate.releaseDate,
                duration = filmUpdate.duration,
                source = filmUpdate.source,
                updateAt = DateTime.Now
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
            if (includeTimestamp)
                films = (await db.Films.ToListAsync()).Select(f => ToFilmInfo(f)).ToList();
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
                                f.thumbnail,
                                f.upvote,
                                f.downvote,
                                f.views,
                                f.duration,
                                f.source
                            })
                    ).Select(f => ToFilmInfo(f)).ToList();

            return films;
        }

        public async Task<FilmInfo> GetFilmAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");

            Film film = null;
            if (includeTimestamp)
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
                    f.thumbnail,
                    f.upvote,
                    f.downvote,
                    f.views,
                    f.duration,
                    f.source
                }, f => f.ID == filmId);

            return ToFilmInfo(film);
        }

        public async Task<PagedList<FilmInfo>> GetFilmsAsync(int pageIndex, int pageSize)
        {
            SqlPagedList<Film> pagedList = null;
            Expression<Func<Film, object>> orderBy = f => new { f.ID };
            if (includeTimestamp)
                pagedList = await db.Films.ToPagedListAsync(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = await db.Films.ToPagedListAsync(
                    f => new
                    {
                        f.ID,
                        f.name,
                        f.description,
                        f.languageId,
                        f.countryId,
                        f.productionCompany,
                        f.releaseDate,
                        f.thumbnail,
                        f.upvote,
                        f.downvote,
                        f.views,
                        f.duration,
                        f.source
                    }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
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
            if (includeTimestamp)
                pagedList = db.Films.ToPagedList(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = db.Films.ToPagedList(
                    f => new
                    {
                        f.ID,
                        f.name,
                        f.description,
                        f.languageId,
                        f.countryId,
                        f.productionCompany,
                        f.releaseDate,
                        f.thumbnail,
                        f.upvote,
                        f.downvote,
                        f.views,
                        f.duration,
                        f.source
                    }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
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
            if (includeTimestamp)
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
                    f.thumbnail,
                    f.upvote,
                    f.downvote,
                    f.views,
                    f.duration,
                    f.source
                }, f => f.ID == filmId);

            return ToFilmInfo(film);
        }

        public async Task<List<FilmInfo>> GetFilmsByCategoryIdAsync(int categoryId)
        {
            if (categoryId <= 0)
                throw new Exception("");

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            if (includeTimestamp)
                sqlCommand.CommandText = @"Select [Film].* from [Film], [CategoryDistribution]
                            where [Film].[ID] = [CategoryDistribution].[filmId]
                                and [CategoryDistribution].[categoryId] = @categoryId";
            else
                sqlCommand.CommandText = @"Select [Film].[ID],[Film].[name],[Film].[description],
                                [Film].[languageId],[Film].[countryId],[Film].[productionCompany],
                                [Film].[releaseDate],[Film].[duration],[Film].[thumbnail],
                                [Film].[upvote],[Film].[downvote],[Film].[views], [Film].[source]
                            from [Film], [CategoryDistribution]
                            where [Film].[ID] = [CategoryDistribution].[filmId]
                                and [CategoryDistribution].[categoryId] = @categoryId";

            sqlCommand.Parameters.Add(new SqlParameter("@categoryId", categoryId));
            return await db.ExecuteReaderAsync<List<FilmInfo>>(sqlCommand);
        }

        public async Task<StateOfCreation> CreateFilmAsync(FilmCreation filmCreation)
        {
            Film film = ToFilm(filmCreation);
            if (string.IsNullOrEmpty(film.name) || film.languageId <= 0
                || string.IsNullOrEmpty(film.productionCompany) || film.countryId <= 0
            )
            {
                throw new Exception("");
            }

            long checkExists = await db.Films.CountAsync(
                f => (f.ID == film.ID) || (f.name == film.name
                    && f.languageId == film.languageId
                    && f.countryId == film.countryId)
            );
            if (checkExists != 0)
                return StateOfCreation.AlreadyExists;

            int affected;
            if (film.description == null)
                affected = await db.Films.InsertAsync(film, new List<string> { "description", "duration", "thumbnail", "source" });
            else
                affected = await db.Films.InsertAsync(film, new List<string> { "duration", "thumbnail", "source" });

            return (affected == 0) ? StateOfCreation.Failed : StateOfCreation.Success;
        }

        public async Task<StateOfUpdate> UpdateFilmAsync(FilmUpdate filmUpdate)
        {
            Film film = ToFilm(filmUpdate);
            if (string.IsNullOrEmpty(film.name) || film.languageId <= 0
                || string.IsNullOrEmpty(film.productionCompany) || film.countryId <= 0
            )
            {
                throw new Exception("");
            }

            int affected;
            if (film.description == null)
                affected = await db.Films.UpdateAsync(film, f => new
                {
                    f.name,
                    f.countryId,
                    f.languageId,
                    f.releaseDate,
                    f.productionCompany,
                    f.updateAt
                }, f => f.ID == film.ID);
            else
                affected = await db.Films.UpdateAsync(film, f => new
                {
                    f.name,
                    f.description,
                    f.countryId,
                    f.languageId,
                    f.releaseDate,
                    f.productionCompany,
                    f.updateAt
                }, f => f.ID == film.ID);

            return (affected == 0) ? StateOfUpdate.Failed : StateOfUpdate.Success;
        }

        public async Task<StateOfDeletion> DeleteFilmAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");

            StateOfDeletion state1 = await DeleteAllCategoryAsync(filmId);
            StateOfDeletion state2 = await DeleteAllTagAsync(filmId);
            StateOfDeletion state3 = await DeleteAllDirectorAsync(filmId);
            StateOfDeletion state4 = await DeleteAllCastAsync(filmId);

            if (state1 == StateOfDeletion.Success && state2 == StateOfDeletion.Success
                && state3 == StateOfDeletion.Success && state4 == StateOfDeletion.Success)
            {
                int affected = await db.Films.DeleteAsync(f => f.ID == filmId);
                return (affected == 0) ? StateOfDeletion.Failed : StateOfDeletion.Success;
            }

            return StateOfDeletion.ConstraintExists;
        }

        public async Task<StateOfCreation> AddCategoryAsync(string filmId, int categoryId)
        {
            if (string.IsNullOrEmpty(filmId) || categoryId <= 0)
                throw new Exception("");

            long checkExists = await db.CategoryDistributions
                .CountAsync(cd => cd.filmId == filmId && cd.categoryId == categoryId);
            if (checkExists != 0)
                return StateOfCreation.AlreadyExists;

            CategoryDistribution categoryDistribution = new CategoryDistribution
            {
                filmId = filmId,
                categoryId = categoryId,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };

            int affected = await db.CategoryDistributions.InsertAsync(categoryDistribution);
            return (affected == 0) ? StateOfCreation.Failed : StateOfCreation.Success;
        }

        public async Task<StateOfDeletion> DeleteCategoryAsync(string filmId, int categoryId)
        {
            if (string.IsNullOrEmpty(filmId) || categoryId <= 0)
                throw new Exception("");

            int affected = await db.CategoryDistributions
                .DeleteAsync(cd => cd.filmId == filmId && cd.categoryId == categoryId);

            return (affected == 0) ? StateOfDeletion.Failed : StateOfDeletion.Success;
        }

        public async Task<StateOfDeletion> DeleteAllCategoryAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");

            int affected = await db.CategoryDistributions
                .DeleteAsync(cd => cd.filmId == filmId);

            return (affected == 0) ? StateOfDeletion.Failed : StateOfDeletion.Success;
        }

        public async Task<StateOfCreation> AddTagAsync(string filmId, int tagId)
        {
            if (string.IsNullOrEmpty(filmId) || tagId <= 0)
                throw new Exception("");

            long checkExists = await db.TagDistributions
                .CountAsync(td => td.filmId == filmId && td.tagId == tagId);
            if (checkExists != 0)
                return StateOfCreation.AlreadyExists;

            TagDistribution tagDistribution = new TagDistribution
            {
                filmId = filmId,
                tagId = tagId,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };

            int affected = await db.TagDistributions.InsertAsync(tagDistribution);
            return (affected == 0) ? StateOfCreation.Failed : StateOfCreation.Success;
        }

        public async Task<StateOfDeletion> DeleteTagAsync(string filmId, int tagId)
        {
            if (string.IsNullOrEmpty(filmId) || tagId <= 0)
                throw new Exception("");

            int affected = await db.TagDistributions
                .DeleteAsync(td => td.filmId == filmId && td.tagId == tagId);

            return (affected == 0) ? StateOfDeletion.Failed : StateOfDeletion.Success;
        }

        public async Task<StateOfDeletion> DeleteAllTagAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");

            int affected = await db.TagDistributions
                .DeleteAsync(td => td.filmId == filmId);

            return (affected == 0) ? StateOfDeletion.Failed : StateOfDeletion.Success;
        }

        public async Task<StateOfCreation> AddDirectorAsync(string filmId, long directorId, string directorRole)
        {
            if (string.IsNullOrEmpty(filmId) || directorId <= 0 || string.IsNullOrEmpty(directorRole))
                throw new Exception("");

            long checkExists = await db.DirectorOfFilms
                .CountAsync(df => df.filmId == filmId && df.directorId == directorId);
            if (checkExists != 0)
                return StateOfCreation.AlreadyExists;

            DirectorOfFilm directorOfFilm = new DirectorOfFilm
            {
                filmId = filmId,
                directorId = directorId,
                role = directorRole,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };

            int affected = await db.DirectorOfFilms.InsertAsync(directorOfFilm);
            return (affected == 0) ? StateOfCreation.Failed : StateOfCreation.Success;
        }

        public async Task<StateOfDeletion> DeleteDirectorAsync(string filmId, long directorId)
        {
            if (string.IsNullOrEmpty(filmId) || directorId <= 0)
                throw new Exception("");

            int affected = await db.DirectorOfFilms
                .DeleteAsync(df => df.filmId == filmId && df.directorId == directorId);

            return (affected == 0) ? StateOfDeletion.Failed : StateOfDeletion.Success;
        }

        public async Task<StateOfDeletion> DeleteAllDirectorAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");

            int affected = await db.DirectorOfFilms
                .DeleteAsync(df => df.filmId == filmId);

            return (affected == 0) ? StateOfDeletion.Failed : StateOfDeletion.Success;
        }

        public async Task<StateOfCreation> AddCastAsync(string filmId, long castId, string castRole)
        {
            if (string.IsNullOrEmpty(filmId) || castId <= 0 || string.IsNullOrEmpty(castRole))
                throw new Exception("");

            long checkExists = await db.CastOfFilms
                .CountAsync(cf => cf.filmId == filmId && cf.castId == castId);
            if (checkExists != 0)
                return StateOfCreation.AlreadyExists;

            CastOfFilm castOfFilm = new CastOfFilm
            {
                filmId = filmId,
                castId = castId,
                role = castRole,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };

            int affected = await db.CastOfFilms.InsertAsync(castOfFilm);
            return (affected == 0) ? StateOfCreation.Failed : StateOfCreation.Success;
        }

        public async Task<StateOfDeletion> DeleteCastAsync(string filmId, long castId)
        {
            if (string.IsNullOrEmpty(filmId) || castId <= 0)
                throw new Exception("");

            int affected = await db.CastOfFilms
                .DeleteAsync(cf => cf.filmId == filmId && cf.castId == castId);

            return (affected == 0) ? StateOfDeletion.Failed : StateOfDeletion.Success;
        }

        public async Task<StateOfDeletion> DeleteAllCastAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");

            int affected = await db.CastOfFilms
                .DeleteAsync(cf => cf.filmId == filmId);

            return (affected == 0) ? StateOfDeletion.Failed : StateOfDeletion.Success;
        }

        public async Task<StateOfUpdate> AddImageAsync(string filmId, string filePath)
        {
            if (string.IsNullOrEmpty(filmId) || string.IsNullOrEmpty(filePath))
                throw new Exception("");

            Film film = await db.Films.SingleOrDefaultAsync(f => new { f.ID, f.thumbnail }, f => f.ID == filmId);
            if (film == null)
                return StateOfUpdate.Failed;

            if (!string.IsNullOrEmpty(film.thumbnail))
                return StateOfUpdate.Failed;

            int affected = await db.Films
                .UpdateAsync(new Film { thumbnail = filePath }, f => new { f.thumbnail }, f => f.ID == film.ID);

            return (affected == 0) ? StateOfUpdate.Failed : StateOfUpdate.Success;
        }

        public async Task<StateOfUpdate> DeleteImageAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");

            int affected = await db.Films
                .UpdateAsync(new Film { thumbnail = null }, f => new { f.thumbnail }, f => f.ID == filmId);

            return (affected == 0) ? StateOfUpdate.Failed : StateOfUpdate.Success;
        }

        public async Task<StateOfUpdate> AddSourceAsync(string filmId, string filePath)
        {
            if (string.IsNullOrEmpty(filmId) || string.IsNullOrEmpty(filePath))
                throw new Exception("");

            Film film = await db.Films.SingleOrDefaultAsync(f => new { f.ID, f.source }, f => f.ID == filmId);
            if (film == null)
                return StateOfUpdate.Failed;

            if (!string.IsNullOrEmpty(film.source))
                return StateOfUpdate.Failed;

            int affected = await db.Films
                .UpdateAsync(new Film { source = filePath }, f => new { f.source }, f => f.ID == film.ID);

            return (affected == 0) ? StateOfUpdate.Failed : StateOfUpdate.Success;
        }

        public async Task<StateOfUpdate> DeleteSourceAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");

            int affected = await db.Films
                .UpdateAsync(new Film { source = null }, f => new { f.source }, f => f.ID == filmId);

            return (affected == 0) ? StateOfUpdate.Failed : StateOfUpdate.Success;
        }

        public StateOfUpdate Upvote(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");

            Film film = db.Films.SingleOrDefault(f => new { f.ID, f.upvote }, f => f.ID == filmId);
            if (film == null)
                return StateOfUpdate.Failed;

            int affected = db.Films
                .Update(new Film { upvote = film.upvote + 1 }, f => new { f.upvote }, f => f.ID == film.ID);

            return (affected == 0) ? StateOfUpdate.Failed : StateOfUpdate.Success;
        }

        public StateOfUpdate Downvote(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");

            Film film = db.Films.SingleOrDefault(f => new { f.ID, f.downvote }, f => f.ID == filmId);
            if (film == null)
                return StateOfUpdate.Failed;

            int affected = db.Films
                .Update(new Film { downvote = film.downvote + 1 }, f => new { f.downvote }, f => f.ID == film.ID);

            return (affected == 0) ? StateOfUpdate.Failed : StateOfUpdate.Success;
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
