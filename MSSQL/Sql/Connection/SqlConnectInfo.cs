﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL.Sql.Connection
{
    public class SqlConnectInfo
    {
        public static string DataSource { get; set; }
        public static string InitialCatalog { get; set; }
        public static string UserID { get; set; }
        public static string Password { get; set; }

        public static string GetConnectionString()
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            sqlConnectionStringBuilder.DataSource = SqlConnectInfo.DataSource;
            sqlConnectionStringBuilder.InitialCatalog = SqlConnectInfo.InitialCatalog;
            sqlConnectionStringBuilder.UserID = SqlConnectInfo.UserID;
            sqlConnectionStringBuilder.Password = SqlConnectInfo.Password;
            return sqlConnectionStringBuilder.ConnectionString;
        }

        public static string ReadFromConfigFile(string name)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            DataSource = sqlConnectionStringBuilder.DataSource;
            InitialCatalog = sqlConnectionStringBuilder.InitialCatalog;
            UserID = sqlConnectionStringBuilder.UserID;
            Password = sqlConnectionStringBuilder.Password;
            return connectionString;
        }

        public static void WriteIntoConfigFile(string name)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings
                .Add(new ConnectionStringSettings(name, SqlConnectInfo.GetConnectionString()));
            config.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("connectionStrings");
        }
    }
}