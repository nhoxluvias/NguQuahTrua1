using System.Collections.Generic;
using MSSQL.Sql.Constraints;

namespace MSSQL.Sql.Tables
{
    public class SqlTable
    {
        public List<SqlProperty> SqlProperties { get; set; }
        public List<SqlConstraint> SqlConstraints { get; set; }
    }
}
