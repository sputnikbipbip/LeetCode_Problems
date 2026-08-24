# Modern C# Features

This note covers language features that make modern C# more concise, expressive, and safe:
**records**, **pattern matching**, **nullable reference types**, **tuples**, **target-typed
`new`**, and **generics with constraints**.

> Targets C# 10+ / .NET 6+, consistent with the repo's `net10.0` target.

## Records

A `record` is a reference type with built-in **value-based equality** and a concise syntax for
immutable data. Two records are equal if their properties are equal (not by reference).

```csharp
public record Person(string FirstName, string LastName, int Age);

var p1 = new Person("Ada", "Lovelace", 36);
var p2 = new Person("Ada", "Lovelace", 36);

Console.WriteLine(p1 == p2); // True (value equality)
```

### `with` expressions

Records support non-destructive mutation — copy with changes via `with`.

```csharp
var older = p1 with { Age = 37 }; // new record, p1 unchanged
Console.WriteLine(p1.Age); // 36
Console.WriteLine(older.Age); // 37
```

### Records vs Classes vs Structs

| Type | Value equality | Mutable | Typical use |
| --- | --- | --- | --- |
| `class` | Reference | Yes | General purpose |
| `record` | Yes | No (immutable) | Data transfer / value objects |
| `record struct` | Yes | Optional | Small value data |

## Pattern Matching

Pattern matching lets you check the shape and content of data concisely.

### Property patterns

```csharp
public string Describe(Person p) => p switch
{
    { Age: >= 18 } => "Adult",
    { Age: >= 13 } => "Teenager",
    _ => "Child"
};
```

### Type patterns + logical patterns

```csharp
public string Classify(object value) => value switch
{
    int n when n > 0 => "positive int",
    int => "non-positive int",
    string { Length: 0 } => "empty string",
    string s => $"string: {s}",
    null => "null",
    _ => "unknown"
};
```

## Switch Expressions

A `switch` expression is a compact, expression-based alternative to a `switch` statement.

```csharp
string GetDayType(DayOfWeek day) => day switch
{
    DayOfWeek.Saturday or DayOfWeek.Sunday => "Weekend",
    _ => "Weekday"
};
```

## Nullable Reference Types

With `<Nullable>enable</Nullable>`, the compiler helps you avoid `null`-related bugs.
Reference types are non-nullable by default; use `?` to opt in to `null`.

```csharp
string name = "Ada";   // non-nullable
string? maybe = null;  // nullable

if (maybe is not null)       // guard before use
{
    Console.WriteLine(maybe.Length);
}

Console.WriteLine(maybe?.Length);      // null-conditional
Console.WriteLine(maybe ?? "default"); // null-coalescing
```

## Tuples & Deconstruction

Tuples let you group values without defining a type.

```csharp
(string, int) person = ("Ada", 36);
var (name, age) = person; // deconstruction
Console.WriteLine($"{name} is {age}");

(int x, int y) point = (3, 4);
```

Deconstruction also works on your own types via a `Deconstruct` method.

## Target-Typed `new`

`new` can infer its type from the target, reducing repetition.

```csharp
Person p = new("Ada", "Lovelace", 36);      // type inferred from declaration
List<Person> people = new();                // infers List<Person>
```

## Generics & Constraints

Generics let you write type-safe, reusable code. Constraints restrict what types can be used.

```csharp
public T Max<T>(T a, T b) where T : IComparable<T>
    => a.CompareTo(b) >= 0 ? a : b;

public class Repository<T> where T : class, new() // reference type with parameterless ctor
{
    public T Create() => new T();
}
```

Common constraints:

| Constraint | Meaning |
| --- | --- |
| `where T : class` | Reference type. |
| `where T : struct` | Value type. |
| `where T : new()` | Has a parameterless constructor. |
| `where T : BaseClass` | Derives from `BaseClass`. |
| `where T : IInterface` | Implements `IInterface`. |

## Key Takeaways

- **Records** give value equality + immutability with minimal code.
- **Pattern matching** and **switch expressions** make conditional logic concise and safe.
- **Nullable reference types** shift null-safety to compile time.
- **Tuples/deconstruction** group values without ceremony.
- **Generics with constraints** deliver type-safe, reusable code.
