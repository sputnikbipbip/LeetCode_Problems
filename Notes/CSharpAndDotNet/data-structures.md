# C# Data Structures

A comprehensive reference of the relevant C# data structures, covering the BCL collections,
thread-safe/immutable variants, low-level memory types, and minimal custom implementations.

> Compatible with .NET 6+ / C# 10+.

## 1. Sequential Collections

### Arrays

Fixed-size, contiguous memory. `O(1)` access — the fastest option for static data.

```csharp
int[] numbers = new int[5];              // default initialization
int[] initialized = { 1, 2, 3, 4, 5 };   // inline init
var multiDim = new int[3, 4];            // 2D array
var jagged = new int[3][];               // array of arrays
```

### `List<T>`

Dynamic array. `O(1)` index access, `O(n)` insert/delete (amortized `O(1)` add).

```csharp
var list = new List<int> { 1, 2, 3 };
list.Add(4);
list.Insert(0, 0);
list.Remove(2);
list[0] = 10;
// list.Capacity auto-grows by ~2x when full
```

### `LinkedList<T>`

Doubly linked nodes. `O(1)` insert/delete at ends/known nodes; `O(n)` index access.

```csharp
var ll = new LinkedList<int>(new[] { 1, 2, 3 });
ll.AddLast(4);
ll.AddFirst(0);
var node = ll.Find(2);
ll.AddAfter(node, 99);
ll.Remove(node);
```

### `Queue<T>`

FIFO. `O(1)` Enqueue/Dequeue, implemented as a circular buffer.

```csharp
var q = new Queue<string>();
q.Enqueue("First");
q.Enqueue("Second");
string first = q.Dequeue();
bool hasNext = q.TryDequeue(out var second);
string peek = q.Peek();
```

### `Stack<T>`

LIFO. `O(1)` Push/Pop, implemented as a dynamic array.

```csharp
var s = new Stack<int>();
s.Push(1);
s.Push(2);
int top = s.Pop();
bool hasMore = s.TryPop(out var next);
int peek = s.Peek();
```

## 2. Associative & Hash-Based

### `Dictionary<TKey, TValue>`

Hash table. `O(1)` average lookup/insert/delete. Keys must override `GetHashCode`/`Equals`
or use an `IEqualityComparer`.

```csharp
var dict = new Dictionary<string, int>
{
    ["A"] = 1,
    ["B"] = 2
};
dict.Add("C", 3);
int val = dict["A"];
bool exists = dict.TryGetValue("B", out int v);
dict.Remove("A");
// Insertion order is preserved in .NET Core 2.0+
```

### `HashSet<T>`

Hash set of unique values. `O(1)` average `Contains`/`Add`/`Remove`. Supports set operations
like `UnionWith`, `IntersectWith`, `ExceptWith`.

```csharp
var set = new HashSet<int> { 1, 2, 3 };
set.Add(2); // ignored (already exists)
bool contains = set.Contains(1);
set.Remove(1);
set.UnionWith(new[] { 3, 4, 5 });
```

## 3. Sorted Collections

### `SortedDictionary<TKey, TValue>`

Red-Black tree. `O(log n)` operations, keys sorted, higher memory overhead than
`Dictionary`.

```csharp
var sorted = new SortedDictionary<string, int> { ["B"] = 2, ["A"] = 1 };
sorted.Add("C", 3); // automatically ordered
foreach (var kv in sorted) Console.WriteLine($"{kv.Key}: {kv.Value}");
```

### `SortedList<TKey, TValue>`

Array-based and sorted. `O(log n)` lookup, `O(n)` insert/delete; lower memory than
`SortedDictionary`.

```csharp
var sl = new SortedList<int, string> { [3] = "C", [1] = "A", [2] = "B" };
string val = sl.Values[0]; // "A" (access by index or key)
```

## 4. Concurrent Collections (Thread-Safe)

### `ConcurrentDictionary<TKey, TValue>`

Lock-free/striped locking, `O(1)` average, safe for multi-threaded reads/writes.

```csharp
var cd = new ConcurrentDictionary<string, int>();
cd.TryAdd("A", 1);
cd.AddOrUpdate("A", 1, (k, v) => v + 1);
cd.TryGetValue("A", out int val);
int computed = cd.GetOrAdd("B", _ => 42);
```

### `ConcurrentQueue<T>`

Lock-free FIFO, thread-safe `Enqueue`/`TryDequeue`.

