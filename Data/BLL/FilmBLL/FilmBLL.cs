using Data.DAL;
using Data.DTO;
using MSSQL_Lite.Connection;
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
        public FilmBLL()
            : base()
        {
            InitDAL();
        }

        public FilmBLL(BusinessLogicLayer bll)
            : base()
        {
            InitDAL(bll.db);
        }

    //    internal FilmInfoForAdmin ToFilmInfoForAdmin(Film film)
    //    {
    //        return new FilmInfoForAdmin
    //        {
    //            ID = film.ID,
    //            name = film.name,
    //            description = film.description,
    //            duration = film.duration,
    //            countryId = film.countryId,
    //            productionCompany = film.productionCompany,
    //            thumbnail = film.thumbnail,
    //            langugeId = film.langugeId,
    //            releaseDate = film.releaseDate,
    //            upvote = film.upvote,
    //            downvote = film.downvote,
    //            view = film.view,
    //            createAt = film.createAt,
    //            updateAt = film.updateAt
    //};
    //    }

        //public async Task<List<FilmInfoForUser>> GetLatestFilmsForUserAsync(int filmNumber = 12)
        //{
        //    string query = @"select top 12 [Film].[ID], [Film].[name], [Film].[thumbnail] 
        //                    from [Film] order by [createAt] desc";
        //    SqlCommand sqlCommand = new SqlCommand();
        //    sqlCommand.CommandType = CommandType.Text;
        //    sqlCommand.CommandText = query;
        //    List<FilmInfoForUser> latestFilms = await db.ExecuteReaderAsync<List<FilmInfoForUser>>(sqlCommand);
        //    if (latestFilms == null)
        //        return null;
        //    foreach (FilmInfoForUser film in latestFilms)
        //    {
        //        film.Categories = await new CategoryBLL.CategoryBLLForUser(this).GetCategoriesByFilmId(film.ID);
        //    }
        //    return latestFilms;
        //}

        //public async Task<List<FilmInfo>> GetFilmsByCategoryId(int categoryId)
        //{
        //    if (categoryId <= 0)
        //        throw new Exception("");
        //    string query = @"Select [Film].* from [Film], [FilmDistribution]
        //                    where [Film].[ID] = [FilmDistribution].[filmId]
        //                        and [FilmDistribution].[categoryId] = @categoryId";
        //    SqlCommand sqlCommand = new SqlCommand();
        //    sqlCommand.CommandType = CommandType.Text;
        //    sqlCommand.CommandText = query;
        //    sqlCommand.Parameters.Add(new SqlParameter("@categoryId", categoryId));
        //    return await db.ExecuteReaderAsync<List<FilmInfo>>(sqlCommand);
        //}

        //public async Task<FilmInfo> GetFilm(string filmId)
        //{
        //    if (string.IsNullOrEmpty(filmId))
        //        throw new Exception("");
        //    Film filmDal = await db.Films.SingleOrDefaultAsync(f => f.ID == filmId);

        //    return new FilmInfo
        //    {
        //        ID = filmDal.ID,
        //        name = filmDal.name,
        //        description = filmDal.description,
        //        duration = filmDal.duration,
        //        releaseDate = filmDal.releaseDate,
        //        productionCompany = filmDal.productionCompany,
        //        Categories = await new CategoryBLL(this).GetCategoriesByFilmId(filmDal.ID),
        //        thumbnail = filmDal.thumbnail,
        //        upvote = filmDal.upvote,
        //        downvote = filmDal.downvote,
        //        view = filmDal.view,
        //        createAt = filmDal.createAt,
        //        updateAt = filmDal.updateAt
        //    };
        //}

        public async Task<long> CountAllAsync()
        {
            return await db.Films.CountAsync();
        }
    }
}
