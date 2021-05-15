using Data.DAL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class RoleBLLForAdmin : RoleBLL
    {
        public RoleBLLForAdmin()
            : base()
        {

        }

        public RoleBLLForAdmin(BusinessLogicLayer bll)
            : base(bll)
        {

        }

        public async Task<List<RoleInfoForAdmin>> GetRolesAsync()
        {
            return (await db.Roles.ToListAsync())
                .Select(c => ToRoleInfoForAdmin(c)).ToList();
        }

        public async Task<RoleInfoForAdmin> GetRoleAsync(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
                throw new Exception("");
            Role role = (await db.Roles.SingleOrDefaultAsync(c => c.ID == roleId));
            return ToRoleInfoForAdmin(role);
        }

        public async Task<bool> CreateRoleAsync(RoleCreation roleCreation)
        {
            Role role = ToRole(roleCreation);
            if (role.name == null)
                throw new Exception("");

            int affected = await db.Roles.InsertAsync(role);

            if (affected == 0)
                return false;
            return true;
        }

        public async Task<bool> UpdateRoleAsync(RoleUpdate roleUpdate)
        {
            Role role = ToRole(roleUpdate);
            if (role.name == null)
                throw new Exception("");

            int affected = await db.Roles
                .UpdateAsync(role, r => new { r.name, r.updateAt }, r => r.ID == role.ID);

            if (affected == 0)
                return false;
            return true;
        }

        public async Task<bool> DeleteAsync(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
                throw new Exception("");
            long userNumberOfRoleId = await db.Users.CountAsync(r => r.roleId == roleId);
            if (userNumberOfRoleId > 0)
                return false;
            int affected = await db.Roles.DeleteAsync(r => r.ID == roleId);
            if (affected == 0)
                return false;
            return true;
        }
    }
}
