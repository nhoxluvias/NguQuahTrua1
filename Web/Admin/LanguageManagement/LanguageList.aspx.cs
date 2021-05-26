using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Admin.LanguageManagement
{
    public partial class LanguageList : System.Web.UI.Page
    {
        private LanguageBLL languageBLL;
        private int selectedIndex;
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                languageBLL = new LanguageBLL(DataAccessLevel.Admin);
                hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_CreateLanguage", null);
                selectedIndex = 0;
                enableTool = false;
                toolDetail = null;
                if (!IsPostBack)
                {
                    await SetGrvLanguage(0);
                    selectedIndex = 0;
                    SetDrdlPage();
                }
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private async Task SetGrvLanguage(int pageIndex)
        {
            PagedList<LanguageInfo> languages = await languageBLL
                .GetLanguagesAsync(drdlPage.SelectedIndex, 20);
            grvLanguage.DataSource = languages.Items;
            grvLanguage.DataBind();

            pageNumber = languages.PageNumber;
            currentPage = languages.CurrentPage;
        }

        private void SetDrdlPage()
        {
            drdlPage.Items.Clear();
            for (int i = 0; i < pageNumber; i++)
            {
                string item = (i + 1).ToString();
                if (i == currentPage)
                    drdlPage.Items.Add(string.Format("{0}*", item));
                else
                    drdlPage.Items.Add(item);
            }
            drdlPage.SelectedIndex = selectedIndex;
        }

        protected async void grvLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int key = (int)grvLanguage.DataKeys[grvLanguage.SelectedIndex].Value;
                LanguageInfo languageInfo = await languageBLL.GetLanguageAsync(key);
                enableTool = true;
                toolDetail = string.Format("{0} -- {1}", languageInfo.ID, languageInfo.name);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_LanguageDetail", new { id = languageInfo.ID });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateLanguage", new { id = languageInfo.ID });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteLanguage", new { id = languageInfo.ID });
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        protected async void drdlPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await SetGrvLanguage(drdlPage.SelectedIndex);
                selectedIndex = drdlPage.SelectedIndex;
                SetDrdlPage();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}