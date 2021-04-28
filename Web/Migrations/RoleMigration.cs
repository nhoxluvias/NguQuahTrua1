using Common.Hash;
using MSSQL_Lite.Access;
using MSSQL_Lite.Migration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Web.Models;

namespace Web.Migrations
{
    public class RoleMigration : SqlMigration<Role>, ISqlMigration
    {
        public RoleMigration()
            : base()
        {

        }

        public async Task AddDataAndRunAsync()
        {
            AddItem(new Role {
                ID = MD5_Hash.Hash(new Random().NextString(10)), 
                name = "Admin", 
                createAt = DateTime.Now, 
                updateAt = DateTime.Now 
            });
            AddItem(new Role {
                ID = MD5_Hash.Hash(new Random().NextString(10)), 
                name = "Editor", 
                createAt = DateTime.Now, 
                updateAt = DateTime.Now 
            });
            AddItem(new Role { 
                ID = MD5_Hash.Hash(new Random().NextString(10)), 
                name = "User", 
                createAt = DateTime.Now, 
                updateAt = DateTime.Now 
            });
            
            await Run();
        }
    }
}