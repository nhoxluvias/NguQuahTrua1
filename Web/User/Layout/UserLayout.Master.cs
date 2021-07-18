using Data.BLL;
using System;
using System.Collections.Generic;
using Data.DTO;
using System.Linq;
using Web.Models;

namespace Web.User.Layout
{
    public partial class UserLayout : System.Web.UI.MasterPage
    {
        private CategoryBLL categoryBLL;
        protected List<CategoryInfo> categories;
        protected void Page_Load(object sender, EventArgs e)
        {
            categoryBLL = new CategoryBLL();
            try
            {
                GetCategories();
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            categoryBLL.Dispose();
        }

        private void GetCategories()
        {
            categories = categoryBLL.GetCategories()
                .Select(c => new CategoryInfo { 
                    ID = c.ID, 
                    name = c.name, 
                    description = c.description,
                    url = GetRouteUrl("User_Category", new { id = c.ID })
                }).ToList();
        }


    }
}