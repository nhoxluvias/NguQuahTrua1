using Data.Common.Hash;
using Data.DAL;
using MSSQL_Lite.Connection;
using MSSQL_Lite.Migration;
using System;
using System.Threading.Tasks;

namespace Data.BLL.Migration
{
    internal class UserMigration : SqlMigration<User>, ISqlMigration
    {
        private bool disposed;

        public UserMigration()
            : base()
        {
            disposed = false;
        }

        public void AddDataAndRun()
        {
            DBContext db = new DBContext(ConnectionType.ManuallyDisconnect);
            long recordNumber = db.Users.Count();
            if(recordNumber == 0)
            {
                MD5_Hash md5 = new MD5_Hash();
                PBKDF2_Hash pbkdf2 = new PBKDF2_Hash();
                string salt = md5.Hash(new Random().NextString(25));
                Role role = db.Roles.SingleOrDefault(r => new { r.ID }, r => r.name == "Admin");
                db.Dispose();
                AddItem(new User
                {
                    ID = Guid.NewGuid().ToString(),
                    userName = "systemadmin",
                    surName = "System",
                    middleName = "",
                    name = "Admin",
                    description = "Tài khoản quản trị cấp cao",
                    email = "systemadmin@admin.com",
                    phoneNumber = "00000000",
                    password = pbkdf2.Hash("admin12341234", salt, 30),
                    salt = salt,
                    roleId = role.ID,
                    active = true,
                    createAt = DateTime.Now,
                    updateAt = DateTime.Now
                });
                md5 = null;
                pbkdf2 = null;
                Run();
            }
        }

        public async Task AddDataAndRunAsync()
        {
            DBContext db = new DBContext(ConnectionType.ManuallyDisconnect);
            long recordNumber = await db.Users.CountAsync();
            if (recordNumber == 0)
            {
                MD5_Hash md5 = new MD5_Hash();
                PBKDF2_Hash pbkdf2 = new PBKDF2_Hash();
                string salt = md5.Hash(new Random().NextString(25));
                Role role = db.Roles.SingleOrDefault(r => new { r.ID }, r => r.name == "Admin");
                db.Dispose();
                AddItem(new User
                {
                    ID = Guid.NewGuid().ToString(),
                    userName = "systemadmin",
                    surName = "System",
                    middleName = "",
                    name = "Admin",
                    description = "Tài khoản quản trị cấp cao",
                    email = "systemadmin@admin.com",
                    phoneNumber = "00000000",
                    password = pbkdf2.Hash("admin12341234", salt, 30),
                    salt = salt,
                    roleId = role.ID,
                    active = true,
                    createAt = DateTime.Now,
                    updateAt = DateTime.Now
                });
                md5 = null;
                pbkdf2 = null;
                await RunAsync();
            }
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
