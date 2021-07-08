﻿using Data.BLL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Models;

namespace Web.Admin.FilmManagement
{
    public partial class EditCategory : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        protected string filmName;
        protected List<CategoryInfo> categoriesByFilmId;
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
                hyplnkEdit_Tag.NavigateUrl = GetRouteUrl("Admin_EditTag_Film", new { id = id });
                hyplnkEdit_Image.NavigateUrl = GetRouteUrl("Admin_EditImage_Film", new { id = id });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateFilm", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteFilm", new { id = id });
                await LoadCategories();
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
                if(filmInfo == null)
                {
                    Response.RedirectToRoute("Admin_FilmList", null);
                }
                else
                {
                    enableShowDetail = true;
                    categoriesByFilmId = filmInfo.Categories;
                    filmName = filmInfo.name;
                }
            }
        }

        private async Task LoadCategories()
        {
            drdlFilmCategory.Items.Clear();
            List<CategoryInfo> categoryInfos = await new CategoryBLL(filmBLL, DataAccessLevel.Admin).GetCategoriesAsync();
            foreach (CategoryInfo categoryInfo in categoryInfos)
            {
                drdlFilmCategory.Items.Add(new ListItem(categoryInfo.name, categoryInfo.ID.ToString()));
            }
            drdlFilmCategory.SelectedIndex = 0;
        }

        protected async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string filmId = GetFilmId();
                string strCategoryId = Request.Form[drdlFilmCategory.UniqueID];
                if (strCategoryId == null)
                {
                    Response.RedirectToRoute("Admin_FilmList", null);
                }
                else
                {
                    int categoryId = int.Parse(strCategoryId);
                    StateOfCreation state = await filmBLL.AddCategoryAsync(filmId, categoryId);
                    await LoadFilmInfo(filmId);
                    enableShowResult = true;
                    if (state == StateOfCreation.Success)
                    {
                        stateString = "Success";
                        stateDetail = "Đã thêm thể loại vào phim thành công";
                    }
                    else if (state == StateOfCreation.AlreadyExists)
                    {
                        stateString = "AlreadyExists";
                        stateDetail = "Thêm thể loại vào phim thất bại. Lý do: Đã tồn tại thể loại trong phim này";
                    }
                    else
                    {
                        stateString = "Failed";
                        stateDetail = "Thêm thể loại vào phim thất bại";
                    }
                }
                filmBLL.Dispose();
            }
            catch(Exception ex)
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
                StateOfDeletion state = await filmBLL.DeleteAllCategoryAsync(filmId);
                await LoadFilmInfo(filmId);
                enableShowResult = true;
                if (state == StateOfDeletion.Success)
                {
                    stateString = "Success";
                    stateDetail = "Đã xóa tất cả thể loại của phim thành công";
                }
                else
                {
                    stateString = "Failed";
                    stateDetail = "Xóa tất cả thể loại của phim thất bại";
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