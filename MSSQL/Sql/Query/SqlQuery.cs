using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL.Sql.Query
{
    public class SqlQuery
    {
        public static string GetWhereStatement<T>(Expression<Func<T, bool>> where)
        {
            return null;
        }

        public static string CreateDatabase(string databaseName)
        {
            return "Create database " + databaseName;
        }

        public static string CreateTable<T>()
        {
            return "";
        }

        public static string UseDatabase(string databaseName)
        {
            return "Use " + databaseName;
        }

        public static string Select<T>()
        {
            return "Select * from " + SqlMapping.GetTableName<T>();
        }

        public static string Select<T>(int recordNumber)
        {
            return "Select top " + recordNumber + " * from " + SqlMapping.GetTableName<T>();
        }

        public static string Select<T>(Expression<Func<T, bool>> where)
        {
            string whereExpression = SqlQuery.GetWhereStatement<T>(where);
            return /*SqlMapping.ConvertToStandardQuery(*/"Select * from " + SqlMapping.GetTableName<T>() + " " + whereExpression/*)*/;
        }

        public static string Delete<T>()
        {
            return "Delete from " + SqlMapping.GetTableName<T>();
        }
    }
}
