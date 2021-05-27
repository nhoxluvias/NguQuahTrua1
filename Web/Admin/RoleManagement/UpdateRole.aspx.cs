using Data.BLL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;
using Web.Validation;

namespace Web.Admin.RoleManagement
{
    public partial class UpdateRole : System.Web.UI.Page
    {
        private RoleBLL roleBLL;
        protected RoleInfo roleInfo;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected bool enableShowForm;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                roleBLL = new RoleBLL(DataAccessLevel.Admin);
                customValidation = new CustomValidation();
                enableShowResult = false;
                enableShowForm = false;
                stateString = null;
                stateDetail = null;
                string id = GetRoleId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_RoleList", null);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_RoleDetail", null);
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteRole", null);
                InitValidation();
                if (IsPostBack)
                {
                    if (IsValidData())
                    {
                        await Update();
                        await LoadRoleInfo(id);
                    }
                }
                else
                {
                    await LoadRoleInfo(id);
                }
                roleBLL.Dispose();
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

        private async Task LoadRoleInfo(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Response.RedirectToRoute("Admin_CategoryList", null);
            }
            else
            {
                RoleInfo roleInfo = await roleBLL.GetRoleAsync(id);
                if (roleInfo == null)
                {
                    Response.RedirectToRoute("Admin_CategoryList", null);
                }
                else
                {
                    hdRoleId.Value = roleInfo.ID;
                    txtRoleName.Text = roleInfo.name;
                }
            }
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvRoleName,
                "txtRoleName",
                "Tên vai trò không hợp lệ",
                true,
                null,
                customValidation.ValidateRoleName
            );
        }

        private void ValidateData()
        {
            cvRoleName.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvRoleName.IsValid;
        }

        private RoleUpdate GetRoleUpdate()
        {
            return new RoleUpdate
            {
                ID = Request.Form[hdRoleId.UniqueID],
                name = Request.Form[txtRoleName.UniqueID],
            };
        }

        private async Task Update()
        {
            RoleUpdate roleUpdate = GetRoleUpdate();
            StateOfUpdate state = await roleBLL.UpdateRoleAsync(roleUpdate);
            enableShowResult = true;
            if (state == StateOfUpdate.Success)
            {
                stateString = "Success";
                stateDetail = "Đã cập nhật vai trò thành công";
            }
            else
            {
                stateString = "Failed";
                stateDetail = "Cập nhật vai trò thất bại";
            }
        }
    }
}