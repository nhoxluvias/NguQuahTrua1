using Data.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Web.Models;
using Data.DTO;
using Common.Upload;
using Common.Rating;


namespace Web.User
{
    public partial class FilmByCategory : System.Web.UI.Page
    {
        private CategoryBLL categoryBLL;
        protected CategoryInfo categoryInfo;
        protected string categoryName;
        protected async Task Page_LoadAsync(object sender, EventArgs e)
        {
            categoryBLL = new CategoryBLL(DataAccessLevel.User);
            await GetCategoryById();
        }
        
        private string getCategoryId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return null;
            return (string)obj;
        }

        private async Task GetCategoryById()
        { 
        object obj = Page.RouteData.Values["id"];
            if(obj == null)
            {

            }
            else
            {
                int categoryId = int.Parse((string)obj);
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "Select [name] from [Category]";
            }
        }
    }
}