using System.Linq.Expressions;

namespace QueryableExtensions.Extensions
{
    internal static class MemberExpressionExtensions
    {
        public static List<string> GetPropertyChain(this MemberExpression memberExpression)
        {
            var properties = new List<string>();
            while (memberExpression != null)
            {
                properties.Insert(0, memberExpression.Member.Name);
                memberExpression = memberExpression.Expression as MemberExpression;
            }

            return properties;
        }
    }
}
