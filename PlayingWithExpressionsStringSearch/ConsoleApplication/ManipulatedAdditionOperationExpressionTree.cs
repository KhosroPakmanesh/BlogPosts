using System.Linq.Expressions;

namespace ConsoleApplication
{
    public class ManipulatedAdditionOperationExpressionTree
    {
        public static void Run()
        {
            // Example of an expression tree for the same addition operation
            Expression<Func<int, int, int>> mathOperationExpressionTree = (a, b) => a + b;

            // Subtract 5 from the result expression
            var modifiedExpressionTree = Expression.Lambda<Func<int, int, int>>(
                Expression.Subtract(mathOperationExpressionTree.Body, Expression.Constant(5)),
                mathOperationExpressionTree.Parameters);

            // Compilation of the modified expression tree into a delegate
            Func<int, int, int> modifiedMathOperationDelegate = modifiedExpressionTree.Compile();

            // Invoke the modified delegate
            int operationResult = modifiedMathOperationDelegate(3, 4);
            // Returns 2
        }
    }
}
