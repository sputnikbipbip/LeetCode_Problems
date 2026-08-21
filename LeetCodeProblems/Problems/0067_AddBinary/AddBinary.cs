using System.Text;

public class AddBinary : IProblem
{
    public string Name => "AddBinary";

    public string[] testCases = new[]
    {
        "11",
        "1",
        "1010",
        "1011",
    };

    public void Run()
    {
        Console.WriteLine("Running Add Binary example...");
        for (int i = 0; i < testCases.Length; i += 2)
        {
            string a = testCases[i];
            string b = testCases[i + 1];
            Console.WriteLine($"Input: a = \"{a}\", b = \"{b}\"");
            string res = AddBinaryFunc(a, b);
            Console.WriteLine($"Result: \"{res}\"");    
        }
    }
    
    private string AddBinaryFunc(string a, string b)
    {
        int i = a.Length - 1;
        int j = b.Length - 1;
        int carry = 0;

        char[] buffer = new char[Math.Max(a.Length, b.Length) + 1];
        int k = buffer.Length - 1;

        while (i >= 0 || j >= 0 || carry > 0)
        {
            int sum = carry;

            if (i >= 0)
                sum += a[i--] - '0';

            if (j >= 0)
                sum += b[j--] - '0';

            buffer[k--] = (char)('0' + (sum % 2));
            carry = sum / 2;
        }

        return new string(buffer, k + 1, buffer.Length - (k + 1));
    }
}