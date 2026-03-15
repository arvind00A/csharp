### C# Best Practices 🟡

> Naming Conventions · Null Safety · Performance Tips · Clean Code Rules · IDisposable

---

## Table of Contents
- [Naming Conventions](#naming-conventions)
- [Null Safety (C# 8+)](#null-safety-c-8)
- [Performance Tips](#performance-tips)
- [Clean Code Rules](#clean-code-rules)
- [IDisposable Pattern](#idisposable-pattern)
- [Quick Reference](#quick-reference)

---

## Naming Conventions

C# follows the official Microsoft style guide. Consistent naming makes code instantly readable.

| Element | Convention | Example |
|---|---|---|
| Class / Record | PascalCase | `CustomerOrder`, `HttpClient` |
| Interface | `I` + PascalCase | `IRepository`, `IDisposable` |
| Method | PascalCase | `GetById()`, `ProcessOrder()` |
| Property | PascalCase | `TotalPrice`, `IsActive` |
| Local variable | camelCase | `totalAmount`, `isValid` |
| Private field | `_` + camelCase | `_repository`, `_logger` |
| Constant | PascalCase | `MaxRetryCount`, `DefaultTimeout` |
| Enum type + values | PascalCase | `OrderStatus.Pending` |
| Generic type param | `T` or `TName` | `T`, `TResult`, `TKey` |
| Async method | PascalCase + `Async` | `GetUsersAsync()` |
| Extension class | PascalCase + `Extensions` | `StringExtensions` |
| DTO / ViewModel | PascalCase + suffix | `OrderDto`, `UserViewModel` |

```csharp
// ── Classes and interfaces ────────────────────────────────────
public interface  IOrderRepository { }    // ✅ I prefix
public class      OrderRepository  { }    // ✅ PascalCase
public abstract class BaseService  { }    // ✅ Base prefix ok
public record     CustomerDto      { }    // ✅ Dto suffix

// ── Fields and properties ─────────────────────────────────────
public class OrderService
{
    private readonly IOrderRepository _repository;   // ✅ _camelCase
    private static readonly int MaxRetries = 3;      // ✅ PascalCase constant
    private ILogger _logger = null!;

    public int    OrderId      { get; init; }         // ✅ PascalCase
    public string CustomerName { get; set; } = "";
    public bool   IsActive     { get; private set; }  // ✅ IsXxx for booleans
}

// ── Methods ───────────────────────────────────────────────────
public Order  GetOrderById(int orderId) { }           // ✅ verb + noun
public bool   TryParseOrder(string s, out Order o) {} // ✅ Try prefix
public async  Task<Order> CreateOrderAsync(OrderDto dto) {} // ✅ Async suffix

// ❌ Avoid: vague, abbreviated, or misleading names
// public Order Get(int i)   { }   ← 'i' means what?
// public void  DoStuff()    { }   ← do what?
// public int   myCoolNum;         ← not PascalCase

// ── var usage rules ───────────────────────────────────────────
var order   = new Order();             // ✅ type is obvious from right side
var results = _repo.GetAll();          // ✅ type inferred from method name
var x       = Calculate();            // ❌ unclear what x is — use explicit type
decimal tax = CalcTax(total);         // ✅ explicit when type matters

// ── File-scoped namespace (C# 10+) ───────────────────────────
namespace MyApp.Services;   // no braces — entire file is in this namespace
```

---

## Null Safety (C# 8+)

Enable in `.csproj`:
```xml
<Nullable>enable</Nullable>
```

### Nullable Annotations

```csharp
string  name  = "Alice";   // non-nullable — compiler warns if null assigned
string? email = null;      // nullable — explicitly signals null is possible
int?    age   = null;      // nullable value type (Nullable<int>)

// Non-nullable property default
public string Name { get; set; } = "";   // avoid null warnings
```

### Null Operators

```csharp
// ── ?. Null-conditional ───────────────────────────────────────
int?    len   = email?.Length;          // null if email is null
string? upper = email?.ToUpper();       // short-circuits the whole chain

// ── ?? Null-coalescing ────────────────────────────────────────
string display = email ?? "(no email)"; // use right side if left is null
int    count   = age   ?? 0;            // default value for nullable int

// ── ??= Null-coalescing assignment ───────────────────────────
email ??= "default@example.com";        // assign ONLY if currently null
_cache ??= new Dictionary<string, int>();

// ── ! Null-forgiving operator ─────────────────────────────────
string forced = email!;   // tells compiler "trust me, not null"
// ⚠️ Use sparingly — defeats the purpose of nullable reference types

// ── Preferred null checks ─────────────────────────────────────
if (obj is null)     { }    // ✅ pattern matching — preferred
if (obj == null)     { }    // ok but == can be overridden
if (obj is not null) { }    // ✅ preferred not-null check
if (obj != null)     { }    // ok
```

### Guard Clauses for Null Arguments

```csharp
// C# 10+ — one-liner guards
public Order CreateOrder(string customerId, OrderDto dto)
{
    ArgumentNullException.ThrowIfNull(customerId);           // throws if null
    ArgumentException.ThrowIfNullOrEmpty(customerId);        // throws if null or ""
    ArgumentException.ThrowIfNullOrWhiteSpace(customerId);   // throws if null/"" /"  "
    ArgumentNullException.ThrowIfNull(dto);
    // ...
}

// Manual — older style
public void SetName(string name)
{
    _name = name ?? throw new ArgumentNullException(nameof(name));
}

// Null-safe pattern matching
if (email is { Length: > 0 })          // null-safe + length check in one
    Console.WriteLine(email.ToUpper());

if (order is { Customer: not null, Items.Count: > 0 })  // property pattern
    ProcessOrder(order);
```

---

## Performance Tips

### Use StringBuilder for String Concatenation in Loops

```csharp
// ❌ O(n²) — creates a new string object every iteration
string result = "";
for (int i = 0; i < 10000; i++) result += i;   // terrible!

// ✅ StringBuilder reuses an internal buffer
var sb = new StringBuilder(capacity: 50_000);   // optional initial capacity
for (int i = 0; i < 10000; i++) sb.Append(i);
string final = sb.ToString();   // one allocation at the end

// ✅ String interpolation for one-off formatting (not in loops)
string msg = $"Hello, {name}! You have {count} messages.";
```

### Choose the Right Collection

```csharp
// List<T>          → general purpose, O(n) Contains/Remove
// HashSet<T>       → O(1) Contains — use when checking membership frequently
// Dictionary<K,V>  → O(1) key lookup
// SortedList<K,V>  → sorted + O(log n) lookup
// Queue<T>         → FIFO order
// Stack<T>         → LIFO order

var ids = new HashSet<int> { 1, 2, 3 };
bool found = ids.Contains(2);   // O(1) — not O(n) like List

// ❌ Using List for Contains checks on large sets
var list = new List<int> { 1, 2, 3 };
bool slow = list.Contains(2);   // O(n) — gets slower with size
```

### Span\<T\> — Zero-Copy Memory Slices

```csharp
// ReadOnlySpan<T> / Span<T> avoids allocating new strings/arrays
string csv = "Alice,Bob,Charlie,Diana";

// Old way — creates new string object
string firstName = csv.Split(',')[0];   // allocates array + string

// New way — zero allocation
ReadOnlySpan<char> span  = csv.AsSpan();
int comma = span.IndexOf(',');
ReadOnlySpan<char> first = span[..comma];   // "Alice" — just a view!
```

### ValueTask vs Task

```csharp
// Use ValueTask<T> when the result is often available synchronously
// Avoids heap allocation of Task object on the fast (cached) path
public async ValueTask<int> GetCountAsync(string key)
{
    if (_cache.TryGetValue(key, out int val)) return val;   // sync path — no Task allocation
    return await FetchFromDbAsync(key);                     // async path when cache miss
}
```

### IDisposable — Always Use `using`

```csharp
// ❌ File lock never released if exception occurs
StreamReader reader = new("file.txt");
string text = reader.ReadToEnd();   // reader never closed!

// ✅ 'using' calls Dispose() automatically — even on exception
using var reader2 = new StreamReader("file.txt");
string text2 = reader2.ReadToEnd();
// reader2.Dispose() called here automatically

// ✅ using block (older syntax)
using (var conn = new SqlConnection(connStr))
{
    conn.Open();
    // ... use conn
}   // conn.Dispose() called here
```

### Object Pooling

```csharp
using System.Buffers;

// Rent a buffer from the pool (reuses memory, avoids GC pressure)
byte[] buffer = ArrayPool<byte>.Shared.Rent(minimumLength: 4096);
try
{
    // use buffer ...
}
finally
{
    ArrayPool<byte>.Shared.Return(buffer);   // always return to pool!
}
```

---

## Clean Code Rules

### Guard Clauses — Return Early to Reduce Nesting

```csharp
// ❌ Arrow-head anti-pattern — deeply nested
public string ProcessOrder(Order? order)
{
    if (order != null)
    {
        if (order.Items.Count > 0)
        {
            if (order.Customer != null)
            {
                if (order.Total > 0)
                    return "processed";
            }
        }
    }
    return "failed";
}

// ✅ Guard clauses — validate first, happy path at bottom
public string ProcessOrder(Order? order)
{
    if (order is null)               return "null order";
    if (order.Items.Count == 0)      return "empty order";
    if (order.Customer is null)      return "no customer";
    if (order.Total <= 0)            return "zero total";
    return "processed";   // ← clear happy path, no nesting
}
```

### Named Constants — No Magic Numbers

```csharp
// ❌ What is 0.15? What is 3?
decimal tax = price * 0.15m;
if (attempts > 3) throw new Exception("...");

// ✅ Self-documenting
private const decimal VatRate         = 0.15m;
private const int     MaxLoginAttempts = 3;

decimal tax2 = price * VatRate;
if (attempts > MaxLoginAttempts) throw new TooManyAttemptsException();
```

### One Responsibility Per Method

```csharp
// ❌ Method doing three jobs
public void SaveOrderAndSendEmailAndLog(Order order) { ... }

// ✅ Separate responsibilities
public void SaveOrder(Order order)        { _repo.Save(order); }
public void SendConfirmation(Order order) { _email.Send(order.Customer.Email, ...); }
public void LogOrder(Order order)         { _logger.Log($"Order {order.Id} saved"); }
```

---

## IDisposable Pattern

Implement when your class directly holds unmanaged resources (file handles, DB connections, native memory).

```csharp
public class DatabaseConnection : IDisposable
{
    private bool        _disposed = false;
    private SqlConnection _conn   = new();

    // Public Dispose — called by 'using' statement
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);   // tell GC: no need to call finalizer
    }

    // Protected virtual — allows subclasses to dispose their resources too
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            _conn.Dispose();    // managed resources — dispose here
        }
        // unmanaged resources would be freed here (if any)

        _disposed = true;
    }

    // Optional finalizer — safety net if consumer forgets 'using'
    ~DatabaseConnection()
    {
        Dispose(disposing: false);
    }
}

// Always use with 'using'
using var conn = new DatabaseConnection();
// Dispose() called automatically at end of block
```

---

## Quick Reference

```
Best Practices
│
├── Naming
│   ├── PascalCase:  Class, Method, Property, Const, Enum
│   ├── camelCase:   localVariable, parameter
│   ├── _camelCase:  _privateField
│   ├── IPascalCase: IInterface
│   └── MethodAsync: async methods
│
├── Null Safety
│   ├── string? email = null;         → nullable annotation
│   ├── email?.Length                 → null-conditional
│   ├── email ?? "default"            → null-coalescing
│   ├── email ??= "default"           → null-coalescing assign
│   └── obj is null / obj is not null → preferred checks
│
├── Performance
│   ├── StringBuilder in loops (not +=)
│   ├── HashSet<T> for Contains()
│   ├── using for IDisposable (always!)
│   ├── Span<T> / ReadOnlySpan<T> for zero-copy
│   └── ValueTask when often synchronous
│
└── Clean Code
    ├── Guard clauses — return early
    ├── Named constants — no magic numbers
    ├── One method = one responsibility
    ├── < 20 lines per method
    └── IDisposable for resource management
```

---
