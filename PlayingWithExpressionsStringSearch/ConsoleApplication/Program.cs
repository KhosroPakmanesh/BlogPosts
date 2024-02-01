namespace ConsoleApplication
{
    public class Program
    {
        static void Main()
        {
            DeclaringASimpleDelegate.Run();
            UsingASimpleLambdaInsteadOfInstantiatingDelegate.Run();
            UsingFuncGenericDelegateTypeInsteadOfDelegateType.Run();
            UsingActionGenericDelegateType.Run();
            AdditionOperationExpressionTree.Run();
            ManipulatedAdditionOperationExpressionTree.Run();
            ExpressionConcepts.Run();
        }
    }
}
