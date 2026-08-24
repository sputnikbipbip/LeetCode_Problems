# `ref` and `out` Parameters

By default, method parameters are passed **by value** — the method works on a copy. The `ref`
and `out` keywords let a method operate on (and modify) the **caller's** variable directly.

## `ref` Parameter

A `ref` parameter passes a variable **by reference**, so the method can modify the original
variable. The variable **must be initialized** before it is passed.

```csharp
void UpdateRef(ref int number)
{
    number += 10;
}

int a = 5;                 // 'a' must be initialized before passing it as a ref parameter
UpdateRef(ref a);
Console.WriteLine($"After UpdateRef: {a}");
```

**Output:**

```text
After UpdateRef: 15
```

## `out` Parameter

An `out` parameter is also passed by reference, but the method is **required to assign** a
value to it before returning. The variable does **not** need to be initialized beforehand.

```csharp
void UpdateOut(out int number)
{
    number = 42; // Must assign a value before exiting the method
}

int b;                   // 'b' does not need to be initialized before passing it as an out parameter
UpdateOut(out b);
Console.WriteLine($"After UpdateOut: {b}");
```

**Output:**

```text
After UpdateOut: 42
```

## `ref` vs `out`

| Aspect | `ref` | `out` |
| --- | --- | --- |
| Must initialize before passing | Yes | No |
| Must assign in the method | No | Yes |
| Typical use | Modify an existing value | Produce a result (e.g. `TryParse`, `TryGetValue`) |

## Key Takeaways

- `ref` requires initialization and may read/write the value.
- `out` need not be initialized and must be assigned before the method returns.
- Both allow a method to modify the caller's variable directly.
