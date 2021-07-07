﻿using Common.SystemInformation;
using System;
using System.Threading.Tasks;
using Data.BLL;
using Common.Web;

namespace Web.Admin
{
    public partial class Index : System.Web.UI.Page
    {
        protected SystemInfo systemInfo;
        protected long pageVisitor;
        protected long movieNumber;
        protected long categoryNumber;
        protected long tagNumber;

        protected async void Page_Load(object sender, EventArgs e)
        {
            systemInfo = new SystemInfo();
            pageVisitor = PageVisitor.Views;
            await LoadOverview();
        }

        private async Task LoadOverview()
        {
            FilmBLL filmBLL = new FilmBLL(DataAccessLevel.Admin);
            movieNumber = await filmBLL.CountAllAsync();
            categoryNumber = await new CategoryBLL(filmBLL, DataAccessLevel.Admin).CountAllAsync();
            tagNumber = await new TagBLL(filmBLL, DataAccessLevel.Admin).CountAllAsync();
            filmBLL.Dispose();
        }
    }
}