using Data.BLL;
using System;

namespace Web.Admin
{
    public partial class CategoryManagement : System.Web.UI.Page
    {
        private CategoryBLL categoryBLL;
        protected async void Page_Load(object sender, EventArgs e)
        {
            categoryBLL = new CategoryBLL(DataAccessLevel.Admin);
            grvCategory.DataSource = await categoryBLL.GetCategoriesAsync();
            grvCategory.DataBind();
            hyplnkCreateCategory.NavigateUrl = GetRouteUrl("Admin_CreateCategory", null);
        }
    }
}