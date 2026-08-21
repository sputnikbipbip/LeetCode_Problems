public class PrintInOrder : IProblem
{
    public string Name => "PrintInOrder";

    private readonly Action<string> printAction;

    public PrintInOrder()
    {
        printAction = Console.WriteLine;
    }

    public void Run()
    {
        Console.WriteLine("Running Print In Order example...");
        var foo = new Foo(printAction);

        // Simulate concurrent calls to the methods in random order
        var tasks = new[]
        {
            Task.Run(() => foo.Second()),
            Task.Run(() => foo.Third()),
            Task.Run(() => foo.First())
        };

        Task.WaitAll(tasks);
    }
}

