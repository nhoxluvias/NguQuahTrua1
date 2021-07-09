using Common.Upload;
using Data.BLL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace Web.User
{
    public partial class Watch : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        protected FilmInfo filmInfo;

        protected async void Page_Load(object sender, EventArgs e)
        {
            filmBLL = new FilmBLL(DataAccessLevel.User);
            await GetFilmInfo();
            filmBLL.Dispose();
        }

        private string GetFilmId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return null;
            return obj.ToString();
        }

        private async Task GetFilmInfo()
        {
            string id = GetFilmId();
            if (id == null)
            {
                Response.RedirectToRoute("User_Home", null);
            }
            else
            {
                filmInfo = await filmBLL.GetFilmAsync(id);
                if (string.IsNullOrEmpty(filmInfo.source))
                    filmInfo.source = VirtualPathUtility
                        .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.VideoFilePath));
                else
                    filmInfo.source = VirtualPathUtility
                        .ToAbsolute(string.Format("{0}/{1}", FileUpload.VideoFilePath, filmInfo.source));
            }
        }
    }
}