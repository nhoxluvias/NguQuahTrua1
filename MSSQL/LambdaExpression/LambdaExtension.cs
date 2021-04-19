using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL.LambdaExpression
{
    public class LambdaExtension
    {
        public static string GetStringExpression<T, TResult>(Expression<Func<T, TResult>> expression, bool getAll = false)
        {
            return (getAll) ? expression.ToString() : expression.Body.ToString();
        }

        public static object GetValueInExpression(Expression expression)
        {
            UnaryExpression unaryExpression = Expression.Convert(expression, typeof(object));
            Func<object> func = Expression.Lambda<Func<object>>(unaryExpression).Compile();
            return func();
        }
    }
}
