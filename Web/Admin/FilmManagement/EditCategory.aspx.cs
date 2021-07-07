using Data.BLL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.FilmManagement
{
    public partial class EditCategory : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        protected string filmName;
        protected List<CategoryInfo> categoriesByFilmId;
        protected bool enableShowDetail;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            enableShowDetail = false;
            filmBLL = new FilmBLL(DataAccessLevel.Admin);
            string id = GetFilmId();
            await LoadCategories();
            if (IsPostBack)
            {
                await LoadFilmInfo(id);
            }
            else
            {
                await LoadFilmInfo(id, true);
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

        private async Task LoadFilmInfo(string id, bool loadCategoryInfo = false)
        {
            if (string.IsNullOrEmpty(id))
            {
                Response.RedirectToRoute("Admin_FilmList", null);
            }
            else
            {
                FilmInfo filmInfo = await filmBLL.GetFilmAsync(id);
                if(filmInfo == null)
                {
                    Response.RedirectToRoute("Admin_FilmList", null);
                }
                else
                {
                    enableShowDetail = true;
                    categoriesByFilmId = (loadCategoryInfo) ? filmInfo.Categories : null;
                    filmName = filmInfo.name;
                }
            }
        }

        private async Task LoadCategories()
        {
            List<CategoryInfo> categoryInfos = await new CategoryBLL(filmBLL, DataAccessLevel.Admin).GetCategoriesAsync();
            foreach(CategoryInfo categoryInfo in categoryInfos)
            {
                drdlFilmCategory.Items.Add(new ListItem(categoryInfo.name, categoryInfo.ID.ToString()));
            }
            drdlFilmCategory.SelectedIndex = 0;
        }

        protected async void btnAddCategory_Click(object sender, EventArgs e)
        {
            CategoryBLL categoryBLL = new CategoryBLL(DataAccessLevel.Admin);
            categoriesByFilmId = await categoryBLL.GetCategoriesByFilmIdAsync(GetFilmId());
            categoryBLL.Dispose();
        }
    }
}