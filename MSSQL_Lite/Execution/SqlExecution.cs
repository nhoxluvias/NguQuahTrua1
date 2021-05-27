using MSSQL_Lite.Mapping;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MSSQL_Lite.Execution
{
    internal class SqlExecution : Connection.SqlConnection
    {
        private SqlConvert sqlConvert;
        private bool disposed;

        public SqlExecution(string connectionString)
            : base(connectionString)
        {
            sqlConvert = new SqlConvert();
            disposed = false;
        }

        public void InitSqlCommand(SqlCommand sqlCommand)
        {
            sqlCommand.Connection = base.connection;
        }

        public async Task<int> ExecuteNonQueryAsync(SqlCommand sqlCommand)
        {
            this.InitSqlCommand(sqlCommand);
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
            this.InitSqlCommand(sqlCommand);
            if (type.Name == "SqlDataReader")
            {
                SqlDataReader reader = await sqlCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                return (T)Convert.ChangeType(reader, type);
            }
            DataSet dataSet = sqlConvert.GetDataSetFromSqlDataAdapter(new SqlDataAdapter(sqlCommand));
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
            DataSet dataSet = sqlConvert.GetDataSetFromSqlDataAdapter(new SqlDataAdapter(sqlCommand));
            return (T)Convert.ChangeType(dataSet, type);
        }

        public async Task<object> ExecuteScalarAsync(SqlCommand sqlCommand)
        {
            this.InitSqlCommand(sqlCommand);
            return await sqlCommand.ExecuteScalarAsync();
        }

        public object ExecuteScalar(SqlCommand sqlCommand)
        {
            this.InitSqlCommand(sqlCommand);
            return sqlCommand.ExecuteScalar();
        }
        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                try
                {
                    if (disposing)
                    {
                        sqlConvert.Dispose();
                        sqlConvert = null;
                    }
                    this.disposed = true;
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
    }
}
