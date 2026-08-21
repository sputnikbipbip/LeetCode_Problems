using System;
using System.Collections.Generic;

public class TwoSum : IProblem
{
    public string Name => "TwoSum";

    public void Run()
    {
        Console.WriteLine("Running Two Sum example...");
        int[] nums = { 2, 7, 11, 15 };
        int target = 9;
        var res = TwoSumFunc(nums, target);
        Console.WriteLine($"Result: [{res[0]}, {res[1]}]");
    }

    private int[] TwoSumFunc(int[] nums, int target)
    {
        var dict = new Dictionary<int, int>();
        for (int i = 0; i < nums.Length; i++)
        {
            int complement = target - nums[i];
            if (dict.TryGetValue(complement, out int idx))
                return new[] { idx, i };
            dict[nums[i]] = i;
        }
        return Array.Empty<int>();
    }
}
