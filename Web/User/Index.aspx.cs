using MSSQL_Lite.Connection;
using MSSQL_Lite.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Models;
using Web.Models.DTO;

namespace Web.User
{
    public partial class Index : System.Web.UI.Page
    {
        private DBContext db;
        protected List<List<Dictionary<string, object>>> filmsByCategory;
        protected List<FilmInfo> latestFilms;

        protected async void Page_Load(object sender, EventArgs e)
        {
            db = new DBContext(ConnectionType.ManuallyDisconnect);
            await GetLatestFilm();
        }

        private async Task GetLatestFilm()
        {
            string query = @"select top 12 [Film].[ID], [Film].[name], [Film].[thumbnail] 
                            from [Film] order by [createAt] desc";
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = query;
            latestFilms = (await db.ExecuteReaderAsync<List<FilmInfo>>(sqlCommand))
                .Select(f => new FilmInfo
                {
                    ID = f.ID,
                    name = f.name,
                    thumbnail = VirtualPathUtility.ToAbsolute("~/images/") + f.thumbnail,
                    url = GetRouteUrl("User_FilmDetails", new { slug = f.name.TextToUrl(), id = f.ID })
                }).ToList();
            foreach(FilmInfo film in latestFilms)
            {
                string subquery = @"Select [Category].[name] from [CategoryDistribution], [Category]
                                where [CategoryDistribution].[categoryID] = [Category].[ID]
                                    and [CategoryDistribution].[filmId] = @filmId";
                SqlCommand sqlCommandOfSubQuery = new SqlCommand();
                sqlCommandOfSubQuery.CommandType = CommandType.Text;
                sqlCommandOfSubQuery.CommandText = subquery;
                sqlCommandOfSubQuery.Parameters.Add(new SqlParameter("@filmId", film.ID));
                List<Models.Category> categories = await db.ExecuteReaderAsync<List<Models.Category>>(sqlCommandOfSubQuery);
                film.Categories = categories;
            }
        }

        //public async Task<List<List<Dictionary<string, object>>>> GetFilmsByCategory()
        //{
        //    List<Models.Category> categories = await db.Categories.ToListAsync();

            
        //}

        //private async Task<List<Dictionary<string, object>>> GetFilmByCategory(string categoryId)
        //{
        //    Models.Category category = await db.Categories
        //        .SingleOrDefaultAsync(c => new { c.ID }, c => c.name == categoryName);
        //    string query = "Select * from [Film] where categoryId = @categoryId";
        //    SqlCommand sqlCommand = new SqlCommand();
        //    sqlCommand.CommandType = CommandType.Text;
        //    sqlCommand.CommandText = query;
        //    sqlCommand.Parameters.Add(new SqlParameter("@categoryId", category.ID));
        //    DataSet dataSet = (DataSet)await db.ExecuteReaderAsync(sqlCommand);
        //    return new SqlConvert().ToDictionaryList(dataSet);
        //}

        //private async Task GetActionFilm()
        //{
        //    actionFilms = await GetFilmByCategory("Phim hành động");
        //}



    }
}