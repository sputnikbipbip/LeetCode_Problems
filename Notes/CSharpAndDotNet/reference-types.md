# Reference Types

Reference types store a **reference** to the actual data, which lives on the **heap**.
They include **classes**, **arrays**, and **strings**.

When you assign a reference type to another variable, both variables point to the **same
object** in memory. Unlike value types, no copy of the data is created.

## Shared Reference Semantics

Because both variables reference the same object, changes made through one are visible
through the other.

```csharp
class MyClass
{
    public int Value { get; set; }
}

var obj1 = new MyClass { Value = 1 };
var obj2 = obj1;      // obj2 references the same object as obj1
obj2.Value = 10;
```

**Output:**

```text
obj1.Value: 10, obj2.Value: 10
```

Notice that `obj1.Value` changed even though we only modified `obj2` — they share one object.

## Common Reference Types

| Type | Notes |
| --- | --- |
| `class` | The primary user-defined reference type. |
| `array` | `int[]`, `string[]`, etc. — reference types. |
| `string` | Immutable reference type. |
| `delegate` | Encapsulates a method reference. |
| `interface` | A contract that classes/structs implement. |

## Value vs Reference Comparison

| Aspect | Value type | Reference type |
| --- | --- | --- |
| Storage | Stack / inline | Heap |
| Assignment | Copies the value | Copies the reference |
| Default value | e.g. `0`, `false` | `null` |
| Examples | `int`, `double`, `bool`, `struct`, `enum` | `class`, `array`, `string`, `delegate` |

## Key Takeaways

- Reference types store a reference to heap data, not the data itself.
- Assigning a reference type shares the object — mutations affect all references.
- `class`, `array`, `string`, `delegate`, and `interface` are reference types.
