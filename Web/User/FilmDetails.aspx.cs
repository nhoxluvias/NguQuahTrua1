using Data.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Web.Models;
using Data.DTO;
using Common.Upload;

namespace Web.User
{
    public partial class FilmDetails : System.Web.UI.Page
    {
     
        private FilmBLL filmBLL;
        protected FilmInfo filmInfo;
        protected async void Page_Load(object sender, EventArgs e)
        {
           
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
                filmInfo = await filmBLL.GetFilmAsync(id);            
                if (string.IsNullOrEmpty(filmInfo.thumbnail))
                    filmInfo.thumbnail = VirtualPathUtility
                        .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.ImageFilePath));
                else
                    filmInfo.thumbnail = VirtualPathUtility
                        .ToAbsolute(string.Format("{0}/{1}", FileUpload.ImageFilePath, filmInfo.thumbnail));
            }
        }
    }
}