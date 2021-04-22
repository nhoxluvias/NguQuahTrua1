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

        public async Task<T> SingleOrDefaultAsync()
        {
            return (await SqlData.ExecuteReaderAsync(SqlQuery.Select<T>(1))).To<T>();
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return (await SqlData.ExecuteReaderAsync(SqlQuery.Select<T>(1))).To<T>();
        }

        public async Task<T> FirstOrDefaultAsync()
        {
            return (await SqlData.ExecuteReaderAsync(SqlQuery.Select<T>(1))).To<T>();
        }

        public async Task<long> CountAsync()
        {
            return long.Parse((string)await SqlData.ExecuteScalarAsync(""));
        }

    }
}
