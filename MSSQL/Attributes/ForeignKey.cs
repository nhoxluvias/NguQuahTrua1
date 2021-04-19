using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]

    public class ForeignKey : System.Attribute
    {
        public string PropertyName { get; set; }
        public string ReferencesToProperty { get; set; }

        public ForeignKey()
        {

        }
    }
}
