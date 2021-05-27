using Data.BLL;
using Data.DTO;
using System;
using System.Threading.Tasks;
using System.Web.UI;
using Web.Models;

namespace Web.Admin.DirectorManagement
{
    public partial class DirectorDetail : System.Web.UI.Page
    {
        private DirectorBLL directorBLL;
        protected DirectorInfo directorInfo;
        protected bool enableShowDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                enableShowDetail = false;
                directorBLL = new DirectorBLL(DataAccessLevel.Admin);
                long id = GetDirectorId();
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_DirectorList", null);
                hyplnkEdit.NavigateUrl = GetRouteUrl("Admin_UpdateDirector", new { id = id });
                hyplnkDelete.NavigateUrl = GetRouteUrl("Admin_DeleteDirector", new { id = id });
                await GetDirectorInfo(id);
                directorBLL.Dispose();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private long GetDirectorId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return long.Parse(obj.ToString());
        }

        private async Task GetDirectorInfo(long id)
        {
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_DirectorList", null);
            }
            else
            {
                enableShowDetail = true;
                directorInfo = await directorBLL.GetDirectorAsync(id);
                if (directorInfo == null)
                {
                    enableShowDetail = false;
                    Response.RedirectToRoute("Admin_DirectorList", null);
                }
            }
        }
    }
}