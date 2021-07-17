using Data.BLL;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Web.Models;
using Data.DTO;
using Common.Upload;
using Common;
using System.Text;

namespace Web.User
{
    public partial class FilmDetail : System.Web.UI.Page
    {
     
        private FilmBLL filmBLL;
        protected FilmInfo filmInfo;
        protected bool enableShowDetail;
        protected string title_HeadTag;
        protected string keywords_MetaTag;
        protected string description_MetaTag;

        protected async void Page_Load(object sender, EventArgs e)
        {
            filmBLL = new FilmBLL();
            enableShowDetail = false;
            try
            {
                await GetFilmById();
                GenerateHeadTag();
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            filmBLL.Dispose();
        }

        private string GetFilmId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return null;
            return obj.ToString();
        }

        private async Task GetFilmById()
        {
            string id = GetFilmId();
            if (id == null)
            {
                Response.RedirectToRoute("User_Home", null);
            }
            else
            {
                filmBLL.IncludeCategory = true;
                filmBLL.IncludeTag = true;
                filmBLL.IncludeCountry = true;
                filmBLL.IncludeLanguage = true;
                filmBLL.IncludeDirector = true;
                filmBLL.IncludeCast = true;
                filmInfo = await filmBLL.GetFilmAsync(id);
                if(filmInfo == null)
                {
                    Response.RedirectToRoute("User_Home", null);
                }
                else
                {
                    Rating rating = new Rating(filmInfo.upvote, filmInfo.downvote);
                    filmInfo.starRating = rating.SolveStar();
                    filmInfo.scoreRating = rating.SolveScore();

                    if (string.IsNullOrEmpty(filmInfo.thumbnail))
                        filmInfo.thumbnail = VirtualPathUtility
                            .ToAbsolute(string.Format("{0}/Default/default.png", FileUpload.ImageFilePath));
                    else
                        filmInfo.thumbnail = VirtualPathUtility
                            .ToAbsolute(string.Format("{0}/{1}", FileUpload.ImageFilePath, filmInfo.thumbnail));

                    filmInfo.url = GetRouteUrl("User_Watch", new { slug = filmInfo.name.TextToUrl(), id = filmInfo.ID });
                    enableShowDetail = true;
                }
            }
        }

        private void GenerateHeadTag()
        {
            if (filmInfo != null)
            {
                title_HeadTag = filmInfo.name;
                description_MetaTag = (string.Format("{0}...", filmInfo.description.TakeStr(100))).Replace("\n", " ");

                StringBuilder stringBuilder = new StringBuilder();
                foreach (TagInfo tagInfo in filmInfo.Tags)
                {
                    stringBuilder.Append(string.Format("{0}, ", tagInfo.name));
                }
                keywords_MetaTag = stringBuilder.ToString().TrimEnd(' ').TrimEnd(',');
            }
        }
    }
}