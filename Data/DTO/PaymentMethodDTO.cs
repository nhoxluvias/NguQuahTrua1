using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO
{
    public class PaymentMethodInfo
    {
        public int ID { get; set; }
        public string name { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }

    public class PaymentMethodCreation
    {
        public string name { get; set; }
    }

    public class PaymentMethodUpdate
    {
        public int ID { get; set; }
        public string name { get; set; }
    }
}
