using MSSQL_Lite.Connection;
using MSSQL_Lite.Execution;
using MSSQL_Lite.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MSSQL_Lite.Access
{
    internal partial class SqlData : SqlExecution
    {
        private object data;
        private bool disposed;
        public static ObjectReceivingData objectReceivingData = ObjectReceivingData.SqlDataReader;

        public SqlData()
            : base(SqlConnectInfo.GetConnectionString())
        {
            data = null;
            disposed = false;
        }

        public void ExecuteReader(SqlCommand sqlCommand)
        {
            if (objectReceivingData == ObjectReceivingData.SqlDataReader)
                data = ExecuteReader<SqlDataReader>(sqlCommand);
            else
                data = ExecuteReader<DataSet>(sqlCommand);
        }


        public Dictionary<string, object> ToDictionary()
        {
            if (!(data is DataSet) && !(data is SqlDataReader))
                throw new Exception("@'data' must be DataSet or SqlDataReader");
            if (data is DataSet)
                return sqlConvert.ToDictionary((DataSet)data);
            return sqlConvert.ToDictionary((SqlDataReader)data);
        }

        public List<Dictionary<string, object>> ToDictionaryList()
        {
            if (!(data is DataSet) && !(data is SqlDataReader))
                throw new Exception("@'data' must be DataSet or SqlDataReader");
            if (data is DataSet)
                return sqlConvert.ToDictionaryList((DataSet)data);
            return sqlConvert.ToDictionaryList((SqlDataReader)data);
        }

        public T To<T>()
        {
            if (!(data is DataSet) && !(data is SqlDataReader))
                throw new Exception("@'data' must be DataSet or SqlDataReader");
            if (data is DataSet)
                return sqlConvert.To<T>((DataSet)data);
            return sqlConvert.To<T>((SqlDataReader)data);
        }

        public List<T> ToList<T>()
        {
            if (!(data is DataSet) && !(data is SqlDataReader))
                throw new Exception("@'data' must be DataSet or SqlDataReader");
            if (data is DataSet)
                return sqlConvert.ToList<T>((DataSet)data);
            return sqlConvert.ToList<T>((SqlDataReader)data);
        }

        public object ToOriginalData()
        {
            return data;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                try
                {
                    if (disposing)
                    {
                        if (data is DataSet)
                            ((DataSet)data).Dispose();
                        if (data is SqlDataReader)
                            ((DataSet)data).Dispose();
                    }
                    disposed = true;
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
    }
}
