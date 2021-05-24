using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BLL
{
    public enum StateOfCreation { Success, Failed, AlreadyExists };
    public enum StateOfUpdate { Success, Failed };
    public enum StateOfDeletion { Success, Failed, ConstraintExists };
}
