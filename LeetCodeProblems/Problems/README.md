# Problems

Each LeetCode problem lives in its own folder under `Problems/` following the
pattern `NNNN_Title`, where `NNNN` is the LeetCode problem number.

Each folder is self-contained: it contains the problem class implementing
`IProblem`, plus any helper classes it needs. The problem class is matched to
the folder by name.

| Folder | Problem | Run with |
| --- | --- | --- |
| `0001_TwoSum` | Two Sum | `dotnet run -- TwoSum` |
| `0005_LongestPalindromicSubstring` | Longest Palindromic Substring | `dotnet run -- LongestPalindromicSubstring` |
| `0006_ZigzagConversion` | Zigzag Conversion | `dotnet run -- ZigzagConversion` |
| `0058_LengthOfLastWord` | Length of Last Word | `dotnet run -- LengthOfLastWord` |
| `0066_PlusOne` | Plus One | `dotnet run -- PlusOne` |
| `0067_AddBinary` | Add Binary | `dotnet run -- AddBinary` |
| `1114_PrintInOrder` | Print in Order | `dotnet run -- PrintInOrder` |

Example:

```
dotnet run -- TwoSum
```

Guidelines:
- Name the folder `NNNN_Title` and the class after the title.
- Keep each problem self-contained in its folder.