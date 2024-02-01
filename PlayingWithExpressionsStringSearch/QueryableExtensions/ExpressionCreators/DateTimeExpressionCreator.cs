using QueryableExtensions.Extensions;
using System.Linq.Expressions;

namespace QueryableExtensions.ExpressionCreators
{
    internal sealed class DateTimeExpressionCreator : IExpressionCreator
    {
        public List<Expression<Func<T, bool>>> CreateExpressions<T>
            (string searchValue, Expression<Func<T, object>> keySelector)
        {
            object typedSearchValue = GetTypedSearchValue(searchValue);
            if (typedSearchValue == null)
            {
                return new List<Expression<Func<T, bool>>>();
            }

            List<MemberExpression> dateTimeMemberExpressions = keySelector.Body
                .ExtractDateTimeMemberExpressions();

            var dateOnlyExpressions = new List<Expression<Func<T, bool>>>();
            foreach (var dateTimeMemberExpression in dateTimeMemberExpressions)
            {
                List<string> properties = dateTimeMemberExpression.GetPropertyChain();

                AddTailProperty(properties, typedSearchValue);

                ParameterExpression parameterExpression = keySelector.Parameters.Single();

                Expression expression = parameterExpression;
                foreach (var property in properties)
                {
                    expression = Expression.Property(expression, property);
                }

                try
                {
                    ConstantExpression constantExpression = CreateConstantExpression(typedSearchValue);

                    BinaryExpression equalityExpression = Expression.Equal
                        (expression, constantExpression);

                    Expression<Func<T, bool>> dateOnlyExpression = Expression.Lambda<Func<T, bool>>
                        (equalityExpression, parameterExpression);

                    dateOnlyExpressions.Add(dateOnlyExpression);
                }
                catch { }
            }

            return dateOnlyExpressions;
        }
        private object GetTypedSearchValue(string searchValue)
        {
            bool dateOnlyParsingResult = DateOnly.TryParse
                (searchValue, out DateOnly searchedDateOnlyValue);
            if (dateOnlyParsingResult)
            {
                return searchedDateOnlyValue;
            }

            bool timeOnlyParsingResult = TimeOnly.TryParse
                (searchValue, out TimeOnly searchedTimeOnlyValue);
            if (timeOnlyParsingResult)
            {
                return searchedTimeOnlyValue;
            }

            bool dateTimeParsingResult = DateTime.TryParse
                (searchValue, out DateTime searchedDateTimeValue);
            if (dateTimeParsingResult)
            {
                return searchedDateTimeValue;
            }

            return null!;
        }
        private void AddTailProperty(List<string> properties, object typedSearchValue)
        {
            string tailProperty = typedSearchValue switch
            {
                DateOnly => "Date",
                TimeOnly => "TimeOfDay",
                _ => string.Empty
            };

            if (!string.IsNullOrWhiteSpace(tailProperty))
            {
                properties.Add(tailProperty);
            }
        }
        private ConstantExpression CreateConstantExpression(object typedSearchValue)
        {
            return typedSearchValue switch
            {
                DateOnly => Expression.Constant(((DateOnly)typedSearchValue).ToDateTime(new TimeOnly())),
                TimeOnly => Expression.Constant(((TimeOnly)typedSearchValue).ToTimeSpan()),
                _ => Expression.Constant((DateTime)typedSearchValue)
            };
        }
    }
}