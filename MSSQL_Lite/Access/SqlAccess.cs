using MSSQL_Lite.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL_Lite.Access
{
    public class SqlAccess<T> : IDisposable
    {
        private SqlQuery sqlQuery;
        public SqlAccess()
        {
            sqlQuery = new SqlQuery();
        }

        public async Task<List<T>> ToListAsync()
        {
            return (await SqlData.ExecuteReaderAsync(sqlQuery.Select<T>())).ToList<T>();
        }

        public List<T> ToList()
        {
            return (SqlData.ExecuteReader(sqlQuery.Select<T>())).ToList<T>();
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where)
        {
            return (await SqlData.ExecuteReaderAsync(sqlQuery.Select<T>(where))).ToList<T>();
        }

        public List<T> ToList(Expression<Func<T, bool>> where)
        {
            return (SqlData.ExecuteReader(sqlQuery.Select<T>(where))).ToList<T>();
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set)
        {
            return (await SqlData.ExecuteReaderAsync(sqlQuery.Select<T>(set))).ToList<T>();
        }

        public List<T> ToList(Expression<Func<T, object>> set)
        {
            return (SqlData.ExecuteReader(sqlQuery.Select<T>(set))).ToList<T>();
        }

        public async Task<List<T>> ToListAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return (await SqlData.ExecuteReaderAsync(sqlQuery.Select<T>(set, where))).ToList<T>();
        }

        public List<T> ToList(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return (SqlData.ExecuteReader(sqlQuery.Select<T>(set, where))).ToList<T>();
        }

        public async Task<T> SingleOrDefaultAsync()
        {
            return (await SqlData.ExecuteReaderAsync(sqlQuery.Select<T>(1))).To<T>();
        }

        public T SingleOrDefault()
        {
            return (SqlData.ExecuteReader(sqlQuery.Select<T>(1))).To<T>();
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> where)
        {
            return (await SqlData.ExecuteReaderAsync(sqlQuery.Select<T>(where, 1))).To<T>();
        }

        public T SingleOrDefault(Expression<Func<T, bool>> where)
        {
            return (SqlData.ExecuteReader(sqlQuery.Select<T>(where, 1))).To<T>();
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, object>> set)
        {
            return (await SqlData.ExecuteReaderAsync(sqlQuery.Select<T>(set, 1))).To<T>();
        }

        public T SingleOrDefault(Expression<Func<T, object>> set)
        {
            return (SqlData.ExecuteReader(sqlQuery.Select<T>(set, 1))).To<T>();
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, object>> set ,Expression<Func<T, bool>> where)
        {
            return (await SqlData.ExecuteReaderAsync(sqlQuery.Select<T>(set, where, 1))).To<T>();
        }

        public T SingleOrDefault(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return (SqlData.ExecuteReader(sqlQuery.Select<T>(set, where, 1))).To<T>();
        }

        public async Task<T> FirstOrDefaultAsync()
        {
            return (await SqlData.ExecuteReaderAsync(sqlQuery.Select<T>(1))).To<T>();
        }

        public T FirstOrDefault()
        {
            return (SqlData.ExecuteReader(sqlQuery.Select<T>(1))).To<T>();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> where)
        {
            return (await SqlData.ExecuteReaderAsync(sqlQuery.Select<T>(where, 1))).To<T>();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> where)
        {
            return (SqlData.ExecuteReader(sqlQuery.Select<T>(where, 1))).To<T>();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, object>> set)
        {
            return (await SqlData.ExecuteReaderAsync(sqlQuery.Select<T>(set, 1))).To<T>();
        }

        public T FirstOrDefault(Expression<Func<T, object>> set)
        {
            return (SqlData.ExecuteReader(sqlQuery.Select<T>(set, 1))).To<T>();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return (await SqlData.ExecuteReaderAsync(sqlQuery.Select<T>(set, where, 1))).To<T>();
        }

        public T FirstOrDefault(Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return (SqlData.ExecuteReader(sqlQuery.Select<T>(set, where, 1))).To<T>();
        }

        public async Task<int> DeleteAsync()
        {
            return (await SqlData.ExecuteNonQueryAsync(sqlQuery.Delete<T>()));
        }

        public int Delete()
        {
            return (SqlData.ExecuteNonQuery(sqlQuery.Delete<T>()));
        }

        public async Task<int> DeleteAsync(Expression<Func<T, bool>> where)
        {
            return (await SqlData.ExecuteNonQueryAsync(sqlQuery.Delete<T>(where)));
        }

        public int Delete(Expression<Func<T, bool>> where)
        {
            return (SqlData.ExecuteNonQuery(sqlQuery.Delete<T>(where)));
        }

        public async Task<int> UpdateAsync(T model, Expression<Func<T, object>> set)
        {
            return (await SqlData.ExecuteNonQueryAsync(sqlQuery.Update<T>(model, set)));
        }

        public int Update(T model, Expression<Func<T, object>> set)
        {
            return (SqlData.ExecuteNonQuery(sqlQuery.Update<T>(model, set)));
        }

        public async Task<int> UpdateAsync(T model, Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return (await SqlData.ExecuteNonQueryAsync(sqlQuery.Update<T>(model, set, where)));
        }

        public int Update(T model, Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return (SqlData.ExecuteNonQuery(sqlQuery.Update<T>(model, set, where)));
        }

        public async Task<int> InsertAsync(T model)
        {
            return (await SqlData.ExecuteNonQueryAsync(sqlQuery.Insert<T>(model)));
        }

        public int Insert(T model)
        {
            return (SqlData.ExecuteNonQuery(sqlQuery.Insert<T>(model)));
        }

        public async Task<int> InsertAsync(T model, List<string> excludeProperties)
        {
            return (await SqlData.ExecuteNonQueryAsync(sqlQuery.Insert<T>(model, excludeProperties)));
        }

        public int Insert(T model, List<string> excludeProperties)
        {
            return (SqlData.ExecuteNonQuery(sqlQuery.Insert<T>(model, excludeProperties)));
        }

        public async Task<long> CountAsync()
        {
            return long.Parse((string)await SqlData.ExecuteScalarAsync(sqlQuery.Count<T>()));
        }

        public long Count()
        {
            return long.Parse((string)SqlData.ExecuteScalar(sqlQuery.Count<T>()));
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> where)
        {
            return long.Parse((string)await SqlData.ExecuteScalarAsync(sqlQuery.Count<T>(where)));
        }

        public long Count(Expression<Func<T, bool>> where)
        {
            return long.Parse((string)SqlData.ExecuteScalar(sqlQuery.Count<T>(where)));
        }

        public async Task<long> CountAsync(string propertyName, Expression<Func<T, bool>> where)
        {
            return long.Parse((string)await SqlData.ExecuteScalarAsync(sqlQuery.Count<T>(propertyName, where)));
        }

        public long Count(string propertyName, Expression<Func<T, bool>> where)
        {
            return long.Parse((string)SqlData.ExecuteScalar(sqlQuery.Count<T>(propertyName, where)));
        }

        public void Dispose()
        {
            this.sqlQuery = null;
            GC.SuppressFinalize(this);
        }
    }
}
