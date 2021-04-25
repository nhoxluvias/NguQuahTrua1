using MSSQL_Lite.Reflection;
using MSSQL_Lite.String;
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
        public static string GetTableName<T>(bool enclosedInSquareBrackets = false)
        {
            string objectName = Obj.GetObjectName<T>();
            return (enclosedInSquareBrackets) ? "[" + objectName + "]" : objectName;
        }

        public static string GetTableName(object obj, bool enclosedInSquareBrackets = false)
        {
            string objectName = Obj.GetObjectName(obj);
            return (enclosedInSquareBrackets) ? "[" + objectName + "]" : objectName;
        }

        public static string GetTableName(PropertyInfo propertyInfo, bool enclosedInSquareBrackets = false)
        {
            Type type = propertyInfo.PropertyType;
            object obj = Activator.CreateInstance(type);
            string objectName = GetTableName(obj);
            return (enclosedInSquareBrackets) ? "[" + objectName + "]" : objectName;
        }

        public static string GetPropertyName(PropertyInfo propertyInfo, bool enclosedInSquareBrackets = false)
        {
            return (enclosedInSquareBrackets) ? "[" + propertyInfo.Name + "]" : propertyInfo.Name;
        }
    }
}
