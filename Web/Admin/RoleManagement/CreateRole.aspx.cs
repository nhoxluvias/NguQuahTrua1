using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;
using Web.Validation;

namespace Web.Admin.RoleManagement
{
    public partial class CreateRole : System.Web.UI.Page
    {
        private RoleBLL roleBLL;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            roleBLL = new RoleBLL(DataAccessLevel.Admin);
            customValidation = new CustomValidation();
            enableShowResult = false;
            stateString = null;
            stateDetail = null;
            hyplnkList.NavigateUrl = GetRouteUrl("Admin_RoleList", null);
            InitValidation();
            if (IsPostBack)
            {
                await Create();
            }
            roleBLL.Dispose();
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

        private RoleCreation GetRoleCreation()
        {
            return new RoleCreation
            {
                name = Request.Form[txtRoleName.UniqueID],
            };
        }

        public async Task Create()
        {
            try
            {
                if (IsValidData())
                {
                    RoleCreation role = GetRoleCreation();
                    StateOfCreation state = await roleBLL.CreateRoleAsync(role);
                    if (state == StateOfCreation.Success)
                    {
                        enableShowResult = true;
                        stateString = "Success";
                        stateDetail = "Đã thêm vai trò thành công";
                    }
                    else if (state == StateOfCreation.AlreadyExists)
                    {
                        enableShowResult = true;
                        stateString = "AlreadyExists";
                        stateDetail = "Thêm vài trò thất bại. Lý do: Đã tồn tại vai trò này";
                    }
                    else
                    {
                        enableShowResult = true;
                        stateString = "Failed";
                        stateDetail = "Thêm vai trò thất bại";
                    }
                }
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}