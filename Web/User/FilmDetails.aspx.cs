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
using Web.Models.DTO;

namespace Web.User
{
    public partial class FilmDetails : System.Web.UI.Page
    {
        private DBContext db;
        protected FilmInfo film;
        protected async void Page_Load(object sender, EventArgs e)
        {
            db = new DBContext(ConnectionType.ManuallyDisconnect);
            await GetFilmById();
        }

        private string GetFilmId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return null;
            return (string)obj;
        }

        private async Task GetFilmById()
        {
            string id = GetFilmId();
            string query = "Select * from [Film] where [Film].[ID] = @filmId";
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = query;
            sqlCommand.Parameters.Add(new SqlParameter("@filmId", id));
            film = await db.ExecuteReaderAsync<FilmInfo>(sqlCommand);
            film.thumbnail = VirtualPathUtility.ToAbsolute("~/images/") + film.thumbnail;
        }
    }
}