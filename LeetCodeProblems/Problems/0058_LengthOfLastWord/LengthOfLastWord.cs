public class LengthOfLastWord : IProblem
{
    public string Name => "LengthOfLastWord";

    public readonly string[] testCases = new[]
    {
        "Hello World",
        "   fly me   to   the moon  ",
        "luffy is still joyboy",
        "",
        "a",
        "a ",
        " a"
    };

    public void Run()
    {
        Console.WriteLine("Running Length of Last Word example...");
        foreach (string s in testCases)
        {
            int res = LengthOfLastWordFunc(s);
            Console.WriteLine($"String value : {s} | Result: {res}");
        }
    }

    private int LengthOfLastWordFunc(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return 0;
        }

        ReadOnlySpan<char> span = s.AsSpan().TrimEnd();
        int lastSpace = span.LastIndexOf(' ');
        return lastSpace >= 0 ? span.Length - lastSpace - 1 : span.Length;
    }
}