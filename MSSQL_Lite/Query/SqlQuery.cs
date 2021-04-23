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
            string expression = where.Body.ToString();
            return null;
        }

        public static void Exp<T>(Expression<Func<T, bool>> where)
        {
            BinaryExpression binaryExpression = (BinaryExpression)where.Body;
            GetPairOfExpression((BinaryExpression)binaryExpression.Left);
        }

        public static KeyValuePair<string, object> GetPairOfExpression(BinaryExpression binaryExpression)
        {
            if (binaryExpression == null)
                throw new Exception("@'binaryExpression' must be not null");
            if(
                binaryExpression.NodeType == ExpressionType.Equal 
                || binaryExpression.NodeType == ExpressionType.NotEqual 
                || binaryExpression.NodeType == ExpressionType.GreaterThan 
                || binaryExpression.NodeType == ExpressionType.LessThan
                || binaryExpression.NodeType == ExpressionType.GreaterThanOrEqual
                || binaryExpression.NodeType == ExpressionType.LessThanOrEqual
                || binaryExpression.NodeType == ExpressionType.Call
            )
            {

                return default(KeyValuePair<string, object>);
            }
            return default(KeyValuePair<string, object>);
        }







        public static string CreateDatabase(string databaseName)
        {
            return "Create database " + databaseName;
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
            string whereExpression = GetWhereStatement<T>(where);
            return "Select * from " + SqlMapping.GetTableName<T>(true) + " " + whereExpression;
        }

        public static string Select<T>(Expression<Func<T, bool>> where, int recordNumber)
        {
            string whereExpression = GetWhereStatement<T>(where);
            return "Select " + recordNumber + " * from " + SqlMapping.GetTableName<T>(true) + " " + whereExpression;
        }

        public static string Select<T>(Expression<Func<T, object>> select)
        {
            return null;
        }

        public static string Select<T>(Expression<Func<T, object>> select, int recordNumber)
        {
            return null;
        }

        public static string Select<T>(Expression<Func<T, object>> select, Expression<Func<T, bool>> where)
        {
            return null;
        }

        public static string Select<T>(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, int recordNumber)
        {
            return null;
        }

        public static string Insert<T>(T model)
        {
            string query = "Insert into " + SqlMapping.GetTableName<T>(true) + "(";
            PropertyInfo[] props = Obj.GetProperties(model);
            string into = null;
            string values = null;
            foreach (PropertyInfo prop in props)
            {
                into += SqlMapping.GetPropertyName(prop, true) + ", ";
                values += SqlMapping.ConvertToStandardDataInSql(prop.GetValue(model)) + ", ";
            }
            into = into.TrimEnd(' ').TrimEnd(',');
            values = values.TrimEnd(' ').TrimEnd(',');
            return query + into + ") values (" + values + ")";
        }

        public static string Update<T>(T model, Expression<Func<T, object>> set)
        {
            return null;
        }

        public static string Update<T>(T model, Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return null;
        }

        public static string Delete<T>()
        {
            return "Delete from " + SqlMapping.GetTableName<T>();
        }

        public static string Delete<T>(Expression<Func<T, bool>> where)
        {
            return null;
        }
    }
}
