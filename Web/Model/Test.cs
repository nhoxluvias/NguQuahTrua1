using MSSQL.Attributes;
using MSSQL.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Model
{
    [Table(Name = "ThuNghiem")]
    public class Test
    {
        [PrimaryKey(Name = "ID", DataType = null, Identity = "(1,1)")]
        public int ID { get; set; }

        [Column(Name = "name", DataType = null, AllowNull = false)]
        public string name { get; set; }

        [Column(Name = "dateTime", DataType = null, AllowNull = false)]
        public DateTime dateTime { get; set; }
    }
}