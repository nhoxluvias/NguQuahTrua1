using System;
using System.Threading.Tasks;

namespace MSSQL_Lite.Connection
{
    internal class SqlConnection : IDisposable
    {
        protected System.Data.SqlClient.SqlConnection connection;
        private bool disposedValue;

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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    connection.Dispose();
                    connection = null;
                }
                disposedValue = true;
            }
        }
        ~SqlConnection()
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
