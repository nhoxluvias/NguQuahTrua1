using MSSQL_Lite.Connection;
using MSSQL_Lite.Query;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace MSSQL_Lite.Access
{
    public partial class SqlAccess<T> : IDisposable
    {
        private SqlQuery sqlQuery;
        private SqlData sqlData;
        private ConnectionType connectionType;

        public SqlAccess()
        {
            sqlQuery = new SqlQuery();
            sqlData = new SqlData();
            connectionType = ConnectionType.DisconnectAfterCompletion;
        }

        public SqlAccess(ConnectionType connectionType)
        {
            sqlQuery = new SqlQuery();
            sqlData = new SqlData();
            this.connectionType = connectionType;
        }

        private List<T> ToList(SqlCommand sqlCommand)
        {
            sqlData.Connect();
            sqlData.ExecuteReader(sqlCommand);
            List<T> items = sqlData.ToList<T>();
            sqlData.Disconnect();
            return items;
        }

        public List<T> ToList()
        {
            return ToList(sqlQuery.Select<T>());
        }

        public List<T> ToList(int recordNumber)
        {
            return ToList(sqlQuery.Select<T>(recordNumber));
        }

        public List<T> ToList(Expression<Func<T, bool>> where)
        {
            return ToList(sqlQuery.Select<T>(where));
        }

        public List<T> ToList(Expression<Func<T, bool>> where, int recordNumber)
        {
            return ToList(sqlQuery.Select<T>(where, recordNumber));
        }

        public List<T> ToList(Expression<Func<T, object>> set)
        {
            return ToList(sqlQuery.Select<T>(set));
        }

        public List<T> ToList(Expression<Func<T, object>> set, int recordNumber)
        {
            return ToList(sqlQuery.Select<T>(set, recordNumber));
        }

        public List<T> ToList(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return ToList(sqlQuery.Select<T>(set, where));
        }

        public List<T> ToList(Expression<Func<T, object>> set, Expression<Func<T, bool>> where, int recordNumber)
        {
            return ToList(sqlQuery.Select<T>(set, where, recordNumber));
        }

        public T SingleOrDefault(SqlCommand sqlCommand)
        {
            sqlData.Connect();
            sqlData.ExecuteReader(sqlCommand);
            T item = sqlData.To<T>();
            sqlData.Disconnect();
            return item;
        }

        public T SingleOrDefault()
        {
            return SingleOrDefault(sqlQuery.Select<T>(1));
        }

        public T SingleOrDefault(Expression<Func<T, bool>> where)
        {
            return SingleOrDefault(sqlQuery.Select<T>(where, 1));
        }

        public T SingleOrDefault(Expression<Func<T, object>> set)
        {
            return SingleOrDefault(sqlQuery.Select<T>(set, 1));
        }

        public T SingleOrDefault(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return SingleOrDefault(sqlQuery.Select<T>(set, where, 1));
        }

        private T FirstOrDefault(SqlCommand sqlCommand)
        {
            sqlData.Connect();
            sqlData.ExecuteReader(sqlCommand);
            T item = sqlData.To<T>();
            sqlData.Disconnect();
            return item;
        }

        public T FirstOrDefault()
        {
            return FirstOrDefault(sqlQuery.Select<T>(1));
        }

        public T FirstOrDefault(Expression<Func<T, bool>> where)
        {
            return FirstOrDefault(sqlQuery.Select<T>(where, 1));
        }

        public T FirstOrDefault(Expression<Func<T, object>> set)
        {
            return FirstOrDefault(sqlQuery.Select<T>(set, 1));
        }

        public T FirstOrDefault(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return FirstOrDefault(sqlQuery.Select<T>(set, where, 1));
        }

        private int Delete(SqlCommand sqlCommand)
        {
            sqlData.Connect();
            int affected = sqlData.ExecuteNonQuery(sqlCommand);
            sqlData.Disconnect();
            return affected;
        }

        public int Delete()
        {
            return Delete(sqlQuery.Delete<T>());
        }

        public int Delete(Expression<Func<T, bool>> where)
        {
            return Delete(sqlQuery.Delete<T>(where));
        }

        private int Update(SqlCommand sqlCommand)
        {
            sqlData.Connect();
            int affected = sqlData.ExecuteNonQuery(sqlCommand);
            sqlData.Disconnect();
            return affected;
        }

        public int Update(T model, Expression<Func<T, object>> set)
        {
            return Update(sqlQuery.Update<T>(model, set));
        }

        public int Update(T model, Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return Update(sqlQuery.Update<T>(model, set, where));
        }

        private int Insert(SqlCommand sqlCommand)
        {
            sqlData.Connect();
            int affected = sqlData.ExecuteNonQuery(sqlCommand);
            sqlData.Disconnect();
            return affected;
        }

        public int Insert(T model)
        {
            return Insert(sqlQuery.Insert<T>(model));
        }

        public int Insert(T model, List<string> excludeProperties)
        {
            return Insert(sqlQuery.Insert<T>(model, excludeProperties));
        }

        private long Count(SqlCommand sqlCommand)
        {
            sqlData.Connect();
            long result = long.Parse((string)sqlData.ExecuteScalar(sqlCommand));
            sqlData.Disconnect();
            return result;
        }

        public long Count()
        {
            return Count(sqlQuery.Count<T>());
        }

        public long Count(Expression<Func<T, bool>> where)
        {
            return Count(sqlQuery.Count<T>(where));
        }

        public long Count(string propertyName, Expression<Func<T, bool>> where)
        {
            return Count(sqlQuery.Count<T>(propertyName, where));
        }

        public void Dispose()
        {
            this.sqlQuery = null;
            sqlData.Dispose();
            sqlData = null;
            GC.SuppressFinalize(this);
        }
    }
}
