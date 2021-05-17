using Data.DAL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class PaymentMethodBLL : BusinessLogicLayer
    {
        private DataAccessLevel dataAccessLevel;
        protected PaymentMethodBLL(DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL();
            this.dataAccessLevel = dataAccessLevel;
        }

        protected PaymentMethodBLL(BusinessLogicLayer bll, DataAccessLevel dataAccessLevel)
            : base()
        {
            InitDAL(bll.db);
            this.dataAccessLevel = dataAccessLevel;
        }

        private PaymentMethodInfo ToPaymentMethodInfo(PaymentMethod paymentMethod)
        {
            if (paymentMethod == null)
                throw new Exception("");
            return new PaymentMethodInfo
            {
                ID = paymentMethod.ID,
                name = paymentMethod.name,
                createAt = paymentMethod.createAt,
                updateAt = paymentMethod.updateAt
            };
        }

        private PaymentMethod ToPaymentMethod(PaymentMethodCreation paymentMethodCreation)
        {
            if (paymentMethodCreation == null)
                throw new Exception("");
            return new PaymentMethod
            {
                name = paymentMethodCreation.name,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };
        }

        private PaymentMethod ToPaymentMethod(PaymentMethodUpdate paymentMethodUpdate)
        {
            if (paymentMethodUpdate == null)
                throw new Exception("");
            return new PaymentMethod
            {
                name = paymentMethodUpdate.name,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };
        }

        public async Task<List<PaymentMethodInfo>> GetPaymentMethodsAsync()
        {
            List<PaymentMethodInfo> paymentMethods = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                paymentMethods = (await db.PaymentMethods.ToListAsync())
                    .Select(p => ToPaymentMethodInfo(p)).ToList();
            else
                paymentMethods = (await db.PaymentMethods.ToListAsync(c => new { c.ID, c.name }))
                    .Select(p => ToPaymentMethodInfo(p)).ToList();
            return paymentMethods;
        }

        public async Task<PaymentMethodInfo> GetMethodInfoAsync(int paymemtMethodId)
        {
            if (paymemtMethodId <= 0)
                throw new Exception("");
            PaymentMethod paymentMethod = null;
            if (dataAccessLevel == DataAccessLevel.Admin)
                paymentMethod = (await db.PaymentMethods.SingleOrDefaultAsync(p => p.ID == paymemtMethodId));
            else
                paymentMethod = (await db.PaymentMethods
                    .SingleOrDefaultAsync(p => new { p.ID, p.name }, p => p.ID == paymemtMethodId));

            return ToPaymentMethodInfo(paymentMethod);
        }

        public async Task<bool> CreateRoleAsync(PaymentMethodCreation paymentMethodCreation)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            PaymentMethod paymentMethod = ToPaymentMethod(paymentMethodCreation);
            if (paymentMethod.name == null)
                throw new Exception("");

            int affected = await db.PaymentMethods.InsertAsync(paymentMethod);

            return (affected != 0);
        }

        public async Task<bool> UpdateRoleAsync(PaymentMethodUpdate paymentMethodUpdate)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            PaymentMethod paymentMethod = ToPaymentMethod(paymentMethodUpdate);
            if (paymentMethod.name == null)
                throw new Exception("");

            int affected = await db.PaymentMethods
                .UpdateAsync(paymentMethod, p => new { p.name, p.updateAt }, p => p.ID == paymentMethod.ID);

            return (affected != 0);
        }

        public async Task<bool> DeleteAsync(int paymentMethodId)
        {
            if (dataAccessLevel == DataAccessLevel.User)
                throw new Exception("");
            if (paymentMethodId <= 0)
                throw new Exception("");

            long paymentInfoNumber = await db.PaymentInfos
                .CountAsync(p => p.paymentMethodId == paymentMethodId);
            if (paymentInfoNumber > 0)
                return false;

            int affected = await db.PaymentMethods.DeleteAsync(p => p.ID == paymentMethodId);
            return (affected != 0);
        }

        public async Task<int> CountAllAsync()
        {
            return (int)await db.PaymentMethods.CountAsync();
        }
    }
}
