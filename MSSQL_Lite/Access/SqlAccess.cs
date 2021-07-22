using MSSQL_Lite.Config;
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
        private bool disposedValue;

        internal SqlAccess(SqlData sqlData)
        {
            sqlQuery = new SqlQuery();
            this.sqlData = sqlData;
            disposedValue = false;
            if (SqlConfig.connectionType == ConnectionType.ManuallyDisconnect)
                sqlData.Connect();
        }

        private List<T> ToList(SqlCommand sqlCommand)
        {
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Connect();
            sqlData.ExecuteReader(sqlCommand);
            List<T> items = sqlData.ToList<T>();
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return items;
        }

        public List<T> ToList()
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>())
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(skip, take))
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(orderBy, sqlOrderByOptions))
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(orderBy, sqlOrderByOptions, skip, take))
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(int top)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(top))
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(top, orderBy, sqlOrderByOptions))
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where))
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, bool>> where, long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where, skip, take))
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where, orderBy, sqlOrderByOptions))
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where, orderBy, sqlOrderByOptions, skip, take))
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, bool>> where, int top)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where, top))
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, bool>> where, int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where, top, orderBy, sqlOrderByOptions))
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> set)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set))
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> select, long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(select, skip, take))
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(select, orderBy, sqlOrderByOption))
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption, long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(select, orderBy, sqlOrderByOption, skip, take))
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> select, int top)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(select, top))
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> select, int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(select, top, orderBy, sqlOrderByOption))
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(select, where))
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(select, where, skip, take))
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(select, where, orderBy, sqlOrderByOptions))
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(select, where, orderBy, sqlOrderByOptions, skip, take))
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, int top)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(select, where, top))
            {
                return ToList(sqlCommand);
            }
        }

        public List<T> ToList(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(select, where, top, orderBy, sqlOrderByOptions))
            {
                return ToList(sqlCommand);
            }
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
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Connect();
            sqlData.ExecuteReader(sqlCommand);
            T item = sqlData.To<T>();
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return item;
        }

        public T SingleOrDefault()
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(1))
            {
                return SingleOrDefault(sqlCommand);
            }
        }

        public T SingleOrDefault(Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where, 1))
            {
                return SingleOrDefault(sqlCommand);
            }
        }

        public T SingleOrDefault(Expression<Func<T, object>> set)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, 1))
            {
                return SingleOrDefault(sqlCommand);
            }
        }

        public T SingleOrDefault(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, where, 1))
            {
                return SingleOrDefault(sqlCommand);
            }
        }

        private T FirstOrDefault(SqlCommand sqlCommand)
        {
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Connect();
            sqlData.ExecuteReader(sqlCommand);
            T item = sqlData.To<T>();
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return item;
        }

        public T FirstOrDefault()
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(1))
            {
                return FirstOrDefault(sqlCommand);
            }
        }

        public T FirstOrDefault(Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where, 1))
            {
                return FirstOrDefault(sqlCommand);
            }
        }

        public T FirstOrDefault(Expression<Func<T, object>> set)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, 1))
            {
                return FirstOrDefault(sqlCommand);
            }
        }

        public T FirstOrDefault(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, where, 1))
            {
                return FirstOrDefault(sqlCommand);
            }
        }

        private int Delete(SqlCommand sqlCommand)
        {
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Connect();
            int affected = sqlData.ExecuteNonQuery(sqlCommand);
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return affected;
        }

        public int Delete()
        {
            using(SqlCommand sqlCommand = sqlQuery.Delete<T>())
            {
                return Delete(sqlCommand);
            }
        }

        public int Delete(Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Delete<T>(where))
            {
                return Delete(sqlCommand);
            }
        }

        private int Update(SqlCommand sqlCommand)
        {
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Connect();
            int affected = sqlData.ExecuteNonQuery(sqlCommand);
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return affected;
        }

        public int Update(T model, Expression<Func<T, object>> set)
        {
            using(SqlCommand sqlCommand = sqlQuery.Update<T>(model, set))
            {
                return Update(sqlCommand);
            }
        }

        public int Update(T model, Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Update<T>(model, set, where))
            {
                return Update(sqlCommand);
            }
        }

        private int Insert(SqlCommand sqlCommand)
        {
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Connect();
            int affected = sqlData.ExecuteNonQuery(sqlCommand);
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return affected;
        }

        public int Insert(T model)
        {
            using(SqlCommand sqlCommand = sqlQuery.Insert<T>(model))
            {
                return Insert(sqlCommand);
            }
        }

        public int Insert(T model, List<string> excludeProperties)
        {
            using(SqlCommand sqlCommand = sqlQuery.Insert<T>(model, excludeProperties))
            {
                return Insert(sqlCommand);
            }
        }

        private long Count(SqlCommand sqlCommand)
        {
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Connect();
            long result = long.Parse((string)sqlData.ExecuteScalar(sqlCommand));
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return result;
        }

        public long Count()
        {
            using (SqlCommand sqlCommand = sqlQuery.Count<T>())
            {
                return Count(sqlCommand);
            }
        }

        public long Count(Expression<Func<T, bool>> where)
        {
            using (SqlCommand sqlCommand = sqlQuery.Count<T>(where))
            {
                return Count(sqlCommand);
            }
        }

        public long Count(string propertyName, Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Count<T>(propertyName, where))
            {
                return Count(sqlCommand);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
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
