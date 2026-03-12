### Events & Event Handling 🟣

> Publisher-Subscriber pattern · event keyword · EventHandler · Custom EventArgs

---

## Table of Contents
- [What are Events?](#what-are-events)
- [event vs Delegate](#event-vs-delegate)
- [Standard Event Pattern](#standard-event-pattern)
- [Custom EventArgs](#custom-eventargs)
- [Subscribing & Unsubscribing](#subscribing--unsubscribing)
- [Quick Reference](#quick-reference)

---

## What are Events?

Events implement the **publisher-subscriber** (observer) pattern:
- A **publisher** class declares and fires an event
- **Subscriber** classes register handlers with `+=`
- When the event fires, all registered handlers are called
- The publisher does **not** know who is listening

```
Publisher                  Subscribers
──────────────────         ──────────────────────────
class Button               class Logger
  event Clicked        ──►   OnClicked(s, e) { log }
                        ├──► class UI
                        │      OnClicked(s, e) { update }
                        └──► class Analytics
                               OnClicked(s, e) { track }
```

---

## event vs Delegate

```csharp
// Plain delegate — anyone can invoke or overwrite
public Action<string> OnMessage;   // ❌ dangerous

someObject.OnMessage  = handler;   // wipes all existing handlers!
someObject.OnMessage("test");      // anyone can fire it from outside!

// event — adds two important restrictions
public event Action<string> OnMessage;   // ✅ safe

someObject.OnMessage  = handler;   // ❌ compile error — cannot assign from outside
someObject.OnMessage += handler;   // ✅ only += and -= allowed from outside
someObject.OnMessage("test");      // ❌ compile error — only the class can invoke it
```

**The `event` keyword adds:**
1. External code can only `+=` (subscribe) or `-=` (unsubscribe) — never `=`
2. Only the **declaring class** can invoke (fire) the event
3. Automatically thread-safe for subscribe/unsubscribe operations

---

## Standard Event Pattern

The conventional C# event pattern uses `EventHandler<TEventArgs>`.

```csharp
// ── Step 1: Custom EventArgs ──────────────────────────────────
public class TemperatureChangedEventArgs : EventArgs
{
    public double OldTemperature { get; }
    public double NewTemperature { get; }
    public DateTime ChangedAt    { get; } = DateTime.Now;

    public TemperatureChangedEventArgs(double oldTemp, double newTemp)
    {
        OldTemperature = oldTemp;
        NewTemperature = newTemp;
    }
}

// ── Step 2: Publisher ─────────────────────────────────────────
public class Thermostat
{
    private double _temperature;

    // Declare event using standard EventHandler<TEventArgs>
    public event EventHandler<TemperatureChangedEventArgs>? TemperatureChanged;

    public double Temperature
    {
        get => _temperature;
        set
        {
            if (value == _temperature) return;
            var args = new TemperatureChangedEventArgs(_temperature, value);
            _temperature = value;
            // Fire event — ?.Invoke() is null-safe and thread-safe
            TemperatureChanged?.Invoke(this, args);
        }
    }

    // Alternatively — protected virtual method (good for inheritance)
    protected virtual void OnTemperatureChanged(TemperatureChangedEventArgs e)
        => TemperatureChanged?.Invoke(this, e);
}

// ── Step 3: Subscriber ────────────────────────────────────────
public class Alarm
{
    // Event handler method signature must match EventHandler<TEventArgs>
    public void OnTemperatureChanged(object? sender, TemperatureChangedEventArgs e)
    {
        if (e.NewTemperature > 30)
            Console.WriteLine($"🚨 ALARM! Temp rose to {e.NewTemperature}°C");
    }
}

public class Logger
{
    public void OnTemperatureChanged(object? sender, TemperatureChangedEventArgs e)
        => Console.WriteLine($"📊 Log: {e.OldTemperature}° → {e.NewTemperature}° at {e.ChangedAt:HH:mm:ss}");
}

// ── Step 4: Wire up ───────────────────────────────────────────
Thermostat thermostat = new();
Alarm      alarm      = new();
Logger     logger     = new();

thermostat.TemperatureChanged += alarm.OnTemperatureChanged;
thermostat.TemperatureChanged += logger.OnTemperatureChanged;
thermostat.TemperatureChanged += (sender, e) =>   // lambda subscriber
    Console.WriteLine($"UI updated: {e.NewTemperature}°");

// ── Step 5: Fire ──────────────────────────────────────────────
thermostat.Temperature = 22;   // fires — all 3 subscribers notified
thermostat.Temperature = 35;   // fires — alarm also triggers
thermostat.Temperature = 35;   // no fire — value unchanged
```

---

## Custom EventArgs

Always inherit from `EventArgs` to follow the .NET standard.

```csharp
// Simple — just data
public class OrderPlacedEventArgs : EventArgs
{
    public int    OrderId   { get; init; }
    public string Customer  { get; init; } = "";
    public decimal Total    { get; init; }
}

// With cancellation support
public class FileDownloadingEventArgs : EventArgs
{
    public string Url       { get; }
    public bool   Cancel    { get; set; }   // subscriber can set this!

    public FileDownloadingEventArgs(string url) => Url = url;
}

// Publisher that checks Cancel
public class Downloader
{
    public event EventHandler<FileDownloadingEventArgs>? Downloading;

    public void Download(string url)
    {
        var args = new FileDownloadingEventArgs(url);
        Downloading?.Invoke(this, args);

        if (args.Cancel)   // subscriber set Cancel = true
        {
            Console.WriteLine("Download cancelled by subscriber");
            return;
        }
        // proceed with download...
    }
}

// Subscriber cancels
downloader.Downloading += (s, e) =>
{
    if (e.Url.Contains("malware"))
        e.Cancel = true;   // cancel the operation
};
```

---

## Subscribing & Unsubscribing

```csharp
Thermostat t = new();

// ── Subscribe with named method ───────────────────────────────
Alarm alarm = new();
t.TemperatureChanged += alarm.OnTemperatureChanged;

// ── Subscribe with lambda ─────────────────────────────────────
t.TemperatureChanged += (sender, e) =>
    Console.WriteLine($"Lambda sees: {e.NewTemperature}");

// ── Subscribe with anonymous method ───────────────────────────
t.TemperatureChanged += delegate(object? s, TemperatureChangedEventArgs e)
{
    Console.WriteLine($"Anon: {e.NewTemperature}");
};

// ── Unsubscribe (named methods only) ─────────────────────────
t.TemperatureChanged -= alarm.OnTemperatureChanged;
// Note: you CANNOT unsubscribe a lambda — save it to a variable first!

// ── How to unsubscribe a lambda ───────────────────────────────
EventHandler<TemperatureChangedEventArgs> handler = (s, e) =>
    Console.WriteLine($"Removable lambda: {e.NewTemperature}");

t.TemperatureChanged += handler;   // add
t.TemperatureChanged -= handler;   // remove ✅

// ── Check if anyone is subscribed ────────────────────────────
// Use null check — event is null when no subscribers
if (t.TemperatureChanged != null)
    Console.WriteLine("Someone is listening");
```

> ⚠️ **Memory leak warning:** If a long-lived publisher holds a reference to a short-lived subscriber through an event handler, the subscriber won't be garbage collected. Always unsubscribe with `-=` when the subscriber is done.

---

## Simple event with no EventArgs

```csharp
// When you don't need to pass data
public class GameTimer
{
    public event EventHandler? Tick;    // no custom args

    public void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            Thread.Sleep(1000);
            Tick?.Invoke(this, EventArgs.Empty);   // fire with empty args
        }
    }
}

GameTimer timer = new();
int ticks = 0;
timer.Tick += (s, e) => Console.WriteLine($"Tick #{++ticks}");
timer.Start();
// Tick #1 ... Tick #5
```

---

## Quick Reference

```
Events
│
├── Declare
│   ├── public event EventHandler? EventName;
│   └── public event EventHandler<MyArgs>? EventName;
│
├── Fire (inside declaring class only)
│   └── EventName?.Invoke(this, args);   ← ?. = null-safe
│
├── Subscribe
│   ├── obj.Event += MethodName;          (named method)
│   ├── obj.Event += (s, e) => { };       (lambda)
│   └── obj.Event += delegate(s, e) { }; (anon method)
│
├── Unsubscribe
│   ├── obj.Event -= MethodName;          (named method)
│   └── obj.Event -= savedLambdaVar;      (must save lambda first!)
│
├── Custom EventArgs
│   ├── public class MyEventArgs : EventArgs
│   ├── Add properties for event data
│   └── Use init or readonly properties
│
└── Key Rules
    ├── event restricts: only += / -= from outside
    ├── event restricts: only declaring class can invoke
    ├── Always unsubscribe to prevent memory leaks
    └── Use ?.Invoke() — thread-safe null check
```

```csharp
// Full minimal example
public class Button
{
    public event EventHandler? Clicked;
    public void Click() => Clicked?.Invoke(this, EventArgs.Empty);
}

var btn = new Button();
btn.Clicked += (s, e) => Console.WriteLine("Button was clicked!");
btn.Click();   // → "Button was clicked!"
```

---

