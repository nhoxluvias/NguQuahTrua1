using MSSQL_Lite.Config;
using MSSQL_Lite.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;

namespace MSSQL_Lite.Access
{
    public partial class SqlContext
    {
        private async Task<int> ExecuteNonQueryAsync(SqlCommand sqlCommand)
        {
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                await sqlData.ConnectAsync();
            int affected = await sqlData.ExecuteNonQueryAsync(sqlCommand);
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return affected;
        }

        private async Task<object> ExecuteReaderAsync(SqlCommand sqlCommand)
        {
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                await sqlData.ConnectAsync();
            await sqlData.ExecuteReaderAsync(sqlCommand);
            object obj = sqlData.ToOriginalData();
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return obj;
        }

        private async Task<T> ExecuteReaderAsync<T>(SqlCommand sqlCommand)
        {
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                await sqlData.ConnectAsync();
            await sqlData.ExecuteReaderAsync(sqlCommand);
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

        private async Task<object> ExecuteScalarAsync(SqlCommand sqlCommand)
        {
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                await sqlData.ConnectAsync();
            object obj = await sqlData.ExecuteScalarAsync(sqlCommand);
            if (SqlConfig.connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return obj;
        }

        public async Task<int> ExecuteNonQueryAsync(string commandText, CommandType commandType)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                return await ExecuteNonQueryAsync(sqlCommand);
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string commandText, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                sqlCommand.Parameters.AddRange(sqlParameters);
                return await ExecuteNonQueryAsync(sqlCommand);
            }
        }

        public async Task<object> ExecuteReaderAsync(string commandText, CommandType commandType)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                return await ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<object> ExecuteReaderAsync(string commandText, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                sqlCommand.Parameters.AddRange(sqlParameters);
                return await ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<T> ExecuteReaderAsync<T>(string commandText, CommandType commandType)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                return await ExecuteReaderAsync<T>(sqlCommand);
            }
        }

        public async Task<T> ExecuteReaderAsync<T>(string commandText, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                sqlCommand.Parameters.AddRange(sqlParameters);
                return await ExecuteReaderAsync<T>(sqlCommand);
            }
        }

        public async Task<object> ExecuteScalarAsync(string commandText, CommandType commandType)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                return await ExecuteScalarAsync(sqlCommand);
            }
        }

        public async Task<object> ExecuteScalarAsync(string commandText, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            if (string.IsNullOrEmpty(commandText))
                throw new Exception("@'commandText' must not be null or empty");

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandText = commandText;
                sqlCommand.CommandType = commandType;
                sqlCommand.Parameters.AddRange(sqlParameters);
                return await ExecuteScalarAsync(sqlCommand);
            }
        }
    }
}
