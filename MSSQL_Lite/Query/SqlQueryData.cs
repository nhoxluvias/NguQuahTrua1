using System.Collections.Generic;

namespace MSSQL_Lite.Query
{
    public class SqlQueryData
    {
        public string Statement { get; set; }
        public List<SqlQueryParameter> SqlQueryParameters { get; set; }
    }
}
