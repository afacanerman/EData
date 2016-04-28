using System;
using System.Linq;
using System.Linq.Dynamic;

namespace Develoopers.Edata
{
    public class QueryFilterBuilder<T> where T : class
    {
        private const string Descending = "DESC";
        private const string Ascending = "ASC";

        public static IQueryable<T> Build(IQueryable<T> queryableCollection, DataQueryModel dataQueryModel)
        {
            if (dataQueryModel != null)
            {
                if (!string.IsNullOrWhiteSpace(dataQueryModel.Filter))
                {
                    var stFilter = new FilterParser(dataQueryModel.Filter);
                    queryableCollection = queryableCollection.Filter<T>(stFilter.Filters);
                }

                if (!string.IsNullOrWhiteSpace(dataQueryModel.SortBy))
                {
                    var orderType = dataQueryModel.Desc ? Descending : Ascending;
                    var seperator = $" {orderType},";

                    var splitedSortBy = dataQueryModel.SortBy.Split(',');
                    var multipleSortByStr = string.Join(seperator, splitedSortBy) + $" {orderType}";

                    queryableCollection = queryableCollection.OrderBy(multipleSortByStr);
                }

                if (dataQueryModel.Skip > 0)
                {
                    queryableCollection = queryableCollection.Skip(dataQueryModel.Skip);
                }

                if (dataQueryModel.Take > 0)
                {
                    queryableCollection = queryableCollection.Take(dataQueryModel.Take);
                }



            }

            return queryableCollection;
        }

        public static object GetPropertyValue(object obj, string propertyName)
        {
            try
            {
                foreach (var prop in propertyName.Split('.').Select(s => obj.GetType().GetProperty(s)))
                {
                    obj = prop.GetValue(obj, null);
                }
                return obj;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }
    }
}
