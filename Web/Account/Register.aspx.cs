using Common.Hash;
using Common.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Common;
using Web.Models;
using Web.Validation;

namespace Web.Account
{
    public partial class Register : System.Web.UI.Page
    {
        private DBContext db;
        protected async void Page_Load(object sender, EventArgs e)
        {
            InitValidation();
            if (CheckLoggedIn())
            {
                Response.RedirectToRoute("Home");
            }
            else
            {
                db = new DBContext();
                if (IsPostBack)
                {
                    await RegisterAccount();
                }
            }
        }

        private bool CheckLoggedIn()
        {
            return (Session["username"] != null);
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
            if (
                cvUsername.IsValid && cvEmail.IsValid && cvPhoneNumber.IsValid && cvPassword.IsValid
                && cmpRePassword.IsValid && cvCardNumber.IsValid && cvCvv.IsValid && cvAccountName.IsValid
                && cvExpirationDate.IsValid
            )
            {
                return true;
            }
            return false;
        }

        private async Task<Models.User> GetUserModel()
        {
            string username = Request.Form["txtUsername"];
            string email = Request.Form["txtEmail"];
            string phoneNumber = Request.Form["txtPhoneNumber"];
            string password = Request.Form["txtPassword"];

            string salt = MD5_Hash.Hash(new Random().NextString(25));

            Role role = await db.Roles.SingleOrDefaultAsync(r => new { r.ID }, r => r.name == "User");
            if (role == null)
            {
                return null;
            }
            else
            {
                Models.User user = new Models.User
                {
                    ID = Guid.NewGuid().ToString(),
                    userName = username,
                    email = email,
                    phoneNumber = phoneNumber,
                    password = PBKDF2_Hash.Hash(password, salt, 30),
                    salt = salt,
                    roleId = role.ID,
                    active = false,
                    createAt = DateTime.Now,
                    updateAt = DateTime.Now
                };
                return user;
            }
        }

        private async Task RegisterAccount()
        {
            ValidateData();
            if (IsValidData())
            {
                Models.User user = await GetUserModel();
                if (user == null)
                {
                    Response.RedirectToRoute("error");
                }
                else
                {
                    Models.User usr = await db.Users.SingleOrDefaultAsync(u => u.userName == user.userName);
                    if(usr != null)
                    {
                        Response.RedirectToRoute("Register_WithParam", new { 
                            registerStatus = "register-failed_already-exist" 
                        });
                    }
                    else
                    {
                        int affected = await db
                        .Users.InsertAsync(user, new List<string> { "surName", "middleName", "name", "description" });

                        if (affected == 0)
                        {
                            Response.RedirectToRoute("Register_WithParam", new { 
                                registerStatus = "register-failed" 
                            });
                        }
                        else
                        {
                            ConfirmCode confirmCode = new ConfirmCode();
                            Session["confirmCode"] = confirmCode.Send(user.email);
                            string confirmToken = confirmCode.CreateToken();
                            Session["confirmToken"] = confirmToken;
                            bool status = await AddPaymentInfo(user.ID);
                            if (status)
                                Response.RedirectToRoute("Confirm", new { 
                                    userId = user.ID,
                                    confirmToken = confirmToken,
                                    status = "register-success" 
                                });
                            else
                                Response.RedirectToRoute("Confirm", new {
                                    userId = user.ID, 
                                    confirmToken = confirmToken,
                                    status = "register-success_no-payment-info"
                                });
                        }
                    }
                }
            }
        }

        private async Task<PaymentInfo> GetPaymentInfoModel()
        {
            string cardNumber = Request.Form["txtCardNumber"];
            string cvv = Request.Form["txtCvv"];
            string accountName = Request.Form["txtAccountName"];
            string expirationDate = Request.Form["txtExpirationDate"];
            string paymentMethod = Request.Form["drdlPaymentMethod"];
            if (
                (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(cvv) || string.IsNullOrEmpty(accountName)
                || string.IsNullOrEmpty(expirationDate) || string.IsNullOrEmpty(paymentMethod)) == false
            )
            {
                Models.PaymentMethod paymntMethod = await db
                    .PaymentMethods.SingleOrDefaultAsync(p => new { p.ID }, p => p.name == paymentMethod);
                if (paymntMethod == null)
                {
                    return null;
                }
                else
                {
                    Models.PaymentInfo paymentInfo = new Models.PaymentInfo
                    {
                        cardNumber = cardNumber,
                        cvv = cvv,
                        owner = accountName,
                        expirationDate = expirationDate,
                        paymentMethodId = paymntMethod.ID,
                        createAt = DateTime.Now,
                        updateAt = DateTime.Now
                    };
                    return paymentInfo;
                }
            }
            return null;
        }

        public async Task<bool> AddPaymentInfo(string userId)
        {
            Models.PaymentInfo paymentInfo = await GetPaymentInfoModel();
            if(paymentInfo == null)
            {
                return false;
            }
            else
            {
                paymentInfo.userId = userId;
                int affected = await db.PaymentInfos.InsertAsync(paymentInfo);
                if (affected == 0)
                    return false;
                return true;
            }
        }
    }
}