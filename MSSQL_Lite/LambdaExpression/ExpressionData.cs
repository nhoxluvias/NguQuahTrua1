using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL_Lite.LambdaExpression
{
    public class ExpressionData
    {
        public ExpressionType NodeType { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
    }
}
