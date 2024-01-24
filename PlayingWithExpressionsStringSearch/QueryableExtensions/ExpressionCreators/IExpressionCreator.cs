using System.Linq.Expressions;

namespace QueryableExtensions.ExpressionCreators
{
    internal interface IExpressionCreator
    {
        List<Expression<Func<T, bool>>> CreateExpressions<T>
            (string searchValue, Expression<Func<T, object>> keySelector);
    }
}
