public class LengthOfLastWord : IProblem
{
    public string Name => "LengthOfLastWord";

    public void Run()
    {
        Console.WriteLine("Running Length of Last Word example...");
        string s = "Hello World";
        int res = LengthOfLastWordFunc(s);
        Console.WriteLine($"Result: {res}");
    }

    private int LengthOfLastWordFunc(string s)
    {

    }
}