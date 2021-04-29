using Common.Hash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web.Models;

namespace Web.Account
{
    public partial class Register : System.Web.UI.Page
    {
        private DBContext db;
        protected async void Page_Load(object sender, EventArgs e)
        {
            db = new DBContext();
            if (CheckLoggedIn())
                Response.RedirectToRoute("home");
            if (IsPostBack)
            {
                await RegisterAccount();
            }
        }

        private bool CheckLoggedIn()
        {
            return (Session["username"] == null) ? false : true;
        }

        private async Task RegisterAccount()
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

            await db.Users.InsertAsync(user, new List<string> { "surName", "middleName", "name"});
        }
    }
}