using System;
using System.Threading.Tasks;

namespace MSSQL_Lite.Connection
{
    public class SqlConnection
    {
        protected System.Data.SqlClient.SqlConnection connection;
        public SqlConnection(string connectionString)
        {
            this.connection = new System.Data.SqlClient.SqlConnection(connectionString);
        }

        public async Task ConnectAsync()
        {
            if (this.connection.State == System.Data.ConnectionState.Closed)
                await this.connection.OpenAsync();
        }

        public void Connect()
        {
            if (this.connection.State == System.Data.ConnectionState.Closed)
                this.connection.Open();
        }

        public void Disconnect()
        {
            if (this.connection.State == System.Data.ConnectionState.Open)
                this.connection.Close();
        }

        public virtual void Dispose()
        {
            this.connection.Dispose();
            this.connection = null;
            GC.SuppressFinalize(this);
        }
    }
}
