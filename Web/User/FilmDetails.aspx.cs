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
    public partial class FilmDetails : System.Web.UI.Page
    {
        private DBContext db;
        private FilmBLL filmBLL;
        protected FilmInfo film;
        protected async void Page_Load(object sender, EventArgs e)
        {
            db = new DBContext(ConnectionType.ManuallyDisconnect);
            filmBLL = new FilmBLL(DataAccessLevel.User);
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
            if (id == null)
            {

            }
            else
            {
                film = await filmBLL.GetFilmAsync(id);
                film.thumbnail = VirtualPathUtility.ToAbsolute("~/images/") + film.thumbnail;
            }
        }
    }
}