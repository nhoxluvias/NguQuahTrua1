using Data.BLL;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;
using Web.Validation;

namespace Web.Account
{
    public partial class NewPassword : System.Web.UI.Page
    {
        private UserBLL userBLL;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                customValidation = new CustomValidation();
                InitValidation();
                enableShowResult = true;
                stateDetail = null;
                if (Session["newPasswordToken"] == null)
                {
                    Response.RedirectToRoute("User_Home");
                }
                else if (!IsValidNewPasswordToken())
                {
                    Response.RedirectToRoute("User_Home");
                }
                else
                {
                    if (IsPostBack)
                    {
                        userBLL = new UserBLL(DataAccessLevel.User);
                        await CreateNewPassword();
                        userBLL.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvPassword,
                "txtNewPassword",
                "Tối thiểu 6 ký tự, tối đa 20 ký tự",
                true,
                null,
                customValidation.ValidatePassword
            );
        }

        private void ValidateData()
        {
            cvPassword.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvPassword.IsValid;
        }

        private string GetUserId()
        {
            return (string)Page.RouteData.Values["userId"];
        }

        private string GetNewPassword()
        {
            return Request.Form["txtNewPassword"];
        }

        private string GetNewPasswordToken()
        {
            return (string)Page.RouteData.Values["newPasswordToken"];
        }

        private bool IsValidNewPasswordToken()
        {
            return GetNewPasswordToken() == Session["newPasswordToken"] as string;
        }

        private async Task CreateNewPassword()
        {
            if (IsValidData())
            {
                string userId = GetUserId();
                string password = GetNewPassword();
                UserBLL.CreateNewPasswordState createNewPasswordState = await userBLL.CreateNewPasswordAsync(userId, password);
                Session["newPasswordToken"] = null;
                if (createNewPasswordState == UserBLL.CreateNewPasswordState.Success)
                    Response.RedirectToRoute("Account_Login");
                else if (createNewPasswordState == UserBLL.CreateNewPasswordState.NotExists)
                    Response.RedirectToRoute("User_Home");
                else
                    Response.RedirectToRoute("Notification_Error");
            }
        }
    }
}