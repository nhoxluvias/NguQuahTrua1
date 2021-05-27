using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;
using Web.Validation;

namespace Web.Admin.CategoryManagement
{
    public partial class UpdateCategory : System.Web.UI.Page
    {
        private CategoryBLL categoryBLL;
        protected CategoryInfo categoryInfo;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected bool enableShowForm;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                categoryBLL = new CategoryBLL(DataAccessLevel.Admin);
                customValidation = new CustomValidation();
                enableShowResult = false;
                enableShowForm = false;
                stateString = null;
                stateDetail = null;
                int id = GetCategoryId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_CategoryList", null);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_CategoryDetail", null);
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteCategory", null);
                InitValidation();
                if (IsPostBack)
                {
                    if (IsValidData())
                    {
                        await Update();
                        await LoadCategoryInfo(id);
                    }
                }
                else
                {
                    await LoadCategoryInfo(id);
                }
                categoryBLL.Dispose();
            }
            catch (Exception ex)
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

        private async Task LoadCategoryInfo(int id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_CategoryList", null);
            }
            else
            {
                CategoryInfo categoryInfo = await categoryBLL.GetCategoryAsync(id);
                if (categoryInfo == null)
                {
                    Response.RedirectToRoute("Admin_CategoryList", null);
                }
                else
                {
                    hdCategoryId.Value = categoryInfo.ID.ToString();
                    txtCategoryName.Text = categoryInfo.name;
                    txtCategoryDescription.Text = categoryInfo.description;
                }
            }
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvCategoryName,
                "txtCategoryName",
                "Tên thể loại không hợp lệ",
                true,
                null,
                customValidation.ValidateCategoryName
            );
        }

        private void ValidateData()
        {
            cvCategoryName.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvCategoryName.IsValid;
        }

        private CategoryUpdate GetCategoryUpdate()
        {
            return new CategoryUpdate
            {
                ID = int.Parse(Request.Form[hdCategoryId.UniqueID]),
                name = Request.Form[txtCategoryName.UniqueID],
                description = Request.Form[txtCategoryDescription.UniqueID]
            };
        }

        private async Task Update()
        {
            CategoryUpdate categoryUpdate = GetCategoryUpdate();
            StateOfUpdate state = await categoryBLL.UpdateCategoryAsync(categoryUpdate);
            enableShowResult = true;
            if (state == StateOfUpdate.Success)
            {
                stateString = "Success";
                stateDetail = "Đã cập nhật thể loại thành công";
            }
            else
            {
                stateString = "Failed";
                stateDetail = "Cập nhật thể loại thất bại";
            }
        }
    }
}