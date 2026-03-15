<div align="center">

# 🚀 C# Learning Journey

### Day-by-Day Revision & Notes
> A structured, beginner-to-proficient C# tutorial series with definitions, syntax, examples, and best practices.

[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)](https://dotnet.microsoft.com/)
[![Visual Studio](https://img.shields.io/badge/Visual%20Studio-2026-5C2D91?style=for-the-badge&logo=visual-studio&logoColor=white)](https://visualstudio.microsoft.com/)
[![Status](https://img.shields.io/badge/Status-Completed-success?style=for-the-badge)](https://github.com/arvind01A)
[![Days](https://img.shields.io/badge/Days%20Completed-10%2F10-blue?style=for-the-badge)](https://github.com/arvind01A)
[![Roadmap](https://img.shields.io/badge/C%23%20Interview%20Roadmap-100%25-brightgreen?style=for-the-badge)](https://github.com/arvind01A)

<br/>

**Author:** Arvind Kumar &nbsp;|&nbsp; **GitHub:** [@arvind01A](https://github.com/arvind01A) &nbsp;|&nbsp; **Started:** March 2026

</div>

---

## 🎉 Journey Completed!

> **10 days. Beginner → Expert.**
> This repository is the complete C# Interview Roadmap — from fundamentals all the way to SOLID principles and Design Patterns.
> Thank you for following along! ⭐ Star this repo if it helped you.

---

## 📖 Overview

This repository documents my complete C# revision — from basics to expert-level concepts.
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

## 🎯 Roadmap — Completed ✅

```
Fundamentals → OOP → Collections → Exception Handling → File I/O → Expert_level-1 → Expert_level-2 → Expert_level-3
     ✅             ✅✅✅          ✅                    ✅           ✅               ✅                ✅
```

---

## 📅 All 10 Days — Complete

| # | Day | Topic | Subtopics | Status |
|---|-----|-------|-----------|--------|
| 1 | **Day 1** | [Fundamentals](#-day-1--fundamentals) | Variables · Operators · Control Flow · Loops | ✅ Done |
| 2 | **Day 2** | [Classes & Objects](#-day-2--classes--objects) | Methods · Parameters · Constructors · Fields | ✅ Done |
| 3 | **Day 3** | [OOP Pillars](#-day-3--oop-pillars) | Encapsulation · Inheritance · Abstraction · Polymorphism | ✅ Done |
| 4 | **Day 4** | [Data Structures](#-day-4--basic-data-structures) | Arrays · Strings · StringBuilder · Tuples | ✅ Done |
| 5 | **Day 5** | [Generics](#-day-5--generics) | Generic Classes · Methods · Constraints | ✅ Done |
| 6 | **Day 6** | [Collections](#-day-6--collections) | Non-Generic · Generic · Specialized · Concurrent | ✅ Done |
| 7 | **Day 7** | [Exception & File Handling](#-day-7--exception--file-handling) | try-catch-finally · throw vs throw ex · Built-in Exceptions · StreamReader/Writer · Async File I/O | ✅ Done |
| 8 | **Day 8** | [Expert I — Functional Programming & Delegates](#-day-8--expert-i--functional-programming--delegates) | Extension Methods · Lambda · LINQ · Pattern Matching · Action/Func/Predicate · Events | ✅ Done |
| 9 | **Day 9** | [Expert II — Multithreading, Async & Serialization](#-day-9--expert-ii--multithreading-async--serialization) | Thread · Task · Parallel.ForEach · Async/Await · IAsyncEnumerable · JSON/XML/Binary | ✅ Done |
| 10 | **Day 10** | [Expert III — Best Practices & Design Patterns](#-day-10--expert-iii--best-practices--design-patterns) | Naming · Clean Code · Null Safety · C# 12 Features · SOLID · Creational · Structural · Behavioral Patterns | ✅ Done |

> 🏁 **Completed:** March 14, 2026

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
if (age >= 18) { Console.WriteLine("Adult"); }
else if (age >= 13) { Console.WriteLine("Teen"); }
else { Console.WriteLine("Child"); }

string label = age switch {
    >= 18 => "Adult",
    >= 13 => "Teen",
    _     => "Child"
};
```

#### 🔁 Loops & Jump Statements
```csharp
for (int i = 0; i < 5; i++) { }
while (condition) { }
do { } while (condition);
foreach (var item in collection) { }

break; continue; return; // jump statements
```

</details>

---

### 🔵 Day 2 — Classes & Objects

<details>
<summary><b>Click to expand</b></summary>

#### 🏗️ Methods & Parameters
```csharp
public int Add(int a, int b) => a + b;
public void Swap(ref int a, ref int b) { (a, b) = (b, a); }
public void TryParse(string s, out int result) { result = int.Parse(s); }
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

    public Person() { }
    public Person(string name) => Name = name;
    public Person(Person other) => (Name, Age) = (other.Name, other.Age);
    static Person() { /* runs once */ }
}
```

#### 🗂️ Fields
```csharp
public class Circle
{
    private double _radius;
    public static int Count = 0;
    public readonly double Id;
}
```

</details>

---

### 🟢 Day 3 — OOP Pillars

<details>
<summary><b>Click to expand</b></summary>

#### 🔒 Encapsulation · 🧬 Inheritance · 🎭 Abstraction · 🔄 Polymorphism

```csharp
// Encapsulation
public class BankAccount
{
    private decimal _balance;
    public decimal Balance { get => _balance; private set => _balance = value >= 0 ? value : 0; }
}

// Inheritance
public class Animal { public virtual void Speak() => Console.WriteLine("..."); }
public class Dog : Animal { public override void Speak() => Console.WriteLine("Woof!"); }

// Abstraction
public abstract class Shape { public abstract double Area(); }
public interface IDrawable { void Draw(); string Color { get; set; } }

// Polymorphism — runtime
Animal animal = new Dog();
animal.Speak();  // "Woof!"
```

**Access Modifiers:**
| Modifier | Accessible From |
|---|---|
| `public` | Anywhere |
| `private` | Same class only |
| `protected` | Same class + subclasses |
| `internal` | Same assembly |
| `protected internal` | Same assembly or subclasses |

</details>

---

### 🟡 Day 4 — Basic Data Structures

<details>
<summary><b>Click to expand</b></summary>

```csharp
// Arrays
int[] nums = { 1, 2, 3, 4, 5 };
int[,] matrix = new int[3, 3];
int[][] jagged = new int[3][];
Array.Sort(nums); Array.Reverse(nums);

// Strings
string msg = $"Name: {name}, Age: {age}";
var sb = new StringBuilder();
sb.Append("Hello"); sb.AppendLine(" World");

// Tuples
var person = (Name: "Arvind", Age: 25, City: "Delhi");
var (name, age, city) = person;
```

</details>

---

### 🟠 Day 5 — Generics

<details>
<summary><b>Click to expand</b></summary>

```csharp
public class Box<T> { public T Value { get; set; } }
public class Pair<TKey, TValue> { public TKey Key; public TValue Value; }
public static void Swap<T>(ref T a, ref T b) => (a, b) = (b, a);
```

| Constraint | Meaning |
|---|---|
| `where T : class` | Reference types only |
| `where T : struct` | Value types only |
| `where T : new()` | Parameterless constructor |
| `where T : IComparable<T>` | Must implement IComparable |

</details>

---

### 🔷 Day 6 — Collections

<details>
<summary><b>Click to expand</b></summary>

```csharp
// Generic (preferred)
List<int> numbers       = new() { 1, 2, 3 };
var scores              = new Dictionary<string, int>();
var set                 = new HashSet<string>();
var sorted              = new SortedList<string, int>();

// Concurrent (thread-safe)
ConcurrentDictionary<string, int> dict = new();
ConcurrentQueue<string> queue           = new();
```

| Collection | Ordered | Unique | Key-Value | Thread-Safe |
|---|---|---|---|---|
| `List<T>` | ✅ | ❌ | ❌ | ❌ |
| `Dictionary<K,V>` | ❌ | ✅ Keys | ✅ | ❌ |
| `HashSet<T>` | ❌ | ✅ | ❌ | ❌ |
| `ConcurrentDictionary` | ❌ | ✅ Keys | ✅ | ✅ |

</details>

---

### 🔴 Day 7 — Exception & File Handling

<details>
<summary><b>Click to expand</b></summary>

```csharp
// try-catch-finally
try   { int r = 10 / int.Parse("0"); }
catch (DivideByZeroException ex) { Console.WriteLine(ex.Message); }
finally { Console.WriteLine("Cleanup!"); }

// throw vs throw ex
catch (Exception ex) { Log(ex); throw; }        // ✅ preserves stack trace
catch (Exception ex) { Log(ex); throw ex; }      // ❌ resets stack trace

// File I/O
using var writer = new StreamWriter("notes.txt");
writer.WriteLine("Hello!");
string content = File.ReadAllText("notes.txt");
await File.WriteAllTextAsync("out.txt", "async write");
string path = Path.Combine("logs", "app.log");
```

</details>

---

### 🟤 Day 8 — Expert I: Functional Programming & Delegates

<details>
<summary><b>Click to expand</b></summary>

```csharp
// Extension Methods
public static string Capitalize(this string s)
    => string.IsNullOrEmpty(s) ? s : char.ToUpper(s[0]) + s[1..];
"hello".Capitalize();  // "Hello"

// Lambda & LINQ
Func<int, int> square = x => x * x;
var result = students
    .Where(s => s.Score >= 80)
    .OrderByDescending(s => s.Score)
    .Select(s => s.Name);

// Pattern Matching
string desc = shape switch
{
    Circle c when c.Radius > 10 => "Large circle",
    Rectangle r                 => $"{r.Width}x{r.Height}",
    _                           => "Unknown"
};

// Delegates & Events
Action<string> greet = name => Console.WriteLine($"Hi {name}!");
Func<int, int> sq    = x => x * x;
Predicate<int> even  = n => n % 2 == 0;

public event EventHandler<OrderEventArgs> OrderPlaced;
OrderPlaced?.Invoke(this, new OrderEventArgs(item));
```

</details>

---

### ⚡ Day 9 — Expert II: Multithreading, Async & Serialization

<details>
<summary><b>Click to expand</b></summary>

```csharp
// Thread
Thread t = new Thread(() => { Thread.Sleep(500); Console.WriteLine("Done"); });
t.IsBackground = true;  t.Start();  t.Join();

// Task (preferred)
Task<int> task = Task.Run(() => 42);
int result = await task;
int[] all = await Task.WhenAll(t1, t2, t3);

// Parallel (CPU-bound)
Parallel.ForEach(items, item => ProcessItem(item));
Parallel.Invoke(() => TaskA(), () => TaskB());

// Async/Await
public async Task<string> FetchAsync(string url)
{
    using var client = new HttpClient();
    return await client.GetStringAsync(url);
}

// Async Streams
await foreach (int num in GenerateNumbersAsync(10))
    Console.WriteLine(num);

// Serialization
string json = JsonSerializer.Serialize(person, new JsonSerializerOptions { WriteIndented = true });
Person? p   = JsonSerializer.Deserialize<Person>(json);
```

| Format | Readable | Speed | Use Case |
|---|---|---|---|
| JSON | ✅ | Fast | APIs, web |
| XML | ✅ | Slow | Legacy, SOAP |
| Binary | ❌ | ⚡ Fastest | Cache, IPC |

</details>

---

### 🏆 Day 10 — Expert III: Best Practices & Design Patterns

<details>
<summary><b>Click to expand</b></summary>

---

## ✅ C# Best Practices

#### 📛 Naming Conventions
```csharp
// Classes, Methods, Properties → PascalCase
public class OrderService { }
public void PlaceOrder() { }
public string CustomerName { get; set; }

// Local variables, parameters → camelCase
int orderCount = 0;
void Process(string orderItem) { }

// Private fields → _camelCase
private readonly string _connectionString;

// Constants → PascalCase
public const int MaxRetries = 3;

// Interfaces → I prefix
public interface IOrderRepository { }

// Generics → T prefix
public class Repository<TEntity> { }
```

---

#### 🧹 Clean Code Principles
```csharp
// ✅ Small, focused methods
public async Task ProcessOrderAsync(Order order)
{
    await _validator.ValidateAsync(order);
    await _pricer.CalculateTotalAsync(order);
    await _repository.SaveAsync(order);
    await _notifier.SendConfirmationAsync(order);
}

// ✅ No magic numbers — use enums
public enum UserRole { Guest = 0, User = 1, Admin = 2 }
if (user.Role == UserRole.Admin) { }

// ✅ Expression bodies for simple members
public string FullName => $"{FirstName} {LastName}";

// ✅ Dispose resources
using var conn = new SqlConnection(connStr);
await using var stream = File.OpenWrite("out.txt");
```

---

#### 🛡️ Null Safety (C# 8+)
```csharp
string  name  = "Arvind";    // non-nullable
string? email = null;         // nullable

int? length = email?.Length;              // null-conditional
string display = name ?? "Anonymous";     // null-coalescing
cache ??= new Dictionary<string, int>();  // null-coalescing assignment

ArgumentNullException.ThrowIfNull(name);  // C# 10+ guard

// Required properties (C# 11+)
public class Order
{
    public required string Id { get; init; }
    public required string CustomerName { get; init; }
}
```

---

#### 🆕 Modern C# Features (C# 10–14)
```csharp
// Records (C# 9+)
public record Person(string Name, int Age);
var p2 = p1 with { Age = 26 };   // non-destructive mutation

// Global usings (C# 10+)
global using System.Text.Json;

// File-scoped namespace (C# 10+)
namespace MyApp.Services;

// Primary constructors (C# 12+)
public class OrderService(IOrderRepository repo, ILogger<OrderService> logger) { }

// Collection expressions (C# 12+)
int[] nums   = [1, 2, 3, 4, 5];
int[] merged = [..nums, 6, 7];

// Raw string literals (C# 11+)
string json = """
    {
        "name": "Arvind",
        "age": 25
    }
    """;
```

---

## 🏗️ SOLID Principles

#### **S** — Single Responsibility
```csharp
// ❌ One class doing everything
// ✅ Separate concerns
public class UserRepository    { public void Save(User u) { } }
public class EmailService      { public void SendWelcome(User u) { } }
public class UserReportService { public string Generate(User u) => ""; }
```

#### **O** — Open/Closed
```csharp
// ✅ Extend by adding new classes, never modify existing
public abstract class Shape       { public abstract double Area(); }
public class Circle    : Shape    { public override double Area() => Math.PI * Radius * Radius; }
public class Rectangle : Shape    { public override double Area() => Width * Height; }
public class Triangle  : Shape    { public override double Area() => 0.5 * Base * Height; }
```

#### **L** — Liskov Substitution
```csharp
// ✅ Subclasses must honour base class contracts
public abstract class Shape    { public abstract double Area(); }
public class Rectangle : Shape { public int Width, Height; public override double Area() => Width * Height; }
public class Square    : Shape { public int Side;           public override double Area() => Side * Side; }
```

#### **I** — Interface Segregation
```csharp
// ✅ Small focused interfaces
public interface IWorkable  { void Work(); }
public interface IEatable   { void Eat(); }
public class HumanWorker : IWorkable, IEatable { public void Work(){} public void Eat(){} }
public class RobotWorker : IWorkable           { public void Work(){} }
```

#### **D** — Dependency Inversion
```csharp
// ✅ Depend on abstractions, inject implementations
public interface IOrderRepository { Task SaveAsync(Order o); }

public class OrderService
{
    private readonly IOrderRepository _repo;
    public OrderService(IOrderRepository repo) => _repo = repo;
    public async Task PlaceOrderAsync(Order o) => await _repo.SaveAsync(o);
}

// ASP.NET Core DI
builder.Services.AddScoped<IOrderRepository, SqlOrderRepository>();
```

---

## 🎨 Design Patterns

#### 🏭 Singleton — one instance globally
```csharp
public sealed class AppConfig
{
    private static readonly Lazy<AppConfig> _instance = new(() => new AppConfig());
    private AppConfig() { }
    public static AppConfig Instance => _instance.Value;
    public string ConnectionString { get; set; } = "";
}
AppConfig.Instance.ConnectionString = "Server=...";
```

#### 🏭 Factory Method — decouple creation
```csharp
public static Notification Create(string type) => type switch
{
    "email" => new EmailNotification(),
    "sms"   => new SmsNotification(),
    "push"  => new PushNotification(),
    _       => throw new ArgumentException($"Unknown: {type}")
};
Notification.Create("email").Send("Order confirmed!");
```

#### 🏭 Builder — step-by-step construction
```csharp
string query = new QueryBuilder()
    .From("Orders")
    .Where("Status = 'Active'")
    .Where("CustomerId = 42")
    .OrderBy("CreatedAt DESC")
    .Limit(10)
    .Build();
```

#### 🔧 Repository — abstract data access
```csharp
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task DeleteAsync(int id);
}
```

#### 🔧 Decorator — add behaviour without inheritance
```csharp
// Base → CachedOrderService → LoggedOrderService
// Each wraps the inner and adds cross-cutting concerns
public class CachedOrderService : IOrderService
{
    private readonly IOrderService _inner;
    private readonly IMemoryCache  _cache;
    public async Task<Order> GetOrderAsync(int id)
    {
        if (_cache.TryGetValue(id, out Order? cached)) return cached!;
        var order = await _inner.GetOrderAsync(id);
        _cache.Set(id, order, TimeSpan.FromMinutes(5));
        return order;
    }
}
```

#### 🔁 Observer — notify subscribers
```csharp
public class StockTicker
{
    public event EventHandler<StockEventArgs>? PriceChanged;
    private decimal _price;
    public decimal Price { get => _price; set { _price = value; PriceChanged?.Invoke(this, new("AAPL", value)); } }
}
ticker.PriceChanged += (s, e) => Console.WriteLine($"[Alert] {e.Symbol}: ${e.Price}");
```

#### 🔁 Strategy — swap algorithms at runtime
```csharp
public interface ISortStrategy<T> { IEnumerable<T> Sort(IEnumerable<T> data); }
public class AscendingSort<T>  : ISortStrategy<T> where T : IComparable<T>
    { public IEnumerable<T> Sort(IEnumerable<T> d) => d.OrderBy(x => x); }
public class DescendingSort<T> : ISortStrategy<T> where T : IComparable<T>
    { public IEnumerable<T> Sort(IEnumerable<T> d) => d.OrderByDescending(x => x); }

var processor = new DataProcessor<int>(new AscendingSort<int>());
processor.SetStrategy(new DescendingSort<int>());  // swap at runtime
```

#### 🔁 Chain of Responsibility — pipeline processing
```csharp
var pipeline = new ValidationHandler();
pipeline.SetNext(new PricingHandler()).SetNext(new PersistenceHandler());
await pipeline.HandleAsync(order);
// ✅ Validated → ✅ Priced → ✅ Saved to DB
```

**Design Patterns Quick Reference:**
| Category | Pattern | Problem Solved |
|---|---|---|
| **Creational** | Singleton | One instance needed globally |
| **Creational** | Factory Method | Decouple object creation |
| **Creational** | Builder | Complex multi-step construction |
| **Structural** | Repository | Abstract data access |
| **Structural** | Decorator | Add behaviour without inheritance |
| **Behavioral** | Observer | Notify multiple subscribers |
| **Behavioral** | Strategy | Swap algorithms at runtime |
| **Behavioral** | Chain of Responsibility | Pipeline / middleware processing |

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
│   ├── Generic-Classes/
│   │        ├── Box<T>
│   │        ├── Pair<TKey
│   │        └── TValue>
│   ├── Generic-Methods/
│   │        ├── Explicit
│   │        └── Type Inference
│   └── Constraints/
│            ├── class
│            ├── struct
│            ├── new()
│            └── IComparable
│
├── 📁 day6/                         ← Collections/ Advance Data Structures
│   ├── Non-Generic/                 
│   │        ├── ArrayList
│   │        ├── Hashtable
│   │        ├── Queue
│   │        └── Stack
│   ├── Generic/                     
│   │        ├── List<T>
│   │        ├── Dictionary
│   │        ├── HashSet
│   │        └── LinkedList
│   └── Specialized/                 
│            └── Concurrent Collections
│
├── 📁 day7/                         ← Exception & File Handling
│   ├── Exception-Handling/
│   └── File-Handling/
│
├── 📁 day8/                         ← Expert I: Functional Programming & Delegates
│   ├── Functional-Programming/
│   ├── Delegates/
│   └── Events/
│
├── 📁 day9/                         ← Expert II: Multithreading, Async & Serialization
│   ├── Multithreading/
│   │   ├── 01-Thread-Class/
│   │   ├── 02-Task-Class/
│   │   └── 03-Parallel-ForEach/
│   ├── Async-Programming/
│   │   ├── 01-Async-and-Await/
│   │   ├── 02-Exception-Handling-in-Async/
│   │   └── 03-Async-Streams-IAsyncEnumerable/
│   └── Serialization/
│       ├── 01-JSON-Serialization/
│       ├── 02-XML-Serialization/
│       └── 03-Binary-Serialization/
│
├── 📁 day10/                        ← Expert III: Best Practices & Design Patterns
│   ├── Best-Practices/
│   │   ├── 01-Naming-Conventions/
│   │   ├── 02-Clean-Code/
│   │   ├── 03-Null-Safety/
│   │   └── 04-Modern-CSharp-Features/
│   └── SOLID-and-Design-Patterns/
│       ├── 01-SOLID-Principles/
│       │   ├── S-Single-Responsibility/
│       │   ├── O-Open-Closed/
│       │   ├── L-Liskov-Substitution/
│       │   ├── I-Interface-Segregation/
│       │   └── D-Dependency-Inversion/
│       ├── 02-Creational-Patterns/
│       │   ├── Singleton/
│       │   ├── Factory-Method/
│       │   └── Builder/
│       ├── 03-Structural-Patterns/
│       │   ├── Repository/
│       │   └── Decorator/
│       └── 04-Behavioral-Patterns/
│           ├── Observer/
│           ├── Strategy/
│           └── Chain-of-Responsibility/
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

### 🏁 10 Days · Complete C# Interview Roadmap

Made with ❤️ and ☕ by [Arvind Kumar](https://github.com/arvind01A)

![Visitor Badge](https://visitor-badge.laobi.icu/badge?page_id=arvind01A.csharp-learning)

*"The best way to learn is to teach."* — Keep pushing forward! 🚀

</div>
