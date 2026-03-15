// ============================================================
//  PROJECT 2 — SOLID & Design Patterns
//  E-Commerce Order Processing System
//  Topics: All 5 SOLID principles, Singleton, Factory,
//          Builder, Adapter, Decorator, Facade,
//          Strategy, Command, State, Observer
// ============================================================


using System;
using System.Collections.Generic;
using System.Text;

namespace Design_Patterns;

// ══════════════════════════════════════════════════════════════
//  MODELS
// ══════════════════════════════════════════════════════════════
public record Product(int Id, string Name, decimal Price, int StockQty);
public record Customer(int Id, string Name, string Email, bool IsPremium);
public class Cart { public List<(Product P, int Qty)> Items { get; } = new(); }

public class Order
{
    public int Id { get; init; }
    public Customer Customer { get; init; } = null!;
    public Cart Cart { get; init; } = new();
    public decimal Subtotal => Cart.Items.Sum(i => i.P.Price * i.Qty);
    public decimal Total { get; set; }
    public string Status { get; set; } = "Pending";
}

// ══════════════════════════════════════════════════════════════
//  ① SINGLETON — App Configuration
// ══════════════════════════════════════════════════════════════
public sealed class AppSettings
{
    private static readonly Lazy<AppSettings> _instance = new(() => new AppSettings());
    private AppSettings() { }
    public static AppSettings Instance => _instance.Value;

    public string StoreName { get; set; } = "MegaShop";
    public decimal DefaultTaxRate { get; set; } = 0.08m;
    public bool FreeShippingOn { get; set; } = true;
    public decimal FreeShipThreshold { get; set; } = 50m;
}

// ══════════════════════════════════════════════════════════════
//  ② STRATEGY — Shipping & Discount (Open/Closed Principle)
// ══════════════════════════════════════════════════════════════

// S: Each strategy = one responsibility
// O: New strategy = new class, no existing code changes
interface IShippingStrategy { string Name { get; } decimal Calculate(Order order); }
interface IDiscountStrategy { string Name { get; } decimal Apply(decimal subtotal, Customer customer); }

class StandardShipping : IShippingStrategy
{
    public string Name => "Standard (5–7 days)";
    public decimal Calculate(Order o)
    {
        var cfg = AppSettings.Instance;
        return cfg.FreeShippingOn && o.Subtotal >= cfg.FreeShipThreshold ? 0m : 5.99m;
    }
}
class ExpressShipping : IShippingStrategy
{
    public string Name => "Express (1–2 days)";
    public decimal Calculate(Order o) => o.Subtotal > 200 ? 12.99m : 19.99m;
}
class OvernightShipping : IShippingStrategy
{
    public string Name => "Overnight";
    public decimal Calculate(Order o) => 34.99m;
}

class NoDiscount : IDiscountStrategy
{
    public string Name => "No discount";
    public decimal Apply(decimal s, Customer c) => 0m;
}
class PremiumDiscount : IDiscountStrategy
{
    public string Name => "Premium 15% off";
    public decimal Apply(decimal s, Customer c) => c.IsPremium ? s * 0.15m : 0m;
}
class VolumeDiscount : IDiscountStrategy
{
    public string Name => "Volume discount";
    public decimal Apply(decimal s, Customer c) =>
        s > 300 ? s * 0.10m : s > 150 ? s * 0.05m : 0m;
}

// ══════════════════════════════════════════════════════════════
//  ③ BUILDER — Order construction (fluent API)
// ══════════════════════════════════════════════════════════════
class OrderBuilder
{
    private static int _nextId = 1000;
    private Customer? _customer;
    private Cart _cart = new();
    private IShippingStrategy _shipping = new StandardShipping();
    private IDiscountStrategy _discount = new NoDiscount();

    public OrderBuilder ForCustomer(Customer c) { _customer = c; return this; }
    public OrderBuilder WithShipping(IShippingStrategy s) { _shipping = s; return this; }
    public OrderBuilder WithDiscount(IDiscountStrategy d) { _discount = d; return this; }
    public OrderBuilder AddItem(Product p, int qty)
    {
        _cart.Items.Add((p, qty));
        return this;
    }

    public Order Build()
    {
        if (_customer is null) throw new InvalidOperationException("Customer is required");
        if (_cart.Items.Count == 0) throw new InvalidOperationException("Cart is empty");

        var order = new Order { Id = _nextId++, Customer = _customer, Cart = _cart };
        decimal subtotal = order.Subtotal;
        decimal discount = _discount.Apply(subtotal, _customer);
        decimal shipping = _shipping.Calculate(order);
        decimal tax = (subtotal - discount) * AppSettings.Instance.DefaultTaxRate;

        order.Total = subtotal - discount + shipping + tax;
        return order;
    }
}

