public class Foo
{

    private readonly ManualResetEventSlim _firstCall = new(false);
    private readonly ManualResetEventSlim _secondCall = new(false);
    private readonly ManualResetEventSlim _thirdCall = new(false);
    private readonly Action<string> printAction;

    public Foo(Action<string> printAction)
    {
        this.printAction = printAction;
    }

    public void First()
    {
        printAction("First method is running...");
        _firstCall.Set();
    }

    public void Second()
    {
        _firstCall.Wait();
        printAction("Second method is running...");
        _secondCall.Set();
    }

    public void Third()
    {
        _firstCall.Wait();
        _secondCall.Wait();
        printAction("Third method is running...");
        _thirdCall.Set();
    }
}