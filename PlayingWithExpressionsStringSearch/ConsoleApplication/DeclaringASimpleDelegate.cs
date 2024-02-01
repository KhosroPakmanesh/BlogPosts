namespace ConsoleApplication
{
    public class DeclaringASimpleDelegate
    {
        // Declaration of a delegate
        delegate int MathOperation(int x, int y);

        public static void Run()
        {
            // Create an instance of the delegate
            MathOperation mathOperationDelegate = new MathOperation(AddOperation);

            // Invoke the delegate
            int opereationReult = mathOperationDelegate(2, 3);
            // Returns 5

            // Another way to invoke the delegate using the Invoke method
            opereationReult = mathOperationDelegate.Invoke(2, 3);
            // Returns 5
        }

        // Method that matches the delegate signature
        static int AddOperation(int number1, int number2)
        {
            return number1 + number2;
        }
    }
}
