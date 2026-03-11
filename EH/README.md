### Exception Handling 🟣

> Graceful error handling — a pillar of production-quality C# code.

---

## Table of Contents
- [What is an Exception?](#what-is-an-exception)
- [try-catch-finally](#try-catch-finally)
- [Throwing Exceptions](#throwing-exceptions)
- [Built-in Exceptions](#built-in-exceptions)
- [Global Exception Handling](#global-exception-handling)
- [Quick Reference](#quick-reference)

---

## What is an Exception?

An exception is a runtime error that disrupts the normal flow of a program. Instead of crashing silently, C# lets you **catch** exceptions and handle them gracefully.

```csharp
// Without exception handling — app crashes!
int result = 10 / 0;   // 💥 DivideByZeroException, app terminates

// With exception handling — app continues
try
{
    int result = 10 / 0;
}
catch (DivideByZeroException ex)
{
    Console.WriteLine($"Handled: {ex.Message}");  // app keeps running ✅
}
```

---

## try-catch-finally

### Basic Structure

```csharp
try
{
    // Code that might throw an exception
    int result = 10 / 0;
}
catch (DivideByZeroException ex)
{
    // Handle specific exception type — runs only if this exception occurs
    Console.WriteLine($"Math error: {ex.Message}");
}
catch (Exception ex)
{
    // Catch-all — always put this LAST
    Console.WriteLine($"Unexpected error: {ex.Message}");
}
finally
{
    // ALWAYS runs — whether success or failure
    // Perfect for: closing files, releasing DB connections, cleanup
    Console.WriteLine("Cleanup done.");
}
```

### Multiple Catch Blocks

```csharp
static int ParseAndDivide(string input, int divisor)
{
    try
    {
        int number = int.Parse(input);    // FormatException if not a number
        return number / divisor;          // DivideByZeroException if divisor is 0
    }
    catch (FormatException)
    {
        Console.WriteLine($"'{input}' is not a valid number");
        return -1;
    }
    catch (DivideByZeroException)
    {
        Console.WriteLine("Cannot divide by zero");
        return -1;
    }
    catch (Exception ex)
    {
        // Catch-all — handles anything not caught above
        Console.WriteLine($"Unexpected: {ex.Message}");
        return -1;
    }
}
```

### when Filter (Conditional Catch)

```csharp
catch (Exception ex) when (ex.Message.Contains("timeout"))
{
    // Only catches if the condition is true — otherwise exception propagates
    Console.WriteLine("Request timed out, retrying...");
}
```

### Exception Properties

```csharp
catch (Exception ex)
{
    Console.WriteLine(ex.Message);           // human-readable description
    Console.WriteLine(ex.StackTrace);        // full call stack where it happened
    Console.WriteLine(ex.GetType().Name);    // exception class name
    Console.WriteLine(ex.InnerException);    // wrapped/original exception (if any)
    Console.WriteLine(ex.Source);            // assembly/object that threw it
}
```

> ✅ **Order matters:** Always put more specific exceptions **before** general ones. C# matches the **first** applicable catch block and ignores the rest.

> ⚠️ **finally vs return:** `finally` runs even if a `return` statement is hit inside `try` or `catch`. It also runs when an unhandled exception propagates up the call stack.

---

## Throwing Exceptions

### throw vs throw ex

```csharp
// ❌ BAD — throw ex resets the stack trace to THIS line
try { RiskyMethod(); }
catch (Exception ex)
{
    Log(ex.Message);
    throw ex;   // stack trace now points HERE — original location lost!
}

// ✅ GOOD — bare throw preserves the original stack trace
try { RiskyMethod(); }
catch (Exception ex)
{
    Log(ex.Message);
    throw;      // keeps original stack trace from RiskyMethod() ✅
}
```

### Throwing New Exceptions

```csharp
public static void SetAge(int age)
{
    if (age < 0)
        throw new ArgumentOutOfRangeException(
            nameof(age), age, "Age cannot be negative");

    if (age > 150)
        throw new ArgumentException($"Age {age} is unrealistic", nameof(age));
}

// ArgumentNullException shorthand (C# 10+)
public static void Greet(string? name)
{
    ArgumentNullException.ThrowIfNull(name);   // one-liner null check
    Console.WriteLine($"Hello, {name}!");
}
```

### Wrapping Exceptions (Inner Exception)

```csharp
try { DatabaseCall(); }
catch (SqlException ex)
{
    // Wrap low-level exception in a domain-specific one
    // Original SqlException becomes InnerException
    throw new DataAccessException("Failed to load user", ex);
}
```

### Custom Exceptions

```csharp
// Convention: name ends in "Exception", extend Exception class
public class InsufficientFundsException : Exception
{
    public decimal Amount  { get; }
    public decimal Balance { get; }

    // Standard 3 constructors (follow .NET convention)
    public InsufficientFundsException()
        : base() { }

    public InsufficientFundsException(decimal amount, decimal balance)
        : base($"Cannot withdraw {amount:C}. Balance: {balance:C}")
    {
        Amount  = amount;
        Balance = balance;
    }

    public InsufficientFundsException(string message, Exception inner)
        : base(message, inner) { }
}

// Usage
void Withdraw(decimal amount, decimal balance)
{
    if (amount > balance)
        throw new InsufficientFundsException(amount, balance);
}

try { Withdraw(500m, 200m); }
catch (InsufficientFundsException ex)
{
    Console.WriteLine(ex.Message);        // from base()
    Console.WriteLine($"Tried: {ex.Amount:C}");   // custom property
    Console.WriteLine($"Had:   {ex.Balance:C}");  // custom property
}
```

---

## Built-in Exceptions

| Exception | When it Happens | Best Prevention |
|---|---|---|
| `ArgumentNullException` | null passed where not allowed | `ThrowIfNull(arg)` |
| `ArgumentOutOfRangeException` | value outside valid range | validate bounds |
| `ArgumentException` | invalid argument value | validate inputs |
| `NullReferenceException` | dereferencing null object | `?.` operator, null checks |
| `IndexOutOfRangeException` | array index ≥ Length | check `.Length` first |
| `InvalidOperationException` | object in wrong state | check state before calling |
| `FormatException` | string not parseable to type | use `TryParse` instead |
| `OverflowException` | arithmetic overflow (in checked context) | use `checked{}` block |
| `DivideByZeroException` | integer divided by zero | check divisor ≠ 0 |
| `StackOverflowException` | infinite recursion | add base case |
| `OutOfMemoryException` | heap exhausted | reduce large allocations |
| `IOException` | general file/disk failure | always wrap IO in try-catch |
| `FileNotFoundException` | file does not exist | `File.Exists()` check |
| `UnauthorizedAccessException` | no permission to access | check permissions |
| `TimeoutException` | operation exceeded time limit | set reasonable timeouts |
| `NotImplementedException` | method stub not yet coded | implement the method |

```csharp
// NullReferenceException — use ?. to avoid
string? s = null;
int len = s?.Length ?? 0;   // 0 instead of crash ✅

// IndexOutOfRangeException — check bounds
int[] arr = { 1, 2, 3 };
int i = 5;
if (i >= 0 && i < arr.Length)
    Console.WriteLine(arr[i]);   // safe ✅

// FormatException — use TryParse
if (int.TryParse("abc", out int result))
    Console.WriteLine(result);
else
    Console.WriteLine("Not a valid number");   // no exception ✅

// OverflowException — use checked block
try
{
    checked
    {
        int big = int.MaxValue + 1;   // throws OverflowException ✅
    }
}
catch (OverflowException)
{
    Console.WriteLine("Integer overflow detected!");
}
```

---

## Global Exception Handling

Catch **any** unhandled exception app-wide — essential for logging and graceful shutdown.

```csharp
// Console app — last-resort handler for unhandled exceptions
AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
{
    var ex = (Exception)e.ExceptionObject;
    Console.WriteLine($"[FATAL] {ex.Message}");
    File.AppendAllText("crash.log", $"{DateTime.Now}: {ex}\n");
    // Note: app will still terminate after this handler
};

// Async/Task exceptions — catch unobserved task failures
TaskScheduler.UnobservedTaskException += (sender, e) =>
{
    Log("Unobserved task exception", e.Exception);
    e.SetObserved();   // prevents app from terminating
};
```

**ASP.NET Core style middleware:**

```csharp
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { error = "Internal server error" });
            Log(ex);   // log full details server-side only!
        }
    }
}
```

> 🚨 **Never expose stack traces or exception details to users!** Log full details server-side only. Return a friendly message to clients — stack traces reveal your internal structure to attackers.

---

## Quick Reference

```
Exception Handling
│
├── try-catch-finally
│   ├── Specific exceptions before Exception (catch-all)
│   ├── finally always runs — use for cleanup
│   └── when filter — conditional catch
│
├── Throwing
│   ├── throw;        ← bare throw — preserves stack trace ✅
│   ├── throw ex;     ← resets stack trace — AVOID ❌
│   └── throw new SomeException("msg", innerEx)
│
├── Custom Exceptions
│   ├── class MyException : Exception
│   ├── 3 standard constructors
│   └── Add custom properties for domain data
│
├── Built-in Exceptions
│   ├── Use TryParse instead of catching FormatException
│   ├── Use ?. instead of catching NullReferenceException
│   └── Check bounds instead of catching IndexOutOfRangeException
│
└── Global Handling
    ├── AppDomain.CurrentDomain.UnhandledException
    ├── TaskScheduler.UnobservedTaskException
    └── ASP.NET Core middleware
```

```csharp
// One-line cheat sheet
try { }
catch (SpecificException ex) { /* handle */ }
catch (Exception)            { throw; }   // bare throw ← preserves stack trace
finally                      { /* always runs */ }
```

---
