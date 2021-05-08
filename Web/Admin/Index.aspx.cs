using Common.SystemInformation;
using System;
using Web.Common;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Models;
using System.Threading.Tasks;

namespace Web.Admin
{
    public partial class Index : System.Web.UI.Page
    {
        private DBContext db;
        protected SystemInfo systemInfo;
        protected long pageVisitor;
        protected long movieNumber;
        protected long categoryNumber;
        protected long tagNumber;

        protected async void Page_Load(object sender, EventArgs e)
        {
            db = new DBContext();
            systemInfo = new SystemInfo();
            pageVisitor = PageVisitor.Views;
            await LoadOverview();
        }

        private async Task LoadOverview()
        {
            movieNumber = await db.Films.CountAsync();
            categoryNumber = await db.Categories.CountAsync();
            tagNumber = await db.Tags.CountAsync();
        }
    }
}