// ══════════════════════════════════════════════════════════════
//  ④ FACTORY — Notification creation
// ══════════════════════════════════════════════════════════════

// I: Small focused interfaces (Interface Segregation)
interface INotifier { void Notify(Customer customer, string subject, string body); }

class EmailNotifier : INotifier
{
    public void Notify(Customer c, string subject, string body)
        => Console.WriteLine($"  📧 EMAIL → {c.Email}: [{subject}] {body}");
}

class SmsNotifier : INotifier
{
    public void Notify(Customer c, string subject, string body)
        => Console.WriteLine($"  📱 SMS   → {c.Name}: {body}");
}

class PushNotifier : INotifier
{
    public void Notify(Customer c, string subject, string body)
        => Console.WriteLine($"  🔔 PUSH  → {c.Name}: {body}");
}

class NullNotifier : INotifier  // Null Object pattern
{
    public void Notify(Customer c, string subject, string body) { }
}

static class NotifierFactory
{
    public static INotifier Create(string channel) => channel switch
    {
        "email" => new EmailNotifier(),
        "sms" => new SmsNotifier(),
        "push" => new PushNotifier(),
        _ => new NullNotifier()
    };
}

// ══════════════════════════════════════════════════════════════
//  ⑤ ADAPTER — Legacy payment gateway
// ══════════════════════════════════════════════════════════════

// Your system's interface
interface IPaymentGateway
{
    bool ProcessPayment(string customerId, decimal amount, string currency);
}

// Third-party legacy SDK (can't be modified)
class LegacyPaypalSdk
{
    public int SendMoney(string email, double usdAmount)
    {
        Console.WriteLine($"  [PayPal SDK] Charging ${usdAmount:F2} to {email}");
        return 200;   // 200 = success
    }
}

class ThirdPartyStripeSdk
{
    public bool Charge(long amountCents, string currency, string customerToken)
    {
        Console.WriteLine($"  [Stripe SDK] Charging {amountCents}¢ {currency} token={customerToken}");
        return true;
    }
}

// Adapters — make legacy SDKs conform to IPaymentGateway
class PayPalAdapter : IPaymentGateway
{
    private readonly LegacyPaypalSdk _sdk = new();
    public bool ProcessPayment(string customerId, decimal amount, string currency)
    {
        int code = _sdk.SendMoney(customerId, (double)amount);
        return code == 200;
    }
}

class StripeAdapter : IPaymentGateway
{
    private readonly ThirdPartyStripeSdk _sdk = new();
    public bool ProcessPayment(string customerId, decimal amount, string currency)
    {
        long cents = (long)(amount * 100);
        return _sdk.Charge(cents, currency, customerId);
    }
}

// ══════════════════════════════════════════════════════════════
//  ⑥ DECORATOR — Logging payment gateway
// ══════════════════════════════════════════════════════════════
class LoggingPaymentDecorator : IPaymentGateway
{
    private readonly IPaymentGateway _inner;
    public LoggingPaymentDecorator(IPaymentGateway inner) => _inner = inner;

    public bool ProcessPayment(string customerId, decimal amount, string currency)
    {
        Console.WriteLine($"  [LOG] Payment attempt: {amount:C2} {currency} for {customerId}");
        bool result = _inner.ProcessPayment(customerId, amount, currency);
        Console.WriteLine($"  [LOG] Payment {(result ? "SUCCEEDED" : "FAILED")}");
        return result;
    }
}

class RetryPaymentDecorator : IPaymentGateway
{
    private readonly IPaymentGateway _inner;
    private readonly int _maxRetries;
    public RetryPaymentDecorator(IPaymentGateway inner, int retries = 3)
        => (_inner, _maxRetries) = (inner, retries);

    public bool ProcessPayment(string customerId, decimal amount, string currency)
    {
        for (int attempt = 1; attempt <= _maxRetries; attempt++)
        {
            if (_inner.ProcessPayment(customerId, amount, currency)) return true;
            if (attempt < _maxRetries)
                Console.WriteLine($"  [RETRY] Attempt {attempt} failed, retrying...");
        }
        return false;
    }
}

// ══════════════════════════════════════════════════════════════
//  ⑦ COMMAND — Order actions with undo support
// ══════════════════════════════════════════════════════════════
interface IOrderCommand { void Execute(); void Undo(); string Description { get; } }

