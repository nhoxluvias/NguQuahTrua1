using Common.Upload;
using Data.BLL;
using Data.DTO;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Web.Models;

namespace Web.Admin.FilmManagement
{
    public partial class FilmList : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                filmBLL = new FilmBLL(DataAccessLevel.Admin);
                hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_CreateFilm", null);
                enableTool = false;
                toolDetail = null;
                if (!IsPostBack)
                {
                    await SetGrvFilm();
                    SetDrdlPage();
                }
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        protected async void drdlPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await SetGrvFilm();
                SetDrdlPage();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }

        }

        private async Task SetGrvFilm()
        {
            PagedList<FilmInfo> films = await filmBLL
                .GetFilmsAsync(drdlPage.SelectedIndex, 20);
            filmBLL.Dispose();
            foreach(FilmInfo film in films.Items)
            {
                if(string.IsNullOrEmpty(film.thumbnail))
                    film.thumbnail = VirtualPathUtility
                        .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.ImageFilePath));
                else
                    film.thumbnail = VirtualPathUtility
                        .ToAbsolute(string.Format("{0}/{1}", FileUpload.ImageFilePath, film.thumbnail));
            }
            grvFilm.DataSource = films.Items;
            grvFilm.DataBind();

            pageNumber = films.PageNumber;
            currentPage = films.CurrentPage;
        }

        private void SetDrdlPage()
        {
            int selectedIndex = drdlPage.SelectedIndex;
            drdlPage.Items.Clear();
            for (int i = 0; i < pageNumber; i++)
            {
                string item = (i + 1).ToString();
                if (i == currentPage)
                    drdlPage.Items.Add(string.Format("{0}*", item));
                else
                    drdlPage.Items.Add(item);
            }
            drdlPage.SelectedIndex = selectedIndex;
        }

        protected async void grvFilm_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string key = (string)grvFilm.DataKeys[grvFilm.SelectedIndex].Value;
                FilmInfo filmInfo = await filmBLL.GetFilmAsync(key);
                filmBLL.Dispose();
                enableTool = true;
                toolDetail = string.Format("{0} -- {1}", filmInfo.ID, filmInfo.name);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_FilmDetail", new { id = filmInfo.ID });
                hyplnkEdit_Category.NavigateUrl = GetRouteUrl("Admin_EditCategory_Film", new { id = filmInfo.ID });
                hyplnkEdit_Tag.NavigateUrl = GetRouteUrl("Admin_EditTag_Film", new { id = filmInfo.ID });
                hyplnkEdit_Director.NavigateUrl = GetRouteUrl("Admin_EditDirector_Film", new { id = filmInfo.ID });
                hyplnkEdit_Cast.NavigateUrl = GetRouteUrl("Admin_EditCast_Film", new { id = filmInfo.ID });
                hyplnkEdit_Image.NavigateUrl = GetRouteUrl("Admin_EditImage_Film", new { id = filmInfo.ID });
                hyplnkEdit_Source.NavigateUrl = GetRouteUrl("Admin_EditSource_Film", new { id = filmInfo.ID });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateFilm", new { id = filmInfo.ID });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteFilm", new { id = filmInfo.ID });
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}