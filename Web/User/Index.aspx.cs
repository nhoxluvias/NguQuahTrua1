using Data.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using Data.DTO;

namespace Web.User
{
    public partial class Index : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        protected List<FilmInfo> latestFilms;
        protected List<List<FilmInfo>> filmsByCategory; 

        protected async void Page_Load(object sender, EventArgs e)
        {
            filmBLL = new FilmBLL(DataAccessLevel.User);
            await GetLatestFilm();
            await GetFilmsByCategory();
        }

        private async Task GetLatestFilm()
        {
            latestFilms = (await filmBLL.GetLatestFilmAsync())
                .Select(f => new FilmInfo
                {
                    ID = f.ID,
                    name = f.name,
                    thumbnail = VirtualPathUtility.ToAbsolute("~/images/") + f.thumbnail,
                    url = GetRouteUrl("User_FilmDetails", new { slug = f.name.TextToUrl(), id = f.ID }),
                    Categories = f.Categories
                }).ToList();
        }

        private async Task GetFilmsByCategory()
        {
            List<CategoryInfo> categories = await new CategoryBLL(filmBLL, DataAccessLevel.User).GetCategoriesAsync();
            filmsByCategory = new List<List<FilmInfo>>();
            foreach(CategoryInfo category in categories)
            {
                filmsByCategory.Add(await filmBLL.GetFilmsByCategoryIdAsync(category.ID));
            }

            filmBLL.Dispose();
        }
    }
}