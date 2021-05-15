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
    public class DirectorBLLForUser : DirectorBLL
    {
        public DirectorBLLForUser()
            : base()
        {

        }

        public DirectorBLLForUser(BusinessLogicLayer bll)
            : base()
        {

        }

        public async Task<List<DirectorInfoForUser>> GetDirectorsAsync()
        {
            return (await db.Directors.ToListAsync(d => new { d.ID, d.name, d.description }))
                .Select(d => ToDirectorInfoForUser(d)).ToList();
        }

        public async Task<DirectorInfoForUser> GetDirectorAsync(long directorId)
        {
            if (directorId <= 0)
                throw new Exception("");
            Director director = await db.Directors
                .SingleOrDefaultAsync(d => new { d.ID, d.name, d.description }, d => d.ID == directorId);
            return ToDirectorInfoForUser(director);
        }

        public async Task<List<DirectorInfoForAdmin>> GetDirectorsByFilmIdAsync(string filmId)
        {
            if (string.IsNullOrEmpty(filmId))
                throw new Exception("");
            string query = @"Select [Director].[ID], [Director].[name], [Director].[descriptionn] 
                            from [Director], [DirectorOfFilm] 
                            where [Director].[ID] = [DirectorOfFilm].[directorId]
                                and [Director].[filmId] = @filmId";
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = query;
            sqlCommand.Parameters.Add(new SqlParameter("@filmId", filmId));
            return await db.ExecuteReaderAsync<List<DirectorInfoForAdmin>>(sqlCommand);
        }
    }
}
