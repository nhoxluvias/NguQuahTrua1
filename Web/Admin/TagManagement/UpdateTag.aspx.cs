using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;
using Web.Validation;

namespace Web.Admin.TagManagement
{
    public partial class UpdateTag : System.Web.UI.Page
    {
        private TagBLL tagBLL;
        protected TagInfo tagInfo;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected bool enableShowForm;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tagBLL = new TagBLL(DataAccessLevel.Admin);
                customValidation = new CustomValidation();
                enableShowResult = false;
                enableShowForm = false;
                stateString = null;
                stateDetail = null;
                long id = GetTagId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_TagList", null);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_TagDetail", null);
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteTag", null);
                InitValidation();
                if (IsPostBack)
                {
                    if (IsValidData())
                    {
                        await Update();
                        await LoadTagInfo(id);
                    }
                }
                else
                {
                    await LoadTagInfo(id);
                }
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

        private async Task LoadTagInfo(long id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_TagList", null);
            }
            else
            {
                TagInfo tagInfo = await tagBLL.GetTagAsync(id);
                if (tagInfo == null)
                {
                    Response.RedirectToRoute("Admin_TagList", null);
                }
                else
                {
                    hdTagId.Value = tagInfo.ID.ToString();
                    txtTagName.Text = tagInfo.name;
                    txtTagDescription.Text = tagInfo.description;
                }
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

        private TagUpdate GetTagUpdate()
        {
            return new TagUpdate
            {
                ID = long.Parse(Request.Form[hdTagId.UniqueID]),
                name = Request.Form[txtTagName.UniqueID],
                description = Request.Form[txtTagDescription.UniqueID]
            };
        }

        private async Task Update()
        {
            TagUpdate tagUpdate = GetTagUpdate();
            StateOfUpdate state = await tagBLL.UpdateTagAsync(tagUpdate);
            enableShowResult = true;
            if (state == StateOfUpdate.Success)
            {
                stateString = "Success";
                stateDetail = "Đã cập nhật thẻ tag thành công";
            }
            else
            {
                stateString = "Failed";
                stateDetail = "Cập nhật thẻ tag thất bại";
            }
        }
    }
}