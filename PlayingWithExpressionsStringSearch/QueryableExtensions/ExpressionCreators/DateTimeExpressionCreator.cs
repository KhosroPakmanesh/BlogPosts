using QueryableExtensions.Extensions;
using System.Linq.Expressions;

namespace QueryableExtensions.ExpressionCreators
{
    internal sealed class DateTimeExpressionCreator : IExpressionCreator
    {
        public List<Expression<Func<T, bool>>> CreateExpressions<T>
            (string searchValue, Expression<Func<T, object?>> keySelector)
        {
            var dateTimeParsingResult = DateTime.TryParse
                (searchValue, out DateTime searchedDateOnlyValue);
            if (!dateTimeParsingResult)
            {
                return new List<Expression<Func<T, bool>>>();
            }

            var dateTimeMemberExpressions = keySelector.Body
                .ExtractDateTimeMemberExpressions();

            var dateTimeExpressions = new List<Expression<Func<T, bool>>>();
            foreach (var dateTimeMemberExpression in dateTimeMemberExpressions)
            {
                var properties = dateTimeMemberExpression.GetPropertyChain();

                var parameterExpression = keySelector.Parameters.Single();

                Expression expression = parameterExpression;
                foreach (var property in properties)
                {
                    expression = Expression.Property(expression, property);
                }

                try
                {
                    var constantExpression = Expression.Constant
                        (searchedDateOnlyValue);

                    var equalityExpression = Expression.Equal
                        (expression, constantExpression);

                    var dateOnlyExpression = Expression.Lambda<Func<T, bool>>
                        (equalityExpression, parameterExpression);

                    dateTimeExpressions.Add(dateOnlyExpression);
                }
                catch { }
            }

            return dateTimeExpressions;
        }
    }
}
