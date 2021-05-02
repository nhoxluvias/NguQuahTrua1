﻿using MSSQL_Lite.Reflection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MSSQL_Lite.Mapping
{
    public class SqlConvert : IDisposable
    {
        private Obj objReflection;
        private bool disposedValue;

        public SqlConvert()
        {
            objReflection = new Obj();
            disposedValue = false;
        }

        public DataTable GetDataTableFromDataSet(DataSet dataSet)
        {
            if (dataSet == null)
                throw new Exception("@'dataSet' must be not null");
            return dataSet.Tables[0];
        }

        public DataSet GetDataSetFromSqlDataAdapter(SqlDataAdapter sqlDataAdapter)
        {
            if (sqlDataAdapter == null)
                throw new Exception("@'sqlDataAdapter' must be not null");
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            return dataSet;
        }

        public Dictionary<string, object> ToDictionary(DataSet dataSet)
        {
            DataTable dataTable = GetDataTableFromDataSet(dataSet);
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

        public List<Dictionary<string, object>> ToDictionaryList(DataSet dataSet)
        {
            DataTable dataTable = GetDataTableFromDataSet(dataSet);
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

        public Dictionary<string, object> ToDictionary(SqlDataReader reader)
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

        public List<Dictionary<string, object>> ToDictionaryList(SqlDataReader reader)
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

        public List<string> GetPrefixKeys(Dictionary<string, object> pairs)
        {
            List<string> keys = new List<string>();
            foreach (string rawKey in pairs.Keys.ToList())
            {
                string key = GetPrefixKey(rawKey);
                if (!keys.Any(k => k == key))
                    keys.Add(key);
            }
            return keys;
        }

        public string GetPrefixKey(string input)
        {
            return input.Substring(0, input.IndexOf('.'));
        }

        public string GetSuffixKey(string input)
        {
            return input.Substring(input.IndexOf('.') + 1);
        }

        public T To<T>(Dictionary<string, object> pairs)
        {
            if (pairs == null)
                return default(T);
            object model = objReflection.CreateInstance<T>();
            model = (T)objReflection.SetValuesForPropertiesOfObject(model, pairs);
            if (model == null)
                return default(T);

            string pattern = "(^.)[a-zA-Z0-9]{1,}[.]{1}[a-zA-Z0-9]{1,}$";
            string bonus = "[.]{1}[a-zA-Z0-9]{1,}";
            int count = 0;
            while (count < 1)
            {
                Dictionary<string, object> subProperties = pairs.Where(p => Regex.IsMatch(p.Key, pattern))
                    .ToDictionary(p => p.Key, p => p.Value);
                List<string> prefixKeys = GetPrefixKeys(subProperties);

                foreach (string prefixKey in prefixKeys)
                {
                    PropertyInfo subPropertyInfo = objReflection.GetProperty(model, prefixKey);
                    if (subPropertyInfo != null)
                    {
                        object subModel = Activator.CreateInstance(subPropertyInfo.PropertyType);
                        Dictionary<string, object> propertiesOfSubModel = subProperties
                            .Where(p => p.Key.SubStr(0, p.Key.IndexOf('.')) == prefixKey)
                            .ToDictionary(p => GetSuffixKey(p.Key), p => p.Value);
                        subModel = objReflection.SetValuesForPropertiesOfObject(subModel, propertiesOfSubModel);
                        if (subModel != null)
                            subPropertyInfo.SetValue(model, subModel);
                    }
                }
                count++;
            }

            return (T)model;
        }

        public T To<T>(DataSet dataSet)
        {
            return To<T>(ToDictionary(dataSet));
        }

        public List<T> ToList<T>(DataSet dataSet)
        {
            List<T> list = new List<T>();
            foreach (Dictionary<string, object> pairs in ToDictionaryList(dataSet))
            {
                list.Add(To<T>(pairs));
            }
            return list;
        }

        public T To<T>(SqlDataReader reader)
        {
            return To<T>(ToDictionary(reader));
        }

        public  List<T> ToList<T>(SqlDataReader reader)
        {
            List<T> list = new List<T>();
            foreach (Dictionary<string, object> pairs in ToDictionaryList(reader))
            {
                list.Add(To<T>(pairs));
            }
            return list;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    objReflection.Dispose();
                    objReflection = null;
                }
                disposedValue = true;
            }
        }

        ~SqlConvert()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
