using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.DTO
{
    public class CategoryInfo
    {
        public int ID { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}