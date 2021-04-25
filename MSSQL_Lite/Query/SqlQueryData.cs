using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL_Lite.Query
{
    public class SqlQueryData
    {
        public string Statement { get; set; }
        public List<SqlQueryParameter> SqlQueryParameters { get; set; }
    }
}
