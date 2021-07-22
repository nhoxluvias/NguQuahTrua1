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
            return items;
        }

        public async Task<List<T>> ToListAsync()
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>())
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(skip, take))
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(orderBy, sqlOrderByOptions))
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(orderBy, sqlOrderByOptions, skip, take))
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(int top)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(top))
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(top, orderBy, sqlOrderByOptions))
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where))
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where, long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where, skip, take))
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where, orderBy, sqlOrderByOptions))
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where, orderBy, sqlOrderByOptions, skip, take))
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where, int top)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where, top))
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where, int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where, top, orderBy, sqlOrderByOptions))
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set))
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, skip, take))
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, orderBy, sqlOrderByOption))
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption, long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, orderBy, sqlOrderByOption, skip, take))
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, int top)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, top))
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOption)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, top, orderBy, sqlOrderByOption))
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, where))
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where, long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, where, skip, take))
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, where, orderBy, sqlOrderByOptions))
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions, long skip, long take)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, where, orderBy, sqlOrderByOptions, skip, take))
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where, int top)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, where, top))
            {
                return await ToListAsync(sqlCommand);
            }
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where, int top, Expression<Func<T, object>> orderBy, SqlOrderByOptions sqlOrderByOptions)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, where, top, orderBy, sqlOrderByOptions))
            {
                return await ToListAsync(sqlCommand);
            }
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
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(1))
            {
                return await SingleOrDefaultAsync(sqlCommand);
            }
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where, 1))
            {
                return await SingleOrDefaultAsync(sqlCommand);
            }
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, object>> set)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, 1))
            {
                return await SingleOrDefaultAsync(sqlCommand);
            }
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, where, 1))
            {
                return await SingleOrDefaultAsync(sqlCommand);
            }
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
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(1))
            {
                return await FirstOrDefaultAsync(sqlCommand);
            }
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(where, 1))
            {
                return await FirstOrDefaultAsync(sqlCommand);
            }
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, object>> set)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, 1))
            {
                return await FirstOrDefaultAsync(sqlCommand);
            }
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Select<T>(set, where, 1))
            {
                return await FirstOrDefaultAsync(sqlCommand);
            }
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
            using(SqlCommand sqlCommand = sqlQuery.Delete<T>())
            {
                return await DeleteAsync(sqlCommand);
            }
        }

        public async Task<int> DeleteAsync(Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Delete<T>(where))
            {
                return await DeleteAsync(sqlCommand);
            }
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
            using(SqlCommand sqlCommand = sqlQuery.Update<T>(model, set))
            {
                return await UpdateAsync(sqlCommand);
            }
        }

        public async Task<int> UpdateAsync(T model, Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Update<T>(model, set, where))
            {
                return await UpdateAsync(sqlCommand);
            }
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
            using(SqlCommand sqlCommand = sqlQuery.Insert<T>(model))
            {
                return await InsertAsync(sqlCommand);
            }
        }

        public async Task<int> InsertAsync(T model, List<string> excludeProperties)
        {
            using(SqlCommand sqlCommand = sqlQuery.Insert<T>(model, excludeProperties))
            {
                return await InsertAsync(sqlCommand);
            }
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
            using(SqlCommand sqlCommand = sqlQuery.Count<T>())
            {
                return await CountAsync(sqlCommand);
            }
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Count<T>(where))
            {
                return await CountAsync(sqlCommand);
            }
        }

        public async Task<long> CountAsync(string propertyName, Expression<Func<T, bool>> where)
        {
            using(SqlCommand sqlCommand = sqlQuery.Count<T>(propertyName, where))
            {
                return await CountAsync(sqlCommand);
            }
        }
    }
}
