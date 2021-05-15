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
    public class DirectorBLLForAdmin : DirectorBLL
    {
        public DirectorBLLForAdmin()
            : base()
        {

        }

        public DirectorBLLForAdmin(BusinessLogicLayer bll)
            : base(bll)
        {

        }

        public async Task<List<DirectorInfoForAdmin>> GetDirectorsAsync()
        {
            return (await db.Directors.ToListAsync())
                .Select(d => ToDirectorInfoForAdmin(d)).ToList();
        }

        public async Task<DirectorInfoForAdmin> GetDirectorAsync(long directorId)
        {
            if (directorId <= 0)
                throw new Exception("");
            Director director = await db.Directors.SingleOrDefaultAsync(d => d.ID == directorId);
            return ToDirectorInfoForAdmin(director);
        }

        public async Task<List<DirectorInfoForAdmin>> GetDirectorsByFilmIdAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");
            string query = @"Select * from [Director], [DirectorOfFilm] 
                            where [Director].[ID] = [DirectorOfFilm].[directorId]
                                and [Director].[filmId] = @filmId";
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = query;
            sqlCommand.Parameters.Add(new SqlParameter("@filmId", filmId));
            return await db.ExecuteReaderAsync<List<DirectorInfoForAdmin>>(sqlCommand);
        }

        public async Task<bool> CreateDirectorAsync(DirectorCreation directorCreation)
        {
            Director director = ToDirector(directorCreation);
            if (director.name == null)
                throw new Exception("");

            int affected;
            if (director.description == null)
                affected = await db.Directors.InsertAsync(director, new List<string> { "ID", "description" });
            else
                affected = await db.Directors.InsertAsync(director, new List<string> { "ID" });

            if (affected == 0)
                return false;
            return true;
        }

        public async Task<bool> UpdateDirectorAsync(DirectorUpdate directorUpdate)
        {
            Director director = ToDirector(directorUpdate);
            if (director.name == null)
                throw new Exception("");

            int affected;
            if (director.description == null)
                affected = await db.Directors.UpdateAsync(
                    director,
                    d => new { d.name, d.updateAt },
                    d => d.ID == director.ID
                );
            else
                affected = await db.Directors.UpdateAsync(
                    director,
                    d => new { d.name, d.description, d.updateAt },
                    d => d.ID == director.ID
                );

            if (affected == 0)
                return false;
            return true;
        }

        public async Task<bool> DeleteDirectorAsync(long directorId)
        {
            if (directorId <= 0)
                throw new Exception("");
            long directorOfFilmNumber = await db.DirectorOfFilms.CountAsync(df => df.directorId == directorId);
            if (directorOfFilmNumber > 0)
                return false;
            int affected = await db.Directors.DeleteAsync(d => d.ID == directorId);
            if (affected == 0)
                return false;
            return true;
        }
    }
}
