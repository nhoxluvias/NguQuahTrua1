using Data.BLL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Models;

namespace Web.Admin.FilmManagement
{
    public partial class FilmDetail : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        protected FilmInfo filmInfo;
        protected bool enableShowDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                enableShowDetail = false;
                filmBLL = new FilmBLL(DataAccessLevel.Admin);
                string id = GetFilmId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_FilmList", null);
                hyplnkEdit_Category.NavigateUrl = GetRouteUrl("Admin_EditCategory_Film", new { id = id });
                hyplnkEdit_Image.NavigateUrl = GetRouteUrl("Admin_EditImage_Film", new { id = id });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateFilm", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteFilm", new { id = id });
                await GetFilmInfo(id);
                filmBLL.Dispose();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private string GetFilmId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return null;
            return obj.ToString();
        }

        private async Task GetFilmInfo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Response.RedirectToRoute("Admin_FilmList", null);
            }
            else
            {
                enableShowDetail = true;
                filmInfo = await filmBLL.GetFilmAsync(id);
                if (filmInfo == null)
                {
                    enableShowDetail = false;
                    Response.RedirectToRoute("Admin_FilmList", null);
                }
            }
        }
    }
}