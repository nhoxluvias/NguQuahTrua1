using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Admin.FilmManagement
{
    public partial class EditImage : System.Web.UI.Page
    {
        protected bool enableShowResult;
        protected string stateString;
        protected string stateDetail;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitValidation();
        }

        private void InitValidation()
        {
            rfvFilmThumbnail.ControlToValidate = "fuFilmThumbnail";
            rfvFilmThumbnail.ErrorMessage = "Bắt buộc phải có ảnh thu nhỏ";
        }

        private Common.Upload.FileUpload.UploadState GetFilmThumbnail(ref string filePath)
        {
            HttpPostedFile httpPostedFile = Request.Files[fuFilmThumbnail.UniqueID];
            Common.Upload.FileUpload fileUpload = new Common.Upload.FileUpload();
            return fileUpload.UploadImage(httpPostedFile, ref filePath);
        }
    }
}