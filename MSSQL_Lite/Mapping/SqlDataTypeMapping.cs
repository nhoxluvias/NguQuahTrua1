using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL_Lite.Mapping
{
    public class SqlDataTypeMapping
    {
        public static string Map(Type type)
        {
            if (type == null)
                throw new Exception("");

            return type.Name;
        }
    }
}
