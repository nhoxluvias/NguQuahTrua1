using MSSQL_Lite.Access;
using MSSQL_Lite.Query;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MSSQL_Lite.Migration
{
    public class SqlMigration<T> : IDisposable
    {
        private SqlData sqlData;
        private List<T> items;
        private List<string> excludeProperties;
        private bool disposedValue;

        public SqlMigration()
        {
            sqlData = new SqlData();
            items = new List<T>();
            excludeProperties = new List<string>();
            disposedValue = false;
        }
        private void Migrate(List<T> items)
        {
            sqlData.Connect();
            foreach (T item in items)
            {
                SqlQuery sqlQuery = new SqlQuery();
                sqlData.ExecuteNonQuery(sqlQuery.Insert<T>(item));
                sqlQuery = null;
            }
            sqlData.Disconnect();
        }

        private async Task MigrateAsync(List<T> items)
        {
            await sqlData.ConnectAsync();
            foreach (T item in items)
            {
                SqlQuery sqlQuery = new SqlQuery();
                await sqlData.ExecuteNonQueryAsync(sqlQuery.Insert<T>(item));
                sqlQuery = null;
            }
            sqlData.Disconnect();
        }

        private void Migrate(List<T> items, List<string> excludeProperties)
        {
            sqlData.Connect();
            foreach (T item in items)
            {
                SqlQuery sqlQuery = new SqlQuery();
                sqlData.ExecuteNonQuery(sqlQuery.Insert<T>(item, excludeProperties));
                sqlQuery = null;
            }
            sqlData.Disconnect();
        }

        private async Task MigrateAsync(List<T> items, List<string> excludeProperties)
        {
            await sqlData.ConnectAsync();
            foreach (T item in items)
            {
                SqlQuery sqlQuery = new SqlQuery();
                await sqlData.ExecuteNonQueryAsync(sqlQuery.Insert<T>(item, excludeProperties));
                sqlQuery = null;
            }
            sqlData.Disconnect();
        }

        protected void AddItem(T item)
        {
            items.Add(item);
        }

        protected void AddExcludeProperty(string excludeProperty)
        {
            excludeProperties.Add(excludeProperty);
        }

        protected void Run()
        {
            if (excludeProperties.Count == 0)
                Migrate(items);
            else
                Migrate(items, excludeProperties);
        }

        protected async Task RunAsync()
        {
            if (excludeProperties.Count == 0)
                await MigrateAsync(items);
            else
                await MigrateAsync(items, excludeProperties);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    sqlData.Dispose();
                    sqlData = null;
                    items.Clear();
                    items = null;
                    excludeProperties.Clear();
                    excludeProperties = null;
                }
                disposedValue = true;
            }
        }
        ~SqlMigration()
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
