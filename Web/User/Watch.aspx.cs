﻿using Common.Upload;
using Data.BLL;
using Data.DTO;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Web.Models;

namespace Web.User
{
    public partial class Watch : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        protected FilmInfo filmInfo;
        protected string title_HeadTag;
        protected string keywords_MetaTag;
        protected string description_MetaTag;

        protected async void Page_Load(object sender, EventArgs e)
        {
            filmBLL = new FilmBLL();
            try
            {
                await GetFilmInfo();
                GenerateHeadTag();
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
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
                filmBLL.IncludeTag = true;
                filmInfo = await filmBLL.GetFilmAsync(id);
                if (string.IsNullOrEmpty(filmInfo.source))
                    filmInfo.source = VirtualPathUtility
                        .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.VideoFilePath));
                else
                    filmInfo.source = VirtualPathUtility
                        .ToAbsolute(string.Format("{0}/{1}", FileUpload.VideoFilePath, filmInfo.source));
            }
        }

        private void GenerateHeadTag()
        {
            if (filmInfo != null)
            {
                title_HeadTag = filmInfo.name;
                description_MetaTag = (string.Format("{0}...", filmInfo.description.TakeStr(100))).Replace("\n", " ");

                StringBuilder stringBuilder = new StringBuilder();
                foreach (TagInfo tagInfo in filmInfo.Tags)
                {
                    stringBuilder.Append(string.Format("{0}, ", tagInfo.name));
                }
                keywords_MetaTag = stringBuilder.ToString().TrimEnd(' ').TrimEnd(',');
            }
        }
    }
}