using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Admin.CountryManagement
{
    public partial class CountryList : System.Web.UI.Page
    {
        private CountryBLL countryBLL;
        private int selectedIndex;
        protected long currentPage;
        protected long pageNumber;
        protected bool enableTool;
        protected string toolDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                countryBLL = new CountryBLL(DataAccessLevel.Admin);
                hyplnkCreate.NavigateUrl = GetRouteUrl("Admin_CreateCountry", null);
                selectedIndex = 0;
                enableTool = false;
                toolDetail = null;
                if (!IsPostBack)
                {
                    await SetGrvCountry();
                    selectedIndex = 0;
                    SetDrdlPage();
                }
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        protected async void drdlPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await SetGrvCountry();
                selectedIndex = drdlPage.SelectedIndex;
                SetDrdlPage();
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private async Task SetGrvCountry()
        {
            PagedList<CountryInfo> countries = await countryBLL
                .GetCountriesAsync(drdlPage.SelectedIndex, 20);
            countryBLL.Dispose();
            grvCountry.DataSource = countries.Items;
            grvCountry.DataBind();

            pageNumber = countries.PageNumber;
            currentPage = countries.CurrentPage;
        }

        private void SetDrdlPage()
        {
            drdlPage.Items.Clear();
            for (int i = 0; i < pageNumber; i++)
            {
                string item = (i + 1).ToString();
                if (i == currentPage)
                    drdlPage.Items.Add(string.Format("{0}*", item));
                else
                    drdlPage.Items.Add(item);
            }
            drdlPage.SelectedIndex = selectedIndex;
        }

        protected async void grvCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int key = (int)grvCountry.DataKeys[grvCountry.SelectedIndex].Value;
                CountryInfo countryInfo = await countryBLL.GetCountryAsync(key);
                countryBLL.Dispose();
                enableTool = true;
                toolDetail = string.Format("{0} -- {1}", countryInfo.ID, countryInfo.name);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_CountryDetail", new { id = countryInfo.ID });
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateCountry", new { id = countryInfo.ID });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteCountry", new { id = countryInfo.ID });
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}