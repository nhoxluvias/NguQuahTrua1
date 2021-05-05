using Common.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Common;
using Web.Models;
using Web.Validation;

namespace Web.Account
{
    public partial class Confirm : System.Web.UI.Page
    {
        private DBContext db;
        protected async void Page_Load(object sender, EventArgs e)
        {
            db = new DBContext();
            InitHyperlink();
            InitValidation();
            if (IsPostBack)
            {
                await ConfirmAccount();
            }
            else
            {
                await ReSendConfirmCode();
            }
        }

        private void InitHyperlink()
        {
            hylnkReConfirm.NavigateUrl = GetRouteUrl("Confirm", new
            {
                userId = GetUserId(),
                confirmToken = GetConfirmToken(),
                status = "re-confirm"
            });
        }

        private void InitValidation()
        {
            CustomValidation
                .Init(cvConfirmCode, "txtConfirmCode", "Không được để trống, từ 6 đến 20 ký tự số", true, null, CustomValidation.ValidateConfirmCode);
        }

        private bool IsReConfirm()
        {
            string status = (string)Page.RouteData.Values["status"];
            return (status == "re-confirm");
        }

        private async Task ReSendConfirmCode()
        {
            if (IsReConfirm())
            {
                Models.User user = await db.Users
                    .SingleOrDefaultAsync(u => new { u.email }, u => u.ID == GetUserId());
                if (user == null)
                {
                    Response.RedirectToRoute("Error");
                }
                else
                {
                    Session["confirmCode"] = new ConfirmCode().Send(user.email);
                }
            }
        }

        private bool CheckConfirmCode()
        {
            string confirmCode = Request.Form["txtConfirmCode"];
            string confirmToken = GetConfirmToken();
            return (confirmCode == Session["confirmCode"] as string) 
                && (confirmToken == Session["confirmToken"] as string);
        }

        private string GetUserId()
        {
            return (string)Page.RouteData.Values["userId"];
        }

        private string GetConfirmToken()
        {
            return (string)Page.RouteData.Values["confirmToken"];
        }

        private async Task ConfirmAccount()
        {
            cvConfirmCode.Validate();
            if (cvConfirmCode.IsValid && CheckConfirmCode())
            {
                string userId = GetUserId();
                int affected = await db.Users.UpdateAsync(new Models.User { active = true }, u => new { u.active });
                if (affected == 0) {
                    Response.RedirectToRoute("Error");
                }
                else
                {
                    Response.RedirectToRoute("Login");
                }
            }
            else
            {
                Response.RedirectToRoute("Confirm", new { 
                    userId = GetUserId(), 
                    status = "confirm-failed" 
                });
            }
        }
    }
}