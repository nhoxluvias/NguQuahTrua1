using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;
using Web.Validation;

namespace Web.Admin.LanguageManagement
{
    public partial class CreateLanguage : System.Web.UI.Page
    {
        private LanguageBLL languageBLL;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            languageBLL = new LanguageBLL(DataAccessLevel.Admin);
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
                cvLanguageName,
                "txtLanguageName",
                "Tên ngôn ngữ không hợp lệ",
                true,
                null,
                customValidation.ValidateCategoryName
            );
        }

        private void ValidateData()
        {
            cvLanguageName.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvLanguageName.IsValid;
        }

        private LanguageCreation GetLanguageCreation()
        {
            return new LanguageCreation
            {
                name = Request.Form[txtLanguageName.UniqueID],
                description = Request.Form[txtLanguageDescription.UniqueID]
            };
        }

        public async Task Create()
        {
            try
            {
                if (IsValidData())
                {
                    LanguageCreation language = GetLanguageCreation();
                    StateOfCreation state = await languageBLL.CreateLanguageAsync(language);
                    if (state == StateOfCreation.Success)
                    {
                        enableShowResult = true;
                        stateString = "Success";
                        stateDetail = "Đã thêm ngôn ngữ thành công";
                    }
                    else if (state == StateOfCreation.AlreadyExists)
                    {
                        enableShowResult = true;
                        stateString = "AlreadyExists";
                        stateDetail = "Thêm ngôn ngữ thất bại. Lý do: Đã tồn tại ngôn ngữ này";
                    }
                    else
                    {
                        enableShowResult = true;
                        stateString = "Failed";
                        stateDetail = "Thêm ngôn ngữ thất bại";
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