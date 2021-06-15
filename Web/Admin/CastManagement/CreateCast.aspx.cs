using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;
using Web.Validation;

namespace Web.Admin.CastManagement
{
    public partial class CreateCast : System.Web.UI.Page
    {
        private CastBLL castBLL;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            castBLL = new CastBLL(DataAccessLevel.Admin);
            customValidation = new CustomValidation();
            enableShowResult = false;
            stateString = null;
            stateDetail = null;
            hyplnkList.NavigateUrl = GetRouteUrl("Admin_CastList", null);
            InitValidation();
            if (IsPostBack)
            {
                await Create();
            }
            castBLL.Dispose();
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

        private CastCreation GetCastCreation()
        {
            return new CastCreation
            {
                name = Request.Form[txtCastName.UniqueID],
                description = Request.Form[txtCastDescription.UniqueID]
            };
        }

        public async Task Create()
        {
            try
            {
                if (IsValidData())
                {
                    CastCreation cast = GetCastCreation();
                    StateOfCreation state = await castBLL.CreateCastAsync(cast);
                    if (state == StateOfCreation.Success)
                    {
                        enableShowResult = true;
                        stateString = "Success";
                        stateDetail = "Đã thêm diễn viên thành công";
                    }
                    else if (state == StateOfCreation.AlreadyExists)
                    {
                        enableShowResult = true;
                        stateString = "AlreadyExists";
                        stateDetail = "Thêm diễn viên thất bại. Lý do: Đã tồn tại diễn viên này";
                    }
                    else
                    {
                        enableShowResult = true;
                        stateString = "Failed";
                        stateDetail = "Thêm diễn viên thất bại";
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