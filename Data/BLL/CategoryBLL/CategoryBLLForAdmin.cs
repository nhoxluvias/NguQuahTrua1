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
    public class CategoryBLLForAdmin : CategoryBLL
    {
        public CategoryBLLForAdmin()
            : base()
        {

        }

        public CategoryBLLForAdmin(BusinessLogicLayer bll)
            : base(bll)
        {

        }

        public async Task<List<CategoryInfoForAdmin>> GetCategoriesAsync()
        {
            List<CategoryInfoForAdmin> categories = (await db.Categories.ToListAsync())
                .Select(c => ToCategoryInfoForAdmin(c)).ToList();
            return categories;
        }

        public async Task<CategoryInfoForAdmin> GetCategoryAsync(int categoryId)
        {
            if (categoryId <= 0)
                throw new Exception("");
            Category category = await db.Categories
                .SingleOrDefaultAsync(c => c.ID == categoryId);
            return ToCategoryInfoForAdmin(category);
        }

        public async Task<List<CategoryInfoForAdmin>> GetCategoriesByFilmIdAsync(string filmId)
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
            return await db.ExecuteReaderAsync<List<CategoryInfoForAdmin>>(sqlCommandOfSubQuery);
        }

        public async Task<bool> CreateCategoryAsync(CategoryCreation categoryCreation)
        {
            Category category = ToCategory(categoryCreation);
            if (category.name == null)
                throw new Exception("");

            int affected;
            if (category.description == null)
                affected = await db.Categories.InsertAsync(category, new List<string> { "ID", "description" });
            else
                affected = await db.Categories.InsertAsync(category, new List<string> { "ID" });

            if (affected == 0)
                return false;
            return true;
        }

        public async Task<bool> UpdateCategoryAsync(CategoryUpdate categoryUpdate)
        {
            Category category = ToCategory(categoryUpdate);
            if (category.name == null)
                throw new Exception("");

            int affected;
            if (category.description == null)
                affected = await db.Categories.UpdateAsync(
                    category,
                    c => new { c.name, c.updateAt },
                    c => c.ID == category.ID
                );
            else
                affected = await db.Categories.UpdateAsync(
                    category,
                    c => new { c.name, c.description, c.updateAt },
                    c => c.ID == category.ID
                );

            if (affected == 0)
                return false;
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            if (categoryId <= 0)
                throw new Exception("");

            long categoryDistributionNumber = await db.CategoryDistributons
                .CountAsync(cd => cd.categoryId == categoryId);
            if (categoryDistributionNumber > 0)
                return true;
            int affected = await db.Categories.DeleteAsync(c => c.ID == categoryId);
            if (affected == 0)
                return false;
            return true;
        }
    }
}
