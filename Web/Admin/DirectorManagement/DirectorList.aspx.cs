using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Admin.DirectorManagement
{
    public partial class DirectorList : System.Web.UI.Page
    {
        private DirectorBLL directorBLL;
        private int selectedIndex;
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                directorBLL = new DirectorBLL(DataAccessLevel.Admin);
                hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_CreateDirector", null);
                selectedIndex = 0;
                enableTool = false;
                toolDetail = null;
                if (!IsPostBack)
                {
                    await SetGrvDirector();
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

        protected async void drdlPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await SetGrvDirector();
                selectedIndex = drdlPage.SelectedIndex;
                SetDrdlPage();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }

        }

        private async Task SetGrvDirector()
        {
            PagedList<DirectorInfo> directors = await directorBLL
                .GetDirectorsAsync(drdlPage.SelectedIndex, 20);
            directorBLL.Dispose();
            grvDirector.DataSource = directors.Items;
            grvDirector.DataBind();

            pageNumber = directors.PageNumber;
            currentPage = directors.CurrentPage;
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

        protected async void grvDirector_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long key = (long)grvDirector.DataKeys[grvDirector.SelectedIndex].Value;
                DirectorInfo directorInfo = await directorBLL.GetDirectorAsync(key);
                directorBLL.Dispose();
                enableTool = true;
                toolDetail = string.Format("{0} -- {1}", directorInfo.ID, directorInfo.name);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_DirectorDetail", new { id = directorInfo.ID });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateDirector", new { id = directorInfo.ID });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteDirector", new { id = directorInfo.ID });
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}