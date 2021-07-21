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
        private bool disposedValue;

        internal SqlAccess(ConnectionType connectionType, SqlData sqlData)
        {
            sqlQuery = new SqlQuery();
            this.sqlData = sqlData;
            disposedValue = false;
            this.connectionType = connectionType;
            if (connectionType == ConnectionType.ManuallyDisconnect)
                sqlData.Connect();
        }

        private List<T> ToList(SqlCommand sqlCommand)
        {
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Connect();
            sqlData.ExecuteReader(sqlCommand);
            List<T> items = sqlData.ToList<T>();
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            sqlCommand.Dispose();
            return items;
        }

        public List<T> ToList()
        {
            return ToList(sqlQuery.Select<T>());
        }

        public List<T> ToList(long skip, long take)
        {
            return ToList(sqlQuery.Select<T>(skip, take));
        }

        public List<T> ToList(Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            return ToList(sqlQuery.Select<T>(orderBy, sqlOrderByOptions));
        }

        public List<T> ToList(Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long skip, long take)
        {
            return ToList(sqlQuery.Select<T>(orderBy, sqlOrderByOptions, skip, take));
        }

        public List<T> ToList(int top)
        {
            return ToList(sqlQuery.Select<T>(top));
        }

        public List<T> ToList(int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            return ToList(sqlQuery.Select<T>(top, orderBy, sqlOrderByOptions));
        }

        public List<T> ToList(Expression<Func<T, bool>> where)
        {
            return ToList(sqlQuery.Select<T>(where));
        }

        public List<T> ToList(Expression<Func<T, bool>> where, long skip, long take)
        {
            return ToList(sqlQuery.Select<T>(where, skip, take));
        }

        public List<T> ToList(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            return ToList(sqlQuery.Select<T>(where, orderBy, sqlOrderByOptions));
        }

        public List<T> ToList(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long skip, long take)
        {
            return ToList(sqlQuery.Select<T>(where, orderBy, sqlOrderByOptions, skip, take));
        }

        public List<T> ToList(Expression<Func<T, bool>> where, int top)
        {
            return ToList(sqlQuery.Select<T>(where, top));
        }

        public List<T> ToList(Expression<Func<T, bool>> where, int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            return ToList(sqlQuery.Select<T>(where, top, orderBy, sqlOrderByOptions));
        }

        public List<T> ToList(Expression<Func<T, object>> set)
        {
            return ToList(sqlQuery.Select<T>(set));
        }

        public List<T> ToList(Expression<Func<T, object>> select, long skip, long take)
        {
            return ToList(sqlQuery.Select<T>(select, skip, take));
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption)
        {
            return ToList(sqlQuery.Select<T>(select, orderBy, sqlOrderByOption));
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption, long skip, long take)
        {
            return ToList(sqlQuery.Select<T>(select, orderBy, sqlOrderByOption, skip, take));
        }

        public List<T> ToList(Expression<Func<T, object>> select, int top)
        {
            return ToList(sqlQuery.Select<T>(select, top));
        }

        public List<T> ToList(Expression<Func<T, object>> select, int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption)
        {
            return ToList(sqlQuery.Select<T>(select, top, orderBy, sqlOrderByOption));
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, bool>> where)
        {
            return ToList(sqlQuery.Select<T>(select, where));
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, long skip, long take)
        {
            return ToList(sqlQuery.Select<T>(select, where, skip, take));
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            return ToList(sqlQuery.Select<T>(select, where, orderBy, sqlOrderByOptions));
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long skip, long take)
        {
            return ToList(sqlQuery.Select<T>(select, where, orderBy, sqlOrderByOptions, skip, take));
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, int top)
        {
            return ToList(sqlQuery.Select<T>(select, where, top));
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            return ToList(sqlQuery.Select<T>(select, where, top, orderBy, sqlOrderByOptions));
        }

        public SqlPagedList<T> ToPagedList(long pageIndex, long pageSize)
        {
            long totalRecord = Count();
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = ToList(pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public SqlPagedList<T> ToPagedList(Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long pageIndex, long pageSize)
        {
            long totalRecord = Count();
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = ToList(orderBy, sqlOrderByOptions, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public SqlPagedList<T> ToPagedList(Expression<Func<T, object>> select, long pageIndex, long pageSize)
        {
            long totalRecord = Count();
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = ToList(select, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public SqlPagedList<T> ToPagedList(Expression<Func<T, object>> select, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long pageIndex, long pageSize)
        {
            long totalRecord = Count();
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = ToList(select, orderBy, sqlOrderByOptions, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public SqlPagedList<T> ToPagedList(Expression<Func<T, bool>> where, long pageIndex, long pageSize)
        {
            long totalRecord = Count(where);
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = ToList(where, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public SqlPagedList<T> ToPagedList(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long pageIndex, long pageSize)
        {
            long totalRecord = Count(where);
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = ToList(where, orderBy, sqlOrderByOptions, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public SqlPagedList<T> ToPagedList(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, long pageIndex, long pageSize)
        {
            long totalRecord = Count(where);
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = ToList(select, where, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public SqlPagedList<T> ToPagedList(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long pageIndex, long pageSize)
        {
            long totalRecord = Count(where);
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = ToList(select, where, orderBy, sqlOrderByOptions, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public T SingleOrDefault(SqlCommand sqlCommand)
        {
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Connect();
            sqlData.ExecuteReader(sqlCommand);
            T item = sqlData.To<T>();
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
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
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Connect();
            sqlData.ExecuteReader(sqlCommand);
            T item = sqlData.To<T>();
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
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
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Connect();
            int affected = sqlData.ExecuteNonQuery(sqlCommand);
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
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
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Connect();
            int affected = sqlData.ExecuteNonQuery(sqlCommand);
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
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
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Connect();
            int affected = sqlData.ExecuteNonQuery(sqlCommand);
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
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
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Connect();
            long result = long.Parse((string)sqlData.ExecuteScalar(sqlCommand));
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    sqlQuery.Dispose();
                    sqlQuery = null;
                }
                disposedValue = true;
            }
        }
        ~SqlAccess()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
