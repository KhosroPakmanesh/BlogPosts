using QueryableExtensions.ExpressionCreators;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace QueryableExtensions.Extensions
{
    public static class EnumerableExtensions
    {
        private static ReadOnlyCollection<IExpressionCreator> ExpressionCreators =>
            new(new IExpressionCreator[]
            {
                new DateTimeExpressionCreator(),
                new NumericTypesExpressionCreator(),
                new StringTypesExpressionCreator()
            });

        public static IEnumerable<T> Search<T>(
            this IEnumerable<T> source,
            Expression<Func<T, object>> keySelector,
            string searchValue)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            if (string.IsNullOrWhiteSpace(searchValue))
            {
                return source;
            }

            var expressions = new List<Expression<Func<T, bool>>>();

            foreach (var expressionCreator in ExpressionCreators)
            {
                var createdExpressions = expressionCreator
                    .CreateExpressions(searchValue, keySelector);
                expressions.AddRange(createdExpressions);
            }

            if (expressions.Any())
            {
                var combinedExpressions =
                    expressions.CombineExpressionsWithOr()!;
                return source.Where(combinedExpressions.Compile());
            }

            return source;
        }
    }
}
