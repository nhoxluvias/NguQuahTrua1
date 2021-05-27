using Data.BLL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Models;

namespace Web.Admin.LanguageManagement
{
    public partial class LanguageDetail : System.Web.UI.Page
    {
        private LanguageBLL languageBLL;
        protected LanguageInfo languageInfo;
        protected bool enableShowDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                enableShowDetail = false;
                languageBLL = new LanguageBLL(DataAccessLevel.Admin);
                int id = GetLanguageId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_LanguageList", null);
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateLanguage", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteLanguage", new { id = id });
                await GetLanguageInfo(id);
                languageBLL.Dispose();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private int GetLanguageId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return int.Parse(obj.ToString());
        }

        private async Task GetLanguageInfo(int id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_LanguageList", null);
            }
            else
            {
                enableShowDetail = true;
                languageInfo = await languageBLL.GetLanguageAsync(id);
                if (languageInfo == null)
                {
                    enableShowDetail = false;
                    Response.RedirectToRoute("Admin_LanguageList", null);
                }
            }
        }
    }
}