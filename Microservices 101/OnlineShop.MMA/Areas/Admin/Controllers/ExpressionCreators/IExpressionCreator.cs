using System.Linq.Expressions;

namespace OnlineShop.MMA.Areas.Admin.Controllers.ExpressionCreators
{
    public interface IExpressionCreator
    {
        List<Expression<Func<T, bool>>> CreateExpressions<T>
            (string searchValue, Expression<Func<T, object?>> keySelector);
    }
}
