using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
            InitValidation();
            if (IsPostBack)
            {
                await ConfirmAccount();
            }
        }

        private void InitValidation()
        {
            CustomValidation
                .Init(cvConfirmCode, "txtConfirmCode", "Không được để trống, từ 6 đến 20 ký tự số", true, null, CustomValidation.ValidateConfirmCode);
        }

        private bool CheckConfirmCode()
        {
            string confirmCode = Request.Form["txtConfirmCode"];
            return (confirmCode == Session["confirmCode"] as string);
        }

        private async Task ConfirmAccount()
        {
            cvConfirmCode.Validate();
            if (cvConfirmCode.IsValid && CheckConfirmCode())
            {
                string userId = (string)Page.RouteData.Values["userId"];
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
                    userId = (string)Page.RouteData.Values["userId"], 
                    status = "confirm-failed" 
                });
            }
        }
    }
}