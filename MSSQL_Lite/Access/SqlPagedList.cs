using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL_Lite.Access
{
    public class SqlPagedList<T>
    {
        private int skip;
        private int take;
        private long totalRecord;
        private List<T> items;
        private int pageNumber;
        private int currentPage;

        public int Take { get { return take; } }
        public int Skip { get { return skip; } }
        public long TotalRecord { get { return totalRecord; } }
        public int PageNumber { get { return pageNumber; } }
        public int CurrentPage { get { return currentPage; } }
        public List<T> Items { get { return items; } set { items = value; } }

        public SqlPagedList()
        {
            skip = 0;
            take = 0;
            totalRecord = 0;
            pageNumber = 0;
            currentPage = 0;
            items = null;
        }

        public void Solve(long totalRecord, int pageIndex, int pageSize)
        {
            this.totalRecord = totalRecord;
            take = pageSize;
            int index = 0;
            skip = 0;
            for (int i = 0; i < totalRecord; i = i + pageSize)
            {
                if (pageIndex == index)
                {
                    skip = i;
                    currentPage = index;
                } 
                index++;
            }
            pageNumber = index;
        }
    }
}
