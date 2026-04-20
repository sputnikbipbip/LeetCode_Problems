param(
    [Parameter(Mandatory=$true)]
    [string]$Problem
)

dotnet run --project LeetCodeProblems/LeetCodeProblems.csproj -- $Problem
