using MSSQL_Lite.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL_Lite.Config
{
    public static class SqlConfig
    {
        public static ObjectReceivingData objectReceivingData = ObjectReceivingData.DataSet;

        public static void Configure()
        {
            SqlData.objectReceivingData = objectReceivingData;
        }
    }
}
