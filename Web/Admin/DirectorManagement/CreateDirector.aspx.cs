using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;
using Web.Validation;

namespace Web.Admin.DirectorManagement
{
    public partial class CreateDirector : System.Web.UI.Page
    {
        private DirectorBLL directorBLL;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            directorBLL = new DirectorBLL(DataAccessLevel.Admin);
            customValidation = new CustomValidation();
            enableShowResult = false;
            stateString = null;
            stateDetail = null;
            InitValidation();
            if (IsPostBack)
            {
                await Create();
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
                customValidation.ValidateCategoryName
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

        private DirectorCreation GetDirectorCreation()
        {
            return new DirectorCreation
            {
                name = Request.Form[txtDirectorName.UniqueID],
                description = Request.Form[txtDirectorDescription.UniqueID]
            };
        }

        public async Task Create()
        {
            try
            {
                if (IsValidData())
                {
                    DirectorCreation director = GetDirectorCreation();
                    StateOfCreation state = await directorBLL.CreateDirectorAsync(director);
                    if (state == StateOfCreation.Success)
                    {
                        enableShowResult = true;
                        stateString = "Success";
                        stateDetail = "Đã thêm đạo diễn thành công";
                    }
                    else if (state == StateOfCreation.AlreadyExists)
                    {
                        enableShowResult = true;
                        stateString = "AlreadyExists";
                        stateDetail = "Thêm đạo diễn thất bại. Lý do: Đã tồn tại đạo diễn này";
                    }
                    else
                    {
                        enableShowResult = true;
                        stateString = "Failed";
                        stateDetail = "Thêm đạo diễn thất bại";
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