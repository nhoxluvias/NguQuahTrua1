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
    public partial class EditDirector : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        private CustomValidation customValidation;
        protected string filmName;
        protected List<DirectorInfo> directorsByFilmId;
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
                hyplnkEdit_Cast.NavigateUrl = GetRouteUrl("Admin_EditCast_Film", new { id = id });
                hyplnkEdit_Image.NavigateUrl = GetRouteUrl("Admin_EditImage_Film", new { id = id });
                hyplnkEdit_Source.NavigateUrl = GetRouteUrl("Admin_EditSource_Film", new { id = id });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateFilm", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteFilm", new { id = id });
                InitValidation();
                await LoadDirectors();
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
                cvFilmDirector_Role,
                "txtFilmDirector_Role",
                "Tên vai trò của đạo điễn không hợp lệ",
                true,
                null,
                customValidation.ValidateDirectorRole
            );
        }

        private void ValidateData()
        {
            cvFilmDirector_Role.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvFilmDirector_Role.IsValid;
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
                    directorsByFilmId = filmInfo.Directors;
                    filmName = filmInfo.name;
                }
            }
        }

        private async Task LoadDirectors()
        {
            drdlFilmDirector.Items.Clear();
            List<DirectorInfo> directorInfos = await new DirectorBLL(filmBLL, DataAccessLevel.Admin).GetDirectorsAsync();
            foreach (DirectorInfo directorInfo in directorInfos)
            {
                drdlFilmDirector.Items.Add(new ListItem(directorInfo.name, directorInfo.ID.ToString()));
            }
            drdlFilmDirector.SelectedIndex = 0;
        }

        protected async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string filmId = GetFilmId();
                if (IsValidData())
                {
                    string strDirectorId = Request.Form[drdlFilmDirector.UniqueID];
                    string directorRole = Request.Form[txtFilmDirector_Role.UniqueID];
                    if (strDirectorId == null)
                    {
                        Response.RedirectToRoute("Admin_FilmList", null);
                    }
                    else
                    {
                        long directorId = long.Parse(strDirectorId);
                        StateOfCreation state = await filmBLL.AddDirectorAsync(filmId, directorId, directorRole);
                        enableShowResult = true;
                        if (state == StateOfCreation.Success)
                        {
                            stateString = "Success";
                            stateDetail = "Đã thêm đạo diễn vào phim thành công";
                        }
                        else if (state == StateOfCreation.AlreadyExists)
                        {
                            stateString = "AlreadyExists";
                            stateDetail = "Thêm đạo diễn vào phim thất bại. Lý do: Đã tồn tại đạo diễn trong phim này";
                        }
                        else
                        {
                            stateString = "Failed";
                            stateDetail = "Thêm đạo diễn vào phim thất bại";
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
                StateOfDeletion state = await filmBLL.DeleteAllDirectorAsync(filmId);
                await LoadFilmInfo(filmId);
                enableShowResult = true;
                if (state == StateOfDeletion.Success)
                {
                    stateString = "Success";
                    stateDetail = "Đã xóa tất cả đạo diễn của phim thành công";
                }
                else
                {
                    stateString = "Failed";
                    stateDetail = "Xóa tất cả đạo diễn của phim thất bại";
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