﻿using Common.Upload;
using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Web.Models;

namespace Web.Admin.FilmManagement
{
    public partial class EditSource : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        protected string filmName;
        protected string film_Source;
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
                string id = GetFilmId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_FilmList", null);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_FilmDetail", new { id = id });
                hyplnkEdit_Category.NavigateUrl = GetRouteUrl("Admin_EditCategory_Film", new { id = id });
                hyplnkEdit_Tag.NavigateUrl = GetRouteUrl("Admin_EditTag_Film", new { id = id });
                hyplnkEdit_Cast.NavigateUrl = GetRouteUrl("Admin_EditCast_Film", new { id = id });
                hyplnkEdit_Director.NavigateUrl = GetRouteUrl("Admin_EditDirector_Film", new { id = id });
                hyplnkEdit_Image.NavigateUrl = GetRouteUrl("Admin_EditImage_Film", new { id = id });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateFilm", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteFilm", new { id = id });
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
                    if (string.IsNullOrEmpty(filmInfo.source))
                        film_Source = VirtualPathUtility
                        .ToAbsolute(string.Format("{0}/Default/default.mp4", FileUpload.VideoFilePath));
                    else
                        film_Source = VirtualPathUtility
                        .ToAbsolute(string.Format("{0}/{1}", FileUpload.VideoFilePath, filmInfo.source));

                    filmName = filmInfo.name;
                }
            }
        }

        public HttpPostedFile GetPostedFile()
        {
            return Request.Files[fuFilm_Source.UniqueID];
        }

        private FileUpload.UploadState UploadVideo(HttpPostedFile httpPostedFile, ref string filePath)
        {
            FileUpload fileUpload = new FileUpload();
            return fileUpload.UploadVideo(httpPostedFile, ref filePath);
        }

        protected async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                enableShowResult = true;
                HttpPostedFile httpPostedFile = GetPostedFile();
                if (httpPostedFile.ContentLength == 0 || string.IsNullOrEmpty(httpPostedFile.FileName))
                {
                    stateString = "Failed";
                    stateDetail = "Chưa chọn video";
                }
                else
                {
                    string filmId = GetFilmId();
                    FilmInfo film = await filmBLL.GetFilmAsync(filmId);
                    if (!string.IsNullOrEmpty(film.source))
                    {
                        stateString = "Failed";
                        stateDetail = "Phim này đã có video. Cần xóa video đang dùng hiện tại để thêm mới video khác";
                    }
                    else
                    {
                        string filePath = null;
                        FileUpload.UploadState uploadState = UploadVideo(httpPostedFile, ref filePath);
                        if (uploadState == FileUpload.UploadState.Success)
                        {
                            StateOfUpdate state = await filmBLL.AddSourceAsync(filmId, filePath);
                            if (state == StateOfUpdate.Success)
                            {
                                stateString = "Success";
                                stateDetail = "Cập nhật video cho phim thành công";
                            }
                            else
                            {
                                stateString = "Failed";
                                stateDetail = "Cập nhật video cho phim thất bại";
                            }
                        }
                        else if (uploadState == FileUpload.UploadState.Failed_AlreadyExist)
                        {
                            stateString = "AlreadyExists";
                            stateDetail = "Tải lên video cho phim thất bại. Lý do: Video với tên này đã tồn tại trên hệ thống";
                        }
                        else if (uploadState == FileUpload.UploadState.Failed_InvalidFile)
                        {
                            stateString = "Failed";
                            stateDetail = "Tải lên video cho phim thất bại. Lý do: Video không hợp lệ";
                        }
                        else
                        {
                            stateString = "Failed";
                            stateDetail = "Tải lên video cho phim thất bại.";
                        }
                    }
                }
                await LoadFilmInfo(GetFilmId());
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
                FilmInfo film = await filmBLL.GetFilmAsync(filmId);
                enableShowResult = true;
                if (string.IsNullOrEmpty(film.source))
                {
                    stateString = "Failed";
                    stateDetail = "Phim này hiện chưa có video. Video bạn đang thấy là video được gán mặc định";
                }
                else
                {
                    StateOfUpdate state = await filmBLL.DeleteSourceAsync(filmId);
                    if (state == StateOfUpdate.Success)
                    {
                        FileUpload fileUpload = new FileUpload();
                        if (fileUpload.RemoveVideo(film.source))
                        {
                            stateString = "Success";
                            stateDetail = "Đã xóa video của phim thành công";
                        }
                        else
                        {
                            stateString = "Failed";
                            stateDetail = "Xóa tập tin video thất bại";
                        }
                    }
                    else
                    {
                        stateString = "Failed";
                        stateDetail = "Xóa video của phim thất bại";
                    }
                }
                await LoadFilmInfo(GetFilmId());
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