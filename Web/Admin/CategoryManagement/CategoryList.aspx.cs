using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Admin.CategoryManagement
{
    public partial class CategoryList : System.Web.UI.Page
    {
        private CategoryBLL categoryBLL;
        private int selectedIndex;
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                categoryBLL = new CategoryBLL(DataAccessLevel.Admin);
                hyplnkCreateCategory.NavigateUrl = GetRouteUrl("Admin_CreateCategory", null);
                selectedIndex = 0;
                enableTool = false;
                toolDetail = null;
                if (!IsPostBack)
                {
                    await SetGrvCategory(0);
                    selectedIndex = 0;
                    SetDrdlPage();
                }
            }catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        protected async void drdlPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await SetGrvCategory(drdlPage.SelectedIndex);
                selectedIndex = drdlPage.SelectedIndex;
                SetDrdlPage();
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            
        }

        private async Task SetGrvCategory(int pageIndex)
        {
            PagedList<CategoryInfo> categories = await categoryBLL
                .GetCategoriesAsync(drdlPage.SelectedIndex, 20);
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

        protected async void grvCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int key = (int)grvCategory.DataKeys[grvCategory.SelectedIndex].Value;
                CategoryInfo categoryInfo = await categoryBLL.GetCategoryAsync(key);
                enableTool = true;
                toolDetail = string.Format("{0} -- {1}", categoryInfo.ID, categoryInfo.name);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_CategoryDetail", new { id = categoryInfo.ID });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateCategory", new { id = categoryInfo.ID });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteCategory", new { id = categoryInfo.ID });
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}