using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.Layout
{
    public partial class AdminLayout : System.Web.UI.MasterPage
    {
        protected string hyplnkOverview;
        protected string hyplnkCategoryManagement;

        protected void Page_Load(object sender, EventArgs e)
        {
            hyplnkOverview = GetRouteUrl("Admin_Overview", null);
            hyplnkCategoryManagement = GetRouteUrl("Admin_CategoryList", null);
        }
    }
}