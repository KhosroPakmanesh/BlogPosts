using System.Linq.Expressions;

namespace ConsoleApplication
{
    public class AdditionOperationExpressionTree
    {
        public static void Run()
        {
            // Example of an expression tree for the same addition operation
            Expression<Func<int, int, int>> mathOperationExpression = (a, b) => a + b;

            // Compilation of the expression tree into a delegate
            Func<int, int, int> mathOperationDelegate = mathOperationExpression.Compile();

            // Invoke the delegate
            int opereationReult = mathOperationDelegate(2, 3);
            // Returns 5
        }
    }
}
