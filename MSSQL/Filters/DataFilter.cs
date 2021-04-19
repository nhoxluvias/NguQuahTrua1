using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL.Filters
{
    public class DataFilter
    {
        public bool IsString(object obj){
            if(obj == null)
                throw new Exception("");
            return (obj is string);
        }

        public bool IsBoolean(object obj)
        {
            if(obj == null)
                throw new Exception("");
            return (obj is bool);
        }

        public bool IsDateTime(object obj)
        {
            if (obj == null)
                throw new Exception("");
            return (obj is DateTime);
        }
    }
}
