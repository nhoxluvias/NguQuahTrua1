using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;

namespace Web.Admin.TagManagement
{
    public partial class DeleteTag : System.Web.UI.Page
    {
        private TagBLL tagBLL;
        protected TagInfo tagInfo;
        protected bool enableShowInfo;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            tagBLL = new TagBLL();
            enableShowInfo = false;
            enableShowResult = false;
            stateString = null;
            stateDetail = null;
            try
            {
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_TagList", null);

                if (CheckLoggedIn())
                {
                    if (!IsPostBack)
                    {
                        await GetTagInfo();
                        tagBLL.Dispose();
                    }
                }
                else
                {
                    Response.RedirectToRoute("Account_Login", null);
                    tagBLL.Dispose();
                }
            }
            catch (Exception ex)
            {
                tagBLL.Dispose();
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private bool CheckLoggedIn()
        {
            object obj = Session["userSession"];
            if (obj == null)
                return false;

            UserSession userSession = (UserSession)obj;
            return (userSession.role == "Admin" || userSession.role == "Editor");
        }

        private long GetTagId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return long.Parse(obj.ToString());
        }

        private async Task GetTagInfo()
        {
            long id = GetTagId();
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_TagList", null);
            }
            else
            {
                tagInfo = await tagBLL.GetTagAsync(id);
                if (tagInfo == null)
                    Response.RedirectToRoute("Admin_TagList", null);
                else
                    enableShowInfo = true;
            }
        }

        private async Task DeleteTagInfo()
        {
            long id = GetTagId();
            StateOfDeletion state = await tagBLL.DeleteTagAsync(id);
            if (state == StateOfDeletion.Success)
            {
                stateString = "Success";
                stateDetail = "Đã xóa thẻ tag thành công";
            }
            else if (state == StateOfDeletion.Failed)
            {
                stateString = "Failed";
                stateDetail = "Xóa thẻ tag thất bại";
            }
            else
            {
                stateString = "ConstraintExists";
                stateDetail = "Không thể xóa thẻ tag. Lý do: Thẻ tag này đang được sử dụng!";
            }
            enableShowResult = true;
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                await DeleteTagInfo();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            tagBLL.Dispose();
        }
    }
}