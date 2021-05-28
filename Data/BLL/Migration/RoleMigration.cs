using Data.Common.Hash;
using Data.DAL;
using MSSQL_Lite.Connection;
using MSSQL_Lite.Migration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.BLL.Migration
{
    internal class RoleMigration : SqlMigration<Role>, ISqlMigration
    {
        private bool disposed;

        public RoleMigration()
            : base()
        {
            disposed = false;
        }

        public void AddDataAndRun()
        {
            DBContext db = new DBContext(ConnectionType.ManuallyDisconnect);
            long recordNumber = db.Roles.Count();
            db.Dispose();
            if (recordNumber == 0)
            {
                List<string> IDs = new List<string>();
                int count = 0;
                while (count < 3)
                {
                    Random random = new Random();
                    string id = MD5_Hash.Hash(random.NextString(10));
                    if (IDs.Any(i => i.Equals(id)) == false)
                    {
                        IDs.Add(id);
                        count++;
                    }
                    random = null;
                }

                AddItem(new Role
                {
                    ID = IDs[0],
                    name = "Admin",
                    createAt = DateTime.Now,
                    updateAt = DateTime.Now
                });
                AddItem(new Role
                {
                    ID = IDs[1],
                    name = "Editor",
                    createAt = DateTime.Now,
                    updateAt = DateTime.Now
                });
                AddItem(new Role
                {
                    ID = IDs[2],
                    name = "User",
                    createAt = DateTime.Now,
                    updateAt = DateTime.Now
                });
                Run();
            }
        }

        public async Task AddDataAndRunAsync()
        {
            DBContext db = new DBContext(ConnectionType.ManuallyDisconnect);
            long recordNumber = await db.Roles.CountAsync();
            db.Dispose();
            if (recordNumber == 0)
            {
                List<string> IDs = new List<string>();
                int count = 0;
                while (count < 3)
                {
                    Random random = new Random();
                    string id = MD5_Hash.Hash(random.NextString(10));
                    if (IDs.Any(i => i.Equals(id)) == false)
                    {
                        IDs.Add(id);
                        count++;
                    }
                    random = null;
                }

                AddItem(new Role
                {
                    ID = IDs[0],
                    name = "Admin",
                    createAt = DateTime.Now,
                    updateAt = DateTime.Now
                });
                AddItem(new Role
                {
                    ID = IDs[1],
                    name = "Editor",
                    createAt = DateTime.Now,
                    updateAt = DateTime.Now
                });
                AddItem(new Role
                {
                    ID = IDs[2],
                    name = "User",
                    createAt = DateTime.Now,
                    updateAt = DateTime.Now
                });
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