class AddItemCommand : IOrderCommand
{
    private readonly Cart _cart;
    private readonly Product _product;
    private readonly int _qty;
    public string Description => $"Add {_qty}x {_product.Name}";
    public AddItemCommand(Cart cart, Product p, int qty) => (_cart, _product, _qty) = (cart, p, qty);
    public void Execute() => _cart.Items.Add((_product, _qty));
    public void Undo() => _cart.Items.RemoveAll(i => i.P.Id == _product.Id);
}

class CommandInvoker
{
    private readonly Stack<IOrderCommand> _history = new();

    public void Execute(IOrderCommand cmd)
    {
        cmd.Execute();
        _history.Push(cmd);
        Console.WriteLine($"  ✅ Executed: {cmd.Description}");
    }

    public void Undo()
    {
        if (_history.TryPop(out var cmd))
        {
            cmd.Undo();
            Console.WriteLine($"  ↩️  Undone: {cmd.Description}");
        }
    }
}

// ══════════════════════════════════════════════════════════════
//  ⑧ STATE — Order lifecycle
// ══════════════════════════════════════════════════════════════
interface IOrderState { string Name { get; } void Advance(OrderContext ctx); void Cancel(OrderContext ctx); }

record OrderContext(Order Order)
{
    public IOrderState State { get; set; } = new PendingOrderState();
    public string Status => State.Name;
    public void Advance() => State.Advance(this);
    public void Cancel() => State.Cancel(this);
}

class PendingOrderState : IOrderState { public string Name => "Pending"; public void Advance(OrderContext c) => c.State = new ConfirmedOrderState(); public void Cancel(OrderContext c) => c.State = new CancelledOrderState(); }
class ConfirmedOrderState : IOrderState { public string Name => "Confirmed"; public void Advance(OrderContext c) => c.State = new ShippedOrderState(); public void Cancel(OrderContext c) => c.State = new CancelledOrderState(); }
class ShippedOrderState : IOrderState { public string Name => "Shipped"; public void Advance(OrderContext c) => c.State = new DeliveredOrderState(); public void Cancel(OrderContext c) { Console.WriteLine("  ⛔ Cannot cancel shipped order"); } }
class DeliveredOrderState : IOrderState { public string Name => "Delivered"; public void Advance(OrderContext c) { } public void Cancel(OrderContext c) { } }
class CancelledOrderState : IOrderState { public string Name => "Cancelled"; public void Advance(OrderContext c) { } public void Cancel(OrderContext c) { } }

// ══════════════════════════════════════════════════════════════
//  ⑨ FACADE — Checkout orchestrator (simplifies subsystems)
//     Also demonstrates SOLID D: depends on interfaces
// ══════════════════════════════════════════════════════════════
class CheckoutFacade
{
    // D: Dependency Inversion — depends on interfaces not concrete types
    private readonly IPaymentGateway _payment;
    private readonly INotifier _notifier;

    public CheckoutFacade(IPaymentGateway payment, INotifier notifier)
    {
        _payment = payment;
        _notifier = notifier;
    }

    // One simple method hides ALL the complexity
    public bool Checkout(Order order)
    {
        Console.WriteLine($"\n  ── Checkout: Order #{order.Id} for {order.Customer.Name} ──");
        Console.WriteLine($"     Items: {order.Cart.Items.Count} | Total: {order.Total:C2}");

        // Step 1: Payment
        bool paid = _payment.ProcessPayment(
            order.Customer.Email, order.Total, "USD");
        if (!paid)
        {
            Console.WriteLine("  ❌ Payment failed");
            return false;
        }

        // Step 2: Update state
        var ctx = new OrderContext(order);
        ctx.Advance();   // Pending → Confirmed
        order.Status = ctx.Status;

        // Step 3: Notify
        _notifier.Notify(order.Customer,
            $"Order #{order.Id} Confirmed",
            $"Thank you! Your order of {order.Total:C2} is confirmed.");

        Console.WriteLine($"  ✅ Checkout complete! Status: {order.Status}");
        return true;
    }
}

