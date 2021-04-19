using MSSQL.String;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MSSQL.Reflection
{
    public partial class ModelObject
    {
        public static PropertyInfo GetProperty(object obj, string propertyName)
        {
            return ModelObject.GetProperty(obj, p => p.Name == propertyName);
        }

        public static PropertyInfo GetProperty<T>(string propertyName)
        {
            return ModelObject.GetProperty<T>(p => p.Name == propertyName);
        }

        public static PropertyInfo[] GetProperties(object obj, string customAttributeName)
        {
            return ModelObject.GetProperties(obj, c => c.AttributeType.Name == customAttributeName);
        }

        public static PropertyInfo[] GetProperties<T>(string customAttributeName)
        {
            return ModelObject.GetProperties<T>(c => c.AttributeType.Name == customAttributeName);
        }

        public static CustomAttributeData GetCustomAttribute(object obj, string customAttributeName)
        {
            return ModelObject.GetCustomAttribute(obj, c => c.AttributeType.Name == customAttributeName);
        }

        public static CustomAttributeData GetCustomAttribute<T>(string customAttributeName)
        {
            return ModelObject.GetCustomAttribute<T>(c => c.AttributeType.Name == customAttributeName);
        }

        public static CustomAttributeNamedArgument GetNamedArgument(PropertyInfo propertyInfo, string memberName)
        {
            CustomAttributeData customAttributeData = propertyInfo.CustomAttributes
                    .SingleOrDefault(c => c.AttributeType.Name == "Column" || c.AttributeType.Name == "PrimaryKey");
            if (customAttributeData != null)
                throw new Exception("");

            return customAttributeData.NamedArguments.SingleOrDefault(n => n.MemberName == memberName);
        }

        public static object SetValuesForPropertiesOfModelObject(object model, Dictionary<string, object> pairs)
        {
            Type type = model.GetType();
            if(type.IsValueType || type.Name == "String")
                return null;
            Dictionary<string, object> properties = pairs.Where(p => Regex.IsMatch(p.Key, "(^.)[a-zA-Z0-9]{1,}$"))
                .ToDictionary(p => p.Key, p => p.Value);
            foreach (KeyValuePair<string, object> property in properties)
            {
                PropertyInfo propertyInfo = ModelObject.GetProperty(model, property.Key);
                if (propertyInfo != null)
                    propertyInfo.SetValue(model, property.Value);
            }
            return model;
        }


    }
}
