using Data.DAL;
using MSSQL_Lite.Connection;
using System;

namespace Data.BLL
{
    public class BusinessLogicLayer : IDisposable
    {
        internal DBContext db;
        private bool disposedValue;
        protected bool includeTimestamp;

        public bool IncludeTimestamp { set { includeTimestamp = value; } }

        protected BusinessLogicLayer()
        {
            db = null;
            disposedValue = false;
        }

        internal void InitDAL()
        {
            db = new DBContext(ConnectionType.ManuallyDisconnect);
        }

        internal void InitDAL(DBContext db)
        {
            this.db = db;
        }

        public virtual void SetDefault()
        {
            includeTimestamp = false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    db.Dispose();
                    db = null;
                }
                disposedValue = true;
            }
        }

        ~BusinessLogicLayer()
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
