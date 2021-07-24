using System.Collections.Generic;

namespace MSSQL_Lite.Query
{
    internal class SqlQueryData
    {
        public string Statement { get; set; }
        public List<SqlQueryParameter> SqlQueryParameters { get; set; }
    }
}
