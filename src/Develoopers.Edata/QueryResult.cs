using System.Collections.Generic;

namespace Develoopers.Edata
{
    public class QueryResult<T>
    {
        public int Count { get; }
        public IEnumerable<T> Results { get; }

        public QueryResult(int count, IEnumerable<T> results)
        {
            Count = count;
            Results = results;
        }
    }
}
