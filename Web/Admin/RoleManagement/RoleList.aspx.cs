using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Admin.RoleManagement
{
    public partial class RoleList : System.Web.UI.Page
    {
        private RoleBLL roleBLL;
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                roleBLL = new RoleBLL(DataAccessLevel.Admin);
                hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_CreateRole", null);
                enableTool = false;
                toolDetail = null;
                if (!IsPostBack)
                {
                    await SetGrvRole();
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
                await SetGrvRole();
                SetDrdlPage();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }

        }

        private async Task SetGrvRole()
        {
            PagedList<RoleInfo> roles = await roleBLL
                .GetRolesAsync(drdlPage.SelectedIndex, 20);
            roleBLL.Dispose();
            grvRole.DataSource = roles.Items;
            grvRole.DataBind();

            pageNumber = roles.PageNumber;
            currentPage = roles.CurrentPage;
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

        protected async void grvRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string key = (string)grvRole.DataKeys[grvRole.SelectedIndex].Value;
                RoleInfo roleInfo = await roleBLL.GetRoleAsync(key);
                roleBLL.Dispose();
                enableTool = true;
                toolDetail = string.Format("{0} -- {1}", roleInfo.ID, roleInfo.name);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_RoleDetail", new { id = roleInfo.ID });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateRole", new { id = roleInfo.ID });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteRole", new { id = roleInfo.ID });
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}