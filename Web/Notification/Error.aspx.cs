using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Models;

namespace Web.Notification
{
    public partial class Error : System.Web.UI.Page
    {
        protected ErrorModel error;
        protected void Page_Load(object sender, EventArgs e)
        {
            GetError();
        }

        private void GetError()
        {
            error = Session["error"] as ErrorModel;
            if (error == null)
                error = new ErrorModel { ErrorTitle = "Lỗi!", ErrorDetail = "Lỗi không xác định" };
        }
    }
}