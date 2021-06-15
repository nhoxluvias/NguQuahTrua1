using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Admin.CastManagement
{
    public partial class CastList : System.Web.UI.Page
    {
        private CastBLL castBLL;
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                castBLL = new CastBLL(DataAccessLevel.Admin);
                hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_CreateCast", null);
                enableTool = false;
                toolDetail = null;
                if (!IsPostBack)
                {
                    await SetGrvCast();
                    SetDrdlPage();
                }
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
                await SetGrvCast();
                SetDrdlPage();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }

        }

        private async Task SetGrvCast()
        {
            PagedList<CastInfo> casts = await castBLL
                .GetCastsAsync(drdlPage.SelectedIndex, 20);
            castBLL.Dispose();
            grvCast.DataSource = casts.Items;
            grvCast.DataBind();

            pageNumber = casts.PageNumber;
            currentPage = casts.CurrentPage;
        }

        private void SetDrdlPage()
        {
            int selectedIndex = drdlPage.SelectedIndex;
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

        protected async void grvCast_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long key = (long)grvCast.DataKeys[grvCast.SelectedIndex].Value;
                CastInfo castInfo = await castBLL.GetCastAsync(key);
                castBLL.Dispose();
                enableTool = true;
                toolDetail = string.Format("{0} -- {1}", castInfo.ID, castInfo.name);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_CastDetail", new { id = castInfo.ID });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateCast", new { id = castInfo.ID });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteCast", new { id = castInfo.ID });
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}