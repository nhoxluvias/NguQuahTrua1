﻿using Common;
using Common.Upload;
using Data.BLL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Web.Models;

namespace Web.User
{
    public partial class Search : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        protected List<FilmInfo> filmInfos;
        protected async void Page_Load(object sender, EventArgs e)
        {
            filmBLL = new FilmBLL();
            try
            {
                await GetFilmsBySearchQueryString();
            }catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            filmBLL.Dispose();
        }

        public async Task GetFilmsBySearchQueryString()
        {
            string searchQuery = Request.QueryString["input"];
            if (string.IsNullOrEmpty(searchQuery))
            {
                Response.RedirectToRoute("User_Home", null);
            }
            else
            {
                filmInfos = await filmBLL.SeachAsync(searchQuery);
                foreach(FilmInfo filmInfo in filmInfos)
                {
                    if (string.IsNullOrEmpty(filmInfo.thumbnail))
                        filmInfo.thumbnail = VirtualPathUtility
                            .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.ImageFilePath));
                    else
                        filmInfo.thumbnail = VirtualPathUtility
                            .ToAbsolute(string.Format("{0}/{1}", FileUpload.ImageFilePath, filmInfo.thumbnail));

                    Rating rating = new Rating(filmInfo.upvote, filmInfo.downvote);
                    filmInfo.scoreRating = rating.SolveScore();
                    filmInfo.url = GetRouteUrl("User_FilmDetail", new { slug = filmInfo.name.TextToUrl(), id = filmInfo.ID });
                }
            }
        }
    }
}