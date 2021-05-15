using Data.DAL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class CategoryBLLForUser : CategoryBLL
    {
        public CategoryBLLForUser()
            : base()
        {

        }

        public CategoryBLLForUser(BusinessLogicLayer bll)
            : base(bll)
        {

        }

        public async Task<List<CategoryInfoForUser>> GetCategoriesAsync()
        {
            List<CategoryInfoForUser> categories = (
                await db.Categories.ToListAsync(c => new { c.ID, c.name, c.description })
            ).Select(c => ToCategoryInfoForUser(c)).ToList();
            return categories;
        }

        public async Task<CategoryInfoForUser> GetCategoryAsync(int categoryId)
        {
            if (categoryId <= 0)
                throw new Exception("");
            Category category = await db.Categories
                .SingleOrDefaultAsync(c => new { c.ID, c.name, c.description }, c => c.ID == categoryId);
            return ToCategoryInfoForUser(category);
        }

        public async Task<List<CategoryInfoForUser>> GetCategoriesByFilmIdAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");
            string query = @"Select [Category].[name] from [CategoryDistribution], [Category]
                                where [CategoryDistribution].[categoryID] = [Category].[ID]
                                    and [CategoryDistribution].[filmId] = @filmId";
            SqlCommand sqlCommandOfSubQuery = new SqlCommand();
            sqlCommandOfSubQuery.CommandType = CommandType.Text;
            sqlCommandOfSubQuery.CommandText = query;
            sqlCommandOfSubQuery.Parameters.Add(new SqlParameter("@filmId", filmId));
            return await db.ExecuteReaderAsync<List<CategoryInfoForUser>>(sqlCommandOfSubQuery);
        }
    }
}
