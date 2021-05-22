using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class PagedList<T>
    {
        public long PageNumber { get; set; }
        public long CurrentPage { get; set; }
        public List<T> Items { get; set; }
    }
}
