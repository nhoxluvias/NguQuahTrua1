using MSSQL_Lite.Connection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;

namespace MSSQL_Lite.Access
{
    public partial class SqlContext
    {
        public async Task<int> ExecuteNonQueryAsync(SqlCommand sqlCommand)
        {
            ThrowExceptionOfQueryString(sqlCommand.CommandText);
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
                await sqlData.ConnectAsync();
            int affected = await sqlData.ExecuteNonQueryAsync(sqlCommand);
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return affected;
        }

        public async Task<object> ExecuteReaderAsync(SqlCommand sqlCommand)
        {
            ThrowExceptionOfQueryString(sqlCommand.CommandText);
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
                await sqlData.ConnectAsync();
            await sqlData.ExecuteReaderAsync(sqlCommand);
            object obj = sqlData.ToOriginalData();
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return obj;
        }

        public async Task<T> ExecuteReaderAsync<T>(SqlCommand sqlCommand)
        {
            ThrowExceptionOfQueryString(sqlCommand.CommandText);
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
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
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return (T)data;
        }

        public async Task<object> ExecuteScalarAsync(SqlCommand sqlCommand)
        {
            ThrowExceptionOfQueryString(sqlCommand.CommandText);
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
                await sqlData.ConnectAsync();
            object obj = await sqlData.ExecuteScalarAsync(sqlCommand);
            if (connectionType == ConnectionType.DisconnectAfterCompletion)
                sqlData.Disconnect();
            return obj;
        }
    }
}
