
using Common.Hash;
using MSSQL_Lite.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Models;
using Web.Validation;

namespace Web.Account
{
    public partial class Login : System.Web.UI.Page
    {
        private DBContext db;

        protected async void Page_Load(object sender, EventArgs e)
        {
            db = new DBContext();
            InitHyperLink();
            ShowLoginStatus();
            InitValidation();
            if (IsPostBack)
            {
                await LoginToAccount();
            }
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

        private void ShowLoginStatus()
        {
            string loginStatus = (string)Page.RouteData.Values["loginStatus"];
            if (string.IsNullOrEmpty(loginStatus))
                txtLoginStatus.InnerText = "Hãy đăng nhập ngay!";
            else if(loginStatus == "success")
            {
                txtLoginStatus.InnerText = "Đăng nhập thành công, chờ 3 giây!";
            }else if(loginStatus == "failed")
            {
                txtLoginStatus.InnerText = "Kiểm tra lại thông tin đăng nhập!";
            }else if(loginStatus == "not-exists")
            {
                txtLoginStatus.InnerText = "Không tồn tại người dùng này!";
            }
            else
            {
                txtLoginStatus.InnerText = "Hãy đăng nhập ngay!";
            }
        }

        private async Task LoginToAccount()
        {
            cvUsername.Validate();
            cvPassword.Validate();
            if(cvUsername.IsValid && cvPassword.IsValid)
            {
                string username = Request.Form["txtUsername"];
                string password = Request.Form["txtPassword"];

                Models.User user = await db.Users
                    .SingleOrDefaultAsync(u => new { u.password, u.salt }, u => u.userName == username);
                if (user == null)
                {
                    Response.RedirectToRoute("Login_WithParam", routeValues: new { loginStatus = "not-exists" });
                }
                else
                {
                    string passwordHashed = PBKDF2_Hash.Hash(password, user.salt, 10);
                    if (user.password == passwordHashed)
                    {
                        Session["username"] = username;
                        Response.RedirectToRoute("Login_WithParam", routeValues: new { loginStatus = "success" });
                    }
                    else
                    {
                        Response.RedirectToRoute("Login_WithParam", routeValues: new { loginStatus = "failed" });
                    }
                }
            }
        }
    }
}