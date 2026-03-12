### Functional Programming 🟣

> Extension Methods · Lambda Expressions · LINQ · Pattern Matching · Pure & Higher-Order Functions

---

## Table of Contents
- [Extension Methods](#extension-methods)
- [Lambda Expressions](#lambda-expressions)
- [LINQ](#linq)
- [Pattern Matching (C# 8+)](#pattern-matching-c-8)
- [Immutability, Pure Functions & Higher-Order Functions](#immutability-pure-functions--higher-order-functions)
- [Quick Reference](#quick-reference)

---

## Extension Methods

Add new methods to existing types — without modifying the original class, subclassing, or needing the source code.

**Rules:**
1. Must be in a **static class**
2. Method must be **static**
3. First parameter uses the `this` keyword to specify the type being extended

```csharp
// Define in a static class
public static class StringExtensions
{
    // 'this string' = "extend the string type"
    public static bool IsPalindrome(this string s)
    {
        string clean = s.ToLower().Replace(" ", "");
        string rev   = new(clean.Reverse().ToArray());
        return clean == rev;
    }

    public static string Truncate(this string s, int maxLen)
        => s.Length <= maxLen ? s : s[..maxLen] + "...";

    public static string Repeat(this string s, int n)
        => string.Concat(Enumerable.Repeat(s, n));
}

public static class IntExtensions
{
    public static bool IsEven(this int n)  => n % 2 == 0;
    public static bool IsOdd(this int n)   => n % 2 != 0;
    public static bool IsPrime(this int n)
    {
        if (n < 2) return false;
        for (int i = 2; i * i <= n; i++)
            if (n % i == 0) return false;
        return true;
    }
    public static int Clamp(this int n, int min, int max)
        => Math.Max(min, Math.Min(max, n));
}

public static class ListExtensions
{
    public static T RandomItem<T>(this List<T> list)
        => list[new Random().Next(list.Count)];
}

// Usage — called exactly like built-in methods
Console.WriteLine("racecar".IsPalindrome());    // true
Console.WriteLine("Hello World".Truncate(5));   // Hello...
Console.WriteLine("ha".Repeat(3));              // hahaha
Console.WriteLine(7.IsPrime());                 // true
Console.WriteLine(150.Clamp(0, 100));           // 100
```

> 💡 Extension methods are how LINQ works — `Where`, `Select`, `OrderBy` etc. are all extension methods on `IEnumerable<T>`.

---

## Lambda Expressions

Anonymous inline functions using the `=>` "fat arrow" operator.

```csharp
// ── Syntax forms ─────────────────────────────────────────────
// Expression lambda (single expression)
x => x * 2                       // one param
(x, y) => x + y                  // two params
() => Console.WriteLine("Hi!")   // no params

// Statement lambda (multiple lines)
x =>
{
    int doubled = x * 2;
    return doubled + 1;
}

// ── Assigning lambdas ─────────────────────────────────────────
Func<int, int>          square  = x => x * x;
Func<int, int, int>     add     = (a, b) => a + b;
Predicate<int>          isEven  = n => n % 2 == 0;
Action<string>          greet   = name => Console.WriteLine($"Hi {name}");

Console.WriteLine(square(5));    // 25
Console.WriteLine(add(3, 4));    // 7
Console.WriteLine(isEven(6));    // true
greet("Alice");                  // Hi Alice

// ── Closures — capture outer variables by reference ──────────
int multiplier = 3;
Func<int, int> tripler = x => x * multiplier;   // captures 'multiplier'
Console.WriteLine(tripler(5));   // 15
multiplier = 10;
Console.WriteLine(tripler(5));   // 50  ← sees updated value!

// ── Passing lambdas to methods ────────────────────────────────
List<int> nums = new() { 1, 2, 3, 4, 5, 6 };
List<int> evens = nums.FindAll(n => n % 2 == 0);          // [2,4,6]
nums.Sort((a, b) => b.CompareTo(a));                       // descending
nums.ForEach(n => Console.Write(n + " "));                 // 6 5 4 3 2 1
```

> ⚠️ **Closure trap in loops:** Lambdas capture variables by reference. In a `for` loop, all lambdas capture the same loop variable. Copy to a local variable inside the loop body to avoid this.

---

## LINQ

Language Integrated Query — query any collection using a unified, functional syntax.

```csharp
using System.Linq;

List<int> nums = new() { 1, 5, 3, 8, 2, 9, 4, 7, 6 };

// ── Method syntax (most common) ───────────────────────────────
var result = nums
    .Where(n => n > 4)           // filter:    [5,8,9,7,6]
    .OrderBy(n => n)             // sort:      [5,6,7,8,9]
    .Select(n => n * 10);        // transform: [50,60,70,80,90]

// ── Query syntax (SQL-like, identical result) ─────────────────
var result2 = from n in nums
              where n > 4
              orderby n
              select n * 10;

// ── Filtering ─────────────────────────────────────────────────
nums.Where(n => n % 2 == 0)              // even numbers
nums.Where(n => n > 3 && n < 8)         // range filter

// ── Aggregation ───────────────────────────────────────────────
nums.Sum()                   // 45
nums.Min()                   // 1
nums.Max()                   // 9
nums.Average()               // 5.0
nums.Count()                 // 9
nums.Count(n => n > 5)       // 4  (with condition)

// ── Searching ─────────────────────────────────────────────────
nums.Any(n => n > 8)         // true  (at least one)
nums.All(n => n > 0)         // true  (every element)
nums.Contains(5)             // true
nums.First(n => n > 5)       // 8    (throws if not found)
nums.FirstOrDefault(n => n > 99)  // 0  (default if not found)
nums.Single(n => n == 5)     // 5    (throws if 0 or 2+ results)
nums.SingleOrDefault(n => n == 5) // 5 or default

// ── Ordering ──────────────────────────────────────────────────
nums.OrderBy(n => n)                  // ascending
nums.OrderByDescending(n => n)        // descending
nums.ThenBy(n => n)                   // secondary sort (after OrderBy)
nums.Reverse()                        // reverse current order

// ── Transformation ────────────────────────────────────────────
nums.Select(n => n * 2)              // map: double each
nums.Select(n => new { n, sq = n*n })// anonymous type projection
nums.SelectMany(list => list)        // flatten nested collections
nums.Distinct()                      // remove duplicates
nums.Take(3)                         // first 3 elements
nums.Skip(2)                         // skip first 2
nums.Skip(2).Take(3)                 // page: elements 3-5

// ── Conversion ────────────────────────────────────────────────
nums.ToList()                        // execute + collect to List<T>
nums.ToArray()                       // execute + collect to T[]
nums.ToDictionary(n => n, n => n*n)  // to Dictionary

// ── LINQ on objects ───────────────────────────────────────────
record Product(string Name, string Category, decimal Price);

List<Product> products = new()
{
    new("Laptop",  "Electronics", 999m),
    new("Phone",   "Electronics", 699m),
    new("Desk",    "Furniture",   299m),
    new("Chair",   "Furniture",   199m),
    new("Monitor", "Electronics", 449m),
};

// Filter + sort + project
var expensive = products
    .Where(p => p.Price > 400)
    .OrderBy(p => p.Price)
    .Select(p => $"{p.Name}: {p.Price:C2}");

// GroupBy — group by category
var byCategory = products.GroupBy(p => p.Category);
foreach (var group in byCategory)
{
    Console.WriteLine($"{group.Key}:");
    foreach (var p in group)
        Console.WriteLine($"  {p.Name} — {p.Price:C2}");
}

// GroupBy with projection
var summary = products
    .GroupBy(p => p.Category)
    .Select(g => new {
        Category = g.Key,
        Count    = g.Count(),
        AvgPrice = g.Average(p => p.Price),
        MaxPrice = g.Max(p => p.Price)
    });
```

> 💡 **Deferred execution:** LINQ queries don't run when defined — they run when enumerated (`foreach`, `ToList()`, `Count()`). Call `.ToList()` to execute immediately and cache the result.

---

## Pattern Matching (C# 8+)

Test values against type, shape, range, and logic patterns — a clean alternative to long if-else chains.

```csharp
// ── is pattern ────────────────────────────────────────────────
object obj = "Hello";
if (obj is string s && s.Length > 3)
    Console.WriteLine($"Long string: {s}");

// Null check with is
if (obj is not null)
    Console.WriteLine("Not null");

// ── switch expression (C# 8+) ────────────────────────────────
string Classify(int n) => n switch
{
    0                 => "zero",
    1 or 2 or 3       => "small",
    > 3 and <= 10     => "medium",
    > 10              => "large",
    _                 => "negative"    // _ = default/discard
};

// ── Type pattern ──────────────────────────────────────────────
decimal GetArea(object shape) => shape switch
{
    Circle    c => Math.PI * c.Radius * c.Radius,
    Rectangle r => r.Width * r.Height,
    Triangle  t => 0.5m * t.Base * t.Height,
    null        => throw new ArgumentNullException(nameof(shape)),
    _           => throw new NotImplementedException()
};

// ── Property pattern ──────────────────────────────────────────
string GetDiscount(Customer c) => c switch
{
    { IsPremium: true, YearsActive: > 5 } => "30% off",
    { IsPremium: true }                    => "20% off",
    { YearsActive: > 3 }                  => "10% off",
    _                                      => "No discount"
};

// ── Tuple pattern ─────────────────────────────────────────────
string RockPaperScissors(string p1, string p2) => (p1, p2) switch
{
    ("rock",     "scissors") => "P1 wins",
    ("scissors", "paper"   ) => "P1 wins",
    ("paper",    "rock"    ) => "P1 wins",
    (var a,      var b     ) when a == b => "Draw",
    _                                    => "P2 wins"
};

// ── when guard ────────────────────────────────────────────────
string Describe(int n) => n switch
{
    var x when x < 0   => "negative",
    0                  => "zero",
    var x when x % 2 == 0 => "positive even",
    _                  => "positive odd"
};
```

---

## Immutability, Pure Functions & Higher-Order Functions

### Pure Functions

```csharp
// ✅ Pure — same input always gives same output, no side effects
static int    Add(int a, int b)   => a + b;
static string Upper(string s)     => s.ToUpper();
static double CircleArea(double r) => Math.PI * r * r;

// ❌ Impure — depends on external state or has side effects
static int  BadRandom()           => new Random().Next();    // non-deterministic
static void SaveToDb(User u)      { /* side effect */ }      // I/O
static int  Counter()             { return _count++; }       // mutable state
```

### Immutability with Records

```csharp
// record = immutable value object by default
record Point(int X, int Y);
record Person(string Name, int Age);

Point p1 = new(1, 2);
Point p2 = p1 with { X = 10 };   // creates NEW instance (1,2)→(10,2)
// p1 is completely unchanged

Person alice = new("Alice", 30);
Person olderAlice = alice with { Age = 31 };   // new instance
```

### Higher-Order Functions

```csharp
// Takes a function as parameter
static List<T> Filter<T>(List<T> items, Func<T, bool> predicate)
    => items.Where(predicate).ToList();

static List<R> Map<T, R>(List<T> items, Func<T, R> transform)
    => items.Select(transform).ToList();

static T Reduce<T>(List<T> items, T seed, Func<T, T, T> combine)
    => items.Aggregate(seed, combine);

// Returns a function (function factory)
static Func<int, int> Multiplier(int factor) => x => x * factor;

var triple = Multiplier(3);
var times5 = Multiplier(5);
Console.WriteLine(triple(4));   // 12
Console.WriteLine(times5(4));   // 20

// Function composition
Func<int, int> addOne   = x => x + 1;
Func<int, int> doubleIt = x => x * 2;
Func<int, int> addOneThenDouble = x => doubleIt(addOne(x));
Console.WriteLine(addOneThenDouble(3));   // (3+1)*2 = 8

// Declarative (functional) vs Imperative
var nums = new List<int> { 1, 2, 3, 4, 5 };

// Imperative — describes HOW
var evens = new List<int>();
foreach (var n in nums)
    if (n % 2 == 0) evens.Add(n);

// Declarative — describes WHAT ← functional style
var evens2 = nums.Where(n => n % 2 == 0).ToList();
```

---

## Quick Reference

```
Functional Programming
│
├── Extension Methods
│   ├── public static class MyExtensions
│   └── public static RetType Method(this TargetType x, ...)
│
├── Lambda Expressions
│   ├── x => expr                  (expression lambda)
│   ├── (x, y) => expr             (multiple params)
│   ├── x => { ...; return val; }  (statement lambda)
│   └── Closures capture by reference — be careful in loops!
│
├── LINQ (System.Linq)
│   ├── Where, Select, OrderBy, OrderByDescending
│   ├── GroupBy, Distinct, Take, Skip
│   ├── Any, All, First, FirstOrDefault, Single
│   ├── Sum, Min, Max, Average, Count
│   ├── ToList(), ToArray(), ToDictionary()
│   └── Deferred execution — runs on enumeration
│
├── Pattern Matching (C# 8+)
│   ├── obj is Type t              (type + capture)
│   ├── value switch { pat => expr }
│   ├── Type/Property/Tuple/Range patterns
│   └── when guard for extra conditions
│
└── Pure / Higher-Order Functions
    ├── Pure: same input → same output, no side effects
    ├── record for immutable value types
    ├── Higher-order: Func<T,R> as parameter or return type
    └── Declarative style: describe WHAT, not HOW
```

---
