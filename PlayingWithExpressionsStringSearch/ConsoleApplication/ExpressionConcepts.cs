using System.Linq.Expressions;

namespace ConsoleApplication
{
    public class ExpressionConcepts
    {
        public static void Run()
        {
            // Example of MemberExpression
            MemberExpression memberExpression = Expression.Field(null, typeof(int), nameof(int.MaxValue));
            Expression<Func<int>> intMaxValueExpressionTree = Expression.Lambda<Func<int>>(memberExpression);
            var maxValueDelegate1 = intMaxValueExpressionTree.Compile();
            var operationResult = maxValueDelegate1.Invoke();
            // Returns 2147483647

            // Example of ParameterExpression
            ParameterExpression parameterExpression = Expression.Parameter(typeof(int), "x");
            Expression<Func<int, int>> squareExpressionTree = Expression.Lambda<Func<int, int>>(
                Expression.Multiply(parameterExpression, parameterExpression), parameterExpression);
            var squareDelegate = squareExpressionTree.Compile();
            operationResult = squareDelegate.Invoke(5);
            // Returns 25

            // Example of ConstantExpression
            ConstantExpression constantExpression = Expression.Constant(6);
            Expression<Func<int, int>> addConstantExpressionTree = Expression.Lambda<Func<int, int>>(
                Expression.Add(parameterExpression, constantExpression), parameterExpression);
            var addConstantDelegate = addConstantExpressionTree.Compile();
            operationResult = addConstantDelegate.Invoke(5);
            // Returns 11

            // Example of Expression.Equal
            BinaryExpression binaryExpression = Expression.Equal(parameterExpression, constantExpression);
            Expression<Func<int, bool>> isEqualExpressionTree = Expression.Lambda<Func<int, bool>>
                (binaryExpression, parameterExpression);
            var isEqualDelegate = isEqualExpressionTree.Compile();
            var booleanOperationResult = isEqualDelegate.Invoke(6);
            // Returns True

            // Example of MethodCallExpression
            var equalsMethodInfo = typeof(int).GetMethod("Equals", new[] { typeof(int) });
            var intConstantExpression1 = Expression.Constant(5);
            var intConstantExpression2 = Expression.Constant(5);
            MethodCallExpression methodCallExpression = Expression.Call(
                intConstantExpression1,
                equalsMethodInfo,
                intConstantExpression2
            );
            Expression<Func<bool>> lambdaExpression = 
                Expression.Lambda<Func<bool>>(methodCallExpression);
            var equalsDelegate = lambdaExpression.Compile();
            booleanOperationResult = equalsDelegate.Invoke();
            // Returns True
        }
    }
}
