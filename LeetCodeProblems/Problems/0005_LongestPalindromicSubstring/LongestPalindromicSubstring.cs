public class LongestPalindromicSubstring : IProblem
{
    public string Name => "LongestPalindromicSubstring";

    public string[] testCases = new[]
    {
        "babad",
        "cbbd",
        "a",
        "ac",
    };

    public void Run()
    {
        Console.WriteLine("Running Longest Palindromic Substring example...");
        foreach (string s in testCases)
        {
            Console.WriteLine($"Input: \"{s}\"");
            string res = LongestPalindromeFunc(s);
            Console.WriteLine($"Result: \"{res}\"");
        }
    }

    private string LongestPalindromeFunc(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }

        int start = 0, maxLength = 1;

        for (int i = 0; i < s.Length; i++) //N
        {
            // Odd length palindromes
            ExpandFromCenter(s, i, i, ref start, ref maxLength);    //N
            // Even length palindromes
            ExpandFromCenter(s, i, i + 1, ref start, ref maxLength);   //N
        }

        return s.Substring(start, maxLength);
    }

      private void ExpandFromCenter(string s, int left, int right, ref int start, ref int maxLength) 
    {
        while (left >= 0 && right < s.Length && s[left] == s[right]) 
        {
            int length = right - left + 1;

            if (length > maxLength) 
            {
                start = left;
                maxLength = length;
            }

            --left;
            ++right;
        } 
    }
}