using MSSQL.Sql.Constraints;
using System.Collections.Generic;

namespace MSSQL.Sql
{
    public class SqlProperty
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public string Identity { get; set; }

        public List<SqlConstraint> SqlConstraints { get; set; }
    }
}
