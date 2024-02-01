using System.Linq.Expressions;
using System.Reflection;
using QueryableExtensions.Extensions;

namespace QueryableExtensions.ExpressionCreators
{
    internal sealed class StringTypesExpressionCreator : IExpressionCreator
    {
        public List<Expression<Func<T, bool>>> CreateExpressions<T>
            (string searchValue, Expression<Func<T, object>> keySelector)
        {
            List<MemberExpression> stringMemberExpressions = 
                keySelector.Body.ExtractStringMemberExpressions();

            var stringExpressions = new List<Expression<Func<T, bool>>>();
            foreach (var stringMemberExpression in stringMemberExpressions)
            {
                List<string> properties = stringMemberExpression.GetPropertyChain();

                ParameterExpression parameterExpression = keySelector.Parameters.Single();

                Expression expression = parameterExpression;
                foreach (var property in properties)
                {
                    expression = Expression.Property(expression, property);
                }

                bool conversionResult =
                    stringMemberExpression.Type.TryChangeType(
                        searchValue, out dynamic stringSearchValue);

                if (!conversionResult)
                {
                    continue;
                }

                MethodInfo containsMethodInfo = stringMemberExpression.Type.GetMethod
                    ("Contains", new[] { stringMemberExpression.Type })!;

                ConstantExpression constantExpression = Expression.Constant
                    (stringSearchValue, stringMemberExpression.Type);

                MethodCallExpression methodCallExpression = Expression.Call
                    (expression, containsMethodInfo, constantExpression);

                Expression<Func<T, bool>> stringExpression = Expression.Lambda<Func<T, bool>>
                    (methodCallExpression, parameterExpression);

                stringExpressions.Add(stringExpression);
            }

            return stringExpressions;
        }
    }
}
