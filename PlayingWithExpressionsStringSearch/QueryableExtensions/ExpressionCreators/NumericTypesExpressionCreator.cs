using System.Linq.Expressions;
using System.Reflection;
using QueryableExtensions.Extensions;

namespace QueryableExtensions.ExpressionCreators
{
    internal sealed class NumericTypesExpressionCreator : IExpressionCreator
    {
        public List<Expression<Func<T, bool>>> CreateExpressions<T>
            (string searchValue, Expression<Func<T, object>> keySelector)
        {
            bool numericParsingResult =
                searchValue.All(char.IsDigit);
            if (!numericParsingResult)
            {
                return new List<Expression<Func<T, bool>>>();
            }

            List<MemberExpression> numericMemberExpressions = 
                keySelector.Body.ExtractNumericMemberExpressions();

            var numericExpressions = new List<Expression<Func<T, bool>>>();
            foreach (var numericMemberExpression in numericMemberExpressions)
            {
                List<string> properties = numericMemberExpression.GetPropertyChain();

                ParameterExpression parameterExpression = keySelector.Parameters.Single();

                Expression expression = parameterExpression;
                foreach (var property in properties)
                {
                    expression = Expression.Property(expression, property);
                }

                bool conversionResult =
                    numericMemberExpression.Type.TryChangeType(
                        searchValue, out dynamic numericSearchValue);

                if (!conversionResult)
                {
                    continue;
                }

                MethodInfo equalsMethodInfo = numericMemberExpression.Type.GetMethod
                    ("Equals", new[] { numericMemberExpression.Type })!;

                ConstantExpression constantExpression = Expression.Constant
                    (numericSearchValue, numericMemberExpression.Type);

                MethodCallExpression methodCallExpression = Expression.Call
                    (expression, equalsMethodInfo, constantExpression);

                Expression<Func<T, bool>> numericExpression = Expression.Lambda<Func<T, bool>>
                    (methodCallExpression, parameterExpression);

                numericExpressions.Add(numericExpression);
            }

            return numericExpressions;
        }
    }
}
