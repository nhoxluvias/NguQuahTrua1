using MSSQL_Lite.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL_Lite.Mapping
{
    using CustomAttribute = CustomAttributeData;
    public class SqlMapping
    {
        public static string GetTableName<T>()
        {
            return Obj.GetModelName<T>();
        }

        public static string GetTableName(object obj)
        {
            return Obj.GetModelName(obj);
        }

        public static string GetTableName(PropertyInfo propertyInfo)
        {
            Type type = propertyInfo.PropertyType;
            object obj = Activator.CreateInstance(type);
            return GetTableName(obj);
        }

        public static string GetPropertyName(PropertyInfo propertyInfo)
        {
            return propertyInfo.Name;
        }

        public static string ConvertToStandardDataInSql<T>(object value)
        {
            if (value == null)
                return "NULL";
            if (value is string)
                return (string)value;
            if (value is bool)
                return ((bool)value) ? "1" : "0";

            if(value is DateTime)
            {
                return null;
            }
            return null;
        }
    }
}
