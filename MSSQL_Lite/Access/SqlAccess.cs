using MSSQL_Lite.Query;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL_Lite.Access
{
    public class SqlAccess<T> : IDisposable
    {
        private SqlQuery sqlQuery;
        private SqlData sqlData;
        public SqlAccess()
        {
            sqlQuery = new SqlQuery();
            sqlData = new SqlData();
        }

        private async Task<List<T>> ToListAsync(SqlCommand sqlCommand)
        {
            await sqlData.ConnectAsync();
            await sqlData.ExecuteReaderAsync(sqlCommand);
            List<T> items = sqlData.ToList<T>();
            sqlData.Disconnect();
            return items;
        }

        private List<T> ToList(SqlCommand sqlCommand)
        {
            sqlData.Connect();
            sqlData.ExecuteReader(sqlCommand);
            List<T> items = sqlData.ToList<T>();
            sqlData.Disconnect();
            return items;
        }

        public async Task<List<T>> ToListAsync()
        {
            return await ToListAsync(sqlQuery.Select<T>());
        }

        public List<T> ToList()
        {
            return ToList(sqlQuery.Select<T>());
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where)
        {
            return await ToListAsync(sqlQuery.Select<T>(where));
        }

        public List<T> ToList(Expression<Func<T, bool>> where)
        {
            return ToList(sqlQuery.Select<T>(where));
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set)
        {
            return await ToListAsync(sqlQuery.Select<T>(set));
        }

        public List<T> ToList(Expression<Func<T, object>> set)
        {
            return ToList(sqlQuery.Select<T>(set));
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return await ToListAsync(sqlQuery.Select<T>(set, where));
        }

        public List<T> ToList(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return ToList(sqlQuery.Select<T>(set, where));
        }

        public async Task<T> SingleOrDefaultAsync(SqlCommand sqlCommand)
        {
            await sqlData.ConnectAsync();
            await sqlData.ExecuteReaderAsync(sqlCommand);
            T item = sqlData.To<T>();
            sqlData.Disconnect();
            return item;
        }

        public T SingleOrDefault(SqlCommand sqlCommand)
        {
            sqlData.Connect();
            sqlData.ExecuteReader(sqlCommand);
            T item = sqlData.To<T>();
            sqlData.Disconnect();
            return item;
        }

        public async Task<T> SingleOrDefaultAsync()
        {
            return await SingleOrDefaultAsync(sqlQuery.Select<T>(1));
        }

        public T SingleOrDefault()
        {
            return SingleOrDefault(sqlQuery.Select<T>(1));
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> where)
        {
            return await SingleOrDefaultAsync(sqlQuery.Select<T>(where, 1));
        }

        public T SingleOrDefault(Expression<Func<T, bool>> where)
        {
            return SingleOrDefault(sqlQuery.Select<T>(where, 1));
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, object>> set)
        {
            return await SingleOrDefaultAsync(sqlQuery.Select<T>(set, 1));
        }

        public T SingleOrDefault(Expression<Func<T, object>> set)
        {
            return SingleOrDefault(sqlQuery.Select<T>(set, 1));
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return await SingleOrDefaultAsync(sqlQuery.Select<T>(set, where, 1));
        }

        public T SingleOrDefault(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return SingleOrDefault(sqlQuery.Select<T>(set, where, 1));
        }

        private async Task<T> FirstOrDefaultAsync(SqlCommand sqlCommand)
        {
            await sqlData.ConnectAsync();
            await sqlData.ExecuteReaderAsync(sqlCommand);
            T item = sqlData.To<T>();
            sqlData.Disconnect();
            return item;
        }

        private T FirstOrDefault(SqlCommand sqlCommand)
        {
            sqlData.Connect();
            sqlData.ExecuteReader(sqlCommand);
            T item = sqlData.To<T>();
            sqlData.Disconnect();
            return item;
        }

        public async Task<T> FirstOrDefaultAsync()
        {
            return await FirstOrDefaultAsync(sqlQuery.Select<T>(1));
        }

        public T FirstOrDefault()
        {
            return FirstOrDefault(sqlQuery.Select<T>(1));
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> where)
        {
            return await FirstOrDefaultAsync(sqlQuery.Select<T>(where, 1));
        }

        public T FirstOrDefault(Expression<Func<T, bool>> where)
        {
            return FirstOrDefault(sqlQuery.Select<T>(where, 1));
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, object>> set)
        {
            return await FirstOrDefaultAsync(sqlQuery.Select<T>(set, 1));
        }

        public T FirstOrDefault(Expression<Func<T, object>> set)
        {
            return FirstOrDefault(sqlQuery.Select<T>(set, 1));
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return await FirstOrDefaultAsync(sqlQuery.Select<T>(set, where, 1));
        }

        public T FirstOrDefault(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return FirstOrDefault(sqlQuery.Select<T>(set, where, 1));
        }

        private async Task<int> DeleteAsync(SqlCommand sqlCommand)
        {
            await sqlData.ConnectAsync();
            int affected = await sqlData.ExecuteNonQueryAsync(sqlCommand);
            sqlData.Disconnect();
            return affected;
        }

        private int Delete(SqlCommand sqlCommand)
        {
            sqlData.Connect();
            int affected = sqlData.ExecuteNonQuery(sqlCommand);
            sqlData.Disconnect();
            return affected;
        }

        public async Task<int> DeleteAsync()
        {
            return await DeleteAsync(sqlQuery.Delete<T>());
        }

        public int Delete()
        {
            return Delete(sqlQuery.Delete<T>());
        }

        public async Task<int> DeleteAsync(Expression<Func<T, bool>> where)
        {
            return await DeleteAsync(sqlQuery.Delete<T>(where));
        }

        public int Delete(Expression<Func<T, bool>> where)
        {
            return Delete(sqlQuery.Delete<T>(where));
        }

        private async Task<int> UpdateAsync(SqlCommand sqlCommand)
        {
            await sqlData.ConnectAsync();
            int affected = await sqlData.ExecuteNonQueryAsync(sqlCommand);
            sqlData.Disconnect();
            return affected;
        }

        private int Update(SqlCommand sqlCommand)
        {
            sqlData.Connect();
            int affected = sqlData.ExecuteNonQuery(sqlCommand);
            sqlData.Disconnect();
            return affected;
        }

        public async Task<int> UpdateAsync(T model, Expression<Func<T, object>> set)
        {
            return await UpdateAsync(sqlQuery.Update<T>(model, set));
        }

        public int Update(T model, Expression<Func<T, object>> set)
        {
            return Update(sqlQuery.Update<T>(model, set));
        }

        public async Task<int> UpdateAsync(T model, Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return await UpdateAsync(sqlQuery.Update<T>(model, set, where));
        }

        public int Update(T model, Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return Update(sqlQuery.Update<T>(model, set, where));
        }

        private async Task<int> InsertAsync(SqlCommand sqlCommand)
        {
            await sqlData.ConnectAsync();
            int affected = await sqlData.ExecuteNonQueryAsync(sqlCommand);
            sqlData.Disconnect();
            return affected;
        }

        private int Insert(SqlCommand sqlCommand)
        {
            sqlData.Connect();
            int affected = sqlData.ExecuteNonQuery(sqlCommand);
            sqlData.Disconnect();
            return affected;
        }

        public async Task<int> InsertAsync(T model)
        {
            return await InsertAsync(sqlQuery.Insert<T>(model));
        }

        public int Insert(T model)
        {
            return Insert(sqlQuery.Insert<T>(model));
        }

        public async Task<int> InsertAsync(T model, List<string> excludeProperties)
        {
            return await InsertAsync(sqlQuery.Insert<T>(model, excludeProperties));
        }

        public int Insert(T model, List<string> excludeProperties)
        {
            return Insert(sqlQuery.Insert<T>(model, excludeProperties));
        }

        private async Task<long> CountAsync(SqlCommand sqlCommand)
        {
            await sqlData.ConnectAsync();
            long result = long.Parse((string)await sqlData.ExecuteScalarAsync(sqlCommand));
            sqlData.Disconnect();
            return result;
        }

        private long Count(SqlCommand sqlCommand)
        {
            sqlData.Connect();
            long result = long.Parse((string)sqlData.ExecuteScalar(sqlCommand));
            sqlData.Disconnect();
            return result;
        }

        public async Task<long> CountAsync()
        {
            return await CountAsync(sqlQuery.Count<T>());
        }

        public long Count()
        {
            return Count(sqlQuery.Count<T>());
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> where)
        {
            return await CountAsync(sqlQuery.Count<T>(where));
        }

        public long Count(Expression<Func<T, bool>> where)
        {
            return Count(sqlQuery.Count<T>(where));
        }

        public async Task<long> CountAsync(string propertyName, Expression<Func<T, bool>> where)
        {
            return await CountAsync(sqlQuery.Count<T>(propertyName, where));
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
