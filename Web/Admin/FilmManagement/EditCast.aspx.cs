using Data.BLL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Models;
using Web.Validation;

namespace Web.Admin.FilmManagement
{
    public partial class EditCast : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        private CustomValidation customValidation;
        protected string filmName;
        protected List<CastInfo> castsByFilmId;
        protected bool enableShowDetail;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                enableShowDetail = false;
                filmBLL = new FilmBLL(DataAccessLevel.Admin);
                customValidation = new CustomValidation();
                string id = GetFilmId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_FilmList", null);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_FilmDetail", new { id = id });
                hyplnkEdit_Category.NavigateUrl = GetRouteUrl("Admin_EditCategory_Film", new { id = id });
                hyplnkEdit_Tag.NavigateUrl = GetRouteUrl("Admin_EditTag_Film", new { id = id });
                hyplnkEdit_Director.NavigateUrl = GetRouteUrl("Admin_EditDirector_Film", new { id = id });
                hyplnkEdit_Image.NavigateUrl = GetRouteUrl("Admin_EditImage_Film", new { id = id });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateFilm", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteFilm", new { id = id });
                InitValidation();
                await LoadCasts();
                if (!IsPostBack)
                {
                    await LoadFilmInfo(id);
                    filmBLL.Dispose();
                }
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvFilmCast_Role,
                "txtFilmCast_Role",
                "Tên vai trò của đạo điễn không hợp lệ",
                true,
                null,
                customValidation.ValidateCastRole
            );
        }

        private void ValidateData()
        {
            cvFilmCast_Role.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvFilmCast_Role.IsValid;
        }

        private string GetFilmId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return null;
            return obj.ToString();
        }

        private async Task LoadFilmInfo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Response.RedirectToRoute("Admin_FilmList", null);
            }
            else
            {
                FilmInfo filmInfo = await filmBLL.GetFilmAsync(id);
                if (filmInfo == null)
                {
                    Response.RedirectToRoute("Admin_FilmList", null);
                }
                else
                {
                    enableShowDetail = true;
                    castsByFilmId = filmInfo.Casts;
                    filmName = filmInfo.name;
                }
            }
        }

        private async Task LoadCasts()
        {
            drdlFilmCast.Items.Clear();
            List<CastInfo> castInfos = await new CastBLL(filmBLL, DataAccessLevel.Admin).GetCastsAsync();
            foreach (CastInfo castInfo in castInfos)
            {
                drdlFilmCast.Items.Add(new ListItem(castInfo.name, castInfo.ID.ToString()));
            }
            drdlFilmCast.SelectedIndex = 0;
        }

        protected async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string filmId = GetFilmId();
                if (IsValidData())
                {
                    string strCastId = Request.Form[drdlFilmCast.UniqueID];
                    string castRole = Request.Form[txtFilmCast_Role.UniqueID];
                    if (strCastId == null)
                    {
                        Response.RedirectToRoute("Admin_FilmList", null);
                    }
                    else
                    {
                        long castId = long.Parse(strCastId);
                        StateOfCreation state = await filmBLL.AddCastAsync(filmId, castId, castRole);
                        enableShowResult = true;
                        if (state == StateOfCreation.Success)
                        {
                            stateString = "Success";
                            stateDetail = "Đã thêm diễn viên vào phim thành công";
                        }
                        else if (state == StateOfCreation.AlreadyExists)
                        {
                            stateString = "AlreadyExists";
                            stateDetail = "Thêm diễn viên vào phim thất bại. Lý do: Đã tồn tại diễn viên trong phim này";
                        }
                        else
                        {
                            stateString = "Failed";
                            stateDetail = "Thêm diễn viên vào phim thất bại";
                        }
                    }
                }
                await LoadFilmInfo(filmId);
                filmBLL.Dispose();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string filmId = GetFilmId();
                StateOfDeletion state = await filmBLL.DeleteAllCastAsync(filmId);
                await LoadFilmInfo(filmId);
                enableShowResult = true;
                if (state == StateOfDeletion.Success)
                {
                    stateString = "Success";
                    stateDetail = "Đã xóa tất cả diễn viên của phim thành công";
                }
                else
                {
                    stateString = "Failed";
                    stateDetail = "Xóa tất cả diễn viên của phim thất bại";
                }
                filmBLL.Dispose();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}