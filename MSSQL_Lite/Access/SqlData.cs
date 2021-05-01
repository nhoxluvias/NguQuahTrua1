﻿using MSSQL_Lite.Connection;
using MSSQL_Lite.Execution;
using MSSQL_Lite.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MSSQL_Lite.Access
{
    public class SqlData : IDisposable
    {
        private object data;
        private SqlExecution sqlExecution = null;
        public static ObjectReceivingData objectReceivingData = ObjectReceivingData.SqlDataReader;

        public SqlData()
        {
            data = null;
            sqlExecution = new SqlExecution(SqlConnectInfo.GetConnectionString());
        }

        public void Connect()
        {
            sqlExecution.Connect();
        }

        public async Task ConnectAsync()
        {
            await sqlExecution.ConnectAsync();
        }

        public void Disconnect()
        {
            sqlExecution.Disconnect();
        }

        public async Task ExecuteReaderAsync(SqlCommand sqlCommand)
        {
            if (objectReceivingData == ObjectReceivingData.SqlDataReader)
                data = await sqlExecution.ExecuteReaderAsync<SqlDataReader>(sqlCommand);
            else
                data = await sqlExecution.ExecuteReaderAsync<DataSet>(sqlCommand);
        }

        public void ExecuteReader(SqlCommand sqlCommand)
        {
            if (objectReceivingData == ObjectReceivingData.SqlDataReader)
                data = sqlExecution.ExecuteReader<SqlDataReader>(sqlCommand);
            else
                data = sqlExecution.ExecuteReader<DataSet>(sqlCommand);
        }

        public async Task<int> ExecuteNonQueryAsync(SqlCommand sqlCommand)
        {
            return await sqlExecution.ExecuteNonQueryAsync(sqlCommand);
        }

        public int ExecuteNonQuery(SqlCommand sqlCommand)
        {
            return sqlExecution.ExecuteNonQuery(sqlCommand);
        }

        public async Task<object> ExecuteScalarAsync(SqlCommand sqlCommand)
        {
            return await sqlExecution.ExecuteScalarAsync(sqlCommand);
        }

        public object ExecuteScalar(SqlCommand sqlCommand)
        {
            return sqlExecution.ExecuteScalar(sqlCommand);
        }

        public Dictionary<string, object> ToDictionary()
        {
            if (!(data is DataSet) && !(data is SqlDataReader))
                throw new Exception("@'data' must be DataSet or SqlDataReader");
            if (data is DataSet)
                return SqlConvert.ToDictionary((DataSet)data);
            return SqlConvert.ToDictionary((SqlDataReader)data);
        }

        public List<Dictionary<string, object>> ToDictionaryList()
        {
            if (!(data is DataSet) && !(data is SqlDataReader))
                throw new Exception("@'data' must be DataSet or SqlDataReader");
            if (data is DataSet)
                return SqlConvert.ToDictionaryList((DataSet)data);
            return SqlConvert.ToDictionaryList((SqlDataReader)data);
        }

        public T To<T>()
        {
            if (!(data is DataSet) && !(data is SqlDataReader))
                throw new Exception("@'data' must be DataSet or SqlDataReader");
            if (data is DataSet)
                return SqlConvert.To<T>((DataSet)data);
            return SqlConvert.To<T>((SqlDataReader)data);
        }

        public List<T> ToList<T>()
        {
            if (!(data is DataSet) && !(data is SqlDataReader))
                throw new Exception("@'data' must be DataSet or SqlDataReader");
            if (data is DataSet)
                return SqlConvert.ToList<T>((DataSet)data);
            return SqlConvert.ToList<T>((SqlDataReader)data);
        }

        public void Dispose()
        {
            this.data = null;
            this.sqlExecution.Dispose();
            this.sqlExecution = null;
            GC.SuppressFinalize(this);
        }
    }
}
