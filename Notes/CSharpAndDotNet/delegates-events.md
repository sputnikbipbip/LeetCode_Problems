# Delegates, Events, and LINQ

This note covers **delegates** (type-safe method references), **events** (notifications based
on delegates), and a brief introduction to **LINQ** (Language Integrated Query).

## Delegates

A **delegate** is a type that represents references to methods with a particular parameter
list and return type. It lets you encapsulate a method as an object that can be passed around
and invoked later. Delegates are often used for event handling and callbacks.

```csharp
delegate void PrintMessage(string message); // delegate declaration

void PrintToConsole(string message)
{
    Console.WriteLine($"Console: {message}");
}

// Use the delegate
PrintMessage printer = PrintToConsole; // assign a method to the delegate
printer("Hello, World!");              // invoke the delegate
```

**Output:**

```text
Console: Hello, World!
```

Note: a delegate can point to multiple methods (multicast), and modern C# often uses
`Func<>` / `Action<>` instead of custom delegate declarations.

## Events

An **event** is a way for a class to notify clients when something of interest occurs. Events
are based on delegates and provide a mechanism to **subscribe** and **unsubscribe** to
notifications.

```csharp
public class Button
{
    // Declare an event based on a delegate
    public event EventHandler? Clicked;

    public void Click()
    {
        Clicked?.Invoke(this, EventArgs.Empty);
    }
}

var button = new Button();
button.Clicked += (sender, e) => Console.WriteLine("Button was clicked!");

button.Click();
```

**Output:**

```text
Button was clicked!
```

Events wrap a delegate so that only the declaring class can invoke it, while outside code can
only add/remove handlers with `+=` / `-=`.

## LINQ (Language Integrated Query)

LINQ provides a consistent, readable syntax for querying and manipulating data sources such
as collections, databases, and XML. It supports operations like **filtering**, **sorting**,
**grouping**, and **projecting**.

```csharp
using System.Linq;

var numbers = new[] { 1, 2, 3, 4, 5 };
var evenNumbers = numbers.Where(n => n % 2 == 0); // lazy: not executed until iterated

foreach (var num in evenNumbers)
{
    Console.WriteLine(num); // Output: 2, 4
}
```

**Output:**

```text
2
4
```

Key LINQ operators:

| Operator | Purpose |
| --- | --- |
| `Where` | Filter elements by a condition. |
| `Select` | Project/transform each element. |
| `OrderBy` / `OrderByDescending` | Sort elements. |
| `GroupBy` | Group elements by a key. |
| `First` / `FirstOrDefault` | Get the first (or default) matching element. |
| `Any` / `All` / `Count` | Predicate/summary checks. |

## Key Takeaways

- **Delegates** are type-safe method references; **events** build on them for notifications.
- Events restrict access — only the declaring class can raise them.
- LINQ offers a readable, composable way to query collections.
