public class PlusOne : IProblem
{
    public string Name => "PlusOne";

    public readonly int[][] testCases = new[]
    {
        new[] { 1, 2, 3 },
        new[] { 4, 3, 2, 1 },
        new[] { 0 },
        new[] { 9 },
        new[] { 9, 9 },
        new[] { 1, 9 }
    };

    public void Run()
    {
        Console.WriteLine("Running Plus One example...");
        foreach (int[] digits in testCases)
        {
            Console.WriteLine($"Input digits: [{string.Join(", ", digits)}]");
            int[] res = PlusOneFunc(digits);
            Console.WriteLine($"Result: [{string.Join(", ", res)}]");
        }
    }

    private int[] PlusOneFunc(int[] digits)
    {
        if (digits.Length == 0)
        {
            return new[] { 1 };
        }

        for (int i = digits.Length - 1; i >= 0; i--)
        {
            if (digits[i] > 9 || digits[i] < 0) {
                throw new InvalidDataException("Please provide a valid input array of digits (0-9).");
            } 

            if (digits[i] == 9)
            {
                digits[i] = 0;
            } 
            else
            {
                digits[i] += 1;
                return digits;
            }
        }

        int[] result = new int[digits.Length + 1];
        result[0] = 1;
        return result;
    }
}