using System;

namespace MSSQL.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]

    public class PrimaryKey : System.Attribute
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public string Identity { get; set; }

        public PrimaryKey()
        {

        }
    }
}