```csharp
var cq = new ConcurrentQueue<int>();
cq.Enqueue(1);
bool success = cq.TryDequeue(out int val);
```

### `ConcurrentStack<T>`

Lock-free LIFO, thread-safe `Push`/`TryPop`.

```csharp
var cs = new ConcurrentStack<int>();
cs.Push(1);
bool success = cs.TryPop(out int val);
```

### `BlockingCollection<T>`

Producer/Consumer wrapper with bounded capacity and cancellation support.

```csharp
var bc = new BlockingCollection<int>(boundedCapacity: 100);
bc.Add(1);
int item = bc.Take(); // blocks if empty
bc.CompleteAdding();  // signals consumers
```

## 5. Immutable Collections

`ImmutableArray<T>`, `ImmutableList<T>`, `ImmutableDictionary<TKey, TValue>`, etc. are
persistent data structures with `O(log n)` or `O(1)` structural sharing — thread-safe by
design.

```csharp
using System.Collections.Immutable;

var arr = ImmutableArray.Create(1, 2, 3);
var newArr = arr.Add(4); // returns new array, original unchanged

var dict = ImmutableDictionary.Create<string, int>();
dict = dict.SetItem("A", 1).SetItem("B", 2);
```

## 6. Low-Level / Zero-Allocation Types

### `Span<T>` & `Memory<T>`

Stack/heap-agnostic views over contiguous memory. Zero allocation — used for
high-performance parsing, networking, and buffers.

```csharp
int[] arr = { 1, 2, 3, 4, 5 };
Span<int> span = arr.AsSpan()[1..3]; // view over arr[1..2]
span[0] = 99; // modifies the original array

// Memory<T> can survive async/await
Memory<int> mem = arr;
```

## 7. Custom Implementations (Educational)

### Minimal Singly Linked List

```csharp
public class SinglyLinkedList<T>
{
    private class Node { public T Value; public Node? Next; public Node(T v) => Value = v; }
    private Node? _head;
    private Node? _tail;
    public int Count { get; private set; }

    public void AddLast(T value)
    {
        var node = new Node(value);
        if (_head == null) _head = _tail = node;
        else { _tail!.Next = node; _tail = node; }
        Count++;
    }

    public T? RemoveFirst()
    {
        if (_head == null) return default;
        T val = _head.Value;
        _head = _head.Next;
        if (_head == null) _tail = null;
        Count--;
        return val;
    }
}
```

### Minimal Binary Search Tree (unbalanced)

```csharp
public class BinarySearchTree<T> where T : IComparable<T>
{
    private class Node { public T Value; public Node? Left, Right; public Node(T v) => Value = v; }
    private Node? _root;
    public int Count { get; private set; }

    public void Insert(T value)
    {
        _root = Insert(_root, value);
        Count++;
    }

    private Node Insert(Node? node, T value)
    {
        if (node == null) return new Node(value);
        if (value.CompareTo(node.Value) < 0) node.Left = Insert(node.Left, value);
        else node.Right = Insert(node.Right, value);
        return node;
    }

    public bool Contains(T value) => Contains(_root, value);
    private bool Contains(Node? node, T value) => node != null && (
        value.CompareTo(node.Value) == 0 ||
        (value.CompareTo(node.Value) < 0 ? Contains(node.Left, value) : Contains(node.Right, value)));
}
```

## Quick Reference

| Structure | Ordering | Complexity | Notes |
| --- | --- | --- | --- |
| `Array` | Indexed | `O(1)` access | Fixed size, fastest for static data |
| `List<T>` | Indexed | `O(1)` access / `O(n)` insert | Auto-growing dynamic array |
| `LinkedList<T>` | Sequential | `O(1)` ends / `O(n)` index | Doubly linked |
| `Queue<T>` | FIFO | `O(1)` ends | Circular buffer |
| `Stack<T>` | LIFO | `O(1)` ends | Dynamic array |
| `Dictionary<,>` | Keyed | `O(1)` avg | Hash table |
| `HashSet<T>` | Keyed | `O(1)` avg | Unique values, set ops |
| `SortedDictionary<,>` | Sorted key | `O(log n)` | Red-Black tree |
| `SortedList<,>` | Sorted key | `O(log n)` lookup / `O(n)` insert | Array-based |
| Concurrent collections | — | — | Thread-safe variants |
| Immutable collections | — | `O(log n)` / `O(1)` | Persistent, thread-safe |
| `Span<T>` / `Memory<T>` | Indexed | — | Zero-allocation views |
