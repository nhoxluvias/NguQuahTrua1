using Data.DAL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class RolBLLForUser : RoleBLL
    {
        public RolBLLForUser()
            : base()
        {

        }

        public RolBLLForUser(BusinessLogicLayer bll)
            : base(bll)
        {

        }

        public async Task<List<RoleInfoForUser>> GetRolesAsync()
        {
            return (await db.Roles.ToListAsync())
                .Select(c => ToRoleInfoForUser(c)).ToList();
        }

        public async Task<RoleInfoForUser> GetRoleAsync(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
                throw new Exception("");
            Role role = (await db.Roles.SingleOrDefaultAsync(c => c.ID == roleId));
            return ToRoleInfoForUser(role);
        }
    }
}
