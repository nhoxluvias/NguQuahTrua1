using Data.BLL;
using Data.DTO;
using MSSQL_Lite.Access;
using MSSQL_Lite.Connection;
using MSSQL_Lite.Query;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Web.Admin
{
    public partial class CategoryManagement : System.Web.UI.Page
    {
        private CategoryBLL categoryBLL;
        private int selectedIndex;
        protected int currentPage;
        protected int pageNumber;
        protected async void Page_Load(object sender, EventArgs e)
        {
            categoryBLL = new CategoryBLL(DataAccessLevel.Admin);
            hyplnkCreateCategory.NavigateUrl = GetRouteUrl("Admin_CreateCategory", null);
            selectedIndex = 0;
            if (!IsPostBack)
            {
                SetGrvCategory(0);
                selectedIndex = 0;
                SetDrdlPage();
            }
        }

        protected void drdlPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetGrvCategory(drdlPage.SelectedIndex);
            selectedIndex = drdlPage.SelectedIndex;
            SetDrdlPage();
        }

        private void SetGrvCategory(int pageIndex)
        {
            PagedList<CategoryInfo> categories = categoryBLL.GetCategories(drdlPage.SelectedIndex, 2);
            grvCategory.DataSource = categories.Items;
            grvCategory.DataBind();

            pageNumber = categories.PageNumber;
            currentPage = categories.CurrentPage;
        }

        private void SetDrdlPage()
        {
            drdlPage.Items.Clear();
            for (int i = 0; i < pageNumber; i++)
            {
                string item = (i + 1).ToString();
                if (i == currentPage)
                    drdlPage.Items.Add(string.Format("{0}*", item));
                else
                    drdlPage.Items.Add(item);
            }
            drdlPage.SelectedIndex = selectedIndex;
        }
    }
}