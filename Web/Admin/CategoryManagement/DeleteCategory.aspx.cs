using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;

namespace Web.Admin.CategoryManagement
{
    public partial class DeleteCategory : System.Web.UI.Page
    {
        private CategoryBLL categoryBLL;
        protected CategoryInfo categoryInfo;
        protected bool enableShowInfo;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                categoryBLL = new CategoryBLL(DataAccessLevel.Admin);
                enableShowInfo = false;
                enableShowResult = false;
                stateString = null;
                stateDetail = null;
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_CategoryList", null);
                if (!IsPostBack)
                {
                    await GetCategoryInfo();
                    categoryBLL.Dispose();
                }
                    
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private int GetCategoryId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return int.Parse(obj.ToString());
        }

        private async Task GetCategoryInfo()
        {
            int id = GetCategoryId();
            if(id <= 0)
            {
                Response.RedirectToRoute("Admin_CategoryList", null);
            }
            else
            {
                categoryInfo = await categoryBLL.GetCategoryAsync(id);
                if (categoryInfo == null)
                {
                    enableShowInfo = false;
                    Response.RedirectToRoute("Admin_CategoryList", null);
                }
                else
                {
                    enableShowInfo = true;
                }
            }
        }

        private async Task DeleteCategoryInfo()
        {
            int id = GetCategoryId();
            StateOfDeletion state = await categoryBLL.DeleteCategoryAsync(id);
            categoryBLL.Dispose();
            enableShowResult = true;
            enableShowInfo = false;
            if (state == StateOfDeletion.Success)
            {
                stateString = "Success";
                stateDetail = "Đã xóa thể loại thành công";
            }
            else if (state == StateOfDeletion.Failed)
            {
                stateString = "Failed";
                stateDetail = "Xóa thể loại thất bại";
            }
            else
            {
                stateString = "ConstraintExists";
                stateDetail = "Không thể xóa thể loại. Lý do: Thể loại này đang được sử dụng!";
            }
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                await DeleteCategoryInfo();
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}