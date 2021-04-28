using MSSQL_Lite.LambdaExpression;
using MSSQL_Lite.Mapping;
using MSSQL_Lite.Reflection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL_Lite.Query
{
    public class SqlQueryBase
    {
        protected static bool IsComparisonOperator(ExpressionType expressionType)
        {
            if (
                expressionType == ExpressionType.Equal
               || expressionType == ExpressionType.NotEqual
               || expressionType == ExpressionType.GreaterThan
               || expressionType == ExpressionType.LessThan
               || expressionType == ExpressionType.GreaterThanOrEqual
               || expressionType == ExpressionType.LessThanOrEqual
            )
            {
                return true;
            }
            return false;
        }

        protected static bool IsLogicalOperator(ExpressionType expressionType)
        {
            if (
                expressionType == ExpressionType.AndAlso
                || expressionType == ExpressionType.OrElse
                || expressionType == ExpressionType.Not
                || expressionType == ExpressionType.And
                || expressionType == ExpressionType.Or
            )
            {
                return true;
            }
            return false;
        }

        protected static ExpressionTree GetExpressionTree<T>(Expression<Func<T, bool>> expression)
        {
            return GetExpressionTree(expression.Body as BinaryExpression);
        }

        protected static ExpressionTree GetExpressionTree(BinaryExpression binaryExpression)
        {
            if (binaryExpression == null)
                throw new Exception("@'binaryExpression' must be not null");
            if (IsLogicalOperator(binaryExpression.NodeType))
            {
                ExpressionTree expressionTreeLeft = GetExpressionTree(binaryExpression.Left as BinaryExpression);
                ExpressionTree expressionTreeRight = GetExpressionTree(binaryExpression.Right as BinaryExpression);
                return new ExpressionTree
                {
                    Data = new ExpressionData { NodeType = binaryExpression.NodeType },
                    Left = expressionTreeLeft,
                    Right = expressionTreeRight
                };
            }
            else if (IsComparisonOperator(binaryExpression.NodeType))
            {
                ExpressionData expressionData = GetExpressionData(binaryExpression);
                return new ExpressionTree
                {
                    Data = expressionData,
                    Left = null,
                    Right = null,
                };
            }
            return null;
        }

        protected static ExpressionData GetExpressionData(BinaryExpression binaryExpression)
        {
            if (binaryExpression == null)
                throw new Exception("@'binaryExpression' must be not null");

            Expression expressionLeft = binaryExpression.Left;
            Expression expressionRight = binaryExpression.Right;

            if (!(expressionLeft is MemberExpression))
                throw new Exception("");

            string key = (expressionLeft as MemberExpression).Member.Name;
            UnaryExpression unaryExpression = Expression.Convert(expressionRight, typeof(object));
            Func<object> func = Expression.Lambda<Func<object>>(unaryExpression).Compile();
            object value = func();

            return new ExpressionData
            {
                Key = key,
                Value = value,
                NodeType = binaryExpression.NodeType
            };
        }

        protected static string GetWherePatternStatement(ExpressionTree expressionTree, bool enclosedInSquareBrackets)
        {
            if (expressionTree == null)
                return null;
            ExpressionData expressionData = expressionTree.Data;
            if (expressionData == null)
                throw new Exception("");
            if (IsLogicalOperator(expressionData.NodeType))
            {
                string left = GetWherePatternStatement(expressionTree.Left, enclosedInSquareBrackets);
                string right = GetWherePatternStatement(expressionTree.Right, enclosedInSquareBrackets);
                string nodeType = ExpresstionExtension.ConvertExpressionTypeToString(expressionData.NodeType);
                return string.Format("({0} {1} {2})", left, nodeType, right);
            }
            else if (IsComparisonOperator(expressionData.NodeType))
            {
                if (expressionData.Key == null)
                    throw new Exception("");
                string propName = (enclosedInSquareBrackets) ? string.Format("[{0}]", expressionData.Key) : expressionData.Key;
                string nodeType = ExpresstionExtension.ConvertExpressionTypeToString(expressionData.NodeType);
                return string.Format("({0} {1} {2})", propName, nodeType, string.Format("@{0}_where", expressionData.Key));
            }
            return null;
        }

        protected static List<SqlQueryParameter> GetKeyAndValueOfExpressionTree(ExpressionTree expressionTree)
        {
            if (expressionTree == null)
                return null;
            ExpressionData expressionData = expressionTree.Data;
            if (expressionData == null)
                throw new Exception("");
            if (IsLogicalOperator(expressionData.NodeType))
            {
                List<SqlQueryParameter> left = GetKeyAndValueOfExpressionTree(expressionTree.Left);
                List<SqlQueryParameter> right = GetKeyAndValueOfExpressionTree(expressionTree.Right);
                return left.Concat(right).ToList();
            }
            else if (IsComparisonOperator(expressionData.NodeType))
            {
                if (expressionData.Key == null)
                    throw new Exception("");
                List<SqlQueryParameter> sqlParameters = new List<SqlQueryParameter>();
                sqlParameters.Add(new SqlQueryParameter
                {
                    Name = string.Format("@{0}_where", expressionData.Key),
                    Value = expressionData.Value,
                    SqlType = default(System.Data.SqlDbType)
                });
                return sqlParameters;
            }
            return null;
        }

        protected static SqlQueryData GetWhereStatement<T>(Expression<Func<T, bool>> where, bool enclosedInSquareBrackets)
        {
            ExpressionTree expressionTree = GetExpressionTree<T>(where);
            string whereStatement = GetWherePatternStatement(expressionTree, enclosedInSquareBrackets);
            List<SqlQueryParameter> sqlParameters = GetKeyAndValueOfExpressionTree(expressionTree);
            return new SqlQueryData { Statement = string.Format("where {0}", whereStatement), SqlQueryParameters = sqlParameters };
        }

        protected static string GetInsertPattern<T>(T model, bool enclosedInSquareBrackets)
        {
            string query = string.Format("Insert into {0}(", SqlMapping.GetTableName<T>(enclosedInSquareBrackets));
            PropertyInfo[] props = Obj.GetProperties(model);
            string into = null;
            string values = null;
            foreach (PropertyInfo prop in props)
            {
                into += string.Format("{0}, ", SqlMapping.GetPropertyName(prop, enclosedInSquareBrackets));
                values += string.Format("{0}, ", "@" + SqlMapping.GetPropertyName(prop, false));
            }
            into = into.TrimEnd(' ').TrimEnd(',');
            values = values.TrimEnd(' ').TrimEnd(',');
            return string.Format("{0} {1}) values ({2})", query, into, values);
        }

        protected static string GetInsertPattern<T>(T model, List<string> excludeProperties, bool enclosedInSquareBrackets)
        {
            if (excludeProperties == null)
                throw new Exception("");
            if (excludeProperties.Count == 0)
                throw new Exception("");
            string query = string.Format("Insert into {0}(", SqlMapping.GetTableName<T>(enclosedInSquareBrackets));
            PropertyInfo[] props = Obj.GetProperties(model);
            string into = null;
            string values = null;
            foreach (PropertyInfo prop in props)
            {
                if (excludeProperties.Any(e => e.Equals(prop.Name)))
                    continue;
                into += string.Format("{0}, ", SqlMapping.GetPropertyName(prop, enclosedInSquareBrackets));
                values += string.Format("{0}, ", "@" + SqlMapping.GetPropertyName(prop, false));
            }
            into = into.TrimEnd(' ').TrimEnd(',');
            values = values.TrimEnd(' ').TrimEnd(',');
            return string.Format("{0} {1}) values ({2})", query, into, values);
        }

        protected static List<SqlQueryParameter> GetParameterOfInsertQuery<T>(T model, bool enclosedInSquareBracket)
        {
            PropertyInfo[] props = Obj.GetProperties(model);
            List<SqlQueryParameter> sqlQueryParameters = new List<SqlQueryParameter>();
            foreach(PropertyInfo prop in props)
            {
                sqlQueryParameters.Add(new SqlQueryParameter
                {
                    Name = "@" + prop.Name,
                    Value = prop.GetValue(model)
                });
            }
            return sqlQueryParameters;
        }

        protected static List<SqlQueryParameter> GetParameterOfInsertQuery<T>(T model, List<string> excludeProperties, bool enclosedInSquareBracket)
        {
            if (excludeProperties == null)
                throw new Exception("");
            if (excludeProperties.Count == 0)
                throw new Exception("");
            PropertyInfo[] props = Obj.GetProperties(model);
            List<SqlQueryParameter> sqlQueryParameters = new List<SqlQueryParameter>();
            foreach (PropertyInfo prop in props)
            {
                if (excludeProperties.Any(e => e.Equals(prop.Name)))
                    continue;
                sqlQueryParameters.Add(new SqlQueryParameter
                {
                    Name = "@" + prop.Name,
                    Value = prop.GetValue(model)
                });
            }
            return sqlQueryParameters;
        }

        protected static SqlQueryData GetInsertQueryData<T>(T model, bool enclosedInSquareBrackets)
        {
            return new SqlQueryData
            {
                Statement = GetInsertPattern<T>(model, enclosedInSquareBrackets),
                SqlQueryParameters = GetParameterOfInsertQuery<T>(model, enclosedInSquareBrackets)
            };
        }

        protected static SqlQueryData GetInsertQueryData<T>(T model, List<string> excludeProperties, bool enclosedInSquareBrackets)
        {
            return new SqlQueryData
            {
                Statement = GetInsertPattern<T>(model, excludeProperties, enclosedInSquareBrackets),
                SqlQueryParameters = GetParameterOfInsertQuery<T>(model, excludeProperties, enclosedInSquareBrackets)
            };
        }

        protected static string GetSelectStatement<T>(Expression<Func<T, object>> select, bool enclosedInSquareBrackets)
        {
            string selectStatement = "Select ";
            if (select.ToString().Contains("<>f__AnonymousType"))
            {
                Func<T, object> func = select.Compile();
                T model = Obj.CreateInstance<T>();
                object obj = func(model);
                PropertyInfo[] properties = Obj.GetProperties(obj);
                foreach (PropertyInfo property in properties)
                {
                    selectStatement += SqlMapping.GetPropertyName(property, enclosedInSquareBrackets) + ", ";
                }
                selectStatement = selectStatement.TrimEnd(' ').TrimEnd(',');
            }
            else
            {
                if (!(select.Body is MemberExpression))
                    throw new Exception("");
                string propName = (select.Body as MemberExpression).Member.Name;
                selectStatement += ((enclosedInSquareBrackets) ? string.Format("[{0}]", propName) : propName);
            }
            return selectStatement;
        }

        protected static SqlQueryData GetSetStatement<T>(T model, Expression<Func<T, object>> set, bool enclosedInSquareBrackets)
        {
            string setStatement = "set ";
            Func<T, object> func = set.Compile();
            object obj = func(model);
            List<SqlQueryParameter> sqlQueryParameters = new List<SqlQueryParameter>();
            string paramName;
            if (set.ToString().Contains("<>f__AnonymousType"))
            {
                PropertyInfo[] properties = Obj.GetProperties(obj);
                foreach (PropertyInfo property in properties)
                {
                    paramName = string.Format("@{0}_set", SqlMapping.GetPropertyName(property, false));
                    setStatement += string.Format(
                        "{0} = {1}, ",
                        SqlMapping.GetPropertyName(property, enclosedInSquareBrackets),
                        paramName
                    );
                    sqlQueryParameters.Add(new SqlQueryParameter
                    {
                        Name = paramName,
                        Value = property.GetValue(obj)
                    });
                }
                setStatement = setStatement.TrimEnd(' ').TrimEnd(',');
            }
            else
            {
                if (!(set.Body is MemberExpression))
                    throw new Exception("");
                string propName = (set.Body as MemberExpression).Member.Name;
                paramName = string.Format("@{0}_set", propName);
                setStatement += string.Format(
                    "{0} = {1}",
                    (enclosedInSquareBrackets) ? string.Format("[{0}]", propName) : propName,
                    paramName
                );
                sqlQueryParameters.Add(new SqlQueryParameter
                {
                    Name = paramName,
                    Value = obj
                });
            }
            return new SqlQueryData { Statement = setStatement, SqlQueryParameters = sqlQueryParameters };
        }

        protected static SqlCommand InitSqlCommand(string commandText, List<SqlQueryParameter> sqlQueryParameters)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = commandText;

            foreach (SqlQueryParameter sqlQueryParameter in sqlQueryParameters)
            {
                sqlCommand.Parameters.Add(new SqlParameter(sqlQueryParameter.Name, sqlQueryParameter.Value));
            }
            return sqlCommand;
        }

        protected static SqlCommand InitSqlCommand(string commandText)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = commandText;
            return sqlCommand;
        }
    }
}
