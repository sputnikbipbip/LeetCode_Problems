Project structure for storing LeetCode exercises

- `Problems/` : each problem in its own folder (e.g. `0001_TwoSum`).
- `IProblem.cs` : interface all problems implement.
- `ProblemRunner.cs` : discovers and runs problems via reflection.

Usage:

```
dotnet run -- <ProblemName>
```

Example:

```
dotnet run -- Example_0001_TwoSum
```

Guidelines:
- Name classes with the folder/problem identifier (recommended).
- Keep each problem self-contained in its folder.
