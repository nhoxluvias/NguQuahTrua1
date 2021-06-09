using Data.Common.Hash;
using Data.DAL;
using Data.DTO;
using MSSQL_Lite.Access;
using MSSQL_Lite.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                return null;
            return new UserInfo
            {
                ID = user.ID,
                userName = user.userName,
                surName = user.surName,
                middleName = user.middleName,
                name = user.name,
                description = user.description,
                phoneNumber = user.phoneNumber,
                email = user.email,
                active = user.active,
                Role = ((user.roleId == null ) ? null : new RoleBLL(this, dataAccessLevel).GetRole(user.roleId)),
                createAt = user.createAt,
                updateAt = user.updateAt
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

        public async Task<List<UserInfo>> GetUsersAsync()
        {
            List<UserInfo> users = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                users = (await db.Users.ToListAsync())
                    .Select(u => ToUserInfo(u)).ToList();
            else
                users = (await db.Users.ToListAsync(u => new {
                    u.ID,
                    u.userName,
                    u.surName,
                    u.middleName,
                    u.name,
                    u.description,
                    u.email,
                    u.phoneNumber,
                    u.roleId
                })).Select(u => ToUserInfo(u)).ToList();
            return users;
        }

        public List<UserInfo> GetUsers()
        {
            List<UserInfo> users = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                users = db.Users.ToList()
                    .Select(u => ToUserInfo(u)).ToList();
            else
                users = db.Users.ToList(u => new {
                    u.ID,
                    u.userName,
                    u.surName,
                    u.middleName,
                    u.name,
                    u.description,
                    u.email,
                    u.phoneNumber,
                    u.roleId
                }).Select(u => ToUserInfo(u)).ToList();
            return users;
        }

        public async Task<PagedList<UserInfo>> GetUsersAsync(int pageIndex, int pageSize)
        {
            SqlPagedList<User> pagedList = null;
            Expression<Func<User, object>> orderBy = u => new { u.ID };
            if (dataAccessLevel == DataAccessLevel.Admin)
                pagedList = await db.Users.ToPagedListAsync(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = await db.Users.ToPagedListAsync(
                    u => new {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber,
                        u.roleId
                    },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<UserInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(u => ToUserInfo(u)).ToList()
            };
        }

        public PagedList<UserInfo> GetUsers(int pageIndex, int pageSize)
        {
            SqlPagedList<User> pagedList = null;
            Expression<Func<User, object>> orderBy = u => new { u.ID };
            if (dataAccessLevel == DataAccessLevel.Admin)
                pagedList = db.Users.ToPagedList(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = db.Users.ToPagedList(
                    u => new {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber,
                        u.roleId
                    },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<UserInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(u => ToUserInfo(u)).ToList()
            };
        }

        public async Task<UserInfo> GetUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("");
            User user = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                user = await db.Users
                     .SingleOrDefaultAsync(u => u.ID == userId);
            else
                user = await db.Users
                    .SingleOrDefaultAsync(u => new {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber,
                        u.roleId
                    }, u => u.ID == userId);

            return ToUserInfo(user);
        }

        public UserInfo GetUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("");
            User user = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                user = db.Users
                     .SingleOrDefault(u => u.ID == userId);
            else
                user = db.Users
                    .SingleOrDefault(u => new {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber,
                        u.roleId
                    }, u => u.ID == userId);

            return ToUserInfo(user);
        }

        public async Task<UserInfo> GetUserByUserNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new Exception("");
            User user = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                user = await db.Users
                     .SingleOrDefaultAsync(u => u.userName == userName);
            else
                user = await db.Users
                    .SingleOrDefaultAsync(u => new {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber,
                        u.roleId
                    }, u => u.userName == userName);

            return ToUserInfo(user);
        }

        public async Task<UserInfo> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new Exception("");
            User user = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                user = await db.Users
                     .SingleOrDefaultAsync(u => u.email == email);
            else
                user = await db.Users
                    .SingleOrDefaultAsync(u => new {
                        u.ID,
                        u.userName,
                        u.surName,
                        u.middleName,
                        u.name,
                        u.description,
                        u.email,
                        u.phoneNumber,
                        u.roleId
                    }, u => u.email == email);

            return ToUserInfo(user);
        }

        public enum LoginState { Success, Unconfirmed, WrongPassword, NotExists };

        public async Task<LoginState> LoginAsync(UserLogin userLogin)
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

        public async Task<bool> IsActiveAsync(string username)
        {
            return (await db.Users
                .SingleOrDefaultAsync(u => new { u.active }, u => u.userName == username)).active;
        }

        public async Task<bool> ActiveUserAsync(string userId)
        {
            int affected = await db.Users.UpdateAsync(new User { active = true }, u => new { u.active }, u => u.ID == userId);
            return (affected != 0);
        } 

        public enum RegisterState { Success, Success_NoPaymentInfo, Failed, AlreadyExist };

        public async Task<RegisterState> RegisterAsync(UserCreation userCreation)
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

            User usr = await db.Users
                .SingleOrDefaultAsync(u => u.userName == userCreation.userName || u.email == userCreation.email || u.phoneNumber == userCreation.phoneNumber);
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

            usr = await db.Users.SingleOrDefaultAsync(u => new { u.ID }, u => u.userName == userCreation.userName);
            userCreation.PaymentInfo.userId = usr.ID;
            StateOfCreation state = await new PaymentInfoBLL(this, dataAccessLevel).CreatePaymentInfoAsync(userCreation.PaymentInfo);
            if (state == StateOfCreation.AlreadyExists || state == StateOfCreation.Failed)
                return RegisterState.Success_NoPaymentInfo;

            return RegisterState.Success;
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                try
                {
                    if (disposing)
                    {

                    }
                    this.disposed = true;
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
    }
}
