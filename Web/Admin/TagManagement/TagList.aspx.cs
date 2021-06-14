using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Admin.TagManagement
{
    public partial class TagList : System.Web.UI.Page
    {
        private TagBLL tagBLL;
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tagBLL = new TagBLL(DataAccessLevel.Admin);
                hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_CreateTag", null);
                enableTool = false;
                toolDetail = null;
                if (!IsPostBack)
                {
                    await SetGrvTag();
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
                await SetGrvTag();
                SetDrdlPage();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }

        }

        private async Task SetGrvTag()
        {
            PagedList<TagInfo> categories = await tagBLL
                .GetTagsAsync(drdlPage.SelectedIndex, 20);
            tagBLL.Dispose();
            grvTag.DataSource = categories.Items;
            grvTag.DataBind();

            pageNumber = categories.PageNumber;
            currentPage = categories.CurrentPage;
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

        protected async void grvTag_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long key = (long)grvTag.DataKeys[grvTag.SelectedIndex].Value;
                TagInfo tagInfo = await tagBLL.GetTagAsync(key);
                tagBLL.Dispose();
                enableTool = true;
                toolDetail = string.Format("{0} -- {1}", tagInfo.ID, tagInfo.name);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_TagDetail", new { id = tagInfo.ID });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateTag", new { id = tagInfo.ID });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteTag", new { id = tagInfo.ID });
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}