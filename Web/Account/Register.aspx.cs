using Common.Hash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Models;
using Web.Validation;

namespace Web.Account
{
    public partial class Register : System.Web.UI.Page
    {
        private DBContext db;
        protected async void Page_Load(object sender, EventArgs e)
        {
            db = new DBContext();
            if (CheckLoggedIn())
                Response.RedirectToRoute("Home");
            InitValidation();
            if (IsPostBack)
            {
                await RegisterAccount();
            }
        }

        private bool CheckLoggedIn()
        {
            return (Session["username"] == null) ? false : true;
        }

        private void InitValidation()
        {
            CustomValidation
                .Init(cvUsername, "txtUsername", "Không được trống, chỉ chứa a-z, 0-9, _ và -", true, null, CustomValidation.ValidateUsername);
            CustomValidation
                .Init(cvEmail, "txtEmail", "Không được để trống và phải hợp lệ", true, null, CustomValidation.ValidateEmail);
            CustomValidation
                .Init(cvPhoneNumber, "txtPhoneNumber", "Số điện thoại không hợp lệ", false, null, CustomValidation.ValidatePhoneNumber);
            CustomValidation
                .Init(cvPassword, "txtPassword", "Tối thiểu 6 ký tự, tối đa 20 ký tự", true, null, CustomValidation.ValidatePassword);
            cmpRePassword.ControlToValidate = "txtPassword";
            cmpRePassword.ControlToCompare = "txtRePassword";
            cmpRePassword.ErrorMessage = "Không khớp với mật khẩu mà bạn đã nhập";
            CustomValidation
                .Init(cvCardNumber, "txtCardNumber", "Số thẻ không hợp lệ", false, null, CustomValidation.ValidateCardNumber);
            CustomValidation
                .Init(cvCvv, "txtCvv", "Số CVV không hợp lệ", false, null, CustomValidation.ValidateCVV);
            CustomValidation
                .Init(cvAccountName, "txtAccountName", "Tên chủ tài khoản không hợp lệ", false, null, CustomValidation.ValidateAccountName);
            CustomValidation
                .Init(cvExpirationDate, "txtExpirationDate", "Ngày hết hạn không hợp lệ", false, null, CustomValidation.ValidateExpirationDate);
        }

        private void ValidateData()
        {
            cvUsername.Validate();
            cvEmail.Validate();
            cvPhoneNumber.Validate();
            cvPassword.Validate();
            cmpRePassword.Validate();
            cvCardNumber.Validate();
            cvCvv.Validate();
            cvAccountName.Validate();
            cvExpirationDate.Validate();
        }

        private bool IsValidData()
        {
            if(
                cvUsername.IsValid && cvEmail.IsValid && cvPhoneNumber.IsValid && cvPassword.IsValid
                && cmpRePassword.IsValid && cvCardNumber.IsValid && cvCvv.IsValid && cvAccountName.IsValid
                && cvExpirationDate.IsValid
            )
            {
                return true;
            }
            return false;
        }

        private async Task RegisterAccount()
        {
            ValidateData();
            if(IsValidData())
            {
                string username = Request.Form["txtUsername"];
                string email = Request.Form["txtEmail"];
                string phoneNumber = Request.Form["txtPhoneNumber"];
                string password = Request.Form["txtPassword"];
                string cardNumber = Request.Form["txtCardNumber"];
                string cvv = Request.Form["txtCvv"];
                string accountName = Request.Form["txtAccountName"];
                string expirationDate = Request.Form["txtExpirationDate"];
                string paymentMethod = Request.Form["drdlPaymentMethod"];

                string salt = MD5_Hash.Hash(new Random().NextString(10));

                Models.User user = new Models.User
                {
                    ID = MD5_Hash.Hash(new Random().NextString(10)),
                    userName = username,
                    email = email,
                    phoneNumber = phoneNumber,
                    password = PBKDF2_Hash.Hash(password, salt, 10),
                    salt = salt,
                    createAt = DateTime.Now,
                    updateAt = DateTime.Now
                };

                Role role = await db.Roles.SingleOrDefaultAsync(r => new { r.ID }, r => r.name == "User");
                if (role == null)
                {
                    Response.RedirectToRoute("error");
                }
                else
                {
                    user.roleId = role.ID;
                    int affected = await db
                        .Users.InsertAsync(user, new List<string> { "surName", "middleName", "name", "description" });
                    if(affected == 0)
                    {
                        Response.RedirectToRoute("Register_WithParam", new { registerStatus = "failed" });
                    }
                    else
                    {
                        Response.RedirectToRoute("Register_WithParam", new { registerStatus = "success" });
                    }
                }
                    
            }
            
        }
    }
}