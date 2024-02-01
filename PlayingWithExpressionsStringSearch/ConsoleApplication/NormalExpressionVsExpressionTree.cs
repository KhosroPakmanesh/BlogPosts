using System.Linq.Expressions;

namespace ConsoleApplication
{
    public class NormalExpressionVsExpressionTree
    {
        static void Run()
        {
            // Normal expression
            Func<int, int, int> mathOperationDelegate = (a, b) => a + b;
            int result = mathOperationDelegate(2, 3); // Directly evaluates at runtime
            // Returns 5

            // Expression tree
            // Example of an expression tree for the same addition operation
            Expression<Func<int, int, int>> mathOperationExpressionTree = (a, b) => a + b;

            // Compilation of the expression tree into a delegate
            Func<int, int, int> mathOperationDelegate2 = mathOperationExpressionTree.Compile();

            // Invoke the delegate
            int opereationReult = mathOperationDelegate2(2, 3);
            // Returns 5
        }
    }
}
