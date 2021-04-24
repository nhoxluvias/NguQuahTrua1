using MSSQL_Lite.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL_Lite.Access
{
    public class SqlAccess<T>
    {
        public SqlAccess()
        {

        }

        public async Task<List<T>> ToListAsync()
        {
            return (await SqlData.ExecuteReaderAsync(SqlQuery.Select<T>())).ToList<T>();
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where)
        {
            return (await SqlData.ExecuteReaderAsync(SqlQuery.Select<T>(where))).ToList<T>();
        }

        public async Task<T> SingleOrDefaultAsync()
        {
            return (await SqlData.ExecuteReaderAsync(SqlQuery.Select<T>(1))).To<T>();
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> where)
        {
            return (await SqlData.ExecuteReaderAsync(SqlQuery.Select<T>(where, 1))).To<T>();
        }

        public async Task<T> FirstOrDefaultAsync()
        {
            return (await SqlData.ExecuteReaderAsync(SqlQuery.Select<T>(1))).To<T>();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> where)
        {
            return (await SqlData.ExecuteReaderAsync(SqlQuery.Select<T>(where, 1))).To<T>();
        }

        public async Task<int> DeleteAsync()
        {
            return (await SqlData.ExecuteNonQueryAsync(SqlQuery.Delete<T>()));
        }

        public async Task<int> DeleteAsync(Expression<Func<T, bool>> where)
        {
            return (await SqlData.ExecuteNonQueryAsync(SqlQuery.Delete<T>(where)));
        }

        public async Task<int> UpdateAsync(T model, Expression<Func<T, object>> set)
        {
            return (await SqlData.ExecuteNonQueryAsync(SqlQuery.Update<T>(model, set)));
        }

        public async Task<int> UpdateAsync(T model, Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return (await SqlData.ExecuteNonQueryAsync(SqlQuery.Update<T>(model, set, where)));
        }

        public async Task<int> InsertAsync(T model)
        {
            return (await SqlData.ExecuteNonQueryAsync(SqlQuery.Insert<T>(model)));
        }

        public async Task<long> CountAsync()
        {
            return long.Parse((string)await SqlData.ExecuteScalarAsync(SqlQuery.Count<T>()));
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> where)
        {
            return long.Parse((string)await SqlData.ExecuteScalarAsync(SqlQuery.Count<T>(where)));
        }

        public async Task<long> CountAsync(string propertyName, Expression<Func<T, bool>> where)
        {
            return long.Parse((string)await SqlData.ExecuteScalarAsync(SqlQuery.Count<T>(propertyName, where)));
        }
    }
}
