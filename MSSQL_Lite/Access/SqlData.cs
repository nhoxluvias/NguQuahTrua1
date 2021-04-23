using MSSQL_Lite.Connection;
using MSSQL_Lite.Execution;
using MSSQL_Lite.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MSSQL_Lite.Access
{
    public class SqlData : IDisposable
    {
        private object data;
        private static SqlExecution sqlExecution = null;
        public static ObjectReceivingData objectReceivingData = ObjectReceivingData.SqlDataReader;

        internal SqlData(object data)
        {
            this.data = data;
        }

        private static void InitSqlExecution()
        {
           sqlExecution = new SqlExecution(SqlConnectInfo.GetConnectionString());
        }

        private static void DestroySqlExecution()
        {
            sqlExecution.Dispose();
            sqlExecution = null;
        }

        public static async Task<SqlData> ExecuteReaderAsync(string queryString, bool newConnection = false)
        {
            if (newConnection)
            {
                DestroySqlExecution();
                InitSqlExecution();
            }
            if (sqlExecution == null)
                InitSqlExecution();
            if (objectReceivingData == ObjectReceivingData.SqlDataReader)
                return new SqlData(await sqlExecution.ExecuteReaderAsync<SqlDataReader>(queryString));
            return new SqlData(await sqlExecution.ExecuteReaderAsync<DataSet>(queryString));
        }

        public static async Task<int> ExecuteNonQueryAsync(string queryString, bool newConnection = false)
        {
            if (newConnection)
            {
                DestroySqlExecution();
                InitSqlExecution();
            }
            if (sqlExecution == null)
                InitSqlExecution();
            return await sqlExecution.ExecuteNonQueryAsync(queryString);
        }

        public static async Task<object> ExecuteScalarAsync(string queryString, bool newConnection = false)
        {
            if (newConnection)
            {
                DestroySqlExecution();
                InitSqlExecution();
            }
            if (sqlExecution == null)
                InitSqlExecution();
            return await sqlExecution.ExecuteScalarAsync(queryString);
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
