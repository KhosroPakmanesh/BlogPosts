namespace ConsoleApplication
{
    public class UsingActionGenericDelegateType
    {
        public static void Run()
        {
            // Use an action generic delegate type
            Action<string, string> printFullName = (firstName, lastName) =>
                Console.WriteLine($"Your fullname is: {firstName} {lastName}");

            // Invoke the delegate
            printFullName("Khosro", "Pakmanesh");
            // Prints 'Your fullname is: Khosro Pakmanesh'

            // Another way to invoke the delegate using the Invoke method
            printFullName.Invoke("Khosro", "Pakmanesh");
            // Prints 'Your fullname is: Khosro Pakmanesh'
        }
    }
}
