using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;
using Web.Validation;

namespace Web.Admin.CountryManagement
{
    public partial class CreateCountry : System.Web.UI.Page
    {
        private CountryBLL countryBLL;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            countryBLL = new CountryBLL(DataAccessLevel.Admin);
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
                cvCountryName,
                "txtCountryName",
                "Tên quốc gia không hợp lệ",
                true,
                null,
                customValidation.ValidateCountryName
            );
        }

        private void ValidateData()
        {
            cvCountryName.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvCountryName.IsValid;
        }

        private CountryCreation GetCountryCreation()
        {
            return new CountryCreation
            {
                name = Request.Form[txtCountryName.UniqueID],
                description = Request.Form[txtCountryDescription.UniqueID]
            };
        }

        public async Task Create()
        {
            try
            {
                if (IsValidData())
                {
                    CountryCreation country = GetCountryCreation();
                    StateOfCreation state = await countryBLL.CreateCountryAsync(country);
                    if (state == StateOfCreation.Success)
                    {
                        enableShowResult = true;
                        stateString = "Success";
                        stateDetail = "Đã thêm quốc gia thành công";
                    }
                    else if (state == StateOfCreation.AlreadyExists)
                    {
                        enableShowResult = true;
                        stateString = "AlreadyExists";
                        stateDetail = "Thêm quốc gia thất bại. Lý do: Đã tồn tại quốc gia này";
                    }
                    else
                    {
                        enableShowResult = true;
                        stateString = "Failed";
                        stateDetail = "Thêm quốc gia thất bại";
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