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
            CustomAttribute customAttribute = Obj.GetCustomAttribute<T>("Table");
            if (customAttribute != null && customAttribute.NamedArguments.Any(n => n.MemberName == "Name"))
                return (string)customAttribute.NamedArguments.SingleOrDefault(n => n.MemberName == "Name").TypedValue.Value;
            else
                return Obj.GetModelName<T>();
        }

        public static string GetTableName(object obj)
        {
            CustomAttribute customAttribute = Obj.GetCustomAttribute(obj, "Table");
            if (customAttribute != null && customAttribute.NamedArguments.Any(n => n.MemberName == "Name"))
                return (string)customAttribute.NamedArguments.SingleOrDefault(n => n.MemberName == "Name").TypedValue.Value;
            else
                return Obj.GetModelName(obj);
        }

        public static string GetTableName(PropertyInfo propertyInfo)
        {
            Type type = propertyInfo.PropertyType;
            object obj = Activator.CreateInstance(type);
            return GetTableName(obj);
        }
    }
}
