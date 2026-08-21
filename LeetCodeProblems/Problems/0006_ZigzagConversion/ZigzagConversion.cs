using System.Text;

public class ZigzagConversion : IProblem
{
    public string Name => "ZigzagConversion";

    public readonly List<string> inputs = new List<string>()
    {
        "PAYPALISHIRING"
    };

    public void Run()
    {
        Console.WriteLine("Running Zigzag Conversion example... \n original string {0}, new string {1}",
            inputs[0],
            Convert(inputs[0], 4)
        );
    }

    public string Convert(string s, int numRows)
    {
        if (string.IsNullOrEmpty(s) || numRows <= 1)
        {
            return s;
        }

        var rows = new StringBuilder[numRows];
        for (int i = 0; i < numRows; i++)
        {
            rows[i] = new StringBuilder();
        }

        int row = 0;
        bool down = false;

        for (int i = 0; i < s.Length; i++)
        {
            rows[row].Append(s[i]);

            if (row == 0)
            {
                down = true;
            }
            else if (row == numRows - 1)
            {
                down = false;
            }

            row += down ? 1 : -1;
        }

        var result = new StringBuilder();
        foreach (var r in rows)
        {
            result.Append(r);
        }

        return result.ToString();
    }
}