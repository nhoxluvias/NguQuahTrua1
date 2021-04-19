using MSSQL.Reflection;
using MSSQL.Sql.Constraints;
using MSSQL.Sql.Tables;
using MSSQL.String;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;



namespace MSSQL.Sql
{
    using NamedArgument = CustomAttributeNamedArgument;
    public class SqlMapping
    {
        public static string GetTableName<T>()
        {
            CustomAttributeData customAttribute = ModelObject.GetCustomAttribute<T>("Table");
            if (customAttribute != null && customAttribute.NamedArguments.Any(n => n.MemberName == "Name"))
                return (string)customAttribute.NamedArguments.SingleOrDefault(n => n.MemberName == "Name").TypedValue.Value;
            else
                return ModelObject.GetModelName<T>();
        }

        public static string GetTableName(object obj)
        {
            CustomAttributeData customAttribute = ModelObject.GetCustomAttribute(obj, "Table");
            if (customAttribute != null && customAttribute.NamedArguments.Any(n => n.MemberName == "Name"))
                return (string)customAttribute.NamedArguments.SingleOrDefault(n => n.MemberName == "Name").TypedValue.Value;
            else
                return ModelObject.GetModelName(obj);
        }

        public static string GetTableName(PropertyInfo propertyInfo)
        {
            Type type = propertyInfo.PropertyType;
            object obj = Activator.CreateInstance(type);
            return SqlMapping.GetTableName(obj);
        }

        public static string GetPropertyName(PropertyInfo propertyInfo)
        {
            NamedArgument namedArgument = ModelObject
                .GetNamedArgument(
                    propertyInfo, 
                    c => c.AttributeType.Name == "PrimaryKey" || c.AttributeType.Name == "Column",
                    n => n.MemberName == "Name"
                );
            if (namedArgument == null)
                return propertyInfo.Name;
            else
                return (string)namedArgument.TypedValue.Value;
        }

        private static CustomAttributeData GetCustomAttribute_ColumnOrPrimaryKey(PropertyInfo property)
        {
            CustomAttributeData customAttribute = ModelObject
                    .GetCustomAttribute(
                        property,
                        c => c.AttributeType.Name == "Column" || c.AttributeType.Name == "PrimaryKey"
                    );
            if (customAttribute == null)
                throw new Exception("");
            return customAttribute;
        }

        public static List<SqlProperty> GetSqlProperties<T>()
        {
            PropertyInfo[] properties = ModelObject.GetProperties<T>(p => p.GetGetMethod().IsVirtual == false);
            if (properties == null)
                throw new Exception("@'properties' cannot be null");
            if (properties.Count() == 0)
                throw new Exception("@'properties' cannot be empty");
            List<SqlProperty> sqlProperties = new List<SqlProperty>();
            foreach (PropertyInfo property in properties)
            {
                sqlProperties.Add(GetSqlProperty<T>(property));
            }
            return sqlProperties;
        }

        public static SqlProperty GetSqlProperty<T>(PropertyInfo propertyInfo)
        {
            CustomAttributeData customAttribute = GetCustomAttribute_ColumnOrPrimaryKey(propertyInfo);
            NamedArgument namedArgument1 = ModelObject
                .GetNamedArgument(customAttribute, n => n.MemberName == "DataType");
            if (namedArgument1 == null)
                throw new Exception("");
            string identity = null;
            if (customAttribute.AttributeType.Name == "PrimaryKey")
            {
                NamedArgument namedArgument2 = ModelObject
                   .GetNamedArgument(customAttribute, n => n.MemberName == "Identity");
                if (namedArgument2 != null)
                    identity = (string)namedArgument2.TypedValue.Value;
            }
            List<SqlConstraint> sqlConstraints = GetSqlConstraints<T>(propertyInfo);
            return new SqlProperty
            {
                Name = GetPropertyName(propertyInfo),
                DataType = (string)namedArgument1.TypedValue.Value,
                Identity = identity,
                SqlConstraints = sqlConstraints
            };
        }

        public static List<SqlConstraint> GetSqlConstraints<T>(PropertyInfo propertyInfo)
        {
            List<SqlConstraint> sqlConstraints = new List<SqlConstraint>();
            CustomAttributeData[] customAttributes = ModelObject.GetCustomAttributes(propertyInfo);
            foreach (CustomAttributeData customAttribute in customAttributes)
            {
                string customAttributeName = customAttribute.AttributeType.Name;
                if (customAttributeName == "PrimaryKey")
                {
                    sqlConstraints.Add(GetSqlPrimaryKeyConstraint<T>());
                }
                if(customAttributeName == "Column")
                {
                    SqlNotNullConstraint sqlNotNullConstraint = GetSqlNotNullConstraint(customAttribute);
                    if (sqlNotNullConstraint != null)
                        sqlConstraints.Add(sqlNotNullConstraint);
                    SqlDefaultConstraint sqlDefaultConstraint = GetSqlDefaultConstraint(customAttribute);
                    if (sqlDefaultConstraint != null)
                        sqlConstraints.Add(sqlDefaultConstraint);
                    SqlUniqueConstraint sqlUniqueConstraint = GetSqlUniqueConstraint<T>(propertyInfo, customAttribute);
                    if (sqlUniqueConstraint != null)
                        sqlConstraints.Add(sqlUniqueConstraint);
                }
            }
            return sqlConstraints;
        }

        public static SqlPrimaryKeyConstraint GetSqlPrimaryKeyConstraint<T>()
        {
            return new SqlPrimaryKeyConstraint { ConstraintName = "PK_" + SqlMapping.GetTableName<T>() };
        }

        public static SqlNotNullConstraint GetSqlNotNullConstraint(CustomAttributeData customAttribute)
        {
            NamedArgument namedArgument = ModelObject.GetNamedArgument(customAttribute, c => c.MemberName == "AllowNull");
            return (namedArgument != null)
                ? new SqlNotNullConstraint { NotNull = (bool)namedArgument.TypedValue.Value }
                : null;
        }

        public static SqlDefaultConstraint GetSqlDefaultConstraint(CustomAttributeData customAttribute)
        {
            NamedArgument namedArgument = ModelObject.GetNamedArgument(customAttribute, c => c.MemberName == "Default");
            return (namedArgument != null)
                ? new SqlDefaultConstraint { DefaultValue = namedArgument.TypedValue.Value }
                : null;
        }

        public static SqlUniqueConstraint GetSqlUniqueConstraint<T>(PropertyInfo propertyInfo, CustomAttributeData customAttribute)
        {
            NamedArgument namedArgument = ModelObject.GetNamedArgument(customAttribute, c => c.MemberName == "Unique");
            return (namedArgument != null)
                ? new SqlUniqueConstraint { ConstraintName = "UNI_" +  GetTableName<T>() + "_" + GetPropertyName(propertyInfo)}
                : null;
        }

        public static List<SqlConstraint> GetSqlForeignKeyConstraints<T>()
        {
            PropertyInfo[] properties = ModelObject.GetProperties<T>(c => c.GetGetMethod().IsVirtual);
            List<SqlConstraint> sqlConstraints = new List<SqlConstraint>();
            foreach (PropertyInfo property in properties)
            {
                CustomAttributeData customAttributeData = ModelObject
                    .GetCustomAttribute(property, c => c.AttributeType.Name == "ForeignKey");
                if (customAttributeData != null)
                {
                    NamedArgument namedArgument1 = ModelObject
                        .GetNamedArgument(customAttributeData, n => n.MemberName == "PropertyName");
                    NamedArgument namedArgument2 = ModelObject
                        .GetNamedArgument(customAttributeData, n => n.MemberName == "ReferencesToProperty");
                    if (namedArgument1 == null)
                        throw new Exception("");
                    if (namedArgument2 == null)
                        throw new Exception("");
                    string toTable = SqlMapping.GetTableName(property);
                    sqlConstraints.Add(new SqlForeignKeyConstraint
                    {
                        ConstraintName = "FK_" + SqlMapping.GetTableName<T>() + "_" + toTable,
                        PropertyName = (string)namedArgument1.TypedValue.Value,
                        ToProperty = (string)namedArgument2.TypedValue.Value,
                        ToTable = toTable
                    });
                }
            }
            return sqlConstraints;
        }

        public static SqlTable GetTable<T>()
        {
            return new SqlTable
            {
                SqlProperties = SqlMapping.GetSqlProperties<T>(),
            };
        }

        public static string ToSqlQuery<T>()
        {

            return null;
        }

        public static string ConvertToStandardDataInSql<T>(object data, string propertyName)
        {
            if (data is string)
                return "\"" + HtmlContent.HtmlEncode((string)data) + "\"";
            if (data is bool)
                return ((bool)data) ? "1" : "0";
            if (data is DateTime)
            {
                PropertyInfo property = ModelObject.GetProperty<T>(propertyName);
                if (property == null)
                    throw new Exception("");
                CustomAttributeData customAttributeOfProperty = property.CustomAttributes
                    .SingleOrDefault(c => c.AttributeType.Name == "PrimaryKey" || c.AttributeType.Name == "Column");
                if (customAttributeOfProperty == null && customAttributeOfProperty.NamedArguments.Any(n => n.MemberName == "DataType"))
                    throw new Exception("");
                string typeName = (string)customAttributeOfProperty.NamedArguments
                    .SingleOrDefault(n => n.MemberName == "DataType").TypedValue.Value;
                if (typeName != "datetime" && typeName != "date")
                    throw new Exception("");
                if (typeName == "date")
                    return "\"" + ((DateTime)data).ToString("yyyy-MM-dd") + "\"";
                return "\"" + ((DateTime)data).ToString("yyyy-MM-dd HH:mm:ss") + "\"";
            }
            return data.ToString();
        }
    }
}
