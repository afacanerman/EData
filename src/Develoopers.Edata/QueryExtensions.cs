using System;
using System.Collections.Generic;
using System.Linq;
using LinqKit;

namespace Develoopers.Edata
{
    public static class QueryExtensions
    {
        public static IQueryable<T> Filter<T>(this IQueryable<T> query, IList<IFilter> filters)
        {
            var predicate = PredicateBuilder.New<T>(true);
            foreach (var filter in filters)
            {
                var expression = ExpressionBuilder.BuildPredicate<T>(filter.Value,
                                                                    filter.FilterType, 
                                                                    filter.PropertyName.Split('.'));
                predicate = predicate.And(expression);
            }

            return query.AsExpandable().Where(predicate);
        }
        

        public static string FirstCharToUpper(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
}