// ══════════════════════════════════════════════════════════════
//  MAIN DEMO
// ══════════════════════════════════════════════════════════════
internal class DesignPatternsDemo
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║  Design Patterns — E-Commerce System 🛍️  ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");

        // Singleton
        Console.WriteLine("\n=== Singleton: AppSettings ===");
        AppSettings.Instance.StoreName = "PatternShop";
        AppSettings.Instance.FreeShipThreshold = 100m;
        Console.WriteLine($"  Store: {AppSettings.Instance.StoreName}");
        Console.WriteLine($"  Free shipping over: ${AppSettings.Instance.FreeShipThreshold}");

        // Products and customers
        var laptop = new Product(1, "MacBook Pro", 2499.00m, 5);
        var mouse = new Product(2, "Magic Mouse", 99.00m, 20);
        var keyboard = new Product(3, "Mech Keyboard", 149.00m, 15);
        var monitor = new Product(4, "4K Monitor", 599.00m, 8);
        var usb = new Product(5, "USB-C Hub", 39.00m, 50);

        var alice = new Customer(1, "Alice", "alice@ex.com", IsPremium: true);
        var bob = new Customer(2, "Bob", "bob@ex.com", IsPremium: false);

        // Builder + Strategy
        Console.WriteLine("\n=== Builder + Strategy: Order Creation ===");
        var aliceOrder = new OrderBuilder()
            .ForCustomer(alice)
            .WithShipping(new ExpressShipping())
            .WithDiscount(new PremiumDiscount())
            .AddItem(laptop, 1)
            .AddItem(mouse, 2)
            .Build();
        Console.WriteLine($"  Alice's order: {aliceOrder.Cart.Items.Count} items, total = {aliceOrder.Total:C2}");

        var bobOrder = new OrderBuilder()
            .ForCustomer(bob)
            .WithShipping(new StandardShipping())
            .WithDiscount(new VolumeDiscount())
            .AddItem(monitor, 1)
            .AddItem(keyboard, 1)
            .AddItem(usb, 3)
            .Build();
        Console.WriteLine($"  Bob's order:   {bobOrder.Cart.Items.Count} items, total = {bobOrder.Total:C2}");

        // Factory
        Console.WriteLine("\n=== Factory: Notifications ===");
        INotifier emailNotifier = NotifierFactory.Create("email");
        INotifier smsNotifier = NotifierFactory.Create("sms");
        emailNotifier.Notify(alice, "Welcome", "Thanks for your order!");
        smsNotifier.Notify(bob, "Shipped", "Your package is on the way!");

        // Adapter + Decorator
        Console.WriteLine("\n=== Adapter + Decorator: Payment Gateway ===");
        IPaymentGateway paypal = new PayPalAdapter();
        IPaymentGateway stripe = new StripeAdapter();

        // Decorate with logging + retry
        IPaymentGateway robustStripe = new LoggingPaymentDecorator(
                                         new RetryPaymentDecorator(stripe, retries: 2));
        robustStripe.ProcessPayment("alice@ex.com", 2499m, "USD");

        // Command with undo
        Console.WriteLine("\n=== Command Pattern: Cart with Undo ===");
        var cart = new Cart();
        var invoker = new CommandInvoker();
        invoker.Execute(new AddItemCommand(cart, laptop, 1));
        invoker.Execute(new AddItemCommand(cart, mouse, 2));
        invoker.Execute(new AddItemCommand(cart, keyboard, 1));
        Console.WriteLine($"  Cart items: {cart.Items.Count}");
        invoker.Undo();
        Console.WriteLine($"  After undo: {cart.Items.Count}");

        // State machine
        Console.WriteLine("\n=== State Pattern: Order Lifecycle ===");
        var ctx = new OrderContext(aliceOrder);
        Console.WriteLine($"  Status: {ctx.Status}");
        ctx.Advance(); Console.WriteLine($"  Status: {ctx.Status}");
        ctx.Advance(); Console.WriteLine($"  Status: {ctx.Status}");
        ctx.Cancel(); Console.WriteLine($"  Status: {ctx.Status}");   // can't cancel shipped
        ctx.Advance(); Console.WriteLine($"  Status: {ctx.Status}");

        // Facade — orchestrates everything
        Console.WriteLine("\n=== Facade: Full Checkout ===");
        var facade = new CheckoutFacade(
            payment: new LoggingPaymentDecorator(new StripeAdapter()),
            notifier: NotifierFactory.Create("email")
        );
        facade.Checkout(bobOrder);

        // SOLID summary
        Console.WriteLine("\n=== SOLID Principles Demonstrated ===");
        Console.WriteLine("  S — Each class has ONE job (OrderBuilder, StripeAdapter, etc.)");
        Console.WriteLine("  O — New shipping/discount = new class, zero changes to old code");
        Console.WriteLine("  L — All IShippingStrategy impls substitutable for each other");
        Console.WriteLine("  I — INotifier, IPaymentGateway, IShippingStrategy = focused interfaces");
        Console.WriteLine("  D — CheckoutFacade depends on IPaymentGateway, INotifier (not concrete)");

        Console.WriteLine("\n✅ Design Patterns demo complete!");
    }
}
