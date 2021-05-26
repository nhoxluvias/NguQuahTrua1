using Data.Common.Hash;
using Data.DAL;
using Data.DTO;
using MSSQL_Lite.Access;
using MSSQL_Lite.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class RoleBLL : BusinessLogicLayer
    {
        private DataAccessLevel dataAccessLevel;
        public RoleBLL(DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL();
            this.dataAccessLevel = dataAccessLevel;
        }

        public RoleBLL(BusinessLogicLayer bll, DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL(bll.db);
            this.dataAccessLevel = dataAccessLevel;
        }

        private RoleInfo ToRoleInfo(Role role)
        {
            if (role == null)
                return null;
            return new RoleInfo
            {
                ID = role.ID,
                name = role.name,
                createAt = role.createAt,
                updateAt = role.updateAt
            };
        }

        private Role ToRole(RoleCreation roleCreation)
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

        private Role ToRole(RoleUpdate roleUpdate)
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

        public async Task<List<RoleInfo>> GetRolesAsync()
        {
            List<RoleInfo> roles = null;
            if(dataAccessLevel == DataAccessLevel.Admin)
                roles = (await db.Roles.ToListAsync())
                    .Select(c => ToRoleInfo(c)).ToList();
            else
                roles = (await db.Roles.ToListAsync(c => new { c.name }))
                    .Select(c => ToRoleInfo(c)).ToList();
            return roles;
        }

        public async Task<RoleInfo> GetRoleAsync(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
                throw new Exception("");
            Role role = null;
            if(dataAccessLevel == DataAccessLevel.Admin)
                role = (await db.Roles.SingleOrDefaultAsync(c => c.ID == roleId));
            else
                role = (await db.Roles.SingleOrDefaultAsync(c => new { c.name }, c => c.ID == roleId));

            return ToRoleInfo(role);
        }

        public PagedList<RoleInfo> GetRoles(int pageIndex, int pageSize)
        {
            SqlPagedList<Role> pagedList = null;
            Expression<Func<Role, object>> orderBy = c => new { c.ID };
            if (dataAccessLevel == DataAccessLevel.Admin)
                pagedList = db.Roles.ToPagedList(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = db.Roles.ToPagedList(
                    c => new { c.name },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<RoleInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToRoleInfo(c)).ToList()
            };
        }

        public async Task<PagedList<RoleInfo>> GetRolesAsync(int pageIndex, int pageSize)
        {
            SqlPagedList<Role> pagedList = null;
            Expression<Func<Role, object>> orderBy = c => new { c.ID };
            if (dataAccessLevel == DataAccessLevel.Admin)
                pagedList = await db.Roles.ToPagedListAsync(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = await db.Roles.ToPagedListAsync(
                    c => new { c.name },
                    orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize
                );

            return new PagedList<RoleInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToRoleInfo(c)).ToList()
            };
        }

        public async Task<StateOfCreation> CreateRoleAsync(RoleCreation roleCreation)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            Role role = ToRole(roleCreation);
            if (role.name == null)
                throw new Exception("");

            int checkExists = (int)await db.Roles.CountAsync(r => r.name == role.name);
            if (checkExists != 0)
                return StateOfCreation.AlreadyExists;

            Random random = new Random();
            role.ID = MD5_Hash.Hash(random.NextString(10));
            random = null;
            int affected = await db.Roles.InsertAsync(role);
            return (affected == 0) ? StateOfCreation.Failed : StateOfCreation.Success;
        }

        public async Task<StateOfUpdate> UpdateRoleAsync(RoleUpdate roleUpdate)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            Role role = ToRole(roleUpdate);
            if (role.name == null)
                throw new Exception("");

            int affected = await db.Roles
                .UpdateAsync(role, r => new { r.name, r.updateAt }, r => r.ID == role.ID);

            return (affected == 0) ? StateOfUpdate.Failed : StateOfUpdate.Success;
        }

        public async Task<StateOfDeletion> DeleteRoleAsync(string roleId)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            if (string.IsNullOrEmpty(roleId))
                throw new Exception("");

            long userNumberOfRoleId = await db.Users.CountAsync(r => r.roleId == roleId);
            if (userNumberOfRoleId > 0)
                return StateOfDeletion.ConstraintExists;

            int affected = await db.Roles.DeleteAsync(r => r.ID == roleId);
            return (affected == 0) ? StateOfDeletion.Failed : StateOfDeletion.Success;
        }

        public async Task<int> CountAllAsync()
        {
            return (int)await db.Roles.CountAsync();
        }
    }
}
