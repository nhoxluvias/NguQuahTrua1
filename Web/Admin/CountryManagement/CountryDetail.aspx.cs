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

namespace Web.Admin.CountryManagement
{
    public partial class CountryDetail : System.Web.UI.Page
    {
        private CountryBLL countryBLL;
        protected CountryInfo countryInfo;
        protected bool enableShowDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                enableShowDetail = false;
                countryBLL = new CountryBLL(DataAccessLevel.Admin);
                int id = GetCountryId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_CountryList", null);
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateCountry", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteCountry", new { id = id });
                await GetCountryInfo(id);
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

        private async Task GetCountryInfo(int id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_CategoryList", null);
            }
            else
            {
                enableShowDetail = true;
                countryInfo = await countryBLL.GetCountryAsync(id);
                if (countryInfo == null)
                {
                    enableShowDetail = false;
                    Response.RedirectToRoute("Admin_CategoryList", null);
                }
            }
        }
    }
}