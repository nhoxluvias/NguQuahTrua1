using Data.BLL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Models;

namespace Web.Admin.DirectorManagement
{
    public partial class DeleteDirector : System.Web.UI.Page
    {
        private DirectorBLL directorBLL;
        protected DirectorInfo directorInfo;
        protected bool enableShowInfo;
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                directorBLL = new DirectorBLL(DataAccessLevel.Admin);
                enableShowInfo = false;
                enableShowResult = false;
                stateString = null;
                stateDetail = null;
                hyplnkList.NavigateUrl = GetRouteUrl("Admin_DirectorList", null);
                if (!IsPostBack)
                {
                    await GetDirectorInfo();
                    directorBLL.Dispose();
                }

            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }

        private int GetDirectorId()
        {
            object obj = Page.RouteData.Values["id"];
            if (obj == null)
                return -1;
            return int.Parse(obj.ToString());
        }

        private async Task GetDirectorInfo()
        {
            int id = GetDirectorId();
            if (id <= 0)
            {
                Response.RedirectToRoute("Admin_DirectorList", null);
            }
            else
            {
                directorInfo = await directorBLL.GetDirectorAsync(id);
                if (directorInfo == null)
                {
                    enableShowInfo = false;
                    Response.RedirectToRoute("Admin_DirectorList", null);
                }
                else
                {
                    enableShowInfo = true;
                }
            }
        }

        private async Task DeleteDirectorInfo()
        {
            int id = GetDirectorId();
            StateOfDeletion state = await directorBLL.DeleteDirectorAsync(id);
            directorBLL.Dispose();
            enableShowResult = true;
            enableShowInfo = false;
            if (state == StateOfDeletion.Success)
            {
                stateString = "Success";
                stateDetail = "Đã xóa đạo diễn thành công";
            }
            else if (state == StateOfDeletion.Failed)
            {
                stateString = "Failed";
                stateDetail = "Xóa đạo diễn thất bại";
            }
            else
            {
                stateString = "ConstraintExists";
                stateDetail = "Không thể xóa đạo diễn. Lý do: Đạo diễn này đang được sử dụng!";
            }
        }

        protected async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                await DeleteDirectorInfo();
            }
            catch (Exception ex)
            {
                Session["error"] = new ErrorModel { ErrorTitle = "Ngoại lệ", ErrorDetail = ex.Message };
                Response.RedirectToRoute("Notification_Error", null);
            }
        }
    }
}