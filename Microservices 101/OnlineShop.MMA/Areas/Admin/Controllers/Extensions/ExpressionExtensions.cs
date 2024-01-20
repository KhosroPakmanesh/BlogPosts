using System.Linq.Expressions;

namespace OnlineShop.MMA.Areas.Admin.Controllers.Extensions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>>? CombineExpressionsWithOr<T> (this List<Expression<Func<T, bool>>> expressions)
        {
            if (!expressions.Any()) 
            {
                return null;
            }

            Expression<Func<T, bool>> combinedExpression = expressions[0];

            for (int i = 1; i < expressions.Count; i++)
            {
                var parameterExpression = combinedExpression.Parameters[0];
                var newBinaryExpression = Expression.OrElse(combinedExpression.Body, expressions[i].Body);
                combinedExpression = Expression.Lambda<Func<T, bool>>(newBinaryExpression, parameterExpression);
            }

            return combinedExpression;
        }
        public static List<MemberExpression> ExtractDateTimeMemberExpressions(this Expression expression)
        {
            var dateTimeMemberExpressions = new List<MemberExpression>();

            if (expression is MemberExpression memberExpression &&
                memberExpression.Type == typeof(DateTime))
            {
                dateTimeMemberExpressions.Add(memberExpression);
            }
            else if (expression is NewExpression newExpression)
            {
                var newMemberExpressions = newExpression.Arguments
                    .Select(ex => ex as MemberExpression)
                    .Where(me => me!.Type == typeof(DateTime))
                    .ToList();
                dateTimeMemberExpressions.AddRange(newMemberExpressions!);
            }

            return dateTimeMemberExpressions;
        }
        public static List<MemberExpression> ExtractNumericMemberExpressions(this Expression expression)
        {
            var numericMemberExpressions = new List<MemberExpression>();

            if (expression is MemberExpression memberExpression &&
                memberExpression.GetType().IsNumeric())
            {
                numericMemberExpressions.Add(memberExpression);
            }
            else if (expression is NewExpression newExpression)
            {
                var newMemberExpressions = newExpression.Arguments
                    .Select(ex => ex as MemberExpression)
                    .Where(me => me!.Type.IsNumeric())
                    .ToList();
                numericMemberExpressions.AddRange(newMemberExpressions!);
            }

            return numericMemberExpressions;
        }
        public static List<MemberExpression> ExtractStringMemberExpressions(this Expression expression)
        {
            var stringMemberExpressions = new List<MemberExpression>();

            if (expression is MemberExpression memberExpression &&
                memberExpression.GetType().IsString())
            {
                stringMemberExpressions.Add(memberExpression);
            }
            else if (expression is NewExpression newExpression)
            {
                var newMemberExpressions = newExpression.Arguments
                    .Select(ex => ex as MemberExpression)
                    .Where(me => me!.Type.IsString())
                    .ToList();
                stringMemberExpressions.AddRange(newMemberExpressions!);
            }

            return stringMemberExpressions;
        }
    }
}
