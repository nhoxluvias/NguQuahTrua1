﻿
using MSSQL_Lite.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Model;
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
            CustomValidation
                .Init(cvUsername, "txtUsername", "Không được trống, chỉ chứa a-z, 0-9, _ và -", true, null, CustomValidation.ValidateUsername);
            CustomValidation
                .Init(cvPassword, "txtPassword", "Tối thiểu 6 ký tự, tối đa 20 ký tự", true, null, CustomValidation.ValidatePassword);
        }

        private async Task<bool> LoginToAccount(string username, string password)
        {
            DBContext db = new DBContext();
            Model.User user = await db.Users.SingleOrDefaultAsync(u => u.userName == username && u.password == password);
            return false;
        }
    }
}