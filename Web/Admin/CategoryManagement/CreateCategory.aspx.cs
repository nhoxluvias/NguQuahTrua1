using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;
using Web.Validation;

namespace Web.Admin.CategoryManagement
{
    public partial class CreateCategory : System.Web.UI.Page
    {
        private CategoryBLL categoryBLL;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            categoryBLL = new CategoryBLL(DataAccessLevel.Admin);
            customValidation = new CustomValidation();
            enableShowResult = false;
            stateString = null;
            stateDetail = null;
            InitValidation();
            if (IsPostBack)
            {
                await Create();
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

        private CategoryCreation GetCategoryCreation()
        {
            return new CategoryCreation
            {
                name = Request.Form[txtCategoryName.UniqueID],
                description = Request.Form[txtCategoryDescription.UniqueID]
            };
        }

        public async Task Create()
        {
            try
            {
                if (IsValidData())
                {
                    CategoryCreation category = GetCategoryCreation();
                    StateOfCreation state = await categoryBLL.CreateCategoryAsync(category);
                    if (state == StateOfCreation.Success)
                    {
                        enableShowResult = true;
                        stateString = "Success";
                        stateDetail = "Đã thêm thể loại thành công";
                    }
                    else if (state == StateOfCreation.AlreadyExists)
                    {
                        enableShowResult = true;
                        stateString = "AlreadyExists";
                        stateDetail = "Thêm thể loại thất bại. Lý do: Đã tồn tại thể loại này";
                    }
                    else
                    {
                        enableShowResult = true;
                        stateString = "Failed";
                        stateDetail = "Thêm thể loại thất bại";
                    }
                }
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}