using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL_Lite.Query
{
    public class SqlQueryParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public SqlDbType SqlType { get; set; }
    }
}
