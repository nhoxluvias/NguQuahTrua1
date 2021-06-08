using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;

namespace Web.Admin.TagManagement
{
    public partial class TagDetail : System.Web.UI.Page
    {
        private TagBLL tagBLL;
        protected TagInfo tagInfo;
        protected bool enableShowDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                enableShowDetail = false;
                tagBLL = new TagBLL(DataAccessLevel.Admin);
                long id = GetTagId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_TagList", null);
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateTag", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteTag", new { id = id });
                await GetTagInfo(id);
                tagBLL.Dispose();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private long GetTagId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return long.Parse(obj.ToString());
        }

        private async Task GetTagInfo(long id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_TagList", null);
            }
            else
            {
                enableShowDetail = true;
                tagInfo = await tagBLL.GetTagAsync(id);
                if (tagInfo == null)
                {
                    enableShowDetail = false;
                    Response.RedirectToRoute("Admin_TagList", null);
                }
            }
        }
    }
}