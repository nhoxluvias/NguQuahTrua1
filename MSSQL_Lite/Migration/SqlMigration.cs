using MSSQL_Lite.Access;
using MSSQL_Lite.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL_Lite.Migration
{
    public class SqlMigration<T>
    {
        public static async Task MigrateAsync(List<T> items)
        {
            foreach(T item in items){
                await SqlData.ExecuteNonQueryAsync(SqlQuery.Insert<T>(item));
            }
        }

        public static async Task MigrateAsync(List<T> items, List<string> excludeProperties)
        {
            foreach (T item in items)
            {
                await SqlData.ExecuteNonQueryAsync(SqlQuery.Insert<T>(item, excludeProperties));
            }
        }

        private List<T> items;
        private List<string> excludeProperties;

        public SqlMigration()
        {
            items = new List<T>();
            excludeProperties = new List<string>();
        }

        public void AddItem(T item)
        {
            items.Add(item);
        }

        public void AddExcludeProperty(string excludeProperty)
        {
            excludeProperties.Add(excludeProperty);
        }

        public async Task Run()
        {
            if (excludeProperties.Count == 0)
                await MigrateAsync(items);
            else
                await MigrateAsync(items, excludeProperties);
        }
    }
}
