# Control Flow

Control flow statements determine the order in which code is executed. C# provides several
looping constructs for repeating a block of code, plus conditional statements for branching.

## `for` Loop

A `for` loop repeats a block a fixed number of times using an init, condition, and iterator.

```csharp
for (int i = 0; i < 5; i++)
{
    Console.WriteLine($"Iteration {i}");
}
```

**Output:**

```text
Iteration 0
Iteration 1
Iteration 2
Iteration 3
Iteration 4
```

## `foreach` Loop

A `foreach` loop iterates over every element of a collection (array, `List<T>`, etc.)
without needing an index.

```csharp
int[] array = new int[] { 1, 2, 3, 4, 5 };

foreach (int num in array)
{
    Console.WriteLine(num);
}
```

**Output:**

```text
1
2
3
4
5
```

## `while` Loop

A `while` loop repeats as long as its condition is `true`. The condition is checked **before**
each iteration, so the body may never run.

```csharp
int count = 0;

while (count < 5)
{
    Console.WriteLine($"Count: {count}");
    count++;
}
```

**Output:**

```text
Count: 0
Count: 1
Count: 2
Count: 3
Count: 4
```

## When to Use Each

| Loop | Use case |
| --- | --- |
| `for` | Fixed number of iterations; you need an index. |
| `foreach` | Iterating every element of a collection (read-only, cleanest). |
| `while` | Repetition controlled by a condition that may change during iteration. |

## Key Takeaways

- `for` is index-based; `foreach` is element-based.
- `while` checks its condition up front.
- Use `break` to exit a loop early and `continue` to skip to the next iteration.
