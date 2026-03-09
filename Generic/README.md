### Generics 🟣

> Write code once, use it with **any type** — with full compile-time type safety.

---

## Table of Contents
- [What are Generics?](#what-are-generics)
- [Generic Classes](#generic-classes)
- [Generic Methods](#generic-methods)
- [Generic Constraints](#generic-constraints)
- [Quick Reference Summary](#quick-reference-summary)

---

## What are Generics?

Generics let you define classes, methods, and interfaces with a **type placeholder** (`T`) that gets filled in at compile time. Instead of writing separate code for each type, you write it once.

```csharp
// ❌ Without generics — copy-paste nightmare
class IntBox    { public int    Value { get; set; } }
class StringBox { public string Value { get; set; } }
class DoubleBox { public double Value { get; set; } }

// ✅ With generics — one class for ALL types
class Box<T>    { public T      Value { get; set; } }
```

> 💡 You already use generics every day: `List<int>`, `Dictionary<string, int>`, `IEnumerable<T>` are all built-in generic types.

---

## Generic Classes

A generic class uses a **type parameter** `<T>` in its definition. Specify the actual type when creating an instance.

### Basic Generic Class

```csharp
public class Box<T>
{
    public T Value { get; set; }

    public Box(T value)
    {
        Value = value;
    }

    public void Display()
    {
        Console.WriteLine($"Box contains: {Value} (Type: {typeof(T).Name})");
    }
}

// Use with any type
Box<int>    intBox    = new Box<int>(42);
Box<string> stringBox = new Box<string>("Hello");
Box<double> doubleBox = new Box<double>(3.14);

intBox.Display();    // Box contains: 42 (Type: Int32)
stringBox.Display(); // Box contains: Hello (Type: String)
```

### Multiple Type Parameters

```csharp
public class Pair<TKey, TValue>
{
    public TKey   Key   { get; }
    public TValue Value { get; }

    public Pair(TKey key, TValue value)
    {
        Key = key; Value = value;
    }

    public override string ToString() => $"[{Key} → {Value}]";
}

var p1 = new Pair<string, int>("Age", 30);
var p2 = new Pair<int, bool>(404, false);
Console.WriteLine(p1);  // [Age → 30]
Console.WriteLine(p2);  // [404 → False]
```

### Generic Stack (Real-World Example)

```csharp
public class MyStack<T>
{
    private T[]  _items = new T[100];
    private int  _count = 0;

    public void Push(T item)  => _items[_count++] = item;
    public T    Pop()         => _items[--_count];
    public T    Peek()        => _items[_count - 1];
    public int  Count         => _count;
    public bool IsEmpty       => _count == 0;
}

var numStack = new MyStack<int>();
numStack.Push(10);
numStack.Push(20);
numStack.Push(30);
Console.WriteLine(numStack.Pop());   // 30 (LIFO)
Console.WriteLine(numStack.Peek());  // 20

var strStack = new MyStack<string>(); // same class, different type!
strStack.Push("hello");
```

> ✅ **Convention:** Use `T` for a single type parameter. For multiple, use descriptive names: `TKey`, `TValue`, `TInput`, `TOutput`.

---

## Generic Methods

A generic method defines its **own** type parameter, independent of the class. The compiler can usually **infer** the type from arguments.

### Basic Syntax

```csharp
public static void Print<T>(T value)
{
    Console.WriteLine($"Value: {value} | Type: {typeof(T).Name}");
}

// Explicit type (verbose)
Print<int>(42);
Print<string>("Hello");

// Type inference — compiler figures it out ✨
Print(42);       // T = int
Print("Hello");  // T = string
Print(3.14);     // T = double
```

### Generic Swap

```csharp
public static void Swap<T>(ref T a, ref T b)
{
    T temp = a;
    a = b;
    b = temp;
}

int x = 10, y = 20;
Swap(ref x, ref y);
Console.WriteLine($"x={x}, y={y}");    // x=20, y=10

string s1 = "Hello", s2 = "World";
Swap(ref s1, ref s2);
Console.WriteLine($"s1={s1}, s2={s2}"); // s1=World, s2=Hello
```

### Generic Array Utilities

```csharp
public static class ArrayUtils
{
    // Works for int[], string[], bool[], etc.
    public static void PrintAll<T>(T[] array)
    {
        Console.Write("[ ");
        foreach (var item in array) Console.Write($"{item} ");
        Console.WriteLine("]");
    }

    public static int FindIndex<T>(T[] array, T target)
    {
        for (int i = 0; i < array.Length; i++)
            if (array[i]!.Equals(target)) return i;
        return -1;
    }

    public static void Fill<T>(T[] array, T value)
    {
        for (int i = 0; i < array.Length; i++) array[i] = value;
    }
}

int[]    nums  = { 3, 7, 1, 9, 4 };
string[] words = { "cat", "dog", "fox" };

ArrayUtils.PrintAll(nums);                         // [ 3 7 1 9 4 ]
ArrayUtils.PrintAll(words);                        // [ cat dog fox ]
Console.WriteLine(ArrayUtils.FindIndex(nums, 9));  // 3
```

---

## Generic Constraints

Constraints (`where T : ...`) **restrict** which types can be used as `T`, and unlock type-specific operations.

### Why Constraints?

```csharp
// ❌ Compile error — T might not have CompareTo()
public static T Max<T>(T a, T b)
{
    return a.CompareTo(b) > 0 ? a : b;  // Error!
}

// ✅ Constraint tells the compiler T has CompareTo()
public static T Max<T>(T a, T b) where T : IComparable<T>
{
    return a.CompareTo(b) > 0 ? a : b;  // Works!
}

Console.WriteLine(Max(10, 25));           // 25
Console.WriteLine(Max("apple", "mango")); // mango
Console.WriteLine(Max(3.5, 2.1));         // 3.5
```

### All Constraint Types

| Constraint | Meaning | Enables |
|---|---|---|
| `where T : class` | Reference type only | Assign null, check for null |
| `where T : struct` | Value type only | Cannot be null |
| `where T : new()` | Has public no-arg constructor | `new T()` inside method |
| `where T : BaseClass` | Must inherit BaseClass | Access base members |
| `where T : IInterface` | Must implement interface | Call interface methods |
| `where T : notnull` | Cannot be nullable | Non-null guarantee |

### Common Constraint Examples

```csharp
// where T : class — can check/assign null
public class NullableWrapper<T> where T : class
{
    public T? Value { get; set; }
    public bool HasValue => Value != null;
}

// where T : new() — can create instances inside method
public static T CreateNew<T>() where T : new()
{
    return new T();
}

// where T : IComparable<T> — can compare values
public static T Clamp<T>(T value, T min, T max)
    where T : IComparable<T>
{
    if (value.CompareTo(min) < 0) return min;
    if (value.CompareTo(max) > 0) return max;
    return value;
}

Console.WriteLine(Clamp(15,  1,   10));    // 10  (clamped to max)
Console.WriteLine(Clamp(-5,  0,  100));    // 0   (clamped to min)
Console.WriteLine(Clamp(7.5, 1.0, 10.0)); // 7.5 (in range)
```

### Multiple Constraints

```csharp
// T must be: a class, implement IAnimal, have no-arg constructor
public class AnimalShelter<T>
    where T : class, IAnimal, new()
{
    private List<T> _animals = new();

    public void AddAnimal(T animal) => _animals.Add(animal);

    public void RollCall()
    {
        foreach (var a in _animals)
        {
            Console.Write($"{a.Name}: ");
            a.Speak();  // safe: T implements IAnimal
        }
    }

    public T CreateEmpty() => new T(); // safe: new() constraint
}
```

> ⚠️ **Order matters!** `class`/`struct` → base class/interface → `new()` (must be last).

---

## Quick Reference Summary

```
C# Day 5 – Generics
│
├── Generic Classes         public class MyClass<T> { }
│   ├── Single type param   Box<T>
│   └── Multiple params     Pair<TKey, TValue>
│
├── Generic Methods         public static void Method<T>(T val) { }
│   ├── Explicit call       Method<int>(42)
│   └── Type inference      Method(42)   ← compiler infers T = int
│
└── Generic Constraints     where T : ...
    ├── where T : class              reference types only
    ├── where T : struct             value types only
    ├── where T : new()              can do new T()
    ├── where T : IComparable<T>     can compare values
    └── Combined: where T : class, IAnimal, new()
```

```csharp
// The three patterns in one glance:

// 1. Generic Class
public class Box<T> { public T Value { get; set; } }

// 2. Generic Method
public static void Swap<T>(ref T a, ref T b) { T t = a; a = b; b = t; }

// 3. Generic Constraint
public static T Max<T>(T a, T b) where T : IComparable<T>
    => a.CompareTo(b) > 0 ? a : b;
```

> 🎯 **Golden Rule:** If you're copy-pasting a class or method for different types — that's a sign you need generics!

---