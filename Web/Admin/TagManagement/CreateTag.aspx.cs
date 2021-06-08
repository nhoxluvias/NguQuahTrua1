using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;
using Web.Validation;

namespace Web.Admin.TagManagement
{
    public partial class CreateTag : System.Web.UI.Page
    {
        private TagBLL tagBLL;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tagBLL = new TagBLL(DataAccessLevel.Admin);
                customValidation = new CustomValidation();
                enableShowResult = false;
                stateString = null;
                stateDetail = null;
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_TagList", null);
                InitValidation();
                if (IsPostBack)
                {
                    await Create();
                }
                tagBLL.Dispose();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvTagName,
                "txtTagName",
                "Tên thẻ tag không hợp lệ",
                true,
                null,
                customValidation.ValidateTagName
            );
        }

        private void ValidateData()
        {
            cvTagName.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvTagName.IsValid;
        }

        private TagCreation GetTagCreation()
        {
            return new TagCreation
            {
                name = Request.Form[txtTagName.UniqueID],
                description = Request.Form[txtTagDescription.UniqueID]
            };
        }

        public async Task Create()
        {
            if (IsValidData())
            {
                TagCreation tag = GetTagCreation();
                StateOfCreation state = await tagBLL.CreateTagAsync(tag);
                enableShowResult = true;
                if (state == StateOfCreation.Success)
                {
                    stateString = "Success";
                    stateDetail = "Đã thêm thẻ tag thành công";
                }
                else if (state == StateOfCreation.AlreadyExists)
                {
                    stateString = "AlreadyExists";
                    stateDetail = "Thêm thẻ tag thất bại. Lý do: Đã tồn tại thẻ tag này";
                }
                else
                {
                    stateString = "Failed";
                    stateDetail = "Thêm thẻ tag thất bại";
                }
            }
        }
    }
}