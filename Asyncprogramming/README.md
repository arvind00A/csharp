### Asynchronous Programming 🔵

> async & await · Exception Handling in Async · Async Streams (IAsyncEnumerable)

---

## Table of Contents
- [Async & Await Basics](#async--await-basics)
- [Return Types](#return-types)
- [Parallel Async Calls](#parallel-async-calls)
- [Exception Handling in Async](#exception-handling-in-async)
- [Async Streams (IAsyncEnumerable)](#async-streams-iasyncenumerable)
- [Common Pitfalls](#common-pitfalls)
- [Quick Reference](#quick-reference)

---

## Async & Await Basics

`async/await` writes non-blocking code that reads like synchronous code. The thread is **freed** while waiting for I/O, then **resumed** automatically.

```csharp
// ── Basic async method ────────────────────────────────────────
public static async Task<string> FetchDataAsync(string url)
{
    using HttpClient client = new();
    // Thread is FREED here while waiting for network response
    string data = await client.GetStringAsync(url);
    // Thread is RESUMED here when response arrives
    return data.ToUpper();
}

// ── Calling an async method ───────────────────────────────────
// From another async method:
string result = await FetchDataAsync("https://api.example.com");

// From non-async (e.g. Main — use async Main instead):
static async Task Main(string[] args)
{
    string r = await FetchDataAsync("https://api.example.com");
    Console.WriteLine(r);
}

// ── Task.Delay — async sleep ──────────────────────────────────
await Task.Delay(1000);                       // 1 second
await Task.Delay(TimeSpan.FromSeconds(2));    // 2 seconds
// Thread.Sleep() BLOCKS the thread — never use in async code!
```

> 💡 **async/await is NOT multithreading.** A single thread can handle thousands of async I/O operations. It suspends on `await` and resumes when the I/O completes — no extra thread needed. This is why web servers scale so well.

---

## Return Types

```csharp
// ── Task — async with no return value ────────────────────────
public static async Task DoWorkAsync()
{
    await Task.Delay(100);
    Console.WriteLine("Done");
}
await DoWorkAsync();

// ── Task<T> — async with return value ────────────────────────
public static async Task<int> GetCountAsync()
{
    await Task.Delay(100);
    return 42;
}
int count = await GetCountAsync();

// ── ValueTask<T> — performance optimization ───────────────────
// Use when the result is often available synchronously (no I/O needed)
// Avoids heap allocation of Task object
public static async ValueTask<int> GetCachedAsync()
{
    if (_cache.TryGetValue("key", out int val))
        return val;                     // synchronous path — no Task allocation
    return await FetchFromDbAsync();   // async path when cache miss
}

// ── async void — ONLY for event handlers ─────────────────────
private async void Button_Click(object? sender, EventArgs e)
{
    var data = await FetchDataAsync("...");
    UpdateUI(data);
}
// ❌ Never return async void from regular methods!
// ❌ Exceptions in async void will crash the application
```

---

## Parallel Async Calls

```csharp
// ❌ WRONG — sequential (each awaits before starting next)
// Total time = sum of all durations
string a = await GetDataAsync("A");   // waits 300ms
string b = await GetDataAsync("B");   // waits 200ms
string c = await GetDataAsync("C");   // waits 100ms
// Total: 600ms

// ✅ CORRECT — start all, then await all
// Total time = max of all durations
Task<string> tA = GetDataAsync("A");   // start immediately
Task<string> tB = GetDataAsync("B");   // start immediately
Task<string> tC = GetDataAsync("C");   // start immediately
string[] results = await Task.WhenAll(tA, tB, tC);
// Total: ~300ms — all run in parallel!

// ── Process many items in parallel ───────────────────────────
List<string> urls = new() { "url1", "url2", "url3", "url4" };

// Start all tasks at once
var tasks = urls.Select(url => FetchDataAsync(url));
string[] allData = await Task.WhenAll(tasks);

// ── WhenAny — first wins (useful for timeouts) ────────────────
Task<string> fetchTask = FetchDataAsync("slow-api.com");
Task timeoutTask = Task.Delay(TimeSpan.FromSeconds(5));

Task winner = await Task.WhenAny(fetchTask, timeoutTask);
if (winner == timeoutTask)
    Console.WriteLine("Request timed out");
else
    Console.WriteLine($"Got: {await fetchTask}");

// ── ConfigureAwait(false) — for library code ──────────────────
// Prevents deadlocks in UI apps and ASP.NET classic
// Not needed in ASP.NET Core (no synchronization context)
public static async Task<string> LibraryMethod()
{
    var data = await FetchAsync().ConfigureAwait(false);
    return data.ToUpper();   // resumes on any available thread
}
```

---

## Exception Handling in Async

```csharp
// ── Basic try-catch works with await ─────────────────────────
try
{
    string data = await FetchDataAsync("bad-url");
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"HTTP error: {ex.Message}");
}
catch (TaskCanceledException)
{
    Console.WriteLine("Request was cancelled or timed out");
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected: {ex.Message}");
}

// ── Task.WhenAll — AggregateException ─────────────────────────
Task t1 = Task.Run(() => throw new InvalidOperationException("T1 failed"));
Task t2 = Task.Run(() => throw new ArgumentException("T2 failed"));
Task combined = Task.WhenAll(t1, t2);

try
{
    await combined;
}
catch (Exception ex)
{
    // await only re-throws the FIRST exception
    Console.WriteLine($"First error: {ex.Message}");

    // To get ALL exceptions — check the Task's AggregateException
    if (combined.Exception is AggregateException agg)
    {
        foreach (var inner in agg.InnerExceptions)
            Console.WriteLine($"  → {inner.GetType().Name}: {inner.Message}");
    }
}

// ── async void — exceptions cannot be caught! ────────────────
async void BadMethod()
{
    throw new Exception("This crashes the app!");   // ❌ uncatchable
}

// Call site — exception propagates nowhere
try
{
    BadMethod();   // no await possible — returns void
}
catch (Exception)
{
    // ❌ This NEVER runs!
}

// ── Safe fire-and-forget pattern ─────────────────────────────
static async Task SafeFireAndForget(Func<Task> work)
{
    try   { await work(); }
    catch (Exception ex) { Logger.LogError(ex, "Background task failed"); }
}

// Usage
_ = SafeFireAndForget(() => SendEmailAsync(user));   // _ discards the Task

// ── CancellationToken timeout ─────────────────────────────────
public static async Task<string> WithTimeout(string url, int seconds)
{
    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(seconds));
    try
    {
        using HttpClient client = new();
        return await client.GetStringAsync(url, cts.Token);
    }
    catch (OperationCanceledException)
    {
        return $"Timed out after {seconds}s";
    }
}

// ── Retry pattern ─────────────────────────────────────────────
public static async Task<T> RetryAsync<T>(Func<Task<T>> operation, int maxAttempts = 3)
{
    for (int attempt = 1; attempt <= maxAttempts; attempt++)
    {
        try
        {
            return await operation();
        }
        catch (Exception ex) when (attempt < maxAttempts)
        {
            Console.WriteLine($"Attempt {attempt} failed: {ex.Message}. Retrying...");
            await Task.Delay(attempt * 1000);   // exponential backoff
        }
    }
    return await operation();   // last attempt — let exception propagate
}
```

---

## Async Streams (IAsyncEnumerable)

Produce and consume a stream of values asynchronously — one at a time.

```csharp
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// ── Producer — async method with yield return ─────────────────
public static async IAsyncEnumerable<int> CountSlowlyAsync(
    int from, int to,
    [EnumeratorCancellation] CancellationToken ct = default)
{
    for (int i = from; i <= to; i++)
    {
        await Task.Delay(100, ct);   // async work between items
        yield return i;              // emit one item
    }
}

// ── Consumer — await foreach ──────────────────────────────────
await foreach (int num in CountSlowlyAsync(1, 10))
    Console.Write($"{num} ");   // 1 2 3 4 5 6 7 8 9 10

// ── Real-world: paginated API ─────────────────────────────────
public static async IAsyncEnumerable<Product> StreamProductsAsync(
    [EnumeratorCancellation] CancellationToken ct = default)
{
    int page = 1;
    while (true)
    {
        var batch = await FetchPageAsync(page++, ct);
        if (batch.Count == 0) yield break;           // no more pages
        foreach (var product in batch)
            yield return product;                    // one at a time
    }
}

// ── Consumer with cancellation ────────────────────────────────
using var cts = new CancellationTokenSource();
await foreach (var product in
    StreamProductsAsync().WithCancellation(cts.Token))
{
    Console.WriteLine(product.Name);
    if (product.Id > 100) cts.Cancel();   // stop early
}

// ── Real-world: file line reader ──────────────────────────────
public static async IAsyncEnumerable<string> ReadLinesAsync(string path)
{
    using StreamReader reader = new(path);
    string? line;
    while ((line = await reader.ReadLineAsync()) != null)
        yield return line;
}

await foreach (string line in ReadLinesAsync("bigfile.txt"))
    Console.WriteLine(line);

// ── LINQ on async streams (System.Linq.Async NuGet) ──────────
// var filtered = stream.WhereAwait(async x => await IsValidAsync(x));
// var first    = await stream.FirstOrDefaultAsync(x => x.Id > 10);
```

> ✅ **IAsyncEnumerable vs IEnumerable:** `IEnumerable` loads all data then iterates. `IAsyncEnumerable` fetches items on-demand asynchronously — perfect for streaming large datasets without loading into memory.

---

## Common Pitfalls

```csharp
// ❌ 1. Async void (except event handlers)
async void Bad() { }      // ❌
async Task Good() { }     // ✅

// ❌ 2. .Result or .Wait() — causes deadlocks in UI/ASP.NET
string data = FetchAsync().Result;    // ❌ can deadlock
string data = await FetchAsync();     // ✅

// ❌ 3. Sequential awaits when you want parallel
var a = await GetAAsync();   // ❌ waits before starting B
var b = await GetBAsync();

var tA = GetAAsync();        // ✅ both start immediately
var tB = GetBAsync();
(var a2, var b2) = (await tA, await tB);

// ❌ 4. async lambda that should return Task<T>
var btn = new Button();
btn.Click += async (s, e) => { await DoWorkAsync(); };   // ✅ event handler = ok

// ❌ 5. Forgetting ConfigureAwait(false) in library code
// (can cause deadlocks in legacy ASP.NET and WinForms)
public async Task LibraryMethod()
{
    await SomeAsyncOp().ConfigureAwait(false);   // ✅ in libraries
}

// ❌ 6. Not passing CancellationToken through
public async Task Fetch(CancellationToken ct)
{
    var data = await client.GetStringAsync(url, ct);   // ✅ pass it down
}
```

---

## Quick Reference

```
Async Programming
│
├── async / await
│   ├── Mark method with 'async'
│   ├── Return Task / Task<T> / ValueTask<T>
│   ├── await frees thread while waiting
│   └── await Task.Delay() — never Thread.Sleep()
│
├── Return Types
│   ├── async Task           → no return value
│   ├── async Task<T>        → with return value
│   ├── async ValueTask<T>   → perf opt for sync-often path
│   └── async void           → event handlers ONLY
│
├── Parallel Async
│   ├── ❌ await A; await B;              (sequential)
│   ├── ✅ var tA=A(); var tB=B();        (start both)
│   │      await Task.WhenAll(tA, tB)    (then wait)
│   └── Task.WhenAny → first wins
│
├── Exception Handling
│   ├── try-catch works normally with await
│   ├── WhenAll → AggregateException (check .Exception)
│   ├── async void → exceptions crash app
│   └── OperationCanceledException → from CancellationToken
│
└── IAsyncEnumerable<T>
    ├── async IAsyncEnumerable<T> Method() { yield return x; }
    ├── await foreach (var item in stream) { }
    ├── .WithCancellation(token) on consumer
    └── [EnumeratorCancellation] on producer param
```

```csharp
// One-line cheat sheet
string r   = await FetchAsync();                           // basic await
string[] a = await Task.WhenAll(Fetch("A"), Fetch("B")); // parallel
await foreach (var x in StreamAsync()) { }               // async stream
using var cts = new CancellationTokenSource(5000);       // 5s timeout
```

---
