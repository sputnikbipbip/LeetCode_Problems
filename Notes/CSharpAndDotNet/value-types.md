# Value Types

Value types store their **value directly** on the stack (or inline within a containing type).
They include primitive types like `int`, `double`, `bool`, and `struct`s.

A key consequence: **when you assign a value type to another variable, a copy of the value
is created.** The two variables are completely independent after the assignment.

## Copy Semantics

Because value types are copied, changing one variable does not affect the other.

```csharp
int a = 1;
int b = a;   // b gets a copy of a's value
b = 10;
```

**Output:**

```text
a: 1, b: 10
```

## Structs

Structs are the canonical user-defined value types. They follow the same copy semantics
as the primitive value types.

```csharp
struct PointStruct
{
    public int X { get; set; }
    public int Y { get; set; }
}

var ps1 = new PointStruct { X = 1, Y = 2 };
var ps2 = ps1;   // ps2 gets a copy of ps1's value
ps2.X = 10;
```

**Output:**

```text
ps1: (1, 2), ps2: (10, 2)
```

Note that `ps1` was **not** changed — only the copy `ps2` was modified.

## Nullable Value Types

A value type normally cannot hold `null`. Wrapping it in `?` (e.g. `int?`) creates a
**nullable value type** that can represent an actual value or no value at all.

```csharp
int? nullableInt = null;

if (nullableInt.HasValue)
{
    Console.WriteLine($"Value: {nullableInt.Value}");
}
else
{
    Console.WriteLine("No value");
}
```

**Output:**

```text
No value
```

Useful members:

- `HasValue` — whether the variable holds a value.
- `Value` — the underlying value (only valid when `HasValue` is `true`).
- `??` / `GetValueOrDefault()` — provide a default when the value is `null`.

## Key Takeaways

- Value types store data directly; reference types store a reference to data on the heap.
- Assigning a value type **copies** the value — no shared state.
- `struct`s, primitives, and enums are value types.
- Nullable value types (`T?`) extend value types to represent "no value".
