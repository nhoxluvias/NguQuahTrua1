using MSSQL.String;
using System;
using System.Linq;

namespace MSSQL.Error
{
    public class ModelError
    {
        public static void IfNull(object obj)
        {
            if (obj == null)
                throw new Exception("object must be not null");
        }

        public static void IfNull(object obj, string objectName)
        {
            if (obj == null)
                throw new Exception("@'" + objectName + "' must be not null");
        }

        public static void IfInvalidType(object obj, string[] validType)
        {
            Type type = obj.GetType();
            if (!validType.Any(t => t == type.Name))
                throw new Exception("Invalid type, must be " + StringExtension.ConvertStringArrayToString(validType, " or "));
        }
    }
}
