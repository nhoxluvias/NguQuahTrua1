using Data.DAL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class FilmBLLForAdmin : FilmBLL
    {
        public FilmBLLForAdmin()
            : base()
        {

        }

        public FilmBLLForAdmin(BusinessLogicLayer bll)
            : base(bll)
        {

        }



        public async Task<List<FilmInfoForAdmin>> GetFilmsAsync()
        {
            return (await db.Films.ToListAsync())
                .Select(f => new FilmInfoForAdmin
                {

                }).ToList();

        }
    }
}
