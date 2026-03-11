<div align="center">

# 🚀 C# Learning Journey

### Day-by-Day Revision & Notes
> A structured, beginner-to-proficient C# tutorial series with definitions, syntax, examples, and best practices.

[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)](https://dotnet.microsoft.com/)
[![Visual Studio](https://img.shields.io/badge/Visual%20Studio-2026-5C2D91?style=for-the-badge&logo=visual-studio&logoColor=white)](https://visualstudio.microsoft.com/)
[![Status](https://img.shields.io/badge/Status-Active-brightgreen?style=for-the-badge)](https://github.com/arvind01A)
[![Days](https://img.shields.io/badge/Days%20Completed-6-blue?style=for-the-badge)](https://github.com/arvind01A)

<br/>

**Author:** Arvind Kumar &nbsp;|&nbsp; **GitHub:** [@arvind01A](https://github.com/arvind01A) &nbsp;|&nbsp; **Started:** March 2026

</div>

---

## 📖 Overview

This repository documents my daily C# revision — from basics to advanced concepts.
Each day includes:

| 📌 What's Inside | Description |
|---|---|
| 📝 **Definitions** | Clear, concise explanations |
| 💻 **Syntax Examples** | Ready-to-reference patterns |
| 📋 **Code Snippets** | Copy-paste ready code |
| ⚠️ **Pitfalls** | Key differences & common mistakes |
| 📊 **Summary Tables** | Quick-reference comparisons |

> 💡 Perfect for **beginners**, **self-learners**, or anyone **refreshing C# skills** in 2026!

---

## 🎯 Goal

```
Fundamentals → OOP → Collections → Exception Hangling → File I/O → Expert_level-1 → Expert_level-2 → Expert_level-3
```

---

## 📅 Progress & Completed Days

| # | Day | Topic | Subtopics | Status |
|---|-----|-------|-----------|--------|
| 1 | **Day 1** | [Fundamentals](#-day-1--fundamentals) | Variables · Operators · Control Flow · Loops | ✅ Done |
| 2 | **Day 2** | [Classes & Objects](#-day-2--classes--objects) | Methods · Parameters · Constructors · Fields | ✅ Done |
| 3 | **Day 3** | [OOP Pillars](#-day-3--oop-pillars) | Encapsulation · Inheritance · Abstraction · Polymorphism | ✅ Done |
| 4 | **Day 4** | [Data Structures](#-day-4--basic-data-structures) | Arrays · Strings · StringBuilder · Tuples | ✅ Done |
| 5 | **Day 5** | [Generics](#-day-5--generics) | Generic Classes · Methods · Constraints | ✅ Done |
| 6 | **Day 6** | [Collections](#-day-6--collections) | Non-Generic · Generic · Specialized · Concurrent | ✅ Done |
| 7 | **Day 7** | [Exception & File Handling](#-day-7--exception--file-handling) | try-catch-finally · throw vs throw ex · Built-in Exceptions · Global Handler · StreamReader/Writer · File & Dir Operations · Async File I/O | ✅ Done |

> 🕒 **Last updated:** March 11, 2026

---

## 📚 Day-by-Day Notes

---

### 🟣 Day 1 — Fundamentals

<details>
<summary><b>Click to expand</b></summary>

#### 📦 Variables & Data Types
```csharp
int age = 25;
double price = 9.99;
string name = "Arvind";
bool isActive = true;
char grade = 'A';
var inferred = 42;          // Type inferred by compiler
const double PI = 3.14159;  // Constant
```

#### ➕ Operators
```csharp
// Arithmetic
int sum = 10 + 5;    // 15
int mod = 10 % 3;    // 1

// Comparison
bool isEqual = (5 == 5);   // true
bool notEqual = (5 != 4);  // true

// Logical
bool result = (true && false);  // false
bool either = (true || false);  // true
```

#### 🔀 Control Statements
```csharp
// if / else if / else
if (age >= 18) { Console.WriteLine("Adult"); }
else if (age >= 13) { Console.WriteLine("Teen"); }
else { Console.WriteLine("Child"); }

// switch expression (C# 8+)
string label = age switch {
    >= 18 => "Adult",
    >= 13 => "Teen",
    _     => "Child"
};
```

#### 🔁 Loops
```csharp
for (int i = 0; i < 5; i++) { /* ... */ }
while (condition) { /* ... */ }
do { /* ... */ } while (condition);
foreach (var item in collection) { /* ... */ }
```

#### ⏭️ Jump Statements
```csharp
break;      // Exit loop
continue;   // Skip to next iteration
return;     // Exit method
goto label; // Jump to label (avoid!)
```

</details>

---

### 🔵 Day 2 — Classes & Objects

<details>
<summary><b>Click to expand</b></summary>

#### 🏗️ Methods & Parameters
```csharp
// Return types
public int Add(int a, int b) => a + b;
public void Print(string msg) => Console.WriteLine(msg);

// Parameter passing
public void Swap(ref int a, ref int b) { (a, b) = (b, a); }
public void TryParse(string s, out int result) { result = int.Parse(s); }

// Optional parameters & named arguments
public void Greet(string name, string greeting = "Hello") =>
    Console.WriteLine($"{greeting}, {name}!");

Greet(name: "Arvind", greeting: "Hi");
```

#### 🏛️ Constructors
```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Person() { }                          // Default
    public Person(string name) => Name = name;   // Parameterized
    public Person(Person other)                  // Copy
        => (Name, Age) = (other.Name, other.Age);

    static Person() { /* Runs once, before first use */ } // Static
}
```

#### 🗂️ Fields
```csharp
public class Circle
{
    private double _radius;          // Private field
    public static int Count = 0;     // Static field
    public readonly double Id;       // Readonly (set in constructor only)
}
```

</details>

---

### 🟢 Day 3 — OOP Pillars

<details>
<summary><b>Click to expand</b></summary>

#### 🔒 Encapsulation
```csharp
public class BankAccount
{
    private decimal _balance;  // private field

    public decimal Balance     // public property
    {
        get => _balance;
        private set => _balance = value >= 0 ? value : 0;
    }
}
```

**Access Modifiers:**
| Modifier | Accessible From |
|---|---|
| `public` | Anywhere |
| `private` | Same class only |
| `protected` | Same class + subclasses |
| `internal` | Same assembly |
| `protected internal` | Same assembly or subclasses |

#### 🧬 Inheritance
```csharp
public class Animal
{
    public virtual void Speak() => Console.WriteLine("...");
}

public class Dog : Animal
{
    public override void Speak() => Console.WriteLine("Woof!");
    public Dog(string name) : base() { }  // base keyword
}
```

#### 🎭 Abstraction
```csharp
public abstract class Shape
{
    public abstract double Area();        // Must override
    public void Display() => Console.WriteLine($"Area: {Area()}");
}

public interface IDrawable
{
    void Draw();
    string Color { get; set; }           // Property in interface
}
```

#### 🔄 Polymorphism
```csharp
// Compile-time (Method Overloading)
public int Multiply(int a, int b) => a * b;
public double Multiply(double a, double b) => a * b;

// Runtime (Method Overriding)
Animal animal = new Dog();
animal.Speak();  // Outputs: "Woof!" → resolved at runtime
```

#### 📇 Indexers
```csharp
public class WordCollection
{
    private string[] _words = new string[10];
    public string this[int index]
    {
        get => _words[index];
        set => _words[index] = value;
    }
}
```

</details>

---

### 🟡 Day 4 — Basic Data Structures

<details>
<summary><b>Click to expand</b></summary>

#### 📐 Arrays
```csharp
// Single-dimensional
int[] nums = { 1, 2, 3, 4, 5 };
int[] sized = new int[5];

// Multi-dimensional
int[,] matrix = new int[3, 3];

// Jagged
int[][] jagged = new int[3][];
jagged[0] = new int[] { 1, 2, 3 };

// Array methods
Array.Sort(nums);
Array.Reverse(nums);
int idx = Array.IndexOf(nums, 3);
int[] copy = (int[])nums.Clone();
```

#### 🔤 Strings
```csharp
string s = "Hello, Arvind!";

// Common operations
s.ToUpper();           // "HELLO, ARVIND!"
s.Contains("Arvind");  // true
s.Replace("Hello", "Hi");
s.Split(',');          // ["Hello", " Arvind!"]
s.Trim();

// Interpolation
string msg = $"Name: {name}, Age: {age}";

// String vs StringBuilder
// ❌ String: creates new object each concat (slow in loops)
// ✅ StringBuilder: mutable, fast for many appends
var sb = new StringBuilder();
sb.Append("Hello");
sb.AppendLine(" World");
string result = sb.ToString();
```

#### 📦 Tuples
```csharp
// Named tuples
var person = (Name: "Arvind", Age: 25, City: "Delhi");
Console.WriteLine(person.Name);  // "Arvind"

// Deconstruction
var (name, age, city) = person;
```

</details>

---

### 🟠 Day 5 — Generics

<details>
<summary><b>Click to expand</b></summary>

#### 📦 Generic Classes
```csharp
// Single type parameter
public class Box<T>
{
    public T Value { get; set; }
    public Box(T value) => Value = value;
}

// Multiple parameters
public class Pair<TKey, TValue>
{
    public TKey Key { get; set; }
    public TValue Value { get; set; }
}

var intBox = new Box<int>(42);
var pair = new Pair<string, int> { Key = "age", Value = 25 };
```

#### ⚙️ Generic Methods
```csharp
public static void Swap<T>(ref T a, ref T b) => (a, b) = (b, a);

// Explicit call
Swap<int>(ref x, ref y);

// Type inference (compiler infers T = int)
Swap(ref x, ref y);
```

#### 📏 Generic Constraints

| Constraint | Meaning |
|---|---|
| `where T : class` | Reference types only |
| `where T : struct` | Value types only |
| `where T : new()` | Must have parameterless constructor |
| `where T : IComparable<T>` | Must implement IComparable |
| `where T : Animal` | Must inherit from Animal |

```csharp
// Combined constraints
public class Repository<T>
    where T : class, IEntity, new()
{
    public T Create() => new T();
}
```

</details>

---

### 🔷 Day 6 — Collections

<details>
<summary><b>Click to expand</b></summary>

#### 🗂️ Non-Generic Collections (`System.Collections`)
```csharp
ArrayList list = new ArrayList();
list.Add(1); list.Add("hello"); // mixed types (avoid!)

Hashtable ht = new Hashtable();
ht["key"] = "value";

Queue queue = new Queue();
queue.Enqueue("first"); queue.Dequeue();

Stack stack = new Stack();
stack.Push("top"); stack.Pop();
```
> ⚠️ **Avoid** non-generic collections — no type safety, uses boxing/unboxing.

#### ✅ Generic Collections (`System.Collections.Generic`)
```csharp
// List<T>
List<int> numbers = new List<int> { 1, 2, 3 };
numbers.Add(4);
numbers.Remove(2);
numbers.Sort();

// Dictionary<TKey, TValue>
var scores = new Dictionary<string, int>();
scores["Arvind"] = 95;
scores.TryGetValue("Arvind", out int score);

// HashSet<T> — unique values only
var set = new HashSet<string> { "a", "b", "a" }; // Count = 2

// Queue<T> & Stack<T>
var queue = new Queue<string>();
queue.Enqueue("first"); var item = queue.Dequeue();

var stack = new Stack<int>();
stack.Push(1); var top = stack.Pop();

// LinkedList<T>
var linked = new LinkedList<int>();
linked.AddFirst(1); linked.AddLast(2);

// SortedList<TKey, TValue>
var sorted = new SortedList<string, int>();
sorted.Add("banana", 2); sorted.Add("apple", 1); // auto-sorted by key
```

#### ⚡ Concurrent Collections (`System.Collections.Concurrent`)
```csharp
// Thread-safe collections
ConcurrentDictionary<string, int> dict = new();
ConcurrentQueue<string> queue = new();
ConcurrentBag<int> bag = new();
BlockingCollection<int> blocking = new();
```

**Quick Comparison:**
| Collection | Ordered | Unique | Key-Value | Thread-Safe |
|---|---|---|---|---|
| `List<T>` | ✅ | ❌ | ❌ | ❌ |
| `Dictionary<K,V>` | ❌ | ✅ Keys | ✅ | ❌ |
| `HashSet<T>` | ❌ | ✅ | ❌ | ❌ |
| `SortedList<K,V>` | ✅ | ✅ Keys | ✅ | ❌ |
| `ConcurrentDictionary` | ❌ | ✅ Keys | ✅ | ✅ |

</details>

## 🔴 Day 7 — Exception & File Handling

<details>
<summary><b>Click to expand</b></summary>

---

## 🛡️ Exception Handling

#### 🔁 try-catch-finally

```csharp
try
{
    int result = 10 / int.Parse("0");   // throws DivideByZeroException
}
catch (DivideByZeroException ex)
{
    Console.WriteLine($"Math error: {ex.Message}");
}
catch (FormatException ex)
{
    Console.WriteLine($"Format error: {ex.Message}");
}
catch (Exception ex)                    // catch-all — always place last
{
    Console.WriteLine($"Unexpected: {ex.Message}");
}
finally
{
    Console.WriteLine("Always runs — use for cleanup (close DB, file, etc.)");
}
```

> 💡 `finally` always executes — even if an exception is thrown or `return` is called inside `try`.

---

#### 🚀 Throwing Exceptions — `throw` vs `throw ex`

| | `throw` | `throw ex` |
|---|---|---|
| **Stack trace** | ✅ Preserved (original call site) | ❌ Reset (loses origin info) |
| **Best practice** | ✅ Always prefer this | ⚠️ Avoid — loses debugging info |

```csharp
// ✅ CORRECT — preserves full stack trace
catch (Exception ex)
{
    Log(ex);
    throw;             // re-throws original exception as-is
}

// ❌ AVOID — resets stack trace to this line
catch (Exception ex)
{
    Log(ex);
    throw ex;          // stack trace origin is now lost!
}

// ✅ Wrapping with context (inner exception preserved)
catch (SqlException ex)
{
    throw new DataAccessException("DB query failed", ex);
}
```

---

#### 🧱 Built-in Exceptions

| Exception | Thrown When |
|---|---|
| `ArgumentNullException` | Argument is `null` when not allowed |
| `ArgumentOutOfRangeException` | Argument is outside valid range |
| `IndexOutOfRangeException` | Array/list index is invalid |
| `NullReferenceException` | Accessing member of a `null` object |
| `InvalidOperationException` | Method call invalid for current state |
| `FormatException` | String format is invalid e.g. `int.Parse("abc")` |
| `DivideByZeroException` | Integer division by zero |
| `OverflowException` | Arithmetic overflow in `checked` context |
| `FileNotFoundException` | File does not exist |
| `NotImplementedException` | Method is not yet implemented |

```csharp
// ✅ Creating a custom exception
public class InsufficientFundsException : Exception
{
    public decimal Amount { get; }
    public InsufficientFundsException(decimal amount)
        : base($"Insufficient funds. Required: {amount}")
    {
        Amount = amount;
    }
}

// Usage
throw new InsufficientFundsException(500.00m);
```

---

#### 🌐 Global Exception Handling

```csharp
// Console / Worker apps
AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
{
    var ex = (Exception)e.ExceptionObject;
    Console.WriteLine($"FATAL: {ex.Message}");
    // Log, alert, graceful shutdown
};

// ASP.NET Core — custom global middleware
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public GlobalExceptionMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        try { await _next(context); }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync($"Error: {ex.Message}");
        }
    }
}
```

---

## 📁 File Handling (`System.IO`)

#### 📖 Reading & Writing — `StreamReader` / `StreamWriter`

```csharp
// ✅ Writing to a file
using (var writer = new StreamWriter("notes.txt"))
{
    writer.WriteLine("Hello, Arvind!");
    writer.WriteLine("Day 7 - File Handling");
}

// ✅ Reading from a file
using (var reader = new StreamReader("notes.txt"))
{
    string? line;
    while ((line = reader.ReadLine()) != null)
        Console.WriteLine(line);
}

// Shorthand helpers
string content  = File.ReadAllText("notes.txt");
string[] lines  = File.ReadAllLines("notes.txt");
```

> 💡 Always use `using` — it calls `Dispose()` automatically and safely closes the stream.

---

#### 🗂️ File & Directory Operations

```csharp
// ── File operations ──────────────────────────────────────
File.Create("new.txt");
File.Delete("old.txt");
File.Copy("source.txt", "dest.txt");
File.Move("source.txt", "moved.txt");
bool exists = File.Exists("notes.txt");
File.WriteAllText("log.txt", "entry");           // overwrites
File.AppendAllText("log.txt", "\nnew entry");    // appends

// File metadata
FileInfo info = new FileInfo("notes.txt");
Console.WriteLine(info.Length);        // size in bytes
Console.WriteLine(info.CreationTime);
Console.WriteLine(info.Extension);    // ".txt"

// ── Directory operations ─────────────────────────────────
Directory.CreateDirectory("logs/2026");
Directory.Delete("logs", recursive: true);
Directory.Move("old-folder", "new-folder");
bool dirExists = Directory.Exists("logs");
string[] files = Directory.GetFiles(".", "*.txt");

// ── Path helpers ─────────────────────────────────────────
string full     = Path.GetFullPath("notes.txt");
string name     = Path.GetFileName("path/notes.txt");          // "notes.txt"
string noExt    = Path.GetFileNameWithoutExtension("n.txt");   // "n"
string ext      = Path.GetExtension("notes.txt");              // ".txt"
string combined = Path.Combine("logs", "2026", "app.log");     // safe OS join
```

---

#### ⚡ Async File Handling

```csharp
// Always prefer async I/O in real apps — never block the thread

string content = await File.ReadAllTextAsync("notes.txt");
string[] lines = await File.ReadAllLinesAsync("notes.txt");
await File.WriteAllTextAsync("output.txt", "Async content");
await File.AppendAllTextAsync("log.txt", $"\n{DateTime.Now}: entry");

// Async StreamReader — best for large files
await using var reader = new StreamReader("bigfile.txt");
while (!reader.EndOfStream)
{
    string? line = await reader.ReadLineAsync();
    Console.WriteLine(line);
}

// Async StreamWriter
await using var writer = new StreamWriter("result.txt");
await writer.WriteLineAsync("Written asynchronously!");
```

> 💡 Use `await using` (C# 8+) for async disposable streams — equivalent to `using` but async-safe.

**Quick Summary:**
| Method | Best For |
|---|---|
| `File.ReadAllText` / `WriteAllText` | Small files, simple operations |
| `File.ReadAllLines` / `WriteAllLines` | Line-by-line processing |
| `StreamReader` / `StreamWriter` | Large files, fine-grained control |
| `File.ReadAllTextAsync` | Non-blocking async I/O |
| `File.AppendAllText` | Logging, appending data |

</details>

---

## 🗂️ Repository Structure

```
csharp-learning/
│
├── 📁 day1/                         ← Fundamentals
│   ├── 01-Variables/
│   ├── 02-Operators/
│   ├── 03-control-statements/
│   ├── 04-jump-statements/
│   └── 05-loops/
│
├── 📁 day2/                         ← Classes & Objects
│   ├── 01-Methods-and-Parameters/
│   │   ├── 01-Return-Types/
│   │   ├── 02-Parameter-Passing/
│   │   ├── 03-Optional-Parameters/
│   │   └── 04-Named-Arguments/
│   ├── 02-Constructors/
│   │   ├── 01-Default/
│   │   ├── 02-Parameterized/
│   │   ├── 03-Overloading/
│   │   └── 04-Static-and-Copy/
│   └── 03-Fields/
│
├── 📁 day3/                         ← OOP 4 Pillars
│   ├── 01-Encapsulation/
│   │   ├── 01-Access-Modifiers/
│   │   └── 02-Properties/
│   ├── 02-Inheritance/
│   │   ├── 01-Types-of-Inheritance/
│   │   └── 02-base-and-this-keywords/
│   ├── 03-Abstraction/
│   │   └── Abstract-Classes-and-Interfaces/
│   ├── 04-Polymorphism/
│   │   ├── 01-Compile-Time/
│   │   └── 02-Runtime/
│   └── 05-Indexers/
│
├── 📁 day4/                         ← Basic Data Structures
│   ├── Arrays/
│   │   ├── Single-Dimensional/
│   │   ├── Array-Methods/
│   │   ├── Array-Class/
│   │   └── Multi-Dim-and-Jagged/
│   ├── Strings/
│   │   ├── String-Operations/
│   │   ├── Interpolation/
│   │   └── String-vs-StringBuilder/
│   └── Tuples/
│       └── Named-Tuples/
│
├── 📁 day5/                         ← Generics
│   ├── Generic-Classes/             → Box<T>, Pair<TKey,TValue>
│   ├── Generic-Methods/             → Explicit · Type Inference
│   └── Constraints/                 → class · struct · new() · IComparable
│
├── 📁 day6/                         ← Collections
│   ├── Non-Generic/                 → ArrayList · Hashtable · Queue · Stack
│   ├── Generic/                     → List<T> · Dictionary · HashSet · LinkedList
│   └── Specialized/                 → Concurrent Collections
│
├── 📁 day7/                         ← Exception & File Handling
│   ├── Exception-Handling/
│   │   ├── 01-try-catch-finally/
│   │   ├── 02-throw-vs-throw-ex/
│   │   ├── 03-Built-in-Exceptions/
│   │   └── 04-Global-Exception-Handling/
│   └── File-Handling/
│       ├── 01-StreamReader-StreamWriter/
│       ├── 02-File-and-Directory-Operations/
│       └── 03-Async-File-Handling/
│
└── 📄 README.md
```

---


## 🛠️ Tools & Environment

| Tool | Version | Purpose |
|---|---|---|
| **Visual Studio** | 2026 | Primary IDE |
| **.NET SDK** | 10.0 | Runtime & tooling |
| **C#** | 14 | Language version |
| **Git** | Latest | Version control |

---

## 🚀 How to Use This Repo

```bash
# 1. Clone the repository
git clone https://github.com/arvind01A/csharp-learning.git
cd csharp-learning

# 2. Navigate to any day
cd day1/01-Variables

# 3. Run any .cs file
dotnet script Program.cs
# or
dotnet run
```

---

## 🤝 Contributing & Feedback

Found a mistake? Have a better explanation?

- ⭐ **Star** this repo if it helped you
- 🐛 **Open an issue** for corrections or suggestions
- 🍴 **Fork & PR** if you'd like to contribute notes

---

<div align="center">

Made with ❤️ and ☕ by [Arvind Kumar](https://github.com/arvind01A)

![Visitor Badge](https://visitor-badge.laobi.icu/badge?page_id=arvind01A.csharp-learning)

*"The best way to learn is to teach."* — Keep pushing forward! 🚀

</div>
