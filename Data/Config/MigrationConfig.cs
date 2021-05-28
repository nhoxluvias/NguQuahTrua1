using Data.BLL.Migration;

namespace Data.Config
{
    public static class MigrationConfig
    {
        public static void Migrate()
        {
            RoleMigration roleMigration = new RoleMigration(); ;
            PaymentMethodMigration paymentMethodMigration = new PaymentMethodMigration();
            UserMigration userMigration = new UserMigration();
            roleMigration.AddDataAndRun();
            paymentMethodMigration.AddDataAndRun();
            userMigration.AddDataAndRun();
            roleMigration.Dispose();
            paymentMethodMigration.Dispose();
            userMigration.Dispose();
            roleMigration = null;
            paymentMethodMigration = null;
            userMigration = null;
        }
    }
}
