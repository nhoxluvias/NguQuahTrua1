using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL.Reflection
{
    using NamedArgument = CustomAttributeNamedArgument;
    using CustomAttribute = CustomAttributeData;
    public partial class ModelObject
    {
        public static T CreateInstance<T>()
        {
            return (T)Activator.CreateInstance(typeof(T));
        }

        public static string GetModelName(object obj)
        {
            return obj.GetType().Name;
        }

        public static string GetModelName<T>()
        {
            return typeof(T).Name;
        }

        public static CustomAttribute[] GetCustomAttributes(object obj)
        {
            return obj.GetType().CustomAttributes.ToArray();
        }

        public static CustomAttribute[] GetCustomAttributes<T>()
        {
            return typeof(T).CustomAttributes.ToArray();
        }

        public static CustomAttribute GetCustomAttribute(object obj, Func<CustomAttribute, bool> predicate)
        {
            return GetCustomAttributes(obj).SingleOrDefault(predicate);
        }

        public static CustomAttribute[] GetCustomAttributes(object obj, Func<CustomAttribute, bool> predicate)
        {
            return GetCustomAttributes(obj).Where(predicate).ToArray();
        }

        public static CustomAttribute GetCustomAttribute<T>(Func<CustomAttribute, bool> predicate)
        {
            return GetCustomAttributes<T>().SingleOrDefault(predicate);
        }

        public static CustomAttribute[] GetCustomAttributes<T>(Func<CustomAttribute, bool> predicate)
        {
            return GetCustomAttributes<T>().Where(predicate).ToArray();
        }

        public static CustomAttribute[] GetCustomAttributes
            (PropertyInfo propertyInfo, Func<CustomAttribute, bool> predicate, bool throwExceptionIfNull = true)
        {
            if (propertyInfo == null)
            {
                if (throwExceptionIfNull)
                    throw new Exception("@'propertyInfo' must be not null");
                return null;
            }
            return propertyInfo.CustomAttributes.Where(predicate).ToArray();
        }

        public static CustomAttribute[] GetCustomAttributes(PropertyInfo propertyInfo, bool throwExceptionIfNull = true)
        {
            if (propertyInfo == null)
            {
                if (throwExceptionIfNull)
                    throw new Exception("@'propertyInfo' must be not null");
                return null;
            }
            return propertyInfo.CustomAttributes.ToArray();
        }

        public static CustomAttribute GetCustomAttribute
            (PropertyInfo propertyInfo, Func<CustomAttribute, bool> predicate, bool throwExceptionIfNull = true)
        {
            if (propertyInfo == null)
            {
                if (throwExceptionIfNull)
                    throw new Exception("@'propertyInfo' must be not null");
                return null;
            }
            return propertyInfo.CustomAttributes.SingleOrDefault(predicate);
        }

        public static NamedArgument[] GetNamedArguments
            (object obj, Func<CustomAttribute, bool> predicate, bool throwExceptionIfNull = true)
        {
            CustomAttribute customAttributeData = GetCustomAttribute(obj, predicate);
            if (customAttributeData == null)
            {
                if (throwExceptionIfNull)
                    throw new Exception("@'customAttributeData' must be not null");
                return null;
            }
            return customAttributeData.NamedArguments.ToArray();
        }

        public static NamedArgument[] GetNamedArguments
            (object obj, Func<CustomAttribute, bool> predicate1, Func<CustomAttributeNamedArgument, bool> predicate2, bool throwExceptionIfNull = true)
        {
            CustomAttribute customAttributeData = GetCustomAttribute(obj, predicate1);
            if (customAttributeData == null)
            {
                if (throwExceptionIfNull)
                    throw new Exception("@'customAttributeData' must be not null");
                return null;
            }
            return customAttributeData.NamedArguments.Where(predicate2).ToArray();
        }

        public static NamedArgument[] GetNamedArguments<T>
            (Func<CustomAttribute, bool> predicate, bool throwExceptionIfNull = true)
        {
            CustomAttribute customAttributeData = GetCustomAttribute<T>(predicate);
            if (customAttributeData == null)
            {
                if (throwExceptionIfNull)
                    throw new Exception("@'customAttributeData' must be not null");
                return null;
            }
            return customAttributeData.NamedArguments.ToArray();
        }

        public static NamedArgument[] GetNamedArguments<T>
            (Func<CustomAttribute, bool> predicate1, Func<NamedArgument, bool> predicate2, bool throwExceptionIfNull = true)
        {
            CustomAttribute customAttributeData = GetCustomAttribute<T>(predicate1);
            if (customAttributeData == null)
            {
                if (throwExceptionIfNull)
                    throw new Exception("@'customAttributeData' must be not null");
                return null;
            }
            return customAttributeData.NamedArguments.Where(predicate2).ToArray();
        }

        public static NamedArgument GetNamedArgument
            (object obj, Func<CustomAttributeData, bool> predicate1, Func<NamedArgument, bool> predicate2, bool throwExceptionIfNull = true)
        {
            CustomAttribute customAttributeData = GetCustomAttribute(obj, predicate1);
            if (customAttributeData == null)
            {
                if (throwExceptionIfNull)
                    throw new Exception("@'customAttributeData' must be not null");
                return default(NamedArgument);
            }
            return customAttributeData.NamedArguments.SingleOrDefault(predicate2);
        }

        public static NamedArgument GetNamedArgument<T>(Func<CustomAttribute, bool> predicate1, Func<NamedArgument, bool> predicate2)
        {
            CustomAttribute customAttributeData = GetCustomAttribute<T>(predicate1);
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.SingleOrDefault(predicate2);
        }

        public static NamedArgument[] GetNamedArguments(CustomAttribute customAttributeData)
        {
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.ToArray();
        }

        public static NamedArgument[] GetNamedArguments(CustomAttribute customAttributeData, Func<NamedArgument, bool> predicate)
        {
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.Where(predicate).ToArray();
        }

        public static NamedArgument GetNamedArgument(CustomAttribute customAttributeData, Func<NamedArgument, bool> predicate)
        {
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.SingleOrDefault(predicate);
        }

        public static NamedArgument[] GetNamedArguments(PropertyInfo propertyInfo, Func<CustomAttribute, bool> predicate)
        {
            CustomAttribute customAttributeData = GetCustomAttribute(propertyInfo, predicate);
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.ToArray();
        }

        public static NamedArgument[] GetNamedArguments(PropertyInfo propertyInfo, Func<CustomAttribute, bool> predicate1, Func<CustomAttributeNamedArgument, bool> predicate2)
        {
            CustomAttribute customAttributeData = GetCustomAttribute(propertyInfo, predicate1);
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.Where(predicate2).ToArray();
        }

        public static NamedArgument GetNamedArgument(PropertyInfo propertyInfo, Func<CustomAttribute, bool> predicate1, Func<NamedArgument, bool> predicate2)
        {
            CustomAttribute customAttributeData = GetCustomAttribute(propertyInfo, predicate1);
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.SingleOrDefault(predicate2);
        }

        public static PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }

        public static PropertyInfo[] GetProperties<T>()
        {
            return typeof(T).GetProperties();
        }

        public static PropertyInfo[] GetProperties(object obj, Func<PropertyInfo, bool> predicate)
        {
            return GetProperties(obj).Where(predicate).ToArray();
        }

        public static PropertyInfo[] GetProperties<T>(Func<PropertyInfo, bool> predicate)
        {
            return GetProperties<T>().Where(predicate).ToArray();
        }

        public static PropertyInfo GetProperty(object obj, Func<PropertyInfo, bool> predicate)
        {
            return GetProperties(obj).SingleOrDefault(predicate);
        }

        public static PropertyInfo GetProperty<T>(Func<PropertyInfo, bool> predicate)
        {
            return GetProperties<T>().SingleOrDefault(predicate);
        }

        public static PropertyInfo[] GetProperties(object obj, Func<CustomAttribute, bool> predicate)
        {
            return GetProperties(obj, p => p.CustomAttributes.Any(predicate));
        }

        public static PropertyInfo[] GetProperties<T>(Func<CustomAttribute, bool> predicate)
        {
            return GetProperties<T>(p => p.CustomAttributes.Any(predicate));
        }
    }
}
