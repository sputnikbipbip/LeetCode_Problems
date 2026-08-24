# Exception Handling

Exception handling allows a program to respond gracefully to runtime errors instead of
crashing. In C# this is done with `try`, `catch`, and `finally` blocks.

## `try` / `catch` / `finally`

- **`try`** — wraps code that may throw an exception.
- **`catch`** — handles a specific exception type when one is thrown.
- **`finally`** — runs regardless of whether an exception occurred (used for cleanup).

```csharp
try
{
    int result = 10 / 0; // This will throw a DivideByZeroException
}
catch (DivideByZeroException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
finally
{
    Console.WriteLine("Cleanup code goes here.");
}
```

**Output:**

```text
Error: Attempted to divide by zero.
Cleanup code goes here.
```

The `catch` block prevents the program from terminating, and the `finally` block always
executes — even if an exception is thrown or the `try` block completes normally.

## Common Exception Types

| Exception | When it occurs |
| --- | --- |
| `DivideByZeroException` | Division by zero. |
| `NullReferenceException` | Accessing a member of a `null` object. |
| `IndexOutOfRangeException` | Accessing an array/collection index out of bounds. |
| `ArgumentException` | An invalid argument passed to a method. |
| `Exception` | Base type; catch this only as a last resort. |

## Best Practices

- Catch the **most specific** exception type you can.
- Avoid swallowing exceptions silently — at minimum log the error.
- Use `finally` (or `using`/`try-finally`) to release resources like files and connections.
- Prefer **exceptions for exceptional conditions**, not for normal control flow.

## Key Takeaways

- `try` guards risky code, `catch` handles errors, `finally` guarantees cleanup.
- Only catch exception types you can meaningfully handle.
- Order matters: more specific `catch` blocks must come before more general ones.
