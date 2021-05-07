using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }
}