using MSSQL_Lite.Config;
using MSSQL_Lite.Connection;
using MSSQL_Lite.Query;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MSSQL_Lite.Access
{
    public partial class SqlAccess<T>
    {
        private async Task<List<T>> ToListAsync(SqlCommand sqlCommand)
        {
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                await sqlData.ConnectAsync();
            await sqlData.ExecuteReaderAsync(sqlCommand);
            List<T> items = sqlData.ToList<T>();
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            sqlCommand.Dispose();
            return items;
        }

        public async Task<List<T>> ToListAsync()
        {
            return await ToListAsync(sqlQuery.Select<T>());
        }

        public async Task<List<T>> ToListAsync(long skip, long take)
        {
            return await ToListAsync(sqlQuery.Select<T>(skip, take));
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            return await ToListAsync(sqlQuery.Select<T>(orderBy, sqlOrderByOptions));
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long skip, long take)
        {
            return await ToListAsync(sqlQuery.Select<T>(orderBy, sqlOrderByOptions, skip, take));
        }

        public async Task<List<T>> ToListAsync(int top)
        {
            return await ToListAsync(sqlQuery.Select<T>(top));
        }

        public async Task<List<T>> ToListAsync(int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            return await ToListAsync(sqlQuery.Select<T>(top, orderBy, sqlOrderByOptions));
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where)
        {
            return await ToListAsync(sqlQuery.Select<T>(where));
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where, long skip, long take)
        {
            return await ToListAsync(sqlQuery.Select<T>(where, skip, take));
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            return await ToListAsync(sqlQuery.Select<T>(where, orderBy, sqlOrderByOptions));
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long skip, long take)
        {
            return await ToListAsync(sqlQuery.Select<T>(where, orderBy, sqlOrderByOptions, skip, take));
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where, int top)
        {
            return await ToListAsync(sqlQuery.Select<T>(where, top));
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where, int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            return await ToListAsync(sqlQuery.Select<T>(where, top, orderBy, sqlOrderByOptions));
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set)
        {
            return await ToListAsync(sqlQuery.Select<T>(set));
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, long skip, long take)
        {
            return await ToListAsync(sqlQuery.Select<T>(set, skip, take));
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption)
        {
            return await ToListAsync(sqlQuery.Select<T>(set, orderBy, sqlOrderByOption));
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption, long skip, long take)
        {
            return await ToListAsync(sqlQuery.Select<T>(set, orderBy, sqlOrderByOption, skip, take));
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, int top)
        {
            return await ToListAsync(sqlQuery.Select<T>(set, top));
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption)
        {
            return await ToListAsync(sqlQuery.Select<T>(set, top, orderBy, sqlOrderByOption));
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return await ToListAsync(sqlQuery.Select<T>(set, where));
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where, long skip, long take)
        {
            return await ToListAsync(sqlQuery.Select<T>(set, where, skip, take));
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            return await ToListAsync(sqlQuery.Select<T>(set, where, orderBy, sqlOrderByOptions));
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long skip, long take)
        {
            return await ToListAsync(sqlQuery.Select<T>(set, where, orderBy, sqlOrderByOptions, skip, take));
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where, int top)
        {
            return await ToListAsync(sqlQuery.Select<T>(set, where, top));
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where, int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            return await ToListAsync(sqlQuery.Select<T>(set, where, top, orderBy, sqlOrderByOptions));
        }

        public async Task<SqlPagedList<T>> ToPagedListAsync(long pageIndex, long pageSize)
        {
            long totalRecord = await CountAsync();
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = await ToListAsync(pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public async Task<SqlPagedList<T>> ToPagedListAsync(Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long pageIndex, long pageSize)
        {
            long totalRecord = await CountAsync();
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = await ToListAsync(orderBy, sqlOrderByOptions, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public async Task<SqlPagedList<T>> ToPagedListAsync(Expression<Func<T, object>> select, long pageIndex, long pageSize)
        {
            long totalRecord = await CountAsync();
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = await ToListAsync(select, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public async Task<SqlPagedList<T>> ToPagedListAsync(Expression<Func<T, object>> select, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long pageIndex, long pageSize)
        {
            long totalRecord = await CountAsync();
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = await ToListAsync(select, orderBy, sqlOrderByOptions, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public async Task<SqlPagedList<T>> ToPagedListAsync(Expression<Func<T, bool>> where, long pageIndex, long pageSize)
        {
            long totalRecord = await CountAsync(where);
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = await ToListAsync(where, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public async Task<SqlPagedList<T>> ToPagedListAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long pageIndex, long pageSize)
        {
            long totalRecord = await CountAsync(where);
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = await ToListAsync(where, orderBy, sqlOrderByOptions, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public async Task<SqlPagedList<T>> ToPagedListAsync(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, long pageIndex, long pageSize)
        {
            long totalRecord = await CountAsync(where);
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = await ToListAsync(select, where, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public async Task<SqlPagedList<T>> ToPagedListAsync(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long pageIndex, long pageSize)
        {
            long totalRecord = await CountAsync(where);
            SqlPagedList<T> pagedList = new SqlPagedList<T>();
            pagedList.Solve(totalRecord, pageIndex, pageSize);
            pagedList.Items = await ToListAsync(select, where, orderBy, sqlOrderByOptions, pagedList.Skip, pagedList.Take);
            return pagedList;
        }

        public async Task<T> SingleOrDefaultAsync(SqlCommand sqlCommand)
        {
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                await sqlData.ConnectAsync();
            await sqlData.ExecuteReaderAsync(sqlCommand);
            T item = sqlData.To<T>();
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return item;
        }

        public async Task<T> SingleOrDefaultAsync()
        {
            return await SingleOrDefaultAsync(sqlQuery.Select<T>(1));
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> where)
        {
            return await SingleOrDefaultAsync(sqlQuery.Select<T>(where, 1));
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, object>> set)
        {
            return await SingleOrDefaultAsync(sqlQuery.Select<T>(set, 1));
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return await SingleOrDefaultAsync(sqlQuery.Select<T>(set, where, 1));
        }

        private async Task<T> FirstOrDefaultAsync(SqlCommand sqlCommand)
        {
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                await sqlData.ConnectAsync();
            await sqlData.ExecuteReaderAsync(sqlCommand);
            T item = sqlData.To<T>();
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return item;
        }

        public async Task<T> FirstOrDefaultAsync()
        {
            return await FirstOrDefaultAsync(sqlQuery.Select<T>(1));
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> where)
        {
            return await FirstOrDefaultAsync(sqlQuery.Select<T>(where, 1));
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, object>> set)
        {
            return await FirstOrDefaultAsync(sqlQuery.Select<T>(set, 1));
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return await FirstOrDefaultAsync(sqlQuery.Select<T>(set, where, 1));
        }

        private async Task<int> DeleteAsync(SqlCommand sqlCommand)
        {
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                await sqlData.ConnectAsync();
            int affected = await sqlData.ExecuteNonQueryAsync(sqlCommand);
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return affected;
        }

        public async Task<int> DeleteAsync()
        {
            return await DeleteAsync(sqlQuery.Delete<T>());
        }

        public async Task<int> DeleteAsync(Expression<Func<T, bool>> where)
        {
            return await DeleteAsync(sqlQuery.Delete<T>(where));
        }

        private async Task<int> UpdateAsync(SqlCommand sqlCommand)
        {
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                await sqlData.ConnectAsync();
            int affected = await sqlData.ExecuteNonQueryAsync(sqlCommand);
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return affected;
        }

        public async Task<int> UpdateAsync(T model, Expression<Func<T, object>> set)
        {
            return await UpdateAsync(sqlQuery.Update<T>(model, set));
        }

        public async Task<int> UpdateAsync(T model, Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return await UpdateAsync(sqlQuery.Update<T>(model, set, where));
        }

        private async Task<int> InsertAsync(SqlCommand sqlCommand)
        {
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                await sqlData.ConnectAsync();
            int affected = await sqlData.ExecuteNonQueryAsync(sqlCommand);
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return affected;
        }

        public async Task<int> InsertAsync(T model)
        {
            return await InsertAsync(sqlQuery.Insert<T>(model));
        }

        public async Task<int> InsertAsync(T model, List<string> excludeProperties)
        {
            return await InsertAsync(sqlQuery.Insert<T>(model, excludeProperties));
        }

        private async Task<long> CountAsync(SqlCommand sqlCommand)
        {
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                await sqlData.ConnectAsync();
            long result = long.Parse((string)await sqlData.ExecuteScalarAsync(sqlCommand));
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return result;
        }

        public async Task<long> CountAsync()
        {
            return await CountAsync(sqlQuery.Count<T>());
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> where)
        {
            return await CountAsync(sqlQuery.Count<T>(where));
        }

        public async Task<long> CountAsync(string propertyName, Expression<Func<T, bool>> where)
        {
            return await CountAsync(sqlQuery.Count<T>(propertyName, where));
        }
    }
}
