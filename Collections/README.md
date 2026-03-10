### Collections & Advanced Data Structures 🟣

> Everything you need to know about C# collections — from legacy ArrayList to thread-safe Concurrent types.

---

## Table of Contents
- [Overview](#overview)
- [Non-Generic Collections](#non-generic-collections-systemcollections)
  - [ArrayList](#arraylist)
  - [Hashtable](#hashtable)
  - [Queue, Stack, SortedList](#queue-stack-sortedlist-non-generic)
- [Generic Collections](#generic-collections-systemcollectionsgeneric)
  - [List\<T\>](#listt)
  - [Dictionary\<TKey, TValue\>](#dictionarytkey-tvalue)
  - [Queue\<T\> & Stack\<T\>](#queuet--stackt)
  - [HashSet\<T\>](#hashsett)
  - [LinkedList\<T\>](#linkedlistt)
  - [SortedList\<TKey, TValue\>](#sortedlisttkey-tvalue)
- [Specialized – Concurrent Collections](#specialized--concurrent-collections)
- [Quick Decision Guide](#quick-decision-guide)

---

## Overview

| Category | Namespace | Type Safety | Use When |
|---|---|---|---|
| Non-Generic | `System.Collections` | ❌ No | Legacy code only |
| Generic | `System.Collections.Generic` | ✅ Yes | **Modern C# (default choice)** |
| Concurrent | `System.Collections.Concurrent` | ✅ Yes | Multi-threaded access |

> 💡 **Rule:** Always prefer Generic collections in modern C#. Non-generic collections store `object` — no type safety, requires casting, and value types get boxed (slower).

---

## Non-Generic Collections (`System.Collections`)

### ArrayList

A resizable array storing `object` — the non-generic predecessor to `List<T>`.

```csharp
using System.Collections;

ArrayList list = new ArrayList();

list.Add(42);           // int — gets boxed to object
list.Add("Hello");      // string
list.Add(3.14);         // double

// Must CAST when reading — risk of InvalidCastException!
int    num  = (int)list[0];      // 42
string text = (string)list[1];   // "Hello"

// Common methods
list.Insert(1, "inserted");  // insert at index 1
list.Remove(42);             // remove by value
list.RemoveAt(0);            // remove by index
list.Contains("Hello");      // true
list.Sort();                 // sort (all same type needed)
list.Reverse();              // reverse order
list.Clear();                // remove all

Console.WriteLine(list.Count);     // item count
Console.WriteLine(list.Capacity);  // internal buffer size

foreach (object item in list)
    Console.WriteLine(item);
```

> ⚠️ **Avoid in modern code.** Use `List<T>` instead. Boxing/unboxing value types is slow and errors only appear at runtime.

---

### Hashtable

A non-generic key-value store. Both keys and values are `object`.

```csharp
Hashtable ht = new Hashtable();

ht.Add("name", "Alice");
ht.Add("age",  30);
ht["city"] = "London";    // add or update

// Must CAST when reading
string name = (string)ht["name"]!;
int    age  = (int)ht["age"]!;

// Check before reading
if (ht.ContainsKey("name"))
    Console.WriteLine(ht["name"]);

ht.Remove("age");
Console.WriteLine(ht.ContainsValue("Alice"));  // true
Console.WriteLine(ht.Count);                   // 2

// Iterate (no guaranteed order)
foreach (DictionaryEntry entry in ht)
    Console.WriteLine($"{entry.Key} = {entry.Value}");
```

> ⚠️ **Avoid in modern code.** Use `Dictionary<TKey, TValue>` instead.

---

### Queue, Stack, SortedList (Non-Generic)

```csharp
// Queue (FIFO)
Queue queue = new Queue();
queue.Enqueue("Task 1");
queue.Enqueue("Task 2");
object first = queue.Dequeue();  // "Task 1"
object peek  = queue.Peek();     // "Task 2" (no removal)

// Stack (LIFO)
Stack stack = new Stack();
stack.Push("Page 1");
stack.Push("Page 2");
object top = stack.Pop();    // "Page 2"
object p   = stack.Peek();   // "Page 1"

// SortedList (auto-sorts by key)
SortedList sl = new SortedList();
sl.Add("banana", 2);
sl.Add("apple",  1);  // inserted out of order
sl.Add("cherry", 3);
// Iterates: apple, banana, cherry (alphabetical)
foreach (DictionaryEntry e in sl)
    Console.Write($"{e.Key} ");
```

---

## Generic Collections (`System.Collections.Generic`)

### List\<T\>

The most commonly used collection. Type-safe, dynamically resizing array.

```csharp
using System.Collections.Generic;

// Create
List<string> names = new List<string>();
List<int>    nums  = new List<int> { 1, 2, 3 };

// Add
names.Add("Alice");
names.Add("Bob");
names.AddRange(new[] { "Charlie", "Diana" });
names.Insert(1, "Zara");      // insert at index 1

// Access
string first = names[0];      // "Alice"
string last  = names[^1];     // last element (C# 8+)
int    count = names.Count;

// Search
bool   has   = names.Contains("Bob");         // true
int    idx   = names.IndexOf("Bob");          // index
string? found = names.Find(n => n.StartsWith("C")); // "Charlie"
List<string> long5 = names.FindAll(n => n.Length > 4);

// Sort & Order
names.Sort();                                 // alphabetical A→Z
names.Sort((a, b) => b.CompareTo(a));        // Z→A (custom)
names.Reverse();

// Remove
names.Remove("Bob");                          // by value
names.RemoveAt(0);                            // by index
names.RemoveAll(n => n.Length < 4);          // by condition
names.Clear();                               // remove all

// Convert
string[] arr = names.ToArray();
names.ForEach(n => Console.WriteLine(n));     // inline loop
```

---

### Dictionary\<TKey, TValue\>

Type-safe key-value store. O(1) average lookup. The most important generic collection after `List<T>`.

```csharp
// Create with initializer
Dictionary<string, int> ages = new()
{
    { "Alice", 30 },
    { "Bob",   25 },
    { "Charlie", 35 }
};

// Add & Update
ages["Diana"] = 28;            // add or update (no exception)
ages.Add("Eve", 22);           // throws if key already exists!
ages.TryAdd("Alice", 99);      // safe — returns false if exists

// Read
int age = ages["Alice"];       // 30 — throws KeyNotFoundException if missing!

// ✅ BEST PRACTICE — use TryGetValue for safe reads
if (ages.TryGetValue("Bob", out int bobAge))
    Console.WriteLine($"Bob is {bobAge}");

// Check
ages.ContainsKey("Alice");     // true
ages.ContainsValue(30);        // true (slower — O(n))

// Remove
ages.Remove("Bob");

// Iterate
foreach (KeyValuePair<string, int> kv in ages)
    Console.WriteLine($"{kv.Key} → {kv.Value}");

// Deconstruction (cleaner syntax)
foreach (var (name, a) in ages)
    Console.WriteLine($"{name}: {a}");

// Keys and Values only
foreach (string key in ages.Keys)   Console.Write(key + " ");
foreach (int    val in ages.Values) Console.Write(val + " ");
```

> ⚠️ Always use `TryGetValue` instead of `dict["key"]` to avoid `KeyNotFoundException`.

---

### Queue\<T\> & Stack\<T\>

```csharp
// Queue<T> — FIFO (First In, First Out)
Queue<string> q = new();
q.Enqueue("Email 1");
q.Enqueue("Email 2");
q.Enqueue("Email 3");

string next = q.Dequeue();        // "Email 1"
string peek = q.Peek();           // "Email 2" (no removal)
bool   has  = q.Contains("Email 3"); // true
Console.WriteLine(q.Count);       // 2
// Use for: print queues, task schedulers, BFS graph traversal

// Stack<T> — LIFO (Last In, First Out)
Stack<int> stack = new();
stack.Push(10);
stack.Push(20);
stack.Push(30);

int top = stack.Pop();    // 30
int tip = stack.Peek();   // 20 (no removal)
Console.WriteLine(stack.Count); // 2
// Use for: undo/redo, call stack simulation, DFS, expression parsing
```

---

### HashSet\<T\>

A set of **unique** values with O(1) lookup. Ignores duplicate adds.

```csharp
HashSet<string> tags = new() { "c#", "dotnet", "azure" };
tags.Add("c#");       // ignored — already exists!
tags.Add("blazor");   // added
Console.WriteLine(tags.Count);  // 4

// Set operations
HashSet<int> a = new() { 1, 2, 3, 4 };
HashSet<int> b = new() { 3, 4, 5, 6 };

a.UnionWith(b);         // {1,2,3,4,5,6} — all items
a.IntersectWith(b);     // {3,4}         — common items only
a.ExceptWith(b);        // {1,2}         — a minus b
a.SymmetricExceptWith(b); // {1,2,5,6}  — not in both

bool isSubset = a.IsSubsetOf(b);
bool overlaps = a.Overlaps(b);
// Use for: unique tags, visited graph nodes, fast membership tests
```

---

### LinkedList\<T\>

Doubly-linked list. O(1) insertion/deletion anywhere — but no random index access.

```csharp
LinkedList<string> ll = new();
ll.AddLast("B");
ll.AddFirst("A");       // A → B
ll.AddLast("C");        // A → B → C

LinkedListNode<string> node = ll.Find("B")!;
ll.AddAfter(node, "B2");   // A → B → B2 → C
ll.AddBefore(node, "A2");  // A → A2 → B → B2 → C
ll.Remove(node);           // removes B

Console.WriteLine(ll.First?.Value);  // "A"
Console.WriteLine(ll.Last?.Value);   // "C"
Console.WriteLine(ll.Count);         // 4
// Use for: frequent middle insertions, music playlists, LRU cache
```

---

### SortedList\<TKey, TValue\>

Like Dictionary but always keeps keys sorted. Binary search for O(log n) lookup.

```csharp
SortedList<string, int> scores = new()
{
    { "Charlie", 85 },
    { "Alice",   92 },   // inserted out of order
    { "Bob",     78 }
};

// Always iterates in alphabetical key order
foreach (var (name, score) in scores)
    Console.WriteLine($"{name}: {score}");
// Output: Alice: 92, Bob: 78, Charlie: 85

scores.ContainsKey("Alice");   // true
scores.IndexOfKey("Bob");      // 1 (sorted index)
scores.RemoveAt(0);            // removes Alice (first sorted key)
```

---

## Specialized – Concurrent Collections

Thread-safe collections for multi-threaded applications (`System.Collections.Concurrent`).

```csharp
using System.Collections.Concurrent;

// ConcurrentDictionary — thread-safe key-value store
ConcurrentDictionary<string, int> visits = new();
visits.TryAdd("homepage", 1);

// AddOrUpdate — atomic increment (no race condition)
visits.AddOrUpdate("homepage",
    addValue: 1,
    updateValueFactory: (key, old) => old + 1);

int val = visits.GetOrAdd("about", 0);   // get or add default
visits.TryRemove("about", out _);

// ConcurrentQueue — thread-safe FIFO
ConcurrentQueue<string> jobs = new();
jobs.Enqueue("Job A");
jobs.Enqueue("Job B");

if (jobs.TryDequeue(out string? job))
    Console.WriteLine($"Processing: {job}");

if (jobs.TryPeek(out string? next))
    Console.WriteLine($"Next: {next}");

// ConcurrentBag — unordered, thread-safe bag
ConcurrentBag<int> results = new();
results.Add(1);
results.Add(2);
if (results.TryTake(out int r))
    Console.WriteLine(r);  // some item (order not guaranteed)
```

| Concurrent Type | Replaces | Pattern |
|---|---|---|
| `ConcurrentDictionary<K,V>` | `Dictionary<K,V>` | Thread-safe key-value |
| `ConcurrentQueue<T>` | `Queue<T>` | Thread-safe FIFO |
| `ConcurrentStack<T>` | `Stack<T>` | Thread-safe LIFO |
| `ConcurrentBag<T>` | `List<T>` | Unordered multi-thread |
| `BlockingCollection<T>` | Queue/Stack | Producer-consumer pipeline |

---

## Quick Decision Guide

```
Need a collection?
│
├── Multiple threads accessing it?
│   └── YES → Use Concurrent collections (ConcurrentDictionary, ConcurrentQueue...)
│
└── Single thread?
    ├── Need key → value lookup?     → Dictionary<TKey, TValue>   O(1)
    ├── Need sorted key-value?       → SortedList<TKey, TValue>   O(log n)
    ├── Need unique values only?     → HashSet<T>                 O(1)
    ├── Need FIFO (queue)?           → Queue<T>
    ├── Need LIFO (stack)?           → Stack<T>
    ├── Need frequent middle insert? → LinkedList<T>
    └── General purpose list?        → List<T>                    ← DEFAULT
```

```csharp
// Quick syntax reference
List<int>                  list  = new();
Dictionary<string, int>    dict  = new();
Queue<string>              queue = new();
Stack<int>                 stack = new();
HashSet<string>            set   = new();
LinkedList<string>         ll    = new();
SortedList<string, int>    sl    = new();
ConcurrentDictionary<string, int> cd = new();
```

---
