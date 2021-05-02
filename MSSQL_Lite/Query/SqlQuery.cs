using MSSQL_Lite.Mapping;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace MSSQL_Lite.Query
{
    public class SqlQuery : SqlQueryBase
    {
        public static bool EnclosedInSquareBrackets = true;
        private SqlMapping sqlMapping;
        private bool disposed;

        public SqlQuery()
            : base()
        {
            sqlMapping = new SqlMapping();
            disposed = false;
        }

        public SqlCommand CreateDatabase(string databaseName)
        {
            string query = string.Format("Create database {0}", databaseName);
            return InitSqlCommand(query);
        }

        public SqlCommand UseDatabase(string databaseName)
        {
            string query = string.Format("Use {0}", databaseName);
            return InitSqlCommand(query);
        }

        public SqlCommand Select<T>()
        {
            string query = string.Format("Select * from {0}", sqlMapping.GetTableName<T>(EnclosedInSquareBrackets));
            return InitSqlCommand(query);
        }

        public SqlCommand Select<T>(int recordNumber)
        {
            string query = string
                .Format("Select top {0} * from {1}", recordNumber, sqlMapping.GetTableName<T>(EnclosedInSquareBrackets));
            return InitSqlCommand(query);
        }

        public SqlCommand Select<T>(Expression<Func<T, bool>> where)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where, EnclosedInSquareBrackets);
            string query = string
                .Format("Select * from {0} {1}", sqlMapping.GetTableName<T>(EnclosedInSquareBrackets), sqlQueryData.Statement);
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Select<T>(Expression<Func<T, bool>> where, int recordNumber)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where, EnclosedInSquareBrackets);
            string query = string
                .Format(
                    "Select top {0} * from {1} {2}",
                    recordNumber, sqlMapping.GetTableName<T>(EnclosedInSquareBrackets), sqlQueryData.Statement
                );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Select<T>(Expression<Func<T, object>> select)
        {
            string query = string
                .Format(
                    "{0} from {1}",
                    GetSelectStatement<T>(select, EnclosedInSquareBrackets),
                    sqlMapping.GetTableName<T>(EnclosedInSquareBrackets)
                );
            return InitSqlCommand(query);
        }

        public SqlCommand Select<T>(Expression<Func<T, object>> select, int recordNumber)
        {
            string selectStatement = GetSelectStatement<T>(select, EnclosedInSquareBrackets)
                .Replace("Select ", string.Format("Select top {0} ", recordNumber));
            string query = string.Format("{0} from {1}", selectStatement, sqlMapping.GetTableName<T>(EnclosedInSquareBrackets));
            return InitSqlCommand(query);
        }

        public SqlCommand Select<T>(Expression<Func<T, object>> select, Expression<Func<T, bool>> where)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where, EnclosedInSquareBrackets);
            string query = string
                .Format(
                    "{0} from {1} {2}",
                    GetSelectStatement<T>(select, EnclosedInSquareBrackets),
                    sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                    sqlQueryData.Statement
                );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Select<T>(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, int recordNumber)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where, EnclosedInSquareBrackets);
            string selectStatement = GetSelectStatement<T>(select, EnclosedInSquareBrackets)
                .Replace("Select ", string.Format("Select top {0} ", recordNumber));
            string query = string
                .Format(
                    "{0} from {1} {2}",
                    selectStatement,
                    sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                    sqlQueryData.Statement
                );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Count<T>()
        {
            string query = string
                .Format("Select cast(count(*) as varchar(20)) from {0}", sqlMapping.GetTableName<T>(EnclosedInSquareBrackets));
            return InitSqlCommand(query);
        }

        public SqlCommand Count<T>(Expression<Func<T, bool>> where)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where, EnclosedInSquareBrackets);
            string query = string
                .Format(
                    "Select cast(count(*) as varchar(20)) from {0} {1}",
                    sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                    sqlQueryData.Statement
                );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Count<T>(string propertyName, Expression<Func<T, bool>> where)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where, EnclosedInSquareBrackets);
            string query = string
                .Format(
                    "Select cast(count({0}) as varchar(20)) from {1} {2}",
                    (EnclosedInSquareBrackets) ? string.Format("[{0}]", propertyName) : propertyName,
                    sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                    sqlQueryData.Statement
                );
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Insert<T>(T model)
        {
            SqlQueryData sqlQueryData = GetInsertQueryData<T>(model, EnclosedInSquareBrackets);
            return InitSqlCommand(sqlQueryData.Statement, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Insert<T>(T model, List<string> excludeProperties)
        {
            if (excludeProperties == null)
                return Insert<T>(model);
            if (excludeProperties.Count == 0)
                return Insert<T>(model);
            SqlQueryData sqlQueryData = GetInsertQueryData<T>(model, excludeProperties, EnclosedInSquareBrackets);
            return InitSqlCommand(sqlQueryData.Statement, sqlQueryData.SqlQueryParameters);
        }
        public SqlCommand Update<T>(T model, Expression<Func<T, object>> set)
        {
            SqlQueryData sqlQueryData = GetSetStatement<T>(model, set, EnclosedInSquareBrackets);
            string query = string
                .Format("Update {0} {1}", sqlMapping.GetTableName<T>(EnclosedInSquareBrackets), sqlQueryData.Statement);
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        public SqlCommand Update<T>(T model, Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            SqlQueryData sqlQueryData1 = GetSetStatement<T>(model, set, EnclosedInSquareBrackets);
            SqlQueryData sqlQueryData2 = GetWhereStatement<T>(where, EnclosedInSquareBrackets);
            string query = string
                .Format(
                    "Update {0} {1} {2}",
                    sqlMapping.GetTableName<T>(EnclosedInSquareBrackets),
                    sqlQueryData1.Statement,
                    sqlQueryData2.Statement
                );
            List<SqlQueryParameter> sqlQueryParameters = sqlQueryData1.SqlQueryParameters
                .Concat(sqlQueryData2.SqlQueryParameters).ToList();

            return InitSqlCommand(query, sqlQueryParameters);
        }

        public SqlCommand Delete<T>()
        {
            string query = string.Format("Delete from {0}", sqlMapping.GetTableName<T>(EnclosedInSquareBrackets));
            return InitSqlCommand(query);
        }

        public SqlCommand Delete<T>(Expression<Func<T, bool>> where)
        {
            SqlQueryData sqlQueryData = GetWhereStatement<T>(where, EnclosedInSquareBrackets);
            string query = string
                .Format("Delete from {0} {1}", sqlMapping.GetTableName<T>(EnclosedInSquareBrackets), sqlQueryData.Statement);
            return InitSqlCommand(query, sqlQueryData.SqlQueryParameters);
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                try
                {
                    if (disposing)
                    {
                    }
                    this.disposed = true;
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
    }
}
