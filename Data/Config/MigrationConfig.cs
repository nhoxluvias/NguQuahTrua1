using Data.BLL.Migration;

namespace Data.Config
{
    public static class MigrationConfig
    {
        public static void Migrate()
        {
            RoleMigration roleMigration = new RoleMigration(); ;
            PaymentMethodMigration paymentMethodMigration = new PaymentMethodMigration(); ;
            roleMigration.AddDataAndRun();
            paymentMethodMigration.AddDataAndRun();
        }
    }
}
