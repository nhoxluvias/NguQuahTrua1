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

namespace Web.Admin.RoleManagement
{
    public partial class DeleteRole : System.Web.UI.Page
    {
        private RoleBLL roleBLL;
        protected RoleInfo roleInfo;
        protected bool enableShowInfo;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                roleBLL = new RoleBLL(DataAccessLevel.Admin);
                enableShowInfo = false;
                enableShowResult = false;
                stateString = null;
                stateDetail = null;
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_RoleList", null);
                if (!IsPostBack)
                {
                    await GetRoleInfo();
                    roleBLL.Dispose();
                }
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private string GetRoleId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return null;
            return obj.ToString();
        }

        private async Task GetRoleInfo()
        {
            string id = GetRoleId();
            if (String.IsNullOrEmpty(id))
            {
                Response.RedirectToRoute("Admin_RoleList", null);
            }
            else
            {
                roleInfo = await roleBLL.GetRoleAsync(id);
                if (roleInfo == null)
                {
                    enableShowInfo = false;
                    Response.RedirectToRoute("Admin_RoleList", null);
                }
                else
                {
                    enableShowInfo = true;
                }
            }
        }

        private async Task DeleteRoleInfo()
        {
            string id = GetRoleId();
            StateOfDeletion state = await roleBLL.DeleteRoleAsync(id);
            roleBLL.Dispose();
            enableShowResult = true;
            enableShowInfo = false;
            if (state == StateOfDeletion.Success)
            {
                stateString = "Success";
                stateDetail = "Đã xóa vai trò thành công";
            }
            else if (state == StateOfDeletion.Failed)
            {
                stateString = "Failed";
                stateDetail = "Xóa vai trò thất bại";
            }
            else
            {
                stateString = "ConstraintExists";
                stateDetail = "Không thể xóa vai trò. Lý do: Vai trò này đang được sử dụng!";
            }
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                await DeleteRoleInfo();
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            } 
        }
    }
}