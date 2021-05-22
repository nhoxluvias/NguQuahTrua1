using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class PagedList<T>
    {
        public int PageNumber { get; set; }
        public int CurrentPage { get; set; }
        public List<T> Items { get; set; }
    }
}
