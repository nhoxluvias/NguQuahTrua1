using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL.Filters
{
    public class ModelFilter
    {
        public static bool IsValid<T>(T model, Func<T, bool> predicate)
        {
            return predicate(model);
        }
    }
}
