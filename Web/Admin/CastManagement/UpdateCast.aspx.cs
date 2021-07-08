using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;
using Web.Validation;

namespace Web.Admin.CastManagement
{
    public partial class UpdateCast : System.Web.UI.Page
    {
        private CastBLL castBLL;
        protected CastInfo castInfo;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected bool enableShowForm;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                castBLL = new CastBLL(DataAccessLevel.Admin);
                customValidation = new CustomValidation();
                enableShowResult = false;
                enableShowForm = false;
                stateString = null;
                stateDetail = null;
                long id = GetCastId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_CastList", null);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_CastDetail", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteCast", new { id = id });
                InitValidation();
                if (IsPostBack)
                {
                    if (IsValidData())
                    {
                        await Update();
                        await LoadCastInfo(id);
                    }
                }
                else
                {
                    await LoadCastInfo(id);
                }
                castBLL.Dispose();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private long GetCastId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return long.Parse(obj.ToString());
        }

        private async Task LoadCastInfo(long id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_CastList", null);
            }
            else
            {
                CastInfo castInfo = await castBLL.GetCastAsync(id);
                if (castInfo == null)
                {
                    Response.RedirectToRoute("Admin_CastList", null);
                }
                else
                {
                    hdCastId.Value = castInfo.ID.ToString();
                    txtCastName.Text = castInfo.name;
                    txtCastDescription.Text = castInfo.description;
                }
            }
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvCastName,
                "txtCastName",
                "Tên diễn viên không hợp lệ",
                true,
                null,
                customValidation.ValidateCastName
            );
        }

        private void ValidateData()
        {
            cvCastName.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvCastName.IsValid;
        }

        private CastUpdate GetCastUpdate()
        {
            return new CastUpdate
            {
                ID = long.Parse(Request.Form[hdCastId.UniqueID]),
                name = Request.Form[txtCastName.UniqueID],
                description = Request.Form[txtCastDescription.UniqueID]
            };
        }

        private async Task Update()
        {
            CastUpdate castUpdate = GetCastUpdate();
            StateOfUpdate state = await castBLL.UpdateCastAsync(castUpdate);
            enableShowResult = true;
            if (state == StateOfUpdate.Success)
            {
                stateString = "Success";
                stateDetail = "Đã cập nhật diễn viên thành công";
            }
            else
            {
                stateString = "Failed";
                stateDetail = "Cập nhật diễn viên thất bại";
            }
        }
    }
}