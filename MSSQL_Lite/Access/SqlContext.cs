﻿using MSSQL_Lite.Config;
using MSSQL_Lite.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;

namespace MSSQL_Lite.Access
{
    public partial class SqlContext : IDisposable
    {
        private bool disposedValue;
        private SqlData sqlData;

        public SqlContext()
        {
            sqlData = new SqlData();
            if (SqlConfig.connectionType == ConnectionType.ManuallyDisconnect)
                sqlData.Connect();

            disposedValue = false;
        }

        protected SqlAccess<T> InitSqlAccess<T>(ref SqlAccess<T> sqlAccess)
        {
            if (sqlAccess == null)
                sqlAccess = new SqlAccess<T>(sqlData);
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

        private int ExecuteNonQuery(SqlCommand sqlCommand)
        {
            if(SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Connect();
            int affected = sqlData.ExecuteNonQuery(sqlCommand);
            if(SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return affected;
        }

        private object ExecuteReader(SqlCommand sqlCommand)
        {
            if(SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Connect();
            sqlData.ExecuteReader(sqlCommand);
            object obj = sqlData.ToOriginalData();
            if(SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return obj;
        }

        private T ExecuteReader<T>(SqlCommand sqlCommand)
        {
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Connect();
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
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return (T)data;
        }

        private object ExecuteScalar(SqlCommand sqlCommand)
        {
            if(SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Connect();
            object obj = sqlData.ExecuteScalar(sqlCommand);
            if(SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return obj;
        }

        public int ExecuteNonQuery(string commandText, CommandType commandType)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using(SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                return ExecuteNonQuery(sqlCommand);
            }
        }

        public int ExecuteNonQuery(string commandText, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using(SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                sqlCommand.Parameters.AddRange(sqlParameters);
                return ExecuteNonQuery(sqlCommand);
            }
        }

        public object ExecuteReader(string commandText, CommandType commandType)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                return ExecuteReader(sqlCommand);
            }
        }

        public object ExecuteReader(string commandText, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                sqlCommand.Parameters.AddRange(sqlParameters);
                return ExecuteReader(sqlCommand);
            }
        }

        public T ExecuteReader<T>(string commandText, CommandType commandType)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                return ExecuteReader<T>(sqlCommand);
            }
        }

        public T ExecuteReader<T>(string commandText, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                sqlCommand.Parameters.AddRange(sqlParameters);
                return ExecuteReader<T>(sqlCommand);
            }
        }

        public object ExecuteScalar(string commandText, CommandType commandType)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                return ExecuteScalar(sqlCommand);
            }
        }

        public object ExecuteScalar(string commandText, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                sqlCommand.Parameters.AddRange(sqlParameters);
                return ExecuteScalar(sqlCommand);
            }
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
