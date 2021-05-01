using MSSQL_Lite.Access;
using MSSQL_Lite.Query;
using System.Collections.Generic;

namespace MSSQL_Lite.Migration
{
    public class SqlMigration<T>
    {
        private SqlQuery sqlQuery;

        public void Migrate(List<T> items)
        {
            foreach(T item in items){
                ////Task<int> task = SqlData.ExecuteNonQueryAsync(SqlQuery.Insert<T>(item));

                //Task task = Task.Run(async () => { await SqlData.ExecuteNonQueryAsync(SqlQuery.Insert<T>(item)); });
                //task.Wait();

                SqlData.ExecuteNonQuery(sqlQuery.Insert<T>(item));
                
            }

            //foreach(Task<int> t in tasks)
            //{
            //    t.R
            //}
        }

        public void Migrate(List<T> items, List<string> excludeProperties)
        {
            foreach (T item in items)
            {
                SqlData.ExecuteNonQuery(sqlQuery.Insert<T>(item, excludeProperties));
            }
        }

        private List<T> items;
        private List<string> excludeProperties;

        public SqlMigration()
        {
            sqlQuery = new SqlQuery();
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

        public void Run()
        {
            if (excludeProperties.Count == 0)
                Migrate(items);
            else
                Migrate(items, excludeProperties);
        }
    }
}
