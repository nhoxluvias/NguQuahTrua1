using Data.DAL;
using MSSQL_Lite.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BLL
{
    public abstract class BusinessLogicLayer
    {
        internal DBContext db;

        protected BusinessLogicLayer()
        {
            db = null;
        }

        internal void InitDAL()
        {
            db = new DBContext(ConnectionType.ManuallyDisconnect);
        }

        internal void InitDAL(DBContext db)
        {
            this.db = db;
        }
    }
}
