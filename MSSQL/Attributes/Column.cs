using System;

namespace MSSQL.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]

    public class Column : System.Attribute
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public bool AllowNull { get; set; }
        public object Default { get; set; }
        public bool Unique { get; set; }

        public Column()
        {

        }
    }
}
