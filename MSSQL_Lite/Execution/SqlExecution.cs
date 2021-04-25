using MSSQL_Lite.Mapping;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MSSQL_Lite.Execution
{
    public class SqlExecution : Connection.SqlConnection, IDisposable
    {
        public SqlExecution(string connectionString)
            : base(connectionString)
        {

        }

        private async Task InitSqlCommandAsync(SqlCommand sqlCommand)
        {
            await base.ConnectAsync();
            sqlCommand.Connection = base.connection;
        }

        public async Task<int> ExecuteNonQueryAsync(SqlCommand sqlCommand)
        {
            await this.InitSqlCommandAsync(sqlCommand);
            return await sqlCommand.ExecuteNonQueryAsync();
        }

        public async Task<T> ExecuteReaderAsync<T>(SqlCommand sqlCommand)
        {
            Type type = typeof(T);
            if (type.Name != "SqlDataReader" && type.Name != "DataSet")
                throw new Exception("Invalid type, must be @'SqlDataReader' or @'DataSet'");
            await this.InitSqlCommandAsync(sqlCommand);
            if (type.Name == "SqlDataReader")
            {
                SqlDataReader reader = await sqlCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                return (T)Convert.ChangeType(reader, type);
            }
            DataSet dataSet = SqlConvert.GetDataSetFromSqlDataAdapter(new SqlDataAdapter(sqlCommand));
            return (T)Convert.ChangeType(dataSet, type);
        }

        public async Task<object> ExecuteScalarAsync(SqlCommand sqlCommand)
        {
            await this.InitSqlCommandAsync(sqlCommand);
            return await sqlCommand.ExecuteScalarAsync();
        }

        public override void Dispose()
        {
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
