using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MSSQL.Sql.Connection
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class SqlConnection : IDisposable
    {
        protected System.Data.SqlClient.SqlConnection connection;
        public SqlConnection(string connectionString)
        {
            this.connection = new System.Data.SqlClient.SqlConnection(connectionString);
        }

        public async Task ConnectAsync()
        {
            if(this.connection.State == System.Data.ConnectionState.Closed)
                await this.connection.OpenAsync();
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

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
