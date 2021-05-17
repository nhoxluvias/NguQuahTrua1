using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO
{
    public class CategoryInfo
    {
        public int ID { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }

    public class CategoryCreation
    {
        public string name { get; set; }
        public string description { get; set; }
    }

    public class CategoryUpdate
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}
