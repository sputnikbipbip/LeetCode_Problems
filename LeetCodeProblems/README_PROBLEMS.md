Project structure for storing LeetCode exercises

- `Problems/` : each problem in its own folder following the pattern `NNNN_Title` (e.g. `0067_AddBinary`).
- `IProblem.cs` : interface all problems implement.
- `ProblemRunner.cs` : discovers and runs problems via reflection.
- `CSHARPANDDOTNET/` : C#/.NET learning notes (excluded from compilation).

Usage:

```
dotnet run -- <Name>
```

Example:

```
dotnet run -- TwoSum
```

Or use the convenience script from the repo root:

```
./run.sh TwoSum
```

Guidelines:
- Name the problem folder `NNNN_Title` and the class after the title, e.g. `0067_AddBinary/AddBinary.cs`.
- Keep each problem self-contained in its folder.
- See `Problems/README.md` for the current list of problems.