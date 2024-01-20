using System.Linq.Expressions;
using OnlineShop.MMA.Areas.Admin.Controllers.Extensions;

namespace OnlineShop.MMA.Areas.Admin.Controllers.ExpressionCreators
{
    public class StringTypesExpressionCreator : IExpressionCreator
    {
        public List<Expression<Func<T, bool>>> CreateExpressions<T>
            (string searchValue, Expression<Func<T, object?>> keySelector)
        {
            var stringMemberExpressions = keySelector.Body.ExtractStringMemberExpressions();

            var stringExpressions = new List<Expression<Func<T, bool>>>();
            foreach (var stringMemberExpression in stringMemberExpressions)
            {
                var properties = stringMemberExpression.GetPropertyChain();

                var parameterExpression = keySelector.Parameters.Single();

                Expression expression = parameterExpression;
                foreach (var property in properties)
                {
                    expression = Expression.Property(expression, property);
                }

                var conversionResult =
                    stringMemberExpression.Type.TryChangeType(
                        searchValue, out dynamic stringSearchValue);

                if (!conversionResult)
                {
                    continue;
                }

                var containsMethodInfo = stringMemberExpression.Type.GetMethod
                    ("Contains", new[] { stringMemberExpression.Type });

                var constantExpression = Expression.Constant
                    (stringSearchValue, stringMemberExpression.Type);

                var methodCallExpression = Expression.Call
                    (expression, containsMethodInfo, constantExpression);

                var stringExpression = Expression.Lambda<Func<T, bool>>
                    (methodCallExpression, parameterExpression);

                stringExpressions.Add(stringExpression);
            }

            return stringExpressions;
        }
    }
}
