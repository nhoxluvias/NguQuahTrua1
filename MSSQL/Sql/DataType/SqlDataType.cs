using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL.Sql
{
    public class SqlDataType
    {
        public static string Int = "Int";
        public static string BigInt = "BigInt";
        public static string Float = "Float";
        public static string DateTime = "DateTime";
        public static string Date = "Date";
        public static string NText = "NText";
        public static string Bit = "Bit";

        public static string NVarChar(int length)
        {
            return "Nvarchar(" + length + ")";
        }

        public static string VarChar(int length)
        {
            return "Varchar(" + length + ")";
        }

        public static string Char(int length)
        {
            return "Char(" + length + ")";
        }

    }
}
