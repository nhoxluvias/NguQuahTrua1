using Common.Upload;
using Data.BLL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Web.Models;

namespace Web.Admin.FilmManagement
{
    public partial class DeleteFilm : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        protected FilmInfo filmInfo;
        protected bool enableShowInfo;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                filmBLL = new FilmBLL(DataAccessLevel.Admin);
                enableShowInfo = false;
                enableShowResult = false;
                stateString = null;
                stateDetail = null;
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_FilmList", null);
                if (!IsPostBack)
                {
                    await GetFilmInfo();
                    filmBLL.Dispose();
                }

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

        private async Task GetFilmInfo()
        {
            string id = GetFilmId();
            if (string.IsNullOrEmpty(id))
            {
                Response.RedirectToRoute("Admin_FilmList", null);
            }
            else
            {
                filmInfo = await filmBLL.GetFilmAsync(id);
                if (filmInfo == null)
                {
                    enableShowInfo = false;
                    Response.RedirectToRoute("Admin_FilmList", null);
                }
                else
                {
                    enableShowInfo = true;
                }
            }
        }

        private async Task DeleteFilmInfo()
        {
            string id = GetFilmId();
            FilmInfo filmInfo = await filmBLL.GetFilmAsync(id);
            enableShowResult = true;
            enableShowInfo = false;

            StateOfDeletion state = await filmBLL.DeleteFilmAsync(id);
            if (state == StateOfDeletion.Success)
            {
                FileUpload fileUpload = new FileUpload();
                bool delImage = fileUpload.RemoveImage(filmInfo.thumbnail);
                bool delVideo = fileUpload.RemoveVideo(filmInfo.source);
                if (delImage && delVideo)
                {
                    stateString = "Success";
                    stateDetail = "Đã xóa phim thành công";
                }
                else if (delImage)
                {
                    stateString = "Success";
                    stateDetail = "Đã xóa phim thành công, tuy nhiên tập tin video vẫn chưa được xóa";
                }
                else
                {
                    stateString = "Success";
                    stateDetail = "Đã xóa phim thành công, tuy nhiên tập tin hình ảnh vẫn chưa được xóa";
                }
            }
            else if (state == StateOfDeletion.Failed)
            {
                stateString = "Failed";
                stateDetail = "Xóa phim thất bại";
            }
            else
            {
                stateString = "ConstraintExists";
                stateDetail = "Không thể xóa phim. Lý do: Tồn tại ràng buộc!";
            }
            filmBLL.Dispose();
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                await DeleteFilmInfo();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}