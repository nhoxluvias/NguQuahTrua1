using MSSQL_Lite.Access;
using MSSQL_Lite.Connection;

namespace Test.DAL
{
    internal class DBContext : SqlContext
    {
        private bool disposed;
        private SqlAccess<Film> films;

        public DBContext(ConnectionType connectionType)
            : base(connectionType)
        {
            films = null;
            disposed = false;
        }

        public SqlAccess<Film> Films { get { return InitSqlAccess<Film>(ref films); } }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                try
                {
                    if (disposing)
                    {
                        DisposeSqlAccess<Film>(ref films);
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
