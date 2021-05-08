using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Models;

namespace Web.Admin
{
    public partial class CategoryManagement : System.Web.UI.Page
    {
        private DBContext db;
        protected async void Page_Load(object sender, EventArgs e)
        {
            db = new DBContext();
            GridView1.DataSource = await db.Categories.ToListAsync();
            GridView1.DataBind();
        }
    }
}