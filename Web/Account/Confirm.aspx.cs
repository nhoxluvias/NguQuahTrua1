using Common.Mail;
using Data.BLL;
using Data.DTO;
using MSSQL_Lite.Connection;
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
        private UserBLL userBLL;
        private CustomValidation customValidation;
        protected async void Page_Load(object sender, EventArgs e)
        {
            customValidation = new CustomValidation();
            InitHyperlink();
            InitValidation();
            if(Session["confirmCode"] == null || Session["confirmToken"] == null)
            {
                Response.RedirectToRoute("User_Home");
            }
            else
            {
                userBLL = new UserBLL(DataAccessLevel.User);
                if (IsPostBack)
                    await ConfirmAccount();
                else
                    await ReSendConfirmCode();

                userBLL.Dispose();
            }
        }

        private void InitHyperlink()
        {
            hylnkReConfirm.NavigateUrl = GetRouteUrl("Confirm", new
            {
                userId = GetUserId(),
                confirmToken = GetConfirmToken(),
                type = "re-confirm"
            });
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvConfirmCode, 
                "txtConfirmCode", 
                "Không được để trống, từ 6 đến 20 ký tự số", 
                true, 
                null, 
                customValidation.ValidateConfirmCode
            );
        }

        private void ValidateData()
        {
            cvConfirmCode.Validate();
        }

        public bool IsValidData()
        {
            ValidateData();
            return cvConfirmCode.IsValid;
        }

        private bool IsReConfirm()
        {
            string status = (string)Page.RouteData.Values["type"];
            return (status == "re-confirm");
        }

        private async Task ReSendConfirmCode()
        {
            if (IsReConfirm())
            {
                UserInfo userInfo = await userBLL.GetUserAsync(GetUserId());

                if (userInfo == null)
                    Response.RedirectToRoute("Error");
                else
                    Session["confirmCode"] = new ConfirmCode().Send(userInfo.email);
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
            if (IsValidData() && CheckConfirmCode())
            {
                string userId = GetUserId();
                bool state = await userBLL.ActiveUserAsync(userId);
                Session["confirmCode"] = null;
                Session["confirmToken"] = null;
                if (state)
                    Response.RedirectToRoute("Login");
                else
                    Response.RedirectToRoute("Error");
            }
            else
            {
                Response.RedirectToRoute("Confirm", new { 
                    userId = GetUserId(), 
                    type = "confirm-failed" 
                });
            }
        }
    }
}