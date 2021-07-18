using MSSQL_Lite.Reflection;
using System;
using System.Reflection;

namespace MSSQL_Lite.Mapping
{
    public class SqlMapping : IDisposable
    {
        private ObjReflection objReflection;
        private bool disposedValue;

        public SqlMapping()
        {
            objReflection = new ObjReflection();
            disposedValue = false;
        }

        public string GetTableName<T>(bool enclosedInSquareBrackets = false)
        {
            string objectName = objReflection.GetObjectName<T>();
            return (enclosedInSquareBrackets) ? "[" + objectName + "]" : objectName;
        }

        public string GetTableName(object obj, bool enclosedInSquareBrackets = false)
        {
            string objectName = objReflection.GetObjectName(obj);
            return (enclosedInSquareBrackets) ? "[" + objectName + "]" : objectName;
        }

        public string GetTableName(PropertyInfo propertyInfo, bool enclosedInSquareBrackets = false)
        {
            Type type = propertyInfo.PropertyType;
            object obj = Activator.CreateInstance(type);
            string objectName = GetTableName(obj);
            return (enclosedInSquareBrackets) ? "[" + objectName + "]" : objectName;
        }

        public string GetPropertyName(PropertyInfo propertyInfo, bool enclosedInSquareBrackets = false)
        {
            return (enclosedInSquareBrackets) ? "[" + propertyInfo.Name + "]" : propertyInfo.Name;
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

        ~SqlMapping()
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
