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
using Web.Validation;

namespace Web.Admin.CountryManagement
{
    public partial class UpdateCountry : System.Web.UI.Page
    {
        private CountryBLL countryBLL;
        protected CountryInfo countryInfo;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected bool enableShowForm;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                countryBLL = new CountryBLL(DataAccessLevel.Admin);
                customValidation = new CustomValidation();
                enableShowResult = false;
                enableShowForm = false;
                stateString = null;
                stateDetail = null;
                int id = GetCountryId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_CountryList", null);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_CountryDetail", null);
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteCountry", null);
                InitValidation();
                if (IsPostBack)
                {
                    if (IsValidData())
                    {
                        await Update();
                        await LoadCountryInfo(id);
                    }
                }
                else
                {
                    await LoadCountryInfo(id);
                }

            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private int GetCountryId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return int.Parse(obj.ToString());
        }

        private async Task LoadCountryInfo(int id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_CountryList", null);
            }
            else
            {
                CountryInfo countryInfo = await countryBLL.GetCountryAsync(id);
                if (countryInfo == null)
                {
                    Response.RedirectToRoute("Admin_CountryList", null);
                }
                else
                {
                    hdCountryId.Value = countryInfo.ID.ToString();
                    txtCountryName.Text = countryInfo.name;
                    txtCountryDescription.Text = countryInfo.description;
                }
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

        private CountryUpdate GetCountryUpdate()
        {
            return new CountryUpdate
            {
                ID = int.Parse(Request.Form[hdCountryId.UniqueID]),
                name = Request.Form[txtCountryName.UniqueID],
                description = Request.Form[txtCountryDescription.UniqueID]
            };
        }

        private async Task Update()
        {
            CountryUpdate countryUpdate = GetCountryUpdate();
            StateOfUpdate state = await countryBLL.UpdateCountryAsync(countryUpdate);
            enableShowResult = true;
            if (state == StateOfUpdate.Success)
            {
                stateString = "Success";
                stateDetail = "Đã cập nhật quốc gia thành công";
            }
            else
            {
                stateString = "Failed";
                stateDetail = "Cập nhật quốc gia thất bại";
            }
        }
    }
}