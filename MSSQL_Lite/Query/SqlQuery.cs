using MSSQL_Lite.Mapping;
using MSSQL_Lite.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL_Lite.Query
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

        public static string Insert<T>(T model)
        {
            string query = "Insert into " + SqlMapping.GetTableName(typeof(T)) + "(";
            PropertyInfo[] props = Obj.GetProperties(model);
            string into = null;
            string values = null;
            foreach (PropertyInfo prop in props)
            {
                into += SqlMapping.GetPropertyName(prop) + ", ";
                //values += SqlMapping.ConvertToStandardDataInSql<T>(prop.GetValue(model), prop.Name) + ", ";
            }
            into = into.TrimEnd(' ').TrimEnd(',');
            values = into.TrimEnd(' ').TrimEnd(',');
            return query + into + ") values (" + values + ")";
        }

        public static string Delete<T>()
        {
            return "Delete from " + SqlMapping.GetTableName<T>();
        }
    }
}
