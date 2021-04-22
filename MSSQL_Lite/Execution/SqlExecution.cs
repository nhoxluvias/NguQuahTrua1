using MSSQL_Lite.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL_Lite.Execution
{
    public class SqlExecution : Connection.SqlConnection, IDisposable
    {
        private SqlCommand command;
        public SqlExecution(string connectionString)
            : base(connectionString)
        {
            command = new SqlCommand();
        }

        private async Task InitSqlCommandAsync(string commandText = null)
        {
            await base.ConnectAsync();
            command.Connection = base.connection;
            if (commandText != null)
                command.CommandText = commandText;
        }

        public async Task<int> ExecuteNonQueryAsync(string query)
        {
            await this.InitSqlCommandAsync(query);
            return await command.ExecuteNonQueryAsync();
        }

        public async Task<T> ExecuteReaderAsync<T>(string query)
        {
            Type type = typeof(T);
            if (type.Name != "SqlDataReader" && type.Name != "DataSet")
                throw new Exception("Invalid type, must be @'SqlDataReader' or @'DataSet'");
            await this.InitSqlCommandAsync(query);
            if (type.Name == "SqlDataReader")
            {
                SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                return (T)Convert.ChangeType(reader, type);
            }
            DataSet dataSet = SqlConvert.GetDataSetFromSqlDataAdapter(new SqlDataAdapter(command));
            return (T)Convert.ChangeType(dataSet, type);
        }

        public async Task<object> ExecuteScalarAsync(string query)
        {
            await this.InitSqlCommandAsync(query);
            return await command.ExecuteScalarAsync();
        }

        public override void Dispose()
        {
            base.Dispose();
            this.command.Dispose();
            this.command = null;
            GC.SuppressFinalize(this);
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
