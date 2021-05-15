using MSSQL_Lite.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Models;
using Web.Models.DTO;

namespace Web.User.Layout
{
    public partial class UserLayout : System.Web.UI.MasterPage
    {
        private DBContext db;
        protected List<CategoryInfo> categories;
        protected void Page_Load(object sender, EventArgs e)
        {
            db = new DBContext(ConnectionType.ManuallyDisconnect);
            GetCategories();
        }

        private void GetCategories()
        {
            categories = db.Categories.ToList(c => new { c.ID, c.name })
                .Select(c => new CategoryInfo {
                    ID = c.ID,
                    name = c.name,
                    url = GetRouteUrl("User_Category", new { id = c.ID })
                }).ToList();

        }


    }
}