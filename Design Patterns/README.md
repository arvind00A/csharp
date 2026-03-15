### SOLID & Design Patterns 🔵

> SOLID Principles · Creational Patterns · Structural Patterns · Behavioral Patterns

---

## Table of Contents
- [SOLID Principles](#solid-principles)
- [Creational Patterns](#creational-patterns)
- [Structural Patterns](#structural-patterns)
- [Behavioral Patterns](#behavioral-patterns)
- [Pattern Selection Guide](#pattern-selection-guide)
- [Quick Reference](#quick-reference)

---

## SOLID Principles

Five object-oriented design principles that make software more maintainable, flexible, and testable.

---

### S — Single Responsibility Principle

> A class should have **one and only one reason to change**.

```csharp
// ❌ Violates SRP — this class does 3 things
class OrderManager
{
    public void Save(Order order)    { /* database */ }
    public void SendEmail(Order o)   { /* email */ }
    public byte[] GeneratePdf(Order o) { /* pdf */ }
}

// ✅ Each class has ONE responsibility
class OrderRepository   { public void Save(Order order)    { /* DB */ } }
class EmailService      { public void Send(string to, ...)  { /* email */ } }
class PdfGenerator      { public byte[] Generate(Order o)  { /* pdf */ } }
```

---

### O — Open/Closed Principle

> Software entities should be **open for extension** but **closed for modification**.

```csharp
// ❌ Violates OCP — must MODIFY class to add new discount type
class DiscountCalculator
{
    public decimal Calculate(string type, decimal price) => type switch
    {
        "vip"    => price * 0.80m,
        "summer" => price * 0.85m,
        // ← must edit this file every time a new type is added
        _        => price
    };
}

// ✅ Add new discount = new class, zero changes to existing code
interface IDiscount { decimal Apply(decimal price); }
class VipDiscount      : IDiscount { public decimal Apply(decimal p) => p * 0.80m; }
class SummerDiscount   : IDiscount { public decimal Apply(decimal p) => p * 0.85m; }
class LoyaltyDiscount  : IDiscount { public decimal Apply(decimal p) => p * 0.90m; }
// Adding a new type: create new class, done.

class OrderPricer
{
    public decimal Price(decimal basePrice, IDiscount discount)
        => discount.Apply(basePrice);   // never needs to change
}
```

---

### L — Liskov Substitution Principle

> Subclasses must be **substitutable** for their base class without breaking the program.

```csharp
// ❌ Violates LSP — Square breaks Rectangle's contract
class Rectangle
{
    public virtual int Width  { get; set; }
    public virtual int Height { get; set; }
    public int Area() => Width * Height;
}

class Square : Rectangle
{
    public override int Width  { set { base.Width = base.Height = value; } get => base.Width; }
    public override int Height { set { base.Width = base.Height = value; } get => base.Height; }
    // Changing Width also changes Height — breaks callers expecting independent dimensions!
}

// ✅ LSP-compliant — share an abstraction without breaking expectations
interface IShape { int Area(); }
class Rectangle : IShape { public int Width, Height; public int Area() => Width * Height; }
class Square    : IShape { public int Side;           public int Area() => Side * Side; }
```

---

### I — Interface Segregation Principle

> Clients should not be forced to depend on interfaces they **don't use**. Prefer many small interfaces.

```csharp
// ❌ Violates ISP — all implementors must implement all methods
interface IWorker
{
    void Work();
    void Eat();    // robots don't eat!
    void Sleep();  // robots don't sleep!
}

class Robot : IWorker
{
    public void Work()  { /* ok */ }
    public void Eat()   => throw new NotImplementedException();    // forced!
    public void Sleep() => throw new NotImplementedException();    // forced!
}

// ✅ Split into focused interfaces
interface IWorkable  { void Work(); }
interface IEatable   { void Eat(); }
interface ISleepable { void Sleep(); }

class Human : IWorkable, IEatable, ISleepable
{
    public void Work()  { }
    public void Eat()   { }
    public void Sleep() { }
}

class Robot : IWorkable   // only implements what it needs
{
    public void Work() { }
}
```

---

### D — Dependency Inversion Principle

> Depend on **abstractions**, not concrete implementations. High-level modules should not depend on low-level modules.

```csharp
// ❌ Violates DIP — high-level class creates its own dependency
class OrderService
{
    private SqlOrderRepository _repo = new SqlOrderRepository();  // tightly coupled!
    // Can't swap out the repository for testing or different DB
}

// ✅ Inject the dependency via interface (Dependency Injection)
interface IOrderRepository
{
    Order? GetById(int id);
    void   Save(Order order);
}

class OrderService
{
    private readonly IOrderRepository _repo;

    // Injected from outside — caller decides which implementation
    public OrderService(IOrderRepository repo) => _repo = repo;

    public void Process(int orderId)
    {
        var order = _repo.GetById(orderId);   // works with ANY implementation
        // ...
    }
}

// Implementations
class SqlOrderRepository   : IOrderRepository { /* SQL Server */ }
class MongoOrderRepository : IOrderRepository { /* MongoDB */ }
class MockOrderRepository  : IOrderRepository { /* for tests */ }

// Wire up with DI container (.NET built-in)
builder.Services.AddScoped<IOrderRepository, SqlOrderRepository>();
```

---

## Creational Patterns

Object creation patterns — how and when objects are created.

### Singleton

Ensures only **one instance** of a class exists.

```csharp
// Thread-safe Singleton using Lazy<T>
public sealed class AppConfig
{
    private static readonly Lazy<AppConfig> _instance =
        new(() => new AppConfig());   // created only when first accessed

    private AppConfig() { }   // private constructor prevents new AppConfig()

    public static AppConfig Instance => _instance.Value;   // ← access point

    public string ConnectionString { get; set; } = "";
    public int    TimeoutSeconds   { get; set; } = 30;
}

// Usage
AppConfig.Instance.ConnectionString = "Server=...";
AppConfig.Instance.TimeoutSeconds   = 60;
```

> ✅ Use for: configuration, loggers, thread pools, caches. ⚠️ Singletons make unit testing harder — prefer DI with `AddSingleton<T>()` in modern .NET.

### Factory Method

Let subclasses decide **which class to instantiate**.

```csharp
interface INotification { void Send(string message); }
class EmailNotification : INotification { public void Send(string m) => Console.WriteLine($"Email: {m}"); }
class SmsNotification   : INotification { public void Send(string m) => Console.WriteLine($"SMS: {m}"); }
class PushNotification  : INotification { public void Send(string m) => Console.WriteLine($"Push: {m}"); }

// Factory — maps a key to a concrete type
class NotificationFactory
{
    public static INotification Create(string type) => type switch
    {
        "email" => new EmailNotification(),
        "sms"   => new SmsNotification(),
        "push"  => new PushNotification(),
        _       => throw new ArgumentException($"Unknown type: {type}")
    };
}

// Usage — caller doesn't know/care which class is created
INotification notif = NotificationFactory.Create("email");
notif.Send("Your order is ready!");
```

### Builder

Construct complex objects **step by step**. Great for fluent APIs.

```csharp
public class QueryBuilder
{
    private string        _table      = "";
    private List<string>  _conditions = new();
    private List<string>  _columns    = new() { "*" };
    private int           _limit      = 100;
    private string?       _orderBy;

    public QueryBuilder From(string table)      { _table = table;              return this; }
    public QueryBuilder Select(params string[] cols) { _columns = cols.ToList(); return this; }
    public QueryBuilder Where(string condition) { _conditions.Add(condition);  return this; }
    public QueryBuilder OrderBy(string col)     { _orderBy = col;              return this; }
    public QueryBuilder Limit(int n)            { _limit = n;                  return this; }

    public string Build()
    {
        string cols  = string.Join(", ", _columns);
        string where = _conditions.Count > 0 ? " WHERE " + string.Join(" AND ", _conditions) : "";
        string order = _orderBy is not null ? $" ORDER BY {_orderBy}" : "";
        return $"SELECT {cols} FROM {_table}{where}{order} LIMIT {_limit}";
    }
}

// Fluent method chaining
string sql = new QueryBuilder()
    .From("orders")
    .Select("id", "customer_name", "total")
    .Where("status = 'active'")
    .Where("total > 100")
    .OrderBy("created_at DESC")
    .Limit(25)
    .Build();
// SELECT id, customer_name, total FROM orders WHERE status = 'active' AND total > 100 ORDER BY created_at DESC LIMIT 25
```

---

## Structural Patterns

How classes and objects are **composed** into larger structures.

### Adapter

Make an incompatible interface **work with** your system.

```csharp
// Your interface
interface ILogger { void Log(string message); }

// Legacy class you can't modify
class LegacyFileLogger
{
    public void WriteEntry(string text, int severity)
        => File.AppendAllText("app.log", $"[{severity}] {text}\n");
}

// Adapter — bridges the gap
class FileLoggerAdapter : ILogger
{
    private readonly LegacyFileLogger _legacy = new();
    public void Log(string message) => _legacy.WriteEntry(message, severity: 1);
}

// Now legacy logger works anywhere ILogger is expected
ILogger logger = new FileLoggerAdapter();
logger.Log("Application started");
```

### Decorator

Add new **behaviour** to an object without changing its class — at runtime.

```csharp
interface ILogger { void Log(string message); }
class ConsoleLogger : ILogger { public void Log(string msg) => Console.WriteLine(msg); }

// Each decorator wraps another ILogger and adds something
class TimestampDecorator : ILogger
{
    private readonly ILogger _inner;
    public TimestampDecorator(ILogger inner) => _inner = inner;
    public void Log(string msg) => _inner.Log($"[{DateTime.Now:HH:mm:ss}] {msg}");
}

class SeverityDecorator : ILogger
{
    private readonly ILogger _inner;
    private readonly string  _level;
    public SeverityDecorator(ILogger inner, string level) => (_inner, _level) = (inner, level);
    public void Log(string msg) => _inner.Log($"[{_level}] {msg}");
}

// Stack decorators — each wraps the previous
ILogger logger = new TimestampDecorator(
                    new SeverityDecorator(
                        new ConsoleLogger(), "INFO"));

logger.Log("User logged in");
// Output: [14:22:01] [INFO] User logged in
```

### Facade

Provide a **simple interface** to a complex subsystem.

```csharp
// Complex subsystems (many classes, many methods each)
class InventoryService  { public bool Reserve(Order o)  { return true; } }
class PaymentService    { public bool Charge(Order o)   { return true; } }
class ShippingService   { public void Schedule(Order o) { } }
class NotificationSvc   { public void Confirm(Order o)  { } }
class AuditService      { public void Log(Order o)      { } }

// Facade — one simple entry point that orchestrates everything
class OrderFacade
{
    private readonly InventoryService  _inv   = new();
    private readonly PaymentService    _pay   = new();
    private readonly ShippingService   _ship  = new();
    private readonly NotificationSvc   _notif = new();
    private readonly AuditService      _audit = new();

    public bool PlaceOrder(Order order)
    {
        if (!_inv.Reserve(order))   return false;
        if (!_pay.Charge(order))    { _inv.Release(order); return false; }
        _ship.Schedule(order);
        _notif.Confirm(order);
        _audit.Log(order);
        return true;
    }
}

// Caller sees ONE simple method — complexity is hidden
var facade = new OrderFacade();
bool success = facade.PlaceOrder(myOrder);
```

---

## Behavioral Patterns

How objects **communicate** and responsibilities are distributed.

### Strategy

Define a **family of algorithms**, encapsulate each one, and make them interchangeable.

```csharp
interface IShippingStrategy { decimal Calculate(Order order); }

class StandardShipping : IShippingStrategy
{ public decimal Calculate(Order o) => 5.99m; }

class ExpressShipping : IShippingStrategy
{ public decimal Calculate(Order o) => 14.99m; }

class FreeShipping : IShippingStrategy
{ public decimal Calculate(Order o) => 0m; }

class ShoppingCart
{
    private IShippingStrategy _shipping = new StandardShipping();

    public void SetShipping(IShippingStrategy strategy) => _shipping = strategy;   // swap at runtime!
    public decimal GetShippingCost(Order order) => _shipping.Calculate(order);
}

var cart = new ShoppingCart();
cart.SetShipping(new ExpressShipping());   // user picks express
Console.WriteLine(cart.GetShippingCost(order));   // 14.99
```

### Command

Encapsulate a request as an **object** — supports undo/redo, queuing, logging.

```csharp
interface ICommand { void Execute(); void Undo(); }

class AddItemCommand : ICommand
{
    private readonly Cart    _cart;
    private readonly Product _product;
    public AddItemCommand(Cart cart, Product product) => (_cart, _product) = (cart, product);
    public void Execute() => _cart.Add(_product);
    public void Undo()    => _cart.Remove(_product);
}

class RemoveItemCommand : ICommand
{
    private readonly Cart    _cart;
    private readonly Product _product;
    public RemoveItemCommand(Cart cart, Product product) => (_cart, _product) = (cart, product);
    public void Execute() => _cart.Remove(_product);
    public void Undo()    => _cart.Add(_product);
}

// Invoker — tracks history for undo
class CommandHistory
{
    private readonly Stack<ICommand> _history = new();

    public void Execute(ICommand command)
    {
        command.Execute();
        _history.Push(command);
    }

    public void Undo()
    {
        if (_history.TryPop(out var command))
            command.Undo();
    }
}

// Usage
var history = new CommandHistory();
history.Execute(new AddItemCommand(cart, laptop));    // add
history.Execute(new AddItemCommand(cart, mouse));     // add
history.Undo();   // remove mouse
history.Undo();   // remove laptop
```

### Observer

Define a **one-to-many dependency** so that when one object changes state, all dependents are notified.

```csharp
// C# events ARE the Observer pattern (see Day 8)
// publisher.Event += handler;   // subscribe
// publisher.Event -= handler;   // unsubscribe
// event?.Invoke(this, args);    // notify all subscribers

// Modern: IObservable<T> / IObserver<T> (Reactive Extensions)
interface IObserver<T>   { void OnNext(T value); void OnError(Exception ex); void OnCompleted(); }
interface IObservable<T> { IDisposable Subscribe(IObserver<T> observer); }
```

### State

Allow an object to **alter its behaviour** when its internal state changes.

```csharp
interface IOrderState
{
    string Name   { get; }
    void   Next(OrderContext ctx);
    void   Cancel(OrderContext ctx);
}

class PendingState : IOrderState
{
    public string Name => "Pending";
    public void Next(OrderContext ctx)   => ctx.State = new ProcessingState();
    public void Cancel(OrderContext ctx) => ctx.State = new CancelledState();
}

class ProcessingState : IOrderState
{
    public string Name => "Processing";
    public void Next(OrderContext ctx)   => ctx.State = new ShippedState();
    public void Cancel(OrderContext ctx) => ctx.State = new CancelledState();
}

class ShippedState : IOrderState
{
    public string Name => "Shipped";
    public void Next(OrderContext ctx)   => ctx.State = new DeliveredState();
    public void Cancel(OrderContext ctx) { /* can't cancel shipped order */ }
}

class DeliveredState : IOrderState
{
    public string Name => "Delivered";
    public void Next(OrderContext ctx)   { }   // terminal state
    public void Cancel(OrderContext ctx) { }
}

class CancelledState : IOrderState
{
    public string Name => "Cancelled";
    public void Next(OrderContext ctx)   { }
    public void Cancel(OrderContext ctx) { }
}

class OrderContext
{
    public IOrderState State  { get; set; } = new PendingState();
    public string      Status => State.Name;
    public void Advance() => State.Next(this);
    public void Cancel()  => State.Cancel(this);
}

// Usage
var order = new OrderContext();
Console.WriteLine(order.Status);   // Pending
order.Advance();
Console.WriteLine(order.Status);   // Processing
order.Advance();
Console.WriteLine(order.Status);   // Shipped
order.Cancel();                    // ignored — can't cancel shipped
order.Advance();
Console.WriteLine(order.Status);   // Delivered
```

---

## Pattern Selection Guide

| When you need… | Use |
|---|---|
| Only one instance globally | Singleton |
| Create objects without specifying exact class | Factory Method |
| Build complex object step by step | Builder |
| Make incompatible interfaces work together | Adapter |
| Add features to object without subclassing | Decorator |
| Simple interface to complex system | Facade |
| Interchangeable algorithms | Strategy |
| Undo/redo, queuing actions | Command |
| Notify many objects of state change | Observer |
| Object behaviour changes with state | State |

---

## Quick Reference

```
SOLID
│  S — Single Responsibility: one class = one reason to change
│  O — Open/Closed: extend via new classes, not modifying old
│  L — Liskov: subclass can substitute base without breaking
│  I — Interface Segregation: small focused interfaces
│  D — Dependency Inversion: depend on IInterface, not ConcreteClass
│
Creational Patterns
│  Singleton   → Lazy<T> + private constructor
│  Factory     → Create(type) → IInterface
│  Builder     → .From().Where().Limit().Build() (fluent)
│
Structural Patterns
│  Adapter     → wrap legacy to match your interface
│  Decorator   → wrap + add behaviour (stackable)
│  Facade      → one simple method over complex subsystem
│
Behavioral Patterns
│  Strategy    → IStrategy field, swap at runtime
│  Command     → Execute() + Undo() + Stack<ICommand>
│  Observer    → event += handler (C# events = Observer)
│  State       → IState field, each state knows next state
```

---
