using Data.DAL;
using MSSQL_Lite.Connection;
using MSSQL_Lite.Migration;
using System;
using System.Threading.Tasks;

namespace Data.BLL.Migration
{
    internal class PaymentMethodMigration : SqlMigration<PaymentMethod>, ISqlMigration
    {

        public PaymentMethodMigration()
            : base()
        {

        }

        public void AddDataAndRun()
        {
            DBContext db = new DBContext(ConnectionType.ManuallyDisconnect);
            long recordNumber = db.PaymentMethods.Count();
            if (recordNumber == 0)
            {
                AddItem(new PaymentMethod
                {
                    name = "Visa",
                    createAt = DateTime.Now,
                    updateAt = DateTime.Now
                });
                AddItem(new PaymentMethod
                {
                    name = "Mastercard",
                    createAt = DateTime.Now,
                    updateAt = DateTime.Now
                });
                AddExcludeProperty("ID");
                Run();
            }
        }

        public async Task AddDataAndRunAsync()
        {
            DBContext db = new DBContext(ConnectionType.ManuallyDisconnect);
            long recordNumber = await db.PaymentMethods.CountAsync();
            if (recordNumber == 0)
            {
                AddItem(new PaymentMethod
                {
                    name = "Visa",
                    createAt = DateTime.Now,
                    updateAt = DateTime.Now
                });
                AddItem(new PaymentMethod
                {
                    name = "Mastercard",
                    createAt = DateTime.Now,
                    updateAt = DateTime.Now
                });
                AddExcludeProperty("ID");
                await RunAsync();
            }
        }
    }
}
