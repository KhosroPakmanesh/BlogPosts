namespace ConsoleApplication
{
    public class UsingASimpleLambdaInsteadOfInstantiatingDelegate
    {
        // Declaration of a delegate
        delegate int MathOperation(int x, int y);

        public static void Run()
        {
            // Use a lambda instead of Instantiating the delegate 
            MathOperation mathOperationDelegate = (a, b) => a + b;

            // Invoke the delegate
            int opereationReult = mathOperationDelegate(2, 3);
            // Returns 5

            // Another way to invoke the delegate using the Invoke method
            opereationReult = mathOperationDelegate.Invoke(2, 3);
            // Returns 5
        }
    }
}
