using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO
{
    public class RoleInfoForAdmin
    {
        public string ID { get; set; }
        public string name { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }

    public class RoleInfoForUser
    {
        public string name { get; set; }
    }

    public class RoleCreation
    {
        public string ID { get; set; }
        public string name { get; set; }
    }

    public class RoleUpdate
    {
        public string ID { get; set; }
        public string name { get; set; }
    }
}
