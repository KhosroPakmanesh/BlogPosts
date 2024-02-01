namespace ConsoleApplication
{
    public class UsingFuncGenericDelegateTypeInsteadOfDelegateType
    {
        public static void Run()
        {
            // Use a func generic delegate type instead of delegate type
            Func<int, int, int> mathOperationDelegate = (a, b) => a + b;

            // Invoke the delegate
            int opereationReult = mathOperationDelegate(2, 3);
            // Returns 5

            // Another way to invoke the delegate using the Invoke method
            opereationReult = mathOperationDelegate.Invoke(2, 3);
            // Returns 5
        }
    }
}
