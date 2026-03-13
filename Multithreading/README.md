### Multithreading 🟢

> Thread Class · Task Class · Parallel Programming (Parallel.ForEach)

---

## Table of Contents
- [Thread Class](#thread-class)
- [Task Class](#task-class)
- [Parallel Programming](#parallel-programming)
- [Thread Safety](#thread-safety)
- [Quick Reference](#quick-reference)

---

## Thread Class

The low-level OS thread. Understand it, but prefer `Task` in modern code.

```csharp
using System.Threading;

// ── Create and start ──────────────────────────────────────────
Thread t1 = new Thread(() =>
{
    for (int i = 0; i < 5; i++)
    {
        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {i}");
        Thread.Sleep(100);   // pause this thread (blocks the thread)
    }
});
t1.Start();   // begin execution
t1.Join();    // block caller until t1 finishes

// ── Pass data to a thread ─────────────────────────────────────
Thread t2 = new Thread(obj =>
{
    string msg = (string)obj!;
    Console.WriteLine($"Received: {msg}");
});
t2.Start("Hello from main thread");
t2.Join();

// ── Thread properties ─────────────────────────────────────────
Thread t3 = new Thread(WorkMethod);
t3.Name         = "WorkerThread";      // name for debugging
t3.IsBackground = true;                // dies when main thread exits
t3.Priority     = ThreadPriority.BelowNormal;
t3.Start();

// ── Thread info ───────────────────────────────────────────────
Console.WriteLine(Thread.CurrentThread.ManagedThreadId);  // thread ID
Console.WriteLine(Thread.CurrentThread.IsBackground);     // true/false
Console.WriteLine(Thread.CurrentThread.ThreadState);      // Running, Stopped, etc.
Console.WriteLine(Environment.ProcessorCount);            // number of CPU cores
```

> ⚠️ Creating raw threads is expensive (~1MB stack each). Prefer `Task` which uses the ThreadPool (reuses threads). Use `Thread` only when you need `IsBackground`, `Priority`, or other fine-grained control.

---

## Task Class

Modern, pool-based concurrency. Supports return values, chaining, and `async/await`.

```csharp
using System.Threading.Tasks;

// ── Fire and forget ───────────────────────────────────────────
Task t1 = Task.Run(() => Console.WriteLine("Running in thread pool"));
await t1;

// ── Task with return value ────────────────────────────────────
Task<int> calc = Task.Run(() =>
{
    Thread.Sleep(500);   // simulate CPU work
    return 42;
});
int result = await calc;   // 42

// ── Task.WhenAll — run in parallel, wait for all ──────────────
Task<string> tA = Task.Run(() => { Thread.Sleep(300); return "A"; });
Task<string> tB = Task.Run(() => { Thread.Sleep(200); return "B"; });
Task<string> tC = Task.Run(() => { Thread.Sleep(100); return "C"; });

string[] results = await Task.WhenAll(tA, tB, tC);
// Total time ≈ 300ms (not 600ms) — all run in parallel!

// ── Task.WhenAny — first to finish ───────────────────────────
Task<string> first = await Task.WhenAny(tA, tB, tC);
Console.WriteLine($"First: {await first}");   // "C" (100ms)

// ── CancellationToken — cancel a running task ─────────────────
CancellationTokenSource cts   = new();
CancellationToken       token = cts.Token;

Task longTask = Task.Run(() =>
{
    for (int i = 0; i < 100; i++)
    {
        token.ThrowIfCancellationRequested();   // check each iteration
        Thread.Sleep(50);
        Console.Write(".");
    }
}, token);

await Task.Delay(300);   // let it run 300ms
cts.Cancel();            // signal cancellation

try   { await longTask; }
catch (OperationCanceledException) { Console.WriteLine("\nCancelled!"); }

// ── CancellationToken with timeout ────────────────────────────
using var ctsWith = new CancellationTokenSource(TimeSpan.FromSeconds(5));
// Auto-cancels after 5 seconds

// ── Task continuation ─────────────────────────────────────────
Task pipeline = Task.Run(() => "Step 1")
    .ContinueWith(prev => { Console.WriteLine(prev.Result); return "Step 2"; })
    .ContinueWith(prev =>   Console.WriteLine(prev.Result));

// ── Task.Delay — async sleep (doesn't block thread) ──────────
await Task.Delay(1000);                        // wait 1 second
await Task.Delay(TimeSpan.FromSeconds(2));     // wait 2 seconds
// Never use Thread.Sleep() in async code — it blocks the thread

// ── Task status ───────────────────────────────────────────────
Console.WriteLine(t1.Status);          // RanToCompletion, Faulted, Canceled
Console.WriteLine(t1.IsCompleted);     // true/false
Console.WriteLine(t1.IsCanceled);      // true/false
Console.WriteLine(t1.IsFaulted);       // true/false
```

| Method | Waits For | Use When |
|---|---|---|
| `Task.WhenAll(t1,t2)` | All tasks complete | Need all results |
| `Task.WhenAny(t1,t2)` | First task completes | Racing / timeout |
| `await task` | Single task | Sequential tasks |

---

## Parallel Programming

Data parallelism across CPU cores. For CPU-bound work on large collections.

```csharp
using System.Threading.Tasks;

int[] data = Enumerable.Range(1, 1_000_000).ToArray();

// ── Parallel.ForEach ──────────────────────────────────────────
// Processes items across all available CPU cores automatically
Parallel.ForEach(data, item =>
{
    ProcessItem(item);   // each item processed independently
});

// With lock for shared state
long total = 0;
object lockObj = new();
Parallel.ForEach(data, item =>
{
    lock (lockObj)        // only one thread at a time
        total += item;
});

// Better: thread-local accumulator (avoids constant locking)
long betterTotal = 0;
Parallel.ForEach(data,
    localInit:   () => 0L,                              // each thread starts with 0
    body:        (item, state, local) => local + item,  // accumulate locally
    localFinally: local =>
        Interlocked.Add(ref betterTotal, local)         // merge at the end (atomic)
);

// ── Parallel.For ──────────────────────────────────────────────
int[] squares = new int[100];
Parallel.For(0, 100, i =>
{
    squares[i] = i * i;   // each i writes to its own index — no lock needed
});

// ── ParallelOptions ───────────────────────────────────────────
var options = new ParallelOptions
{
    MaxDegreeOfParallelism = 4,          // max 4 threads (default = CPU count)
    CancellationToken      = cts.Token   // support cancellation
};
Parallel.ForEach(data, options, item => { /* ... */ });

// ── Break early ───────────────────────────────────────────────
Parallel.For(0, 1000, (i, state) =>
{
    if (i > 100) state.Break();   // stop processing new items
    Process(i);
});

// ── PLINQ — Parallel LINQ ─────────────────────────────────────
var results = data
    .AsParallel()                        // switch to parallel mode
    .WithDegreeOfParallelism(4)          // limit threads
    .WithCancellation(cts.Token)
    .Where(n => n % 2 == 0)
    .Select(n => n * n)
    .ToList();

// Preserve order (slower)
var ordered = data
    .AsParallel()
    .AsOrdered()                         // maintain original order
    .Select(n => n * 2)
    .ToList();
```

---

## Thread Safety

Protecting shared state when multiple threads run concurrently.

```csharp
// ── lock — mutual exclusion ───────────────────────────────────
object _lock = new();
int counter  = 0;

void Increment()
{
    lock (_lock)       // only one thread enters at a time
    {
        counter++;     // safe!
    }
}

// ── Interlocked — atomic operations (faster than lock) ───────
int atomicCounter = 0;
Interlocked.Increment(ref atomicCounter);      // thread-safe ++
Interlocked.Decrement(ref atomicCounter);      // thread-safe --
Interlocked.Add(ref atomicCounter, 10);        // thread-safe += 10
int old = Interlocked.Exchange(ref atomicCounter, 0);        // swap
int prev = Interlocked.CompareExchange(ref atomicCounter, 1, 0); // CAS

// ── Monitor (lock is syntactic sugar for this) ────────────────
Monitor.Enter(_lock);
try   { counter++; }
finally { Monitor.Exit(_lock); }

// ── ReaderWriterLockSlim — many readers, one writer ──────────
ReaderWriterLockSlim rwLock = new();

void Read()
{
    rwLock.EnterReadLock();    // many can read simultaneously
    try   { /* read data */ }
    finally { rwLock.ExitReadLock(); }
}

void Write()
{
    rwLock.EnterWriteLock();   // exclusive — blocks all readers
    try   { /* write data */ }
    finally { rwLock.ExitWriteLock(); }
}

// ── Thread-safe collections ───────────────────────────────────
using System.Collections.Concurrent;

ConcurrentDictionary<string, int> dict  = new();
ConcurrentQueue<string>           queue = new();
ConcurrentBag<int>                bag   = new();

dict.TryAdd("key", 1);
dict.AddOrUpdate("key", 1, (k, v) => v + 1);   // atomic increment
queue.Enqueue("item");
queue.TryDequeue(out string? item);
```

---

## Quick Reference

```
Multithreading
│
├── Thread Class (low-level)
│   ├── new Thread(() => { }) → .Start() → .Join()
│   ├── IsBackground = true   → dies with main thread
│   └── lock(obj) { }         → mutual exclusion
│
├── Task Class (modern, prefer this)
│   ├── Task.Run(() => { })       → fire on thread pool
│   ├── Task.Run(() => value)     → Task<T> with return
│   ├── await Task.WhenAll(t1,t2) → parallel, wait ALL
│   ├── await Task.WhenAny(t1,t2) → parallel, first wins
│   ├── CancellationTokenSource   → cancel running tasks
│   └── await Task.Delay(ms)      → async sleep
│
├── Parallel (data parallelism)
│   ├── Parallel.ForEach(list, item => { })
│   ├── Parallel.For(0, n, i => { })
│   ├── ParallelOptions.MaxDegreeOfParallelism
│   └── .AsParallel()  → PLINQ
│
└── Thread Safety
    ├── lock(obj) { }            → mutual exclusion
    ├── Interlocked.Increment()  → atomic, no lock needed
    ├── ReaderWriterLockSlim     → many readers, one writer
    └── ConcurrentDictionary / Queue / Bag → safe collections
```

```csharp
// One-line cheat sheet
var t = Task.Run(() => DoWork());             // start task
string[] r = await Task.WhenAll(t1, t2, t3); // parallel + wait all
Parallel.ForEach(list, item => Process(item));// all CPU cores
Interlocked.Increment(ref counter);          // atomic, no lock
```

---
