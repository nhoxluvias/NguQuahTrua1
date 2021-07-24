using System.Data;

namespace MSSQL_Lite.Query
{
    internal class SqlQueryParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public SqlDbType SqlType { get; set; }
    }
}
