using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;
using Web.Validation;

namespace Web.Admin.DirectorManagement
{
    public partial class UpdateDirector : System.Web.UI.Page
    {
        private DirectorBLL directorBLL;
        protected DirectorInfo directorInfo;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected bool enableShowForm;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                directorBLL = new DirectorBLL(DataAccessLevel.Admin);
                customValidation = new CustomValidation();
                enableShowResult = false;
                enableShowForm = false;
                stateString = null;
                stateDetail = null;
                long id = GetDirectorId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_DirectorList", null);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_DirectorDetail", null);
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteDirector", null);
                InitValidation();
                if (IsPostBack)
                {
                    if (IsValidData())
                    {
                        await Update();
                        await LoadDirectorInfo(id);
                    }
                }
                else
                {
                    await LoadDirectorInfo(id);
                }
                directorBLL.Dispose();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private long GetDirectorId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return long.Parse(obj.ToString());
        }

        private async Task LoadDirectorInfo(long id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_DirectorList", null);
            }
            else
            {
                DirectorInfo directorInfo = await directorBLL.GetDirectorAsync(id);
                if (directorInfo == null)
                {
                    Response.RedirectToRoute("Admin_DirectorList", null);
                }
                else
                {
                    hdDirectorId.Value = directorInfo.ID.ToString();
                    txtDirectorName.Text = directorInfo.name;
                    txtDirectorDescription.Text = directorInfo.description;
                }
            }
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvDirectorName,
                "txtDirectorName",
                "Tên đạo diễn không hợp lệ",
                true,
                null,
                customValidation.ValidateDirectorName
            );
        }

        private void ValidateData()
        {
            cvDirectorName.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvDirectorName.IsValid;
        }

        private DirectorUpdate GetDirectorUpdate()
        {
            return new DirectorUpdate
            {
                ID = long.Parse(Request.Form[hdDirectorId.UniqueID]),
                name = Request.Form[txtDirectorName.UniqueID],
                description = Request.Form[txtDirectorDescription.UniqueID]
            };
        }

        private async Task Update()
        {
            DirectorUpdate directorUpdate = GetDirectorUpdate();
            StateOfUpdate state = await directorBLL.UpdateDirectorAsync(directorUpdate);
            enableShowResult = true;
            if (state == StateOfUpdate.Success)
            {
                stateString = "Success";
                stateDetail = "Đã cập nhật đạo diễn thành công";
            }
            else
            {
                stateString = "Failed";
                stateDetail = "Cập nhật đạo diễn thất bại";
            }
        }
    }
}