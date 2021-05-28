using Data.Common.Hash;
using Data.DAL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class UserBLL : BusinessLogicLayer
    {
        private DataAccessLevel dataAccessLevel;
        private bool disposed;
        public UserBLL(DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL();
            this.dataAccessLevel = dataAccessLevel;
            disposed = false;
        }

        public UserBLL(BusinessLogicLayer bll, DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL(bll.db);
            this.dataAccessLevel = dataAccessLevel;
            disposed = false;
        }

        private UserInfo ToUserInfo(User user)
        {
            if (user == null)
                throw new Exception("");
            return new UserInfo
            {

            };
        }

        private async Task<User> ToUser(UserCreation userCreation)
        {
            if (userCreation == null)
                throw new Exception("");

            string salt = MD5_Hash.Hash(new Random().NextString(25));
            Role role = await db.Roles.SingleOrDefaultAsync(r => new { r.ID }, r => r.name == "User");
            if (role == null)
                throw new Exception("");

            return new User
            {
                ID = Guid.NewGuid().ToString(),
                userName = userCreation.userName,
                email = userCreation.email,
                phoneNumber = userCreation.phoneNumber,
                password = PBKDF2_Hash.Hash(userCreation.password, salt, 30),
                salt = salt,
                roleId = role.ID,
                active = false,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };
        }

        private User ToUser(UserUpdate userUpdate)
        {
            if (userUpdate == null)
                throw new Exception("");
            return new User
            {

            };
        }

        public enum LoginState { Success, Unconfirmed, WrongPassword, NotExists };

        public async Task<LoginState> Login(UserLogin userLogin)
        {
            if (userLogin == null)
                throw new Exception("");
            if (userLogin.userName == null || userLogin.password == null)
                throw new Exception("");

            User user = await db.Users.SingleOrDefaultAsync(
                    u => new { u.userName, u.password, u.salt, u.active }, 
                    u => u.userName == userLogin.userName
                );
            if (user == null)
                return LoginState.NotExists;

            string passwordHashed = PBKDF2_Hash.Hash(userLogin.password, user.salt, 30);
            if (user.password != passwordHashed)
                return LoginState.WrongPassword;
            if (!user.active)
                return LoginState.Unconfirmed;
            return LoginState.Success;
        }

        public async Task<bool> IsActive(string username)
        {
            return (await db.Users
                .SingleOrDefaultAsync(u => new { u.active }, u => u.userName == username)).active;
        }

        public enum RegisterState { Success, Success_NoPaymentInfo, Failed, AlreadyExist };

        public async Task<RegisterState> Register(UserCreation userCreation)
        {
            if (userCreation == null)
                throw new Exception("");
            if (
                userCreation.userName == null || userCreation.password == null
                ||userCreation.email == null || userCreation.phoneNumber == null
            )
            {
                throw new Exception("");
            }

            User usr = await db.Users.SingleOrDefaultAsync(u => u.userName == userCreation.userName);
            if (usr != null)
                return RegisterState.AlreadyExist;

            User userRegister = await ToUser(userCreation);
            int affected = await db.Users.InsertAsync(
                userRegister,
                new List<string> { "surName", "middleName", "name", "description" }
            );
            if (affected == 0)
                return RegisterState.Failed;
            if (userCreation.PaymentInfo == null)
                return RegisterState.Success_NoPaymentInfo;

            StateOfCreation state = await new PaymentInfoBLL(this, dataAccessLevel).CreatePaymentInfoAsync(userCreation.PaymentInfo);
            if (state == StateOfCreation.AlreadyExists || state == StateOfCreation.Failed)
                return RegisterState.Success_NoPaymentInfo;

            return RegisterState.Success;
        }
    }
}
