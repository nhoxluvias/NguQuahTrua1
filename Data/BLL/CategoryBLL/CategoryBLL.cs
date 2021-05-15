using Data.DAL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class CategoryBLL : BusinessLogicLayer
    {
        protected CategoryBLL()
            : base()
        {
            InitDAL();
        }

        protected CategoryBLL(BusinessLogicLayer bll)
            : base()
        {
            InitDAL(bll.db);
        }

        internal CategoryInfoForAdmin ToCategoryInfoForAdmin(Category category)
        {
            if (category == null)
                throw new Exception("");
            return new CategoryInfoForAdmin
            {
                ID = category.ID,
                name = category.name,
                description = category.description,
                createAt = category.createAt,
                updateAt = category.updateAt
            };
        }

        internal CategoryInfoForUser ToCategoryInfoForUser(Category category)
        {
            if (category == null)
                throw new Exception("");
            return new CategoryInfoForUser
            {
                ID = category.ID,
                name = category.name,
                description = category.description
            };
        }

        internal Category ToCategory(CategoryUpdate categoryUpdate)
        {
            if (categoryUpdate == null)
                throw new Exception("");
            return new Category
            {
                ID = categoryUpdate.ID,
                name = categoryUpdate.name,
                description = categoryUpdate.description,
                updateAt = DateTime.Now
            };
        }

        internal Category ToCategory(CategoryCreation categoryCreation)
        {
            if (categoryCreation == null)
                throw new Exception("");
            return new Category
            {
                name = categoryCreation.name,
                description = categoryCreation.description,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };
        }

        public async Task<int> CountAllAsync()
        {
            return (int)await db.Categories.CountAsync();
        }
    }
}
