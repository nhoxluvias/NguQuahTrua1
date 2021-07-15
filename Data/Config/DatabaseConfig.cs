using MSSQL_Lite.Access;
using MSSQL_Lite.Connection;

namespace Data.Config
{
    public static class DatabaseConfig
    {
        public static void ManualConfig(string dataSource, string initialCatalog, string userId, string password)
        {
            SqlConnectInfo.DataSource = dataSource;
            SqlConnectInfo.InitialCatalog = initialCatalog;
            SqlConnectInfo.UserID = userId;
            SqlConnectInfo.Password = password;

            SetObjectReceivingData();
        }

        public static void ReadFromConfigFile(string connectionName)
        {
            SqlConnectInfo.ReadFromConfigFile(connectionName);
            SetObjectReceivingData();
        }

        private static void SetObjectReceivingData()
        {
            SqlData.objectReceivingData = ObjectReceivingData.DataSet;
        }
    }
}
