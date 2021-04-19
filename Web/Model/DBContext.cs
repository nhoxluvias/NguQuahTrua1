using MSSQL.Sql;
using MSSQL.Sql.Access;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Web.Model
{
    public class DBContext:SqlContext
    {
        public DBContext()
            :base()
        {

        }

        public SqlAccess<Test> Tests { get { return new SqlAccess<Test>(); } }
    }
}