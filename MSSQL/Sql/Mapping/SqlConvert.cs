using MSSQL.Error;
using MSSQL.Reflection;
using MSSQL.String;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MSSQL.Sql
{
    public class SqlConvert
    {
        public static DataTable GetDataTableFromDataSet(DataSet dataSet)
        {
            ModelError.IfNull(dataSet, "dataSet");
            return dataSet.Tables[0];
        }

        public static DataSet GetDataSetFromSqlDataAdapter(SqlDataAdapter sqlDataAdapter)
        {
            ModelError.IfNull(sqlDataAdapter, "sqlDataAdapter");
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            return dataSet;
        }

        public static Dictionary<string, object> ToDictionary(DataSet dataSet)
        {
            DataTable dataTable = SqlConvert.GetDataTableFromDataSet(dataSet);
            Dictionary<string, object> dict = null;
            DataRow row = dataTable.AsEnumerable().FirstOrDefault();
            if (row == null)
                return null;
            dict = new Dictionary<string, object>();
            foreach (DataColumn column in row.Table.Columns)
            {
                dict.Add(column.Caption, row[column.Caption]);
            }
            return dict;
        }

        public static List<Dictionary<string, object>> ToDictionaryList(DataSet dataSet)
        {
            DataTable dataTable = SqlConvert.GetDataTableFromDataSet(dataSet);
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow row in dataTable.AsEnumerable())
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                foreach (DataColumn column in row.Table.Columns)
                {
                    dict.Add(column.Caption, row[column.Caption]);
                }
                list.Add(dict);
            }
            return list;
        }

        public static Dictionary<string, object> ToDictionary(SqlDataReader reader)
        {
            Dictionary<string, object> dict = null;
            if (reader.Read())
            {
                dict = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dict.Add(reader.GetName(i), reader.GetValue(i));
                }
            }
            reader.Close();
            return dict;
        }

        public static List<Dictionary<string, object>> ToDictionaryList(SqlDataReader reader)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            while (reader.Read())
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dict.Add(reader.GetName(i), reader.GetValue(i));
                }
                list.Add(dict);
            }
            reader.Close();
            return list;
        }

        public static List<string> GetPrefixKeys(Dictionary<string, object> pairs)
        {
            List<string> keys = new List<string>();
            foreach (string rawKey in pairs.Keys.ToList())
            {
                string key = SqlConvert.GetPrefixKey(rawKey);
                if (!keys.Any(k => k == key))
                    keys.Add(key);
            }
            return keys;
        }

        public static string GetPrefixKey(string input)
        {
            return input.Substring(0, input.IndexOf('.'));
        }

        public static string GetSuffixKey(string input)
        {
            return input.Substring(input.IndexOf('.') + 1);
        }

        public static T To<T>(Dictionary<string, object> pairs)
        {
            if (pairs == null)
                return default(T);
            object model = ModelObject.CreateInstance<T>();
            model = (T)ModelObject.SetValuesForPropertiesOfModelObject(model, pairs);
            if (model == null)
                return default(T);

            string pattern = "(^.)[a-zA-Z0-9]{1,}[.]{1}[a-zA-Z0-9]{1,}$";
            string bonus = "[.]{1}[a-zA-Z0-9]{1,}";
            int count = 0;
            while (count < 1)
            {
                Dictionary<string, object> subProperties = pairs.Where(p => Regex.IsMatch(p.Key, pattern))
                    .ToDictionary(p => p.Key, p => p.Value);
                List<string> prefixKeys = SqlConvert.GetPrefixKeys(subProperties);

                foreach (string prefixKey in prefixKeys)
                {
                    PropertyInfo subPropertyInfo = ModelObject.GetProperty(model, prefixKey);
                    if (subPropertyInfo != null)
                    {
                        object subModel = Activator.CreateInstance(subPropertyInfo.PropertyType);
                        Dictionary<string, object> propertiesOfSubModel = subProperties
                            .Where(p => StringExtension.Substring(p.Key, 0, p.Key.IndexOf('.')) == prefixKey)
                            .ToDictionary(p => SqlConvert.GetSuffixKey(p.Key), p => p.Value);
                        subModel = ModelObject.SetValuesForPropertiesOfModelObject(subModel, propertiesOfSubModel);
                        if (subModel != null)
                            subPropertyInfo.SetValue(model, subModel);
                    }
                }
                count++;
            }

            return (T)model;
        }

        public static T To<T>(DataSet dataSet)
        {
            return SqlConvert.To<T>(SqlConvert.ToDictionary(dataSet));
        }

        public static List<T> ToList<T>(DataSet dataSet)
        {
            List<T> list = new List<T>();
            foreach (Dictionary<string, object> pairs in SqlConvert.ToDictionaryList(dataSet))
            {
                list.Add(SqlConvert.To<T>(pairs));
            }
            return list;
        }

        public static T To<T>(SqlDataReader reader)
        {
            return SqlConvert.To<T>(SqlConvert.ToDictionary(reader));
        }

        public static List<T> ToList<T>(SqlDataReader reader)
        {
            List<T> list = new List<T>();
            foreach (Dictionary<string, object> pairs in SqlConvert.ToDictionaryList(reader))
            {
                list.Add(SqlConvert.To<T>(pairs));
            }
            return list;
        }

    }
}
