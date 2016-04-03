using System.Linq.Expressions;

namespace StudyTravel.Common.QueryFilter
{
    public enum Filter
    {
        Contains,
        StartsWith,
        EndsWith,
        Equals = ExpressionType.Equal,
        Gt= ExpressionType.GreaterThan,
        Ge = ExpressionType.GreaterThanOrEqual,
        Lt = ExpressionType.LessThan,
        Le = ExpressionType.LessThanOrEqual,
        NotEqual = ExpressionType.NotEqual
    }
}