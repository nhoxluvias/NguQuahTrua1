using MSSQL_Lite.LambdaExpression;
using MSSQL_Lite.Mapping;
using MSSQL_Lite.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL_Lite.Query
{
    public class SqlQuery
    {
        public static bool EnclosedInSquareBrackets = true;

        public static string GetWhereStatement<T>(Expression<Func<T, bool>> where)
        {
            return "where " + GetWhereStatement((BinaryExpression)where.Body);
        }

        public static string GetWhereStatement(BinaryExpression binaryExpression)
        {
            if (binaryExpression == null)
                throw new Exception("@'binaryExpression' must be not null");
            string exp = null;
            if(
                binaryExpression.NodeType == ExpressionType.AndAlso
                || binaryExpression.NodeType == ExpressionType.OrElse
                || binaryExpression.NodeType == ExpressionType.Not
                || binaryExpression.NodeType == ExpressionType.And
                || binaryExpression.NodeType == ExpressionType.Or
            )
            {
                string expLeft = GetWhereStatement((BinaryExpression)binaryExpression.Left);
                string expRight = GetWhereStatement((BinaryExpression)binaryExpression.Right);
                exp += "(" + expLeft + " " 
                    + ExpresstionExtension.ConvertExpressionTypeToString(binaryExpression.NodeType) 
                    + " " + expRight + ")";
                return exp;
            }else if(
                binaryExpression.NodeType == ExpressionType.Equal
                || binaryExpression.NodeType == ExpressionType.NotEqual
                || binaryExpression.NodeType == ExpressionType.GreaterThan
                || binaryExpression.NodeType == ExpressionType.LessThan
                || binaryExpression.NodeType == ExpressionType.GreaterThanOrEqual
                || binaryExpression.NodeType == ExpressionType.LessThanOrEqual
                || binaryExpression.NodeType == ExpressionType.Call
            )
            {
                ExpressionData expressionData = GetPairOfExpression(binaryExpression);
                return "(" + expressionData.Key + " " 
                    + ExpresstionExtension.ConvertExpressionTypeToString(binaryExpression.NodeType) + " " 
                    + expressionData.Value + ")";
            }
            return null;
        }

        public static ExpressionData GetPairOfExpression(BinaryExpression binaryExpression)
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


            return new ExpressionData {
                Key = (EnclosedInSquareBrackets) ? "[" + key + "]" : key,
                NodeType = binaryExpression.NodeType,
                Value = SqlMapping.ConvertToStandardDataInSql(value)
            };
        }

        public static string GetSelectStatement<T>(Expression<Func<T, object>> select)
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
                    selectStatement += SqlMapping.GetPropertyName(property, EnclosedInSquareBrackets) + ", ";
                }
                selectStatement = selectStatement.TrimEnd(' ').TrimEnd(',');
            }
            else
            {
                if (!(select.Body is MemberExpression))
                    throw new Exception("");
                string propName = (select.Body as MemberExpression).Member.Name;
                selectStatement += ((EnclosedInSquareBrackets) ? "[" + propName + "]" : propName);
            }
            return selectStatement;
        }

        public static string GetSetStatement<T>(T model, Expression<Func<T, object>> set)
        {
            string setStatement = "set ";
            Func<T, object> func = set.Compile();
            object obj = func(model);
            if (set.ToString().Contains("<>f__AnonymousType"))
            {
                PropertyInfo[] properties = Obj.GetProperties(obj);
                foreach (PropertyInfo property in properties)
                {
                    setStatement += SqlMapping.GetPropertyName(property, EnclosedInSquareBrackets) 
                        + " = " + SqlMapping.ConvertToStandardDataInSql(property.GetValue(obj)) + ", ";
                }
                setStatement = setStatement.TrimEnd(' ').TrimEnd(',');
            }
            else
            {
                if (!(set.Body is MemberExpression))
                    throw new Exception("");
                string propName = (set.Body as MemberExpression).Member.Name;
                setStatement += ((EnclosedInSquareBrackets) ? "[" + propName + "]" : propName) + " = "
                    + SqlMapping.ConvertToStandardDataInSql(obj);
            }
            return setStatement;
        }

        public static string CreateDatabase(string databaseName)
        {
            return "Create database " + databaseName;
        }

        public static string UseDatabase(string databaseName)
        {
            return "Use " + databaseName;
        }

        public static string Select<T>()
        {
            return "Select * from " + SqlMapping.GetTableName<T>(EnclosedInSquareBrackets);
        }

        public static string Select<T>(int recordNumber)
        {
            return "Select top " + recordNumber + " * from " + SqlMapping.GetTableName<T>(EnclosedInSquareBrackets);
        }

        public static string Select<T>(Expression<Func<T, bool>> where)
        {
            return "Select * from " + SqlMapping.GetTableName<T>(EnclosedInSquareBrackets) + " " + GetWhereStatement<T>(where);
        }

        public static string Select<T>(Expression<Func<T, bool>> where, int recordNumber)
        {
            return "Select " + recordNumber + " * from " 
                + SqlMapping.GetTableName<T>(EnclosedInSquareBrackets) + " " + GetWhereStatement<T>(where);
        }

        public static string Select<T>(Expression<Func<T, object>> select)
        {
            return GetSelectStatement<T>(select) + " from " + SqlMapping.GetTableName<T>(EnclosedInSquareBrackets);
        }

        public static string Select<T>(Expression<Func<T, object>> select, int recordNumber)
        {
            string selectStatement = GetSelectStatement<T>(select).Replace("Select ", "Select top " + recordNumber + " ");
            return selectStatement + " from " + SqlMapping.GetTableName<T>(EnclosedInSquareBrackets);
        }

        public static string Select<T>(Expression<Func<T, object>> select, Expression<Func<T, bool>> where)
        {
            return GetSelectStatement<T>(select) + " from " + SqlMapping.GetTableName<T>(EnclosedInSquareBrackets) 
                + " " + GetWhereStatement<T>(where);
        }

        public static string Select<T>(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, int recordNumber)
        {
            string selectStatement = GetSelectStatement<T>(select).Replace("Select ", "Select top " + recordNumber + " ");
            return selectStatement + " from " + SqlMapping.GetTableName<T>(EnclosedInSquareBrackets)
                + " " + GetWhereStatement<T>(where);
        }

        public static string Insert<T>(T model)
        {
            string query = "Insert into " + SqlMapping.GetTableName<T>(EnclosedInSquareBrackets) + "(";
            PropertyInfo[] props = Obj.GetProperties(model);
            string into = null;
            string values = null;
            foreach (PropertyInfo prop in props)
            {
                into += SqlMapping.GetPropertyName(prop, EnclosedInSquareBrackets) + ", ";
                values += SqlMapping.ConvertToStandardDataInSql(prop.GetValue(model)) + ", ";
            }
            into = into.TrimEnd(' ').TrimEnd(',');
            values = values.TrimEnd(' ').TrimEnd(',');
            return query + into + ") values (" + values + ")";
        }

        public static string Update<T>(T model, Expression<Func<T, object>> set)
        {
            return "Update " + SqlMapping.GetTableName<T>(EnclosedInSquareBrackets) + " " + GetSetStatement<T>(model, set);
        }

        public static string Update<T>(T model, Expression<Func<T, object>> set, Expression<Func<T, bool>> where)
        {
            return "Update " + SqlMapping.GetTableName<T>(EnclosedInSquareBrackets) + " " + GetSetStatement<T>(model, set)
                + " " + GetWhereStatement<T>(where);
        }

        public static string Delete<T>()
        {
            return "Delete from " + SqlMapping.GetTableName<T>(EnclosedInSquareBrackets);
        }

        public static string Delete<T>(Expression<Func<T, bool>> where)
        {
            return "Delete from " + SqlMapping.GetTableName<T>(EnclosedInSquareBrackets) + " where " + GetWhereStatement(where);
        }
    }
}
