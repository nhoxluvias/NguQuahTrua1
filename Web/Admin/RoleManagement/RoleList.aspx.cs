using MSSQL_Lite.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Models;

namespace Web.Admin.RoleManagement
{
    public partial class RoleList : System.Web.UI.Page
    {
        private DBContext db;
        protected async void Page_Load(object sender, EventArgs e)
        {
            db = new DBContext(ConnectionType.ManuallyDisconnect);
            GridView1.DataSource = await db.Roles.ToListAsync();
            GridView1.DataBind();
        }
    }
}