using QueryableExtensions.Extensions;
using System.Linq.Expressions;

namespace QueryableExtensions.ExpressionCreators
{
    internal sealed class DateOnlyExpressionCreator : IExpressionCreator
    {
        public List<Expression<Func<T, bool>>> CreateExpressions<T>
            (string searchValue, Expression<Func<T, object?>> keySelector)
        {
            var dateOnlyParsingResult = DateOnly.TryParse
                (searchValue, out DateOnly searchedDateOnlyValue);
            if (!dateOnlyParsingResult)
            {
                return new List<Expression<Func<T, bool>>>();
            }

            var dateTimeMemberExpressions = keySelector.Body
                .ExtractDateTimeMemberExpressions();

            var dateOnlyExpressions = new List<Expression<Func<T, bool>>>();
            foreach (var dateTimeMemberExpression in dateTimeMemberExpressions)
            {
                var properties = dateTimeMemberExpression.GetPropertyChain();
                properties.Add("Date");

                var parameterExpression = keySelector.Parameters.Single();

                Expression expression = parameterExpression;
                foreach (var property in properties)
                {
                    expression = Expression.Property(expression, property);
                }

                try
                {
                    var constantExpression = Expression.Constant
                        (searchedDateOnlyValue.ToDateTime(new TimeOnly()));

                    var equalityExpression = Expression.Equal
                        (expression, constantExpression);

                    var dateOnlyExpression = Expression.Lambda<Func<T, bool>>
                        (equalityExpression, parameterExpression);

                    dateOnlyExpressions.Add(dateOnlyExpression);
                }
                catch { }
            }

            return dateOnlyExpressions;
        }
    }
}
