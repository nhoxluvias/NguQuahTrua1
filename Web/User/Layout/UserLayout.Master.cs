﻿using Data.BLL;
using System;
using System.Collections.Generic;
using Data.DTO;
using System.Linq;
using Web.Models;

namespace Web.User.Layout
{
    public partial class UserLayout : System.Web.UI.MasterPage
    {
        protected List<CategoryInfo> categories;
        protected string hyplnkSearch;
        protected string hyplnkWatchedList;
        protected string hyplnkHome;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                object obj = Session["userSession"];
                if (obj == null)
                {
                    hyplnkAccount.HRef = GetRouteUrl("Account_Login", null);
                    hyplnkAccount.InnerText = "Đăng nhập / Đăng ký";
                }
                else
                {
                    hyplnkAccount.HRef = GetRouteUrl("Account_Logout", null);
                    hyplnkAccount.InnerText = "Đăng xuất";
                }
                hyplnkSearch = GetRouteUrl("User_Search", null);
                hyplnkWatchedList = GetRouteUrl("User_WatchedList", null);
                hyplnkHome = GetRouteUrl("User_Home", null);
                GetCategories();
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private void GetCategories()
        {
            using(CategoryBLL categoryBLL = new CategoryBLL())
            {
                categories = categoryBLL.GetCategories()
                .Select(c => new CategoryInfo
                {
                    ID = c.ID,
                    name = c.name,
                    description = c.description,
                    url = GetRouteUrl("User_FilmsByCategory", new { slug = c.name.TextToUrl(), id = c.ID })
                }).ToList();
            }
        }
    }
}