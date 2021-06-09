using Data.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Validation;

namespace Web.Account
{
    public partial class NewPassword : System.Web.UI.Page
    {
        private UserBLL userBLL;
        private CustomValidation customValidation;

        protected void Page_Load(object sender, EventArgs e)
        {
            customValidation = new CustomValidation();
            InitValidation();
            if (IsPostBack)
            {
                userBLL = new UserBLL(DataAccessLevel.User);

                userBLL.Dispose();
            }
        }

        private void InitValidation()
        {
            customValidation.Init(
                cvPassword,
                "txtNewPassword",
                "Tối thiểu 6 ký tự, tối đa 20 ký tự",
                true,
                null,
                customValidation.ValidatePassword
            );
        }

        private void ValidateData()
        {
            cvPassword.Validate();
        }

        private bool IsValidData()
        {
            ValidateData();
            return cvPassword.IsValid;
        }

        private async Task CreateNewPassword()
        {
            
        }

    }
}