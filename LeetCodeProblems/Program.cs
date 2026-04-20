using System;

Console.WriteLine("LeetCode Problem Runner");

var problems = ProblemRunner.DiscoverProblems();

if (args.Length == 0)
{
	Console.WriteLine("Available problems:");
	foreach (var p in problems)
		Console.WriteLine($"- {p.Name}");
	Console.WriteLine("\nRun a problem: dotnet run -- <ProblemName>");
	return;
}

var name = args[0];
var problem = ProblemRunner.FindByName(name);
if (problem is null)
{
	Console.WriteLine($"Problem '{name}' not found.");
	return;
}

problem.Run();
