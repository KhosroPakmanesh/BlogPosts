using OnlineShop.MMA.Areas.Admin.Controllers.Extensions;
using System.Linq.Expressions;

namespace OnlineShop.MMA.Areas.Admin.Controllers.ExpressionCreators
{
    public class TimeOnlyExpressionCreator : IExpressionCreator
    {
        public List<Expression<Func<T, bool>>> CreateExpressions<T>
            (string searchValue, Expression<Func<T, object?>> keySelector)
        {
            var timeOnlyParsingResult = TimeOnly.TryParse
                (searchValue, out TimeOnly searchedDateOnlyValue);
            if (!timeOnlyParsingResult)
            {
                return new List<Expression<Func<T, bool>>>();
            }

            var dateTimeMemberExpressions = keySelector.Body
                .ExtractDateTimeMemberExpressions();

            var timeOnlyExpressions = new List<Expression<Func<T, bool>>>();
            foreach (var dateTimeMemberExpression in dateTimeMemberExpressions)
            {
                var properties = dateTimeMemberExpression.GetPropertyChain();
                properties.Add("TimeOfDay");

                var parameterExpression = keySelector.Parameters.Single();

                Expression expression = parameterExpression;
                foreach (var property in properties)
                {
                    expression = Expression.Property(expression, property);
                }

                try
                {
                    var constantExpression = Expression
                        .Constant(searchedDateOnlyValue.ToTimeSpan());

                    var equalityExpression = Expression
                        .Equal(expression, constantExpression);

                    var timeOnlyExpression = Expression
                        .Lambda<Func<T, bool>>(equalityExpression, parameterExpression);

                    timeOnlyExpressions.Add(timeOnlyExpression);
                }
                catch { }
            }

            return timeOnlyExpressions;
        }
    }
}
