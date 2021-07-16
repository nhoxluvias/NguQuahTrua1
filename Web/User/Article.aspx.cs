using Data.BLL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.User
{
    public partial class Article : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FilmBLL filmBLL = new FilmBLL();

            CategoryBLL categoryBLL = new CategoryBLL(filmBLL);
            TagBLL tagBLL = new TagBLL(filmBLL);

            filmBLL.Dispose();
            List<CategoryInfo> categories = categoryBLL.GetCategories();
        }
    }
}