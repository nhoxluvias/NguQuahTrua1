using MSSQL.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Model
{
    [Table(Name = "User")]
    class User
    {
        [PrimaryKey(Name = "ID", DataType = "int", Identity = "(1,1)")]
        public int ID { get; set; }

        [Column(Name = "username", DataType = "nvarchar(100)", AllowNull = false, Default = "phanxuanchanh", Unique = true)]
        public string username { get; set; }

        [Column(Name = "email", DataType = "nvarchar(100)", AllowNull = true, Unique = true)]
        public string email { get; set; }

        [Column(Name = "genderId", DataType = "int", AllowNull = true, Default = 1)]
        public int genderId { get; set; }



        [ForeignKey(PropertyName = "genderId", ReferencesToProperty = "ID")]
        public virtual Gender Gender { get; set; }
    }
}
