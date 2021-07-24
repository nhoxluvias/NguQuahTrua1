using System.Linq.Expressions;

namespace MSSQL_Lite.LambdaExpression
{
    internal class ExpressionData
    {
        public ExpressionType NodeType { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
    }
}
