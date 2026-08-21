// CSharpDataStructures.cs
// Compatible with .NET 6+ / C# 10+
// Compile: dotnet new console -n DSRef && replace Program.cs with this file && dotnet run

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CSharpDataStructures
{
    /// <summary>
    /// Comprehensive reference of relevant C# data structures.
    /// Covers BCL collections, thread-safe/immutable variants, 
    /// low-level memory types, and minimal custom implementations.
    /// </summary>
    public static class DataStructuresReference
    {
        #region 1. SEQUENTIAL COLLECTIONS

        /// <summary>
        /// Array: Fixed-size, contiguous memory. O(1) access. Fastest for static data.
        /// </summary>
        public static void Arrays()
        {
            int[] numbers = new int[5];              // Default initialization
            int[] initialized = { 1, 2, 3, 4, 5 };   // Inline init
            var multiDim = new int[3, 4];            // 2D array
            var jagged = new int[3][];               // Array of arrays
        }

        /// <summary>
        /// List<T>: Dynamic array. O(1) index access, O(n) insert/delete (amortized O(1) add).
        /// </summary>
        public static void Lists()
        {
            var list = new List<int> { 1, 2, 3 };
            list.Add(4);
            list.Insert(0, 0);
            list.Remove(2);
            list[0] = 10;
            // Capacity vs Count: list.Capacity auto-grows by ~2x when full
        }

        /// <summary>
        /// LinkedList<T>: Doubly linked nodes. O(1) insert/delete at ends/known nodes. O(n) index access.
        /// </summary>
        public static void LinkedLists()
        {
            var ll = new LinkedList<int>(new[] { 1, 2, 3 });
            ll.AddLast(4);
            ll.AddFirst(0);
            var node = ll.Find(2);
            ll.AddAfter(node, 99);
            ll.Remove(node);
        }

        /// <summary>
        /// Queue<T>: FIFO. O(1) Enqueue/Dequeue. Implemented as circular buffer.
        /// </summary>
        public static void Queues()
        {
            var q = new Queue<string>();
            q.Enqueue("First");
            q.Enqueue("Second");
            string first = q.Dequeue();
            bool hasNext = q.TryDequeue(out var second);
            string peek = q.Peek();
        }

        /// <summary>
        /// Stack<T>: LIFO. O(1) Push/Pop. Implemented as dynamic array.
        /// </summary>
        public static void Stacks()
        {
            var s = new Stack<int>();
            s.Push(1);
            s.Push(2);
            int top = s.Pop();
            bool hasMore = s.TryPop(out var next);
            int peek = s.Peek();
        }

        #endregion

        #region 2. ASSOCIATIVE & HASH-BASED

        /// <summary>
        /// Dictionary<TKey, TValue>: Hash table. O(1) avg lookup/insert/delete. Keys must override GetHashCode/Equals or use IEqualityComparer.
        /// </summary>
        public static void Dictionaries()
        {
            var dict = new Dictionary<string, int>
            {
                ["A"] = 1,
                ["B"] = 2
            };
            dict.Add("C", 3);
            int val = dict["A"];
            bool exists = dict.TryGetValue("B", out int v);
            dict.Remove("A");
            // Order: Insertion order preserved in .NET Core 2.0+
        }

        /// <summary>
        /// HashSet<T>: Hash set of unique values. O(1) avg Contains/Add/Remove. Set operations: UnionWith, IntersectWith, ExceptWith.
        /// </summary>
        public static void HashSets()
        {
            var set = new HashSet<int> { 1, 2, 3 };
            set.Add(2); // Ignored (already exists)
            bool contains = set.Contains(1);
            set.Remove(1);
            set.UnionWith(new[] { 3, 4, 5 });
        }

        #endregion

        #region 3. SORTED COLLECTIONS

        /// <summary>
        /// SortedDictionary<TKey, TValue>: Red-Black tree. O(log n) operations. Keys sorted. Higher memory overhead than Dictionary.
        /// </summary>
        public static void SortedDictionaries()
        {
            var sorted = new SortedDictionary<string, int> { ["B"] = 2, ["A"] = 1 };
            sorted.Add("C", 3); // Automatically ordered
            foreach (var kv in sorted) Console.WriteLine($"{kv.Key}: {kv.Value}");
        }

        /// <summary>
        /// SortedList<TKey, TValue>: Array-based sorted. O(log n) lookup, O(n) insert/delete. Lower memory than SortedDictionary.
        /// </summary>
        public static void SortedLists()
        {
            var sl = new SortedList<int, string> { [3] = "C", [1] = "A", [2] = "B" };
            // Access by index or key
            string val = sl.Values[0]; // "A"
        }

        #endregion

        #region 4. CONCURRENT COLLECTIONS (Thread-Safe)

        /// <summary>
        /// ConcurrentDictionary<TKey, TValue>: Lock-free/striped locking. O(1) avg. Safe for multi-threaded reads/writes.
        /// </summary>
        public static void ConcurrentDictionaries()
        {
            var cd = new ConcurrentDictionary<string, int>();
            cd.TryAdd("A", 1);
            cd.AddOrUpdate("A", 1, (k, v) => v + 1);
            cd.TryGetValue("A", out int val);
            int computed = cd.GetOrAdd("B", _ => 42);
        }

        /// <summary>
        /// ConcurrentQueue<T>: Lock-free FIFO. Thread-safe Enqueue/TryDequeue.
        /// </summary>
        public static void ConcurrentQueues()
        {
            var cq = new ConcurrentQueue<int>();
            cq.Enqueue(1);
            bool success = cq.TryDequeue(out int val);
        }

        /// <summary>
        /// ConcurrentStack<T>: Lock-free LIFO. Thread-safe Push/TryPop.
        /// </summary>
        public static void ConcurrentStacks()
        {
            var cs = new ConcurrentStack<int>();
            cs.Push(1);
            bool success = cs.TryPop(out int val);
        }

        /// <summary>
        /// BlockingCollection<T>: Producer/Consumer wrapper. Bounded capacity, cancellation support.
        /// </summary>
        public static void BlockingCollections()
        {
            var bc = new BlockingCollection<int>(boundedCapacity: 100);
            bc.Add(1);
            int item = bc.Take(); // Blocks if empty
            bc.CompleteAdding();  // Signals consumers
        }

        #endregion

        #region 5. IMMUTABLE COLLECTIONS

        /// <summary>
        /// ImmutableArray<T>, ImmutableList<T>, ImmutableDictionary<TKey, TValue>, etc.
        /// Persistent data structures. O(log n) or O(1) structural sharing. Thread-safe by design.
        /// Requires System.Collections.Immutable NuGet/package reference in .NET Framework.
        /// </summary>
        public static void ImmutableCollections()
        {
            var arr = ImmutableArray.Create(1, 2, 3);
            var newArr = arr.Add(4); // Returns new array, original unchanged

            var dict = ImmutableDictionary.Create<string, int>();
            dict = dict.SetItem("A", 1).SetItem("B", 2);
        }

        #endregion

        #region 6. LOW-LEVEL / ZERO-ALLOC TYPES

        /// <summary>
        /// Span<T> & Memory<T>: Stack/heap agnostic views over contiguous memory.
        /// Zero allocations, used for high-performance parsing, networking, buffers.
        /// </summary>
        public static void SpansAndMemory()
        {
            int[] arr = { 1, 2, 3, 4, 5 };
            Span<int> span = arr.AsSpan()[1..3]; // View over arr[1..2]
            span[0] = 99; // Modifies original array

            // Memory<T> can survive async/await
            Memory<int> mem = arr;
        }

        #endregion

        #region 7. CUSTOM IMPLEMENTATIONS (Educational)

        /// <summary>
        /// Minimal Singly Linked List<T>
        /// </summary>
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

        /// <summary>
        /// Minimal Binary Search Tree<T> (no balancing)
        /// </summary>
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

        #endregion

        #region DEMO / ENTRY POINT

        /// <summary>
        /// Quick demonstration of instantiating and using each structure.
        /// </summary>
        public static void RunDemo()
        {
            Console.WriteLine("=== C# Data Structures Demo ===");
            Arrays();
            Lists();
            LinkedLists();
            Queues();
            Stacks();
            Dictionaries();
            HashSets();
            SortedDictionaries();
            ConcurrentDictionaries();
            ImmutableCollections();
            SpansAndMemory();

            var customList = new SinglyLinkedList<int>();
            customList.AddLast(10); customList.AddLast(20);
            Console.WriteLine($"SinglyLinkedList RemoveFirst: {customList.RemoveFirst()}");

            var bst = new BinarySearchTree<int>();
            bst.Insert(5); bst.Insert(3); bst.Insert(7);
            Console.WriteLine($"BST Contains 3: {bst.Contains(3)}");
            Console.WriteLine("Demo complete.");
        }

        #endregion
    }

    /// <summary>
    /// Console entry point
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            DataStructuresReference.RunDemo();
        }
    }
}