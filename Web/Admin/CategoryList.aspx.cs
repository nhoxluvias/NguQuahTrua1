using Data.BLL;
using MSSQL_Lite.Connection;
using System;
using System.Data;
using System.Data.SqlClient;
using Web.Models;

namespace Web.Admin
{
    public partial class CategoryManagement : System.Web.UI.Page
    {
        private CategoryBLL categoryBLL;
        protected async void Page_Load(object sender, EventArgs e)
        {
            categoryBLL = new CategoryBLL(DataAccessLevel.Admin);
            //grvCategory.DataSource = await categoryBLL.GetCategoriesAsync();
            //grvCategory.DataBind();

            grvCategory.DataSource = new DBContext(ConnectionType.ManuallyDisconnect).Categories.ToList(0, 2);
            grvCategory.DataBind();

            hyplnkCreateCategory.NavigateUrl = GetRouteUrl("Admin_CreateCategory", null);
        }
    }
}