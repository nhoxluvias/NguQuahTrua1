using Common.SystemInformation;
using System;
using System.Threading.Tasks;
using Data.BLL;
using Common.Web;
using Web.Models;

namespace Web.Admin
{
    public partial class Index : System.Web.UI.Page
    {
        private FilmBLL filmBLL;
        protected SystemInfo systemInfo;
        protected long pageVisitor;
        protected long movieNumber;
        protected long categoryNumber;
        protected long tagNumber;

        protected async void Page_Load(object sender, EventArgs e)
        {
            filmBLL = new FilmBLL(DataAccessLevel.Admin);
            pageVisitor = PageVisitor.Views;
            try
            {
                systemInfo = new SystemInfo();
                await LoadOverview();
            }
            catch(Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
            filmBLL.Dispose();
        }

        private async Task LoadOverview()
        {
            movieNumber = await filmBLL.CountAllAsync();
            categoryNumber = await new CategoryBLL(filmBLL, DataAccessLevel.Admin).CountAllAsync();
            tagNumber = await new TagBLL(filmBLL, DataAccessLevel.Admin).CountAllAsync();
        }
    }
}