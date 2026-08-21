# C# and .NET Learning Notes

Reference snippets covering C# language features and .NET concepts. These are
study notes, not part of the compiled `LeetCodeProblems` project — they are
kept here so they don't interfere with the project's build.

> Note: these files are reference material and may not build as-is. Several
> use top-level statements or types (e.g. ASP.NET Core) that only work in a
> dedicated context.

| File | Topic |
| --- | --- |
| `AsyncProgramming.cs` | `async`/`await` examples and error handling |
| `CSharpDataStructures.cs` | Data structures overview with a runnable demo |
| `DataTypes.cs` | Value vs reference types, nullables, ref/out |
| `DelegateEvents.cs` | Delegates, events, and LINQ basics |
| `DesignPatterns.cs` | Singleton, Factory, Adapter, Decorator, Observer |
| `NetCoreBasics.cs` | ASP.NET Core middleware, DI, IoC |
| `Oop.cs` | Encapsulation, inheritance, polymorphism, abstraction |

To run `CSharpDataStructures.cs` standalone:

```
dotnet new console -o dsref
cp CSharpDataStructures.cs dsref/Program.cs
dotnet run --project dsref
```