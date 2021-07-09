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

namespace Web.Admin.FilmManagement
{
    public partial class UpdateFilm : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        protected FilmInfo filmInfo;
        private CustomValidation customValidation;
        protected bool enableShowResult;
        protected bool enableShowForm;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                filmBLL = new FilmBLL(DataAccessLevel.Admin);
                customValidation = new CustomValidation();
                enableShowResult = false;
                enableShowForm = false;
                stateString = null;
                stateDetail = null;
                string id = GetFilmId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_FilmList", null);
                hyplnkDetail.NavigateUrl = GetRouteUrl("Admin_FilmDetail", new { id = id });
                hyplnkEdit_Category.NavigateUrl = GetRouteUrl("Admin_EditCategory_Film", new { id = id });
                hyplnkEdit_Tag.NavigateUrl = GetRouteUrl("Admin_EditTag_Film", new { id = id });
                hyplnkEdit_Cast.NavigateUrl = GetRouteUrl("Admin_EditCast_Film", new { id = id });
                hyplnkEdit_Director.NavigateUrl = GetRouteUrl("Admin_EditDirector_Film", new { id = id });
                hyplnkEdit_Image.NavigateUrl = GetRouteUrl("Admin_EditImage_Film", new { id = id });
                hyplnkEdit_Source.NavigateUrl = GetRouteUrl("Admin_EditSource_Film", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteFilm", new { id = id });
                InitValidation();
                if (IsPostBack)
                {
                    if (IsValidData())
                    {
                        await Update();
                        await LoadFilmInfo(id);
                    }
                }
                else
                {
                    await LoadFilmInfo(id);
                }
                filmBLL.Dispose();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private string GetFilmId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return null;
            return obj.ToString();
        }

        private async Task LoadFilmInfo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Response.RedirectToRoute("Admin_FilmList", null);
            }
            else
            {
                FilmInfo filmInfo = await filmBLL.GetFilmAsync(id);
                if (filmInfo == null)
                {
                    Response.RedirectToRoute("Admin_FilmList", null);
                }
                else
                {
                    hdFilmId.Value = filmInfo.ID.ToString();
                    txtFilmName.Text = filmInfo.name;
                    txtFilmDescription.Text = filmInfo.description;
                    txtProductionCompany.Text = filmInfo.productionCompany;
                    txtReleaseDate.Text = filmInfo.releaseDate;

                    drdlFilmCountry.Items.Clear();
                    List<CountryInfo> countryInfos = await new CountryBLL(filmBLL, DataAccessLevel.Admin).GetCountriesAsync();
                    foreach (CountryInfo countryInfo in countryInfos)
                    {
                        drdlFilmCountry.Items.Add(new ListItem(countryInfo.name, countryInfo.ID.ToString()));
                    }
                    drdlFilmCountry.Items.FindByValue(filmInfo.Country.ID.ToString()).Selected = true;

                    drdlFilmLanguage.Items.Clear();
                    List<LanguageInfo> languageInfos = await new LanguageBLL(filmBLL, DataAccessLevel.Admin).GetLanguagesAsync();
                    foreach (LanguageInfo languageInfo in languageInfos)
                    {
                        drdlFilmLanguage.Items.Add(new ListItem(languageInfo.name, languageInfo.ID.ToString()));
                    }
                    drdlFilmLanguage.Items.FindByValue(filmInfo.Language.ID.ToString()).Selected = true;
                }
            }
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvFilmName,
                "txtFilmName",
                "Tên phim không hợp lệ",
                true,
                null,
                customValidation.ValidateFilmName
            );
            customValidation.Init(
                cvProductionCompany,
                "txtProductionCompany",
                "Tên công ty sản xuất không hợp lệ",
                true,
                null,
                customValidation.ValidateProductionCompany
            );
            customValidation.Init(
                cvReleaseDate,
                "txtReleaseDate",
                "Năm phát hành không hợp lệ",
                true,
                null,
                customValidation.ValidateReleaseDate
            );
        }

        private void ValidateData()
        {
            cvFilmName.Validate();
            cvProductionCompany.Validate();
            cvReleaseDate.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return (cvFilmName.IsValid && cvProductionCompany.IsValid && cvReleaseDate.IsValid);
        }

        private FilmUpdate GetFilmUpdate()
        {
            return new FilmUpdate
            {
                ID = Request.Form[hdFilmId.UniqueID],
                name = Request.Form[txtFilmName.UniqueID],
                countryId = int.Parse(Request.Form[drdlFilmCountry.UniqueID]),
                productionCompany = Request.Form[txtProductionCompany.UniqueID],
                languageId = int.Parse(Request.Form[drdlFilmLanguage.UniqueID]),
                releaseDate = Request.Form[txtReleaseDate.UniqueID],
                description = Request.Form[txtFilmDescription.UniqueID]
            };
        }

        private async Task Update()
        {
            FilmUpdate filmUpdate = GetFilmUpdate();
            StateOfUpdate state = await filmBLL.UpdateFilmAsync(filmUpdate);
            enableShowResult = true;
            if (state == StateOfUpdate.Success)
            {
                stateString = "Success";
                stateDetail = "Đã cập nhật phim thành công";
            }
            else
            {
                stateString = "Failed";
                stateDetail = "Cập nhật phim thất bại";
            }
        }
    }
}