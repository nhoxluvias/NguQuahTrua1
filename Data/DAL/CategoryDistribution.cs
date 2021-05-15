using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAL
{
    internal class CategoryDistribution
    {
        public int categoryId { get; set; }
        public string filmId { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }
}
