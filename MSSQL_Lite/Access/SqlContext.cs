using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MSSQL_Lite.Access
{
    public class SqlContext : IDisposable
    {
        private bool disposedValue;

        public SqlContext()
        {
            disposedValue = false;
        }

        private void ThrowExceptionOfQueryString(string queryString)
        {
            if (queryString == null)
                throw new Exception("@'queryString' must be not null");
            if (queryString == "")
                throw new Exception("@'queryString' must be not empty");
        }

        public async Task<int> ExecuteNonQueryAsync(SqlCommand sqlCommand)
        {
            ThrowExceptionOfQueryString(sqlCommand.CommandText);
            SqlData sqlData = new SqlData();
            await sqlData.ConnectAsync();
            int affected = await sqlData.ExecuteNonQueryAsync(sqlCommand);
            sqlData.Disconnect();
            return affected;
        }

        public int ExecuteNonQuery(SqlCommand sqlCommand)
        {
            ThrowExceptionOfQueryString(sqlCommand.CommandText);
            SqlData sqlData = new SqlData();
            sqlData.Connect();
            int affected = sqlData.ExecuteNonQuery(sqlCommand);
            sqlData.Disconnect();
            return affected;
        }

        public async Task<object> ExecuteReaderAsync(SqlCommand sqlCommand)
        {
            ThrowExceptionOfQueryString(sqlCommand.CommandText);
            SqlData sqlData = new SqlData();
            await sqlData.ConnectAsync();
            await sqlData.ExecuteReaderAsync(sqlCommand);
            object obj = null;
            sqlData.Disconnect();
            return obj;
        }

        public object ExecuteReader(SqlCommand sqlCommand)
        {
            ThrowExceptionOfQueryString(sqlCommand.CommandText);
            SqlData sqlData = new SqlData();
            sqlData.Connect();
            sqlData.ExecuteReader(sqlCommand);
            object obj = null;
            sqlData.Disconnect();
            return obj;
        }

        public async Task<object> ExecuteReaderAsync(SqlCommand sqlCommand, Type type)
        {
            ThrowExceptionOfQueryString(sqlCommand.CommandText);
            SqlData sqlData = new SqlData();
            await sqlData.ExecuteReaderAsync(sqlCommand);
            if (type == null)
                throw new Exception("@'type' must be not null");
            if (type.Equals(typeof(Dictionary<string, object>)))
                return sqlData.ToDictionary();
            else if (type.Equals(typeof(List<Dictionary<string, object>>)))
                return sqlData.ToDictionaryList();
            else
                throw new Exception("@'type' is not valid");
        }

        public object ExecuteReader(SqlCommand sqlCommand, Type type)
        {
            ThrowExceptionOfQueryString(sqlCommand.CommandText);
            SqlData sqlData = new SqlData();
            sqlData.ExecuteReader(sqlCommand);
            if (type == null)
                throw new Exception("@'type' must be not null");
            if (type.Equals(typeof(Dictionary<string, object>)))
                return sqlData.ToDictionary();
            else if (type.Equals(typeof(List<Dictionary<string, object>>)))
                return sqlData.ToDictionaryList();
            else
                throw new Exception("@'type' is not valid");
        }

        public async Task<object> ExecuteScalarAsync(SqlCommand sqlCommand)
        {
            ThrowExceptionOfQueryString(sqlCommand.CommandText);
            SqlData sqlData = new SqlData();
            await sqlData.ConnectAsync();
            object obj = await sqlData.ExecuteScalarAsync(sqlCommand);
            sqlData.Disconnect();
            return obj;
        }

        public object ExecuteScalar(SqlCommand sqlCommand)
        {
            ThrowExceptionOfQueryString(sqlCommand.CommandText);
            SqlData sqlData = new SqlData();
            sqlData.Connect();
            object obj = sqlData.ExecuteScalar(sqlCommand);
            sqlData.Disconnect();
            return obj;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }
                disposedValue = true;
            }
        }
        ~SqlContext()
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
