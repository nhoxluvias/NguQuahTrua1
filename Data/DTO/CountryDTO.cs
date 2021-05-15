using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO
{
    public class CountryInfoForAdmin
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }

    public class CountryInfoForUser
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class CountryCreation
    {
        public string name { get; set; }
        public string description { get; set; }
    }

    public class CountryUpdate
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}
