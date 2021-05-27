using Data.DAL;
using MSSQL_Lite.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BLL
{
    public abstract class BusinessLogicLayer : IDisposable
    {
        internal DBContext db;
        private bool disposedValue;

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
