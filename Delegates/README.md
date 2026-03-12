### Delegates 🟣

> Action · Func · Predicate · Anonymous Methods — type-safe function pointers

---

## Table of Contents
- [What is a Delegate?](#what-is-a-delegate)
- [Built-in Delegates: Action, Func, Predicate](#built-in-delegates-action-func-predicate)
- [Multicast Delegates](#multicast-delegates)
- [Anonymous Methods](#anonymous-methods)
- [Custom Delegate Types](#custom-delegate-types)
- [Quick Reference](#quick-reference)

---

## What is a Delegate?

A delegate is a **type-safe function pointer** — a variable that holds a reference to a method. You can pass methods as arguments, store them in variables, and invoke them later.

```csharp
// Delegates are the foundation of:
// - Lambda expressions   (shorthand delegates)
// - LINQ                 (delegates applied to collections)
// - Events               (delegates with restrictions)
// - Callbacks            (pass a function to be called later)
```

---

## Built-in Delegates: Action, Func, Predicate

C# provides three generic delegate types that cover almost every scenario:

| Type | Signature | Returns | Use For |
|---|---|---|---|
| `Action` | `Action<T1, T2, ...>` | `void` | Do something, no return value |
| `Func` | `Func<T1, T2, ..., TResult>` | `TResult` | Transform or compute a value |
| `Predicate` | `Predicate<T>` | `bool` | Test a condition — true or false |

### Action — void return

```csharp
// Action — no return value
Action                   sayHi   = () => Console.WriteLine("Hello!");
Action<string>           greet   = name => Console.WriteLine($"Hi {name}!");
Action<string, int>      log     = (msg, code) => Console.WriteLine($"[{code}] {msg}");
Action<int, int, int>    add3    = (a, b, c) => Console.WriteLine(a + b + c);

sayHi();                    // Hello!
greet("Alice");             // Hi Alice!
log("Not Found", 404);      // [404] Not Found
add3(1, 2, 3);              // 6

// Pass Action to a method
static void RepeatAction(Action action, int times)
{
    for (int i = 0; i < times; i++)
        action();
}

RepeatAction(() => Console.Write("*"), 5);   // *****
```

### Func — returns a value

```csharp
// Func<input1, input2, ..., returnType>
// Last type parameter is ALWAYS the return type

Func<int>                  random  = () => new Random().Next(100);
Func<int, int>             square  = x => x * x;
Func<int, int, int>        add     = (a, b) => a + b;
Func<string, int>          len     = s => s.Length;
Func<string, int, string>  repeat  = (s, n) => string.Concat(Enumerable.Repeat(s, n));
Func<double, double>       sqrt    = Math.Sqrt;   // method reference!

Console.WriteLine(square(7));           // 49
Console.WriteLine(add(3, 4));           // 7
Console.WriteLine(len("Hello"));        // 5
Console.WriteLine(repeat("ha", 3));     // hahaha

// Pass Func to a method
static List<R> Transform<T, R>(List<T> items, Func<T, R> selector)
    => items.Select(selector).ToList();

var lengths = Transform(new List<string> { "Hi", "Hello", "Hey" }, s => s.Length);
// [2, 5, 3]
```

### Predicate — returns bool

```csharp
// Equivalent to Func<T, bool>
Predicate<int>    isEven   = n => n % 2 == 0;
Predicate<int>    isPositive = n => n > 0;
Predicate<string> isEmpty  = s => string.IsNullOrEmpty(s);
Predicate<string> isLong   = s => s.Length > 10;

Console.WriteLine(isEven(4));       // true
Console.WriteLine(isEven(7));       // false
Console.WriteLine(isEmpty(""));     // true

// Used directly with List<T> methods
List<int> nums = new() { 1, 2, 3, 4, 5, 6 };
List<int> evens   = nums.FindAll(isEven);    // [2, 4, 6]
int       first   = nums.Find(isEven);       // 2
bool      hasEven = nums.Exists(isEven);     // true
int       idx     = nums.FindIndex(isEven);  // 1

// Combining predicates
Predicate<int> isEvenAndPositive = n => isEven(n) && isPositive(n);
```

---

## Multicast Delegates

A delegate can hold **multiple methods** — all are called when the delegate is invoked.

```csharp
// Build a pipeline with +=
Action<string> pipeline  = s => Console.Write(s.ToUpper() + " ");
pipeline += s => Console.Write(s.ToLower() + " ");
pipeline += s => Console.WriteLine($"(length={s.Length})");

pipeline("Hello");
// Output: HELLO hello (length=5)
// All three run in order!

// Remove a method with -=
Action<string> logger = s => Console.WriteLine($"Log: {s}");
pipeline += logger;
pipeline -= logger;   // removed — won't fire anymore

// GetInvocationList — see all subscribed methods
foreach (Delegate d in pipeline.GetInvocationList())
    Console.WriteLine(d.Method.Name);
```

> ⚠️ For `Func` and multicast delegates, only the **last** return value is kept. Use `GetInvocationList()` and invoke manually if you need all return values.

---

## Anonymous Methods

Inline methods defined without a name using the `delegate` keyword — the predecessor to lambda expressions.

```csharp
// Anonymous method (old syntax, C# 2.0)
Action<string> greet = delegate(string name)
{
    Console.WriteLine($"Hello, {name}!");
};
greet("Alice");   // Hello, Alice!

// Equivalent lambda (modern, preferred)
Action<string> greetLambda = name => Console.WriteLine($"Hello, {name}!");

// Side-by-side comparison
Func<int, int, int> addOld = delegate(int a, int b) { return a + b; };
Func<int, int, int> addNew = (a, b) => a + b;    // cleaner ✅

// Anonymous method in event handler (still seen in old code)
button.Click += delegate(object? sender, EventArgs e)
{
    Console.WriteLine("Clicked!");
};

// Lambda version (preferred)
button.Click += (sender, e) => Console.WriteLine("Clicked!");

// Unique feature: anonymous methods can OMIT parameters
button.Click += delegate   // no parameter list at all — lambdas can't do this
{
    Console.WriteLine("Clicked! (no params)");
};
```

> 📌 In modern C# always prefer **lambda expressions**. Anonymous methods with `delegate` are mainly encountered in legacy code. The one advantage of anonymous methods: they can omit the parameter list entirely with `delegate { ... }`.

---

## Custom Delegate Types

Define your own delegate type when the built-in ones don't fit — rarely needed today.

```csharp
// Custom delegate type definition
public delegate int    MathOperation(int a, int b);
public delegate void   Logger(string level, string message);
public delegate bool   Validator<T>(T value);

// Usage
MathOperation multiply = (a, b) => a * b;
MathOperation divide   = delegate(int a, int b) { return a / b; };
Logger        log      = (level, msg) => Console.WriteLine($"[{level}] {msg}");
Validator<string> notEmpty = s => !string.IsNullOrEmpty(s);

Console.WriteLine(multiply(3, 4));        // 12
log("INFO", "App started");               // [INFO] App started
Console.WriteLine(notEmpty("hello"));     // true

// Before Func/Action existed (pre C# 3.0), this was the only way
// Today: always use Action<>, Func<>, or Predicate<> instead
```

---

## Quick Reference

```
Delegates
│
├── Action<T1, T2, ...>          → void return
│   ├── Action                   → no params, no return
│   ├── Action<string>           → one param, no return
│   └── Action<string, int>      → two params, no return
│
├── Func<T1, T2, ..., TResult>   → returns TResult
│   ├── Func<int>                → no params, returns int
│   ├── Func<int, int>           → one param, returns int
│   └── Func<int, int, bool>     → two params, returns bool
│
├── Predicate<T>                 → bool return (= Func<T, bool>)
│   └── Used with: FindAll, Find, Exists, FindIndex
│
├── Multicast
│   ├── += to add methods
│   ├── -= to remove methods
│   └── All methods run in subscription order
│
└── Anonymous Methods
    ├── delegate(params) { body }   (old)
    └── params => expr              (modern lambdas ← prefer this)
```

```csharp
// One-line cheat sheet
Action<string>      print   = s => Console.WriteLine(s);
Func<int, int>      square  = x => x * x;
Predicate<int>      isEven  = n => n % 2 == 0;

// Multicast
Action pipeline = () => Console.Write("A");
pipeline += () => Console.Write("B");
pipeline();   // AB
```

---
