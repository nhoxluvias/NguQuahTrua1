using MSSQL_Lite.Connection;
using MSSQL_Lite.Execution;
using MSSQL_Lite.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL_Lite.Access
{
    public class SqlData : IDisposable
    {
        private object data;
        private static SqlExecution sqlExecution = null;
        internal SqlData(object data)
        {
            this.data = data;
        }

        private static void InitSqlExecution()
        {
            SqlData.sqlExecution = new SqlExecution(SqlConnectInfo.GetConnectionString());
        }

        private static void DestroySqlExecution()
        {
            SqlData.sqlExecution.Dispose();
            SqlData.sqlExecution = null;
        }

        public static async Task<SqlData> ExecuteReaderAsync(string queryString, bool newConnection = false)
        {
            if (newConnection)
            {
                SqlData.DestroySqlExecution();
                SqlData.InitSqlExecution();
            }
            if (SqlData.sqlExecution == null)
                SqlData.InitSqlExecution();
            SqlDataReader reader = await SqlData.sqlExecution.ExecuteReaderAsync<SqlDataReader>(queryString);
            return new SqlData(reader);
        }

        public static async Task<int> ExecuteNonQueryAsync(string queryString, bool newConnection = false)
        {
            if (newConnection)
            {
                SqlData.DestroySqlExecution();
                SqlData.InitSqlExecution();
            }
            if (SqlData.sqlExecution == null)
                SqlData.InitSqlExecution();
            return await SqlData.sqlExecution.ExecuteNonQueryAsync(queryString);
        }

        public static async Task<object> ExecuteScalarAsync(string queryString, bool newConnection = false)
        {
            if (newConnection)
            {
                SqlData.DestroySqlExecution();
                SqlData.InitSqlExecution();
            }
            if (SqlData.sqlExecution == null)
                SqlData.InitSqlExecution();
            return await SqlData.sqlExecution.ExecuteScalarAsync(queryString);
        }

        public Dictionary<string, object> ToDictionary()
        {
            if (!(data is DataSet) && !(data is SqlDataReader))
                throw new Exception("@'data' must be DataSet or SqlDataReader");
            if (data is DataSet)
                return SqlConvert.ToDictionary((DataSet)data);
            return SqlConvert.ToDictionary((SqlDataReader)data);
        }

        public List<Dictionary<string, object>> ToDictionaryList()
        {
            if (!(data is DataSet) && !(data is SqlDataReader))
                throw new Exception("@'data' must be DataSet or SqlDataReader");
            if (data is DataSet)
                return SqlConvert.ToDictionaryList((DataSet)data);
            return SqlConvert.ToDictionaryList((SqlDataReader)data);
        }

        public T To<T>()
        {
            if (!(data is DataSet) && !(data is SqlDataReader))
                throw new Exception("@'data' must be DataSet or SqlDataReader");
            if (data is DataSet)
                return SqlConvert.To<T>((DataSet)data);
            return SqlConvert.To<T>((SqlDataReader)data);
        }

        public List<T> ToList<T>()
        {
            if (!(data is DataSet) && !(data is SqlDataReader))
                throw new Exception("@'data' must be DataSet or SqlDataReader");
            if (data is DataSet)
                return SqlConvert.ToList<T>((DataSet)data);
            return SqlConvert.ToList<T>((SqlDataReader)data);
        }

        public void Dispose()
        {
            this.data = null;
            GC.SuppressFinalize(this);
        }
    }
}
