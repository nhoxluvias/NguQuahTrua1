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

namespace Web.Admin.SourceManagement
{
    public partial class SourceList : System.Web.UI.Page
    {
        private SourceBLL sourceBLL;
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                sourceBLL = new SourceBLL(DataAccessLevel.Admin);
                hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_CreateSource", null);
                enableTool = false;
                toolDetail = null;
                if (!IsPostBack)
                {
                    await SetGrvSource();
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
                await SetGrvSource();
                SetDrdlPage();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }

        }

        private async Task SetGrvSource()
        {
            PagedList<SourceInfo> sources = await sourceBLL
                .GetSourcesAsync(drdlPage.SelectedIndex, 20);
            sourceBLL.Dispose();
            grvSource.DataSource = sources.Items;
            grvSource.DataBind();

            pageNumber = sources.PageNumber;
            currentPage = sources.CurrentPage;
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

        protected async void grvSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    int key = (int)grvSource.DataKeys[grvSource.SelectedIndex].Value;
            //    SourceInfo sourceInfo = await sourceBLL.GetSourceAsync(key);
            //    sourceBLL.Dispose();
            //    enableTool = true;
            //    toolDetail = string.Format("{0} -- {1}", sourceInfo.ID, sourceInfo.name);
            //    hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_SourceDetail", new { id = sourceInfo.ID });
            //    hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateSource", new { id = sourceInfo.ID });
            //    hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteSource", new { id = sourceInfo.ID });
            //}
            //catch (Exception ex)
            //{
            //    Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
            //    Response.RedirectToRoute("Notification_Error", null);
            //}
        }
    }
}