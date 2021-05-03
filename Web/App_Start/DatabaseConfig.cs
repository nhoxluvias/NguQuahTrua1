using MSSQL_Lite.Access;
using MSSQL_Lite.Connection;
using Web.Migrations;

namespace Web.App_Start
{
    public class DatabaseConfig
    {
        public static void RegisterDatabase()
        {
            SqlConnectInfo.DataSource = @"LAPTOP-B78E1G5S\MSSQLSERVER2019";
            SqlConnectInfo.InitialCatalog = "Movie";
            SqlConnectInfo.UserID = "sa";
            SqlConnectInfo.Password = "123456789";
            SqlData.objectReceivingData = ObjectReceivingData.DataSet;

            RoleMigration roleMigration = new RoleMigration();
            roleMigration.AddDataAndRun();

            PaymentMethodMigration paymentMethodMigration = new PaymentMethodMigration();
            paymentMethodMigration.AddDataAndRun();
        }
    }
}