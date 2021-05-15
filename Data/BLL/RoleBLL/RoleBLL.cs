using Data.DAL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class RoleBLL : BusinessLogicLayer
    {
        protected RoleBLL()
            : base()
        {
            InitDAL();
        }

        protected RoleBLL(BusinessLogicLayer bll)
            : base()
        {
            InitDAL(bll.db);
        }

        internal RoleInfoForAdmin ToRoleInfoForAdmin(Role role)
        {
            if (role == null)
                throw new Exception("");
            return new RoleInfoForAdmin
            {
                ID = role.ID,
                name = role.name,
                createAt = role.createAt,
                updateAt = role.updateAt
            };
        }

        internal RoleInfoForUser ToRoleInfoForUser(Role role)
        {
            if (role == null)
                throw new Exception("");
            return new RoleInfoForUser
            {
                name = role.name
            };
        }

        internal Role ToRole(RoleCreation roleCreation)
        {
            if (roleCreation == null)
                throw new Exception("");
            return new Role
            {
                ID = roleCreation.ID,
                name = roleCreation.name,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };
        }

        internal Role ToRole(RoleUpdate roleUpdate)
        {
            if (roleUpdate == null)
                throw new Exception("");
            return new Role
            {
                ID = roleUpdate.ID,
                name = roleUpdate.name,
                updateAt = DateTime.Now
            };
        }

        public async Task<int> CountAllAsync()
        {
            return (int)await db.Roles.CountAsync();
        }
    }
}
