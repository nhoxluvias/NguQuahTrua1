using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BLL.Migration
{
    public class MigrationConfig
    {
        private RoleMigration roleMigration;
        private PaymentMethodMigration paymentMethodMigration;

        public MigrationConfig()
        {
            roleMigration = new RoleMigration();
            paymentMethodMigration = new PaymentMethodMigration();
        }

        public void Start()
        {
            roleMigration.AddDataAndRun();
            paymentMethodMigration.AddDataAndRun();
        }
    }
}
