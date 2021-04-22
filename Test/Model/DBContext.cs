using MSSQL_Lite.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Model
{
    public class DBContext : SqlContext
    {
        public DBContext()
            : base()
        {

        }

        public SqlAccess<District> Districts { get { return new SqlAccess<District>(); } }
    }
}
