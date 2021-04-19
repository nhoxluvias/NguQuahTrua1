using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL.Reflection
{
    public partial class ModelObject
    {
        public static T CreateInstance<T>()
        {
            return (T)Activator.CreateInstance(typeof(T));
        }

        public static string GetModelName(object obj){
            return obj.GetType().Name;
        }

        public static string GetModelName<T>(){
            return typeof(T).Name;
        }

        public static CustomAttributeData[] GetCustomAttributes(object obj){
            return obj.GetType().CustomAttributes.ToArray();
        }

        public static CustomAttributeData[] GetCustomAttributes<T>()
        {
            return typeof(T).CustomAttributes.ToArray();
        }

        public static CustomAttributeData GetCustomAttribute(object obj, Func<CustomAttributeData, bool> predicate)
        {
            return ModelObject.GetCustomAttributes(obj).SingleOrDefault(predicate);
        }

        public static CustomAttributeData[] GetCustomAttributes(object obj, Func<CustomAttributeData, bool> predicate)
        {
            return ModelObject.GetCustomAttributes(obj).Where(predicate).ToArray();
        }

        public static CustomAttributeData GetCustomAttribute<T>(Func<CustomAttributeData, bool> predicate)
        {
            return ModelObject.GetCustomAttributes<T>().SingleOrDefault(predicate);
        }

        public static CustomAttributeData[] GetCustomAttributes<T>(Func<CustomAttributeData, bool> predicate)
        {
            return ModelObject.GetCustomAttributes<T>().Where(predicate).ToArray();
        }

        public static CustomAttributeData[] GetCustomAttributes(PropertyInfo propertyInfo, Func<CustomAttributeData, bool> predicate)
        {
            if (propertyInfo == null)
                throw new Exception("");
            return propertyInfo.CustomAttributes.Where(predicate).ToArray();
        }

        public static CustomAttributeData[] GetCustomAttributes(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new Exception("");
            return propertyInfo.CustomAttributes.ToArray();
        }

        public static CustomAttributeData GetCustomAttribute(PropertyInfo propertyInfo, Func<CustomAttributeData, bool> predicate)
        {
            if (propertyInfo == null)
                throw new Exception("");
            return propertyInfo.CustomAttributes.SingleOrDefault(predicate);
        }

        public static CustomAttributeNamedArgument[] GetNamedArguments(object obj, Func<CustomAttributeData, bool> predicate)
        {
            CustomAttributeData customAttributeData = ModelObject.GetCustomAttribute(obj, predicate);
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.ToArray();
        }

        public static CustomAttributeNamedArgument[] GetNamedArguments(object obj, Func<CustomAttributeData, bool> predicate1, Func<CustomAttributeNamedArgument, bool> predicate2)
        {
            CustomAttributeData customAttributeData = ModelObject.GetCustomAttribute(obj, predicate1);
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.Where(predicate2).ToArray();
        }

        public static CustomAttributeNamedArgument[] GetNamedArguments<T>(Func<CustomAttributeData, bool> predicate)
        {
            CustomAttributeData customAttributeData = ModelObject.GetCustomAttribute<T>(predicate);
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.ToArray();
        }

        public static CustomAttributeNamedArgument[] GetNamedArguments<T>(Func<CustomAttributeData, bool> predicate1, Func<CustomAttributeNamedArgument, bool> predicate2)
        {
            CustomAttributeData customAttributeData = ModelObject.GetCustomAttribute<T>(predicate1);
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.Where(predicate2).ToArray();
        }

        public static CustomAttributeNamedArgument GetNamedArgument(object obj, Func<CustomAttributeData, bool> predicate1, Func<CustomAttributeNamedArgument, bool> predicate2)
        {
            CustomAttributeData customAttributeData = ModelObject.GetCustomAttribute(obj, predicate1);
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.SingleOrDefault(predicate2);
        }

        public static CustomAttributeNamedArgument GetNamedArgument<T>(Func<CustomAttributeData, bool> predicate1, Func<CustomAttributeNamedArgument, bool> predicate2)
        {
            CustomAttributeData customAttributeData = ModelObject.GetCustomAttribute<T>(predicate1);
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.SingleOrDefault(predicate2);
        }

        public static CustomAttributeNamedArgument[] GetNamedArguments(CustomAttributeData customAttributeData)
        {
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.ToArray();
        }

        public static CustomAttributeNamedArgument[] GetNamedArguments(CustomAttributeData customAttributeData, Func<CustomAttributeNamedArgument, bool> predicate)
        {
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.Where(predicate).ToArray();
        }

        public static CustomAttributeNamedArgument GetNamedArgument(CustomAttributeData customAttributeData, Func<CustomAttributeNamedArgument, bool> predicate)
        {
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.SingleOrDefault(predicate);
        }

        public static CustomAttributeNamedArgument[] GetNamedArguments(PropertyInfo propertyInfo, Func<CustomAttributeData, bool> predicate)
        {
            CustomAttributeData customAttributeData = ModelObject.GetCustomAttribute(propertyInfo, predicate);
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.ToArray();
        }

        public static CustomAttributeNamedArgument[] GetNamedArguments(PropertyInfo propertyInfo, Func<CustomAttributeData, bool> predicate1, Func<CustomAttributeNamedArgument, bool> predicate2)
        {
            CustomAttributeData customAttributeData = ModelObject.GetCustomAttribute(propertyInfo, predicate1);
            if (customAttributeData == null)
                throw new Exception("");
            return customAttributeData.NamedArguments.Where(predicate2).ToArray();
        }

        public static CustomAttributeNamedArgument GetNamedArgument(PropertyInfo propertyInfo, Func<CustomAttributeData, bool> predicate1, Func<CustomAttributeNamedArgument, bool> predicate2)
        {
            CustomAttributeData customAttributeData = ModelObject.GetCustomAttribute(propertyInfo, predicate1);
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
            return ModelObject.GetProperties(obj).Where(predicate).ToArray();
        }

        public static PropertyInfo[] GetProperties<T>(Func<PropertyInfo, bool> predicate)
        {
            return ModelObject.GetProperties<T>().Where(predicate).ToArray();
        }

        public static PropertyInfo GetProperty(object obj, Func<PropertyInfo, bool> predicate)
        {
            return ModelObject.GetProperties(obj).SingleOrDefault(predicate);
        }

        public static PropertyInfo GetProperty<T>(Func<PropertyInfo, bool> predicate)
        {
            return ModelObject.GetProperties<T>().SingleOrDefault(predicate);
        }

        public static PropertyInfo[] GetProperties(object obj, Func<CustomAttributeData, bool> predicate)
        {
            return ModelObject.GetProperties(obj, p => p.CustomAttributes.Any(predicate));
        }

        public static PropertyInfo[] GetProperties<T>(Func<CustomAttributeData, bool> predicate)
        {
            return ModelObject.GetProperties<T>(p => p.CustomAttributes.Any(predicate));
        }
    }
}
