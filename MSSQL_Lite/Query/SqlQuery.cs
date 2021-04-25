using MSSQL_Lite.LambdaExpression;
using MSSQL_Lite.Mapping;
using MSSQL_Lite.Reflection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL_Lite.Query
{
    public class SqlQuery : SqlQueryBase
    {
        public static bool EnclosedInSquareBrackets = true;

        public static SqlCommand CreateDatabase(string databaseName)
        {
            string query = string.Format("Create database {0}", databaseName);
            return InitSqlCommand(query);
        }

        public static SqlCommand UseDatabase(string databaseName)
        {
            string query = string.Format("Use {0}", databaseName);
            return InitSqlCommand(query); 
        }

        public static SqlCommand Select<T>()
        {
            string query = string.Format("Select * from {0}", SqlMapping.GetTableName<T>(EnclosedInSquareBrackets));
            return InitSqlCommand(query);
        }

        public static SqlCommand Select<T>(int recordNumber)
        {
            string query = string
                .Format("Select top {0} * from {1}", recordNumber, SqlMapping.GetTableName<T>(EnclosedInSquareBrackets));
            return InitSqlCommand(query);
        }

        public static SqlCommand Select<T>(Expression<Func<T, bool>> where)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where, EnclosedInSquareBrackets);
            string query = string
                .Format("Select * from {0} {1}", SqlMapping.GetTableName<T>(EnclosedInSquareBrackets), sqlQueryData.Statement);
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public static SqlCommand Select<T>(Expression<Func<T, bool>> where, int recordNumber)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where, EnclosedInSquareBrackets);
            string query = string
                .Format(
                    "Select top {0} * from {1} {2}",
                    recordNumber, SqlMapping.GetTableName<T>(EnclosedInSquareBrackets), sqlQueryData.Statement
                );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public static SqlCommand Select<T>(Expression<Func<T, object>> select)
        {
            string query = string
                .Format(
                    "{0} from {1}", 
                    GetSelectStatement<T>(select, EnclosedInSquareBrackets), 
                    SqlMapping.GetTableName<T>(EnclosedInSquareBrackets)
                );
            return InitSqlCommand(query);
        }

        public static SqlCommand Select<T>(Expression<Func<T, object>> select, int recordNumber)
        {
            string selectStatement = GetSelectStatement<T>(select, EnclosedInSquareBrackets)
                .Replace("Select ", string.Format("Select top {0} ", recordNumber));
            string query = string.Format("{0} from {1}", selectStatement, SqlMapping.GetTableName<T>(EnclosedInSquareBrackets));
            return InitSqlCommand(query);
        }

        public static SqlCommand Select<T>(Expression<Func<T, object>> select, Expression<Func<T, bool>> where)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where, EnclosedInSquareBrackets);
            string query = string
                .Format(
                    "{0} from {1} {2}",
                    GetSelectStatement<T>(select, EnclosedInSquareBrackets),
                    SqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                    sqlQueryData.Statement
                );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public static SqlCommand Select<T>(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, int recordNumber)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where, EnclosedInSquareBrackets);
            string selectStatement = GetSelectStatement<T>(select, EnclosedInSquareBrackets)
                .Replace("Select ", string.Format("Select top {0} ", recordNumber));
            string query = string
                .Format(
                    "{0} from {1} {2}",
                    selectStatement,
                    SqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                    sqlQueryData.Statement
                );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public static SqlCommand Count<T>()
        {
            string query = string
                .Format("Select cast(count(*) as varchar(20) from {0}", SqlMapping.GetTableName<T>(EnclosedInSquareBrackets));
            return InitSqlCommand(query);
        }

        public static SqlCommand Count<T>(Expression<Func<T, bool>> where)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where, EnclosedInSquareBrackets);
            string query = string
                .Format(
                    "Select cast(count(*) as varchar(20)) from {0} {1}",
                    SqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                    sqlQueryData.Statement
                );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public static SqlCommand Count<T>(string propertyName, Expression<Func<T, bool>> where)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where, EnclosedInSquareBrackets);
            string query = string
                .Format(
                    "Select cast(count({0}) as varchar(20)) from {1} {2}",
                    (EnclosedInSquareBrackets) ? string.Format("[{0}]", propertyName) : propertyName,
                    SqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                    sqlQueryData.Statement
                );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public static SqlCommand Insert<T>(T model)
        {
            string query = "Insert into " + SqlMapping.GetTableName<T>(EnclosedInSquareBrackets) + "(";
            PropertyInfo[] props = Obj.GetProperties(model);
            string into = null;
            string values = null;
            foreach (PropertyInfo prop in props)
            {
                into += SqlMapping.GetPropertyName(prop, EnclosedInSquareBrackets) + ", ";
                values += SqlMapping.ConvertToStandardDataInSql(prop.GetValue(model)) + ", ";
            }
            into = into.TrimEnd(' ').TrimEnd(',');
            values = values.TrimEnd(' ').TrimEnd(',');
            //return query + into + ") values (" + values + ")";
            return InitSqlCommand(query);
        }

        public static SqlCommand Update<T>(T model, Expression<Func<T, object>> set)
        {
            SqlQueryData sqlQueryData = GetSetStatement<T>(model, set, EnclosedInSquareBrackets);
            string query = string
                .Format("Update {0} {1}", SqlMapping.GetTableName<T>(EnclosedInSquareBrackets), sqlQueryData.Statement);
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public static SqlCommand Update<T>(T model, Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            SqlQueryData sqlQueryData1 = GetSetStatement<T>(model, set, EnclosedInSquareBrackets);
            SqlQueryData sqlQueryData2 = GetWhereStatement<T>(where, EnclosedInSquareBrackets);
            string query = string
                .Format(
                    "Update {0} {1} {2}",
                    SqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                    sqlQueryData1.Statement,
                    sqlQueryData2.Statement
                );
            List<SqlQueryParameter> sqlQueryParameters = sqlQueryData1.SqlQueryParameters
                .Concat(sqlQueryData2.SqlQueryParameters).ToList();

            return InitSqlCommand(query, sqlQueryParameters);
        }

        public static SqlCommand Delete<T>()
        {
            string query = string.Format("Delete from {0}", SqlMapping.GetTableName<T>(EnclosedInSquareBrackets));
            return InitSqlCommand(query);
        }

        public static SqlCommand Delete<T>(Expression<Func<T, bool>> where)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where, EnclosedInSquareBrackets);
            string query = string
                .Format("Delete from {0} {1}", SqlMapping.GetTableName<T>(EnclosedInSquareBrackets), sqlQueryData.Statement);
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }
    }
}
