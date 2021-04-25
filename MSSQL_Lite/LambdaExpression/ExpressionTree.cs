using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL_Lite.LambdaExpression
{
    public class ExpressionTree
    {
        public ExpressionData Data { get; set; }
        public ExpressionTree Left { get; set; }
        public ExpressionTree Right { get; set; }
    }
}
