
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Model
{
    class User
    {
        public int ID { get; set; }

        public string username { get; set; }

        public string email { get; set; }

        public int genderId { get; set; }

        public virtual Gender Gender { get; set; }
    }
}
