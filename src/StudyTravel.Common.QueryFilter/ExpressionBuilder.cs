using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using static System.Int32;

namespace StudyTravel.Common.QueryFilter
{
    public class ExpressionBuilder
    {
        public static Expression<Func<T, bool>> BuildPredicate<T>(object value, Filter comparer,
            params string[] properties)
        {
            var parameterExpression = Expression.Parameter(typeof(T), typeof(T).Name);
            return
                (Expression<Func<T, bool>>)BuildNavigationExpression(parameterExpression, comparer, value, properties.Length > 1,  properties);
        }

        private static Expression BuildNavigationExpression(Expression parameter, Filter comparer,
            object value, bool isNested = false, params string[] properties)
        {
            Expression resultExpression = null;
            Expression childParameter, predicate;
            Type childType = null;

            if (properties.Count() > 1)
            {
                //build path
                parameter = Expression.Property(parameter, properties[0]);
                var isCollection = typeof(IEnumerable).IsAssignableFrom(parameter.Type);
                //if it´s a collection we later need to use the predicate in the methodexpressioncall
                if (isCollection)
                {
                    childType = parameter.Type.GetGenericArguments()[0];
                    childParameter = Expression.Parameter(childType, childType.Name);
                }
                else
                {
                    childParameter = parameter;
                }
                //skip current property and get navigation property expression recursivly
                var innerProperties = properties.Skip(1).ToArray();
                
                predicate = BuildNavigationExpression(childParameter, comparer, value, isNested, innerProperties);
                if (isCollection)
                {
                    //build subquery
                    resultExpression = BuildSubQuery(parameter, childType, predicate);
                }
                else
                {
                    resultExpression = predicate;
                }
            }
            else
            {
                //build final predicate
                
                resultExpression = BuildCondition(parameter, properties[0], comparer, value, isNested);
            }
            return resultExpression;
        }

        private static Expression BuildSubQuery(Expression parameter, Type childType, Expression predicate)
        {
            var anyMethod =
                typeof(Enumerable).GetMethods().Single(m => m.Name == "Any" && m.GetParameters().Length == 2);
            anyMethod = anyMethod.MakeGenericMethod(childType);
            predicate = Expression.Call(anyMethod, parameter, predicate);
            return MakeLambda(parameter, predicate);
        }

        private static Expression BuildCondition(Expression parameter, string property, Filter comparer, object value, bool isNested)
        {
            var childProperty = parameter.Type.GetProperty(property);

            var right = GetRightExpression(childProperty, value);
            var left = Expression.Property(parameter, childProperty);
            var buildComparsion = BuildComparsion(left, comparer, right);

            if (isNested)
            {
                var predicate = Expression.NotEqual(parameter, Expression.Constant(null, typeof(object)));
                predicate = Expression.AndAlso(predicate, buildComparsion);
                return MakeLambda(parameter, predicate);
            }

            return MakeLambda(parameter,buildComparsion);
        }

        private static Expression GetRightExpression(PropertyInfo childProperty, object value)
        {
            Expression right = null;
            if (childProperty.PropertyType == typeof(int))
            {
                right = Expression.Constant(Convert.ToInt32(value));
            }
            else if (childProperty.PropertyType == typeof(int?))
            {
                var newValue = ToNullableInt32(value.ToString());
                right = Expression.Constant(newValue, typeof(int?));
            }
            else if (childProperty.PropertyType == typeof(double))
            {
                right = Expression.Constant(Convert.ToDouble(value));
            }
            else if (childProperty.PropertyType == typeof(DateTime))
            {
                right = Expression.Constant(Convert.ToDateTime(value));
            }
            else if (childProperty.PropertyType == typeof(DateTimeOffset))
            {
                right = Expression.Constant(Convert.ToDateTime(value));
            }
            else if (childProperty.PropertyType == typeof(bool))
            {
                right = Expression.Constant(Convert.ToBoolean(value));
            }
            else if (childProperty.PropertyType == typeof(bool?))
            {
                var newValue = ToNullableBoolean(value.ToString());
                right = Expression.Constant(newValue, typeof(bool?));
            }
            else if (childProperty.PropertyType == typeof(string))
            {
                right = Expression.Constant(Convert.ToString(value));
            }

            return right;
        }

        public static int? ToNullableInt32(string s)
        {
            int i;
            if (TryParse(s, out i)) return i;
            return null;
        }

        public static bool? ToNullableBoolean(string s)
        {
            bool i;
            if (bool.TryParse(s, out i)) return i;
            return null;
        }

        private static Expression BuildComparsion(Expression left, Filter comparer, Expression right)
        {
            var mask = new List<Filter>
            {
                Filter.Contains,
                Filter.StartsWith,
                Filter.EndsWith
            };
            if (mask.Contains(comparer) && left.Type != typeof(string))
            {
                comparer = Filter.Equals;
            }
            if (!mask.Contains(comparer))
            {
                var comparerExpression = Expression.MakeBinary((ExpressionType)comparer, left, Expression.Convert(right, left.Type));
                return comparerExpression;
            }
            return BuildStringCondition(left, comparer, right);
        }

        private static Expression BuildStringCondition(Expression left, Filter comparer, Expression right)
        {
            var compareMethod =
                typeof(string).GetMethods()
                    .Single(
                        m =>
                            m.Name.Equals(Enum.GetName(typeof(Filter), comparer)) &&
                            m.GetParameters().Count() == 1);
            //we assume ignoreCase, so call ToLower on paramter and memberexpression
            var toLowerMethod =
                typeof(string).GetMethods().Single(m => m.Name.Equals("ToLower") && m.GetParameters().Count() == 0);
            left = Expression.Call(left, toLowerMethod);
            right = Expression.Call(right, toLowerMethod);
            return Expression.Call(left, compareMethod, right);
        }

        private static Expression MakeLambda(Expression parameter, Expression predicate)
        {
            var resultParameterVisitor = new ParameterVisitor();
            resultParameterVisitor.Visit(parameter);
            var resultParameter = resultParameterVisitor.Parameter;
            return Expression.Lambda(predicate, (ParameterExpression)resultParameter);
        }

        private class ParameterVisitor : ExpressionVisitor
        {
            public Expression Parameter { get; private set; }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                Parameter = node;
                return node;
            }
        }
    }
}