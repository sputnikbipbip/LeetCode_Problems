# Asynchronous Programming

Asynchronous programming allows you to perform tasks without blocking the main thread,
improving responsiveness and performance. In C#, `async` and `await` are used to write
non-blocking code that reads much like synchronous code.

- The **`async`** keyword marks a method as asynchronous.
- The **`await`** keyword suspends the method until an awaited operation completes, without
  blocking the thread.

## A Basic `async` Method

```csharp
async Task<int> GetDataAsync()
{
    // Simulate an asynchronous operation
    await Task.Delay(1000);
    return 42; // return some data after the delay
}
```

## Calling an Async Method

Use `await` to get the result. The calling method must itself be `async` to use `await`.

```csharp
async void CallAsyncMethod()
{
    Console.WriteLine("Calling asynchronous method...");
    int result = await GetDataAsync(); // await the asynchronous method
    Console.WriteLine($"Result: {result}");
}
```

**Output:**

```text
Calling asynchronous method...
Result: 42
```

## Error Handling with `try` / `catch`

An `await` in a `try` block lets you catch exceptions thrown by the async operation, just like
with synchronous code.

```csharp
async Task<int> GetDataWithErrorAsync()
{
    await Task.Delay(1000);
    throw new Exception("Something went wrong!"); // simulate an error
}

async void CallAsyncMethodWithErrorHandling()
{
    try
    {
        Console.WriteLine("Calling asynchronous method with error handling...");
        int result = await GetDataWithErrorAsync();
        Console.WriteLine($"Result: {result}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}
```

**Output:**

```text
Calling asynchronous method with error handling...
Error: Something went wrong!
```

## Return Types for Async Methods

| Return type | Use case |
| --- | --- |
| `Task<T>` | Returns a result value. |
| `Task` | Performs work but returns no result. |
| `ValueTask<T>` | High-performance alternative when results are often already available. |
| `void` | Only for event handlers — avoid elsewhere because errors can't be awaited/caught. |

## Best Practices

- **Avoid `async void`** except for event handlers — it makes exceptions un-catchable.
- **Use `Task`-based APIs** consistently (don't mix with `Wait()`/`.Result` which can deadlock).
- **Let `async` flow up** — `async` methods should return `Task`, not `void`.
- **`ConfigureAwait(false)`** in library code to avoid capturing the synchronization context.

## Key Takeaways

- `async`/`await` provide non-blocking, readable asynchronous code.
- `await` suspends the method without blocking the thread.
- Catch exceptions from awaited calls with normal `try`/`catch`.
- Prefer `Task`/`Task<T>` returns; reserve `async void` for event handlers.
