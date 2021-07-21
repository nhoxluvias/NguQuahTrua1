using MSSQL_Lite.Connection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;

namespace MSSQL_Lite.Access
{
    public partial class SqlContext : IDisposable
    {
        private bool disposedValue;
        private SqlData sqlData;
        private ConnectionType connectionType;

        public SqlContext(ConnectionType connectionType)
        {
            this.connectionType = connectionType;
            sqlData = new SqlData();
            if (this.connectionType == ConnectionType.ManuallyDisconnect)
                sqlData.Connect();
            disposedValue = false;
        }

        protected SqlAccess<T> InitSqlAccess<T>(ref SqlAccess<T> sqlAccess)
        {
            if (sqlAccess == null)
                sqlAccess = new SqlAccess<T>(connectionType, sqlData);
            return sqlAccess;
        }

        protected void DisposeSqlAccess<T>(ref SqlAccess<T> sqlAccess)
        {
            if (sqlAccess != null)
            {
                sqlAccess.Dispose();
                sqlAccess = null;
            }
        }

        private void ThrowExceptionOfQueryString(string queryString)
        {
            if (queryString == null)
                throw new Exception("@'queryString' must be not null");
            if (queryString == "")
                throw new Exception("@'queryString' must be not empty");
        }

        public int ExecuteNonQuery(SqlCommand sqlCommand)
        {
            ThrowExceptionOfQueryString(sqlCommand.CommandText);
            if(connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Connect();
            int affected = sqlData.ExecuteNonQuery(sqlCommand);
            if(connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return affected;
        }

        public object ExecuteReader(SqlCommand sqlCommand)
        {
            ThrowExceptionOfQueryString(sqlCommand.CommandText);
            if(connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Connect();
            sqlData.ExecuteReader(sqlCommand);
            object obj = sqlData.ToOriginalData();
            if(connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return obj;
        }

        public T ExecuteReader<T>(SqlCommand sqlCommand)
        {
            ThrowExceptionOfQueryString(sqlCommand.CommandText);
            SqlData sqlData = new SqlData();
            sqlData.ExecuteReader(sqlCommand);
            Type type = typeof(T);
            object data = null;
            if (type.Equals(typeof(List<Dictionary<string, object>>)))
            {
                data = sqlData.ToDictionaryList();
            }
            else if (type.Equals(typeof(Dictionary<string, object>)))
            {
                data = sqlData.ToDictionary();
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                Type t = type.GetGenericArguments()[0];
                MethodInfo methodInfo = sqlData.GetType().GetTypeInfo().GetDeclaredMethod("ToList");
                data = methodInfo.MakeGenericMethod(t).Invoke(sqlData, null);
            }
            else
            {
                data = sqlData.To<T>();
            }
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return (T)data;
        }

        public object ExecuteScalar(SqlCommand sqlCommand)
        {
            ThrowExceptionOfQueryString(sqlCommand.CommandText);
            if(connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Connect();
            object obj = sqlData.ExecuteScalar(sqlCommand);
            if(connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return obj;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    sqlData.Disconnect();
                    sqlData.Dispose();
                    sqlData = null;
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
