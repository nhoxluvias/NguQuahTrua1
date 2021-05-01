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

        public void InitSqlCommand(SqlCommand sqlCommand)
        {
            base.Connect();
            sqlCommand.Connection = base.connection;
        }

        public async Task<int> ExecuteNonQueryAsync(SqlCommand sqlCommand)
        {
            await this.InitSqlCommandAsync(sqlCommand);
            return await sqlCommand.ExecuteNonQueryAsync();
        }

        public int ExecuteNonQuery(SqlCommand sqlCommand)
        {
            this.InitSqlCommand(sqlCommand);
            return sqlCommand.ExecuteNonQuery();
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

        public T ExecuteReader<T>(SqlCommand sqlCommand)
        {
            Type type = typeof(T);
            if (type.Name != "SqlDataReader" && type.Name != "DataSet")
                throw new Exception("Invalid type, must be @'SqlDataReader' or @'DataSet'");
            this.InitSqlCommand(sqlCommand);
            if (type.Name == "SqlDataReader")
            {
                SqlDataReader reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
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

        public object ExecuteScalar(SqlCommand sqlCommand)
        {
            this.InitSqlCommand(sqlCommand);
            return sqlCommand.ExecuteScalar();
        }

        public override void Dispose()
        {
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
