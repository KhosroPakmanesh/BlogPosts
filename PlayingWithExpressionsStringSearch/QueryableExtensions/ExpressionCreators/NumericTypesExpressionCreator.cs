using System.Linq.Expressions;
using QueryableExtensions.Extensions;

namespace QueryableExtensions.ExpressionCreators
{
    internal sealed class NumericTypesExpressionCreator : IExpressionCreator
    {
        public List<Expression<Func<T, bool>>> CreateExpressions<T>
            (string searchValue, Expression<Func<T, object>> keySelector)
        {
            var numericParsingResult =
                searchValue.All(char.IsDigit);
            if (!numericParsingResult)
            {
                return new List<Expression<Func<T, bool>>>();
            }

            var numericMemberExpressions = keySelector.Body.ExtractNumericMemberExpressions();

            var numericExpressions = new List<Expression<Func<T, bool>>>();
            foreach (var numericMemberExpression in numericMemberExpressions)
            {
                var properties = numericMemberExpression.GetPropertyChain();

                var parameterExpression = keySelector.Parameters.Single();

                Expression expression = parameterExpression;
                foreach (var property in properties)
                {
                    expression = Expression.Property(expression, property);
                }

                var conversionResult =
                    numericMemberExpression.Type.TryChangeType(
                        searchValue, out dynamic numericSearchValue);

                if (!conversionResult)
                {
                    continue;
                }

                var equalsMethodInfo = numericMemberExpression.Type.GetMethod
                    ("Equals", new[] { numericMemberExpression.Type });

                var constantExpression = Expression.Constant
                    (numericSearchValue, numericMemberExpression.Type);

                var methodCallExpression = Expression.Call
                    (expression, equalsMethodInfo, constantExpression);

                var numericExpression = Expression.Lambda<Func<T, bool>>
                    (methodCallExpression, parameterExpression);

                numericExpressions.Add(numericExpression);
            }

            return numericExpressions;
        }
    }
}
