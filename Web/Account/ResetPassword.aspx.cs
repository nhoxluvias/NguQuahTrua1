using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Validation;

namespace Web.Account
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitValidation();
        }

        private void InitValidation()
        {
            CustomValidation
                .Init(cvEmail, "txtEmail", "Địa chỉ Email không hợp lệ", true, null, CustomValidation.ValidateEmail);
            CustomValidation
                .Init(cvConfirmCode, "txtConfirmCode", "Không được để trống, từ 6 đến 20 ký tự số", true, null, CustomValidation.ValidateConfirmCode);
        }
    }
}