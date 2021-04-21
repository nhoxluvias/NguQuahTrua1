using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Validation;

namespace Web.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitHyperLink();
            InitValidation();
        }

        private void InitHyperLink()
        {
            hylnkResetPassword.NavigateUrl = GetRouteUrl("ResetPassword", null);
            hylnkRegister.NavigateUrl = GetRouteUrl("Register", null);
            hylnkFeedback.NavigateUrl = "#";
            hylnkContact.NavigateUrl = "#";
            hylnkTermOfUse.NavigateUrl = "#";
        }

        private void InitValidation()
        {
            cvUsername.ErrorMessage = "Không được trống, chỉ chứa a-z, 0-9, _ và -";
            cvUsername.ControlToValidate = "txtUsername";
            cvUsername.ValidateEmptyText = true;
            cvUsername.ServerValidate += new ServerValidateEventHandler(CheckUsername);

            cvPassword.ErrorMessage = "Tối thiểu 6 ký tự, tối đa 20 ký tự";
            cvPassword.ControlToValidate = "txtPassword";
            cvPassword.ValidateEmptyText = true;
            cvPassword.ServerValidate += new ServerValidateEventHandler(CheckPassword);
        }

        private void CheckUsername(object source, ServerValidateEventArgs args)
        {
            CustomValidation.UsernameValidate(source, args);
        }

        private void CheckPassword(object source, ServerValidateEventArgs args)
        {
            CustomValidation.PasswordValidate(source, args);
        }
    }
}