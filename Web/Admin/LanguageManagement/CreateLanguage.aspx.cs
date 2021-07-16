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
            languageBLL = new LanguageBLL();
            customValidation = new CustomValidation();
            enableShowResult = false;
            stateString = null;
            stateDetail = null;
            try
            {
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_LanguageList", null);
                InitValidation();

                if (CheckLoggedIn())
                {
                    if (IsPostBack)
                    {
                        await Create();
                    }
                }
                else
                {
                    Response.RedirectToRoute("Account_Login", null);
                }
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            languageBLL.Dispose();
        }

        private bool CheckLoggedIn()
        {
            object obj = Session["userSession"];
            if (obj == null)
                return false;

            UserSession userSession = (UserSession)obj;
            return (userSession.role == "Admin" || userSession.role == "Editor");
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
            if (IsValidData())
            {
                LanguageCreation language = GetLanguageCreation();
                StateOfCreation state = await languageBLL.CreateLanguageAsync(language);
                if (state == StateOfCreation.Success)
                {
                    stateString = "Success";
                    stateDetail = "Đã thêm ngôn ngữ thành công";
                }
                else if (state == StateOfCreation.AlreadyExists)
                {
                    stateString = "AlreadyExists";
                    stateDetail = "Thêm ngôn ngữ thất bại. Lý do: Đã tồn tại ngôn ngữ này";
                }
                else
                {
                    stateString = "Failed";
                    stateDetail = "Thêm ngôn ngữ thất bại";
                }
                enableShowResult = true;
            }
        }
    }
}