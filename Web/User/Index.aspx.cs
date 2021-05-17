using Data.BLL;
using MSSQL_Lite.Connection;
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
using Data.DTO;

namespace Web.User
{
    public partial class Index : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        protected List<FilmInfo> latestFilms;

        protected async void Page_Load(object sender, EventArgs e)
        {
            filmBLL = new FilmBLL(DataAccessLevel.User);
            await GetLatestFilm();
        }

        private async Task GetLatestFilm()
        {
            latestFilms = (await filmBLL.GetLatestFilmAsync())
                .Select(f => new FilmInfo
                {
                    ID = f.ID,
                    name = f.name,
                    thumbnail = VirtualPathUtility.ToAbsolute("~/images/") + f.thumbnail,
                    url = GetRouteUrl("User_FilmDetails", new { slug = f.name.TextToUrl(), id = f.ID }),
                    Categories = f.Categories
                }).ToList();
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