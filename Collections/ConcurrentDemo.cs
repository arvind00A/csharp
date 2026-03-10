// ============================================================
//  PROJECT 3 — Specialized / Concurrent Collections
//  Multi-Threaded Job Processing System
//  Topics: ConcurrentDictionary, ConcurrentQueue,
//          ConcurrentBag, ConcurrentStack, BlockingCollection
// ============================================================


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Collections
{
    internal class ConcurrentDemo
    {
        static void Main()
        {
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║  Concurrent Collections — Job Processor      ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝\n");

            Demo_WhyWeNeedConcurrent();
            Demo_ConcurrentDictionary();
            Demo_ConcurrentQueue();
            Demo_ConcurrentBag();
            Demo_ConcurrentStack();
            Demo_BlockingCollection_ProducerConsumer();
        }

        // ── WHY: Show the problem without concurrent collections ──
        static void Demo_WhyWeNeedConcurrent()
        {
            Console.WriteLine("=== WHY: Regular Dictionary vs Concurrent ===\n");

            Console.WriteLine("     Regular Dictionary in multi-threaded code:");
            Console.WriteLine("     dict[key] = dict[key] + 1  <- RACE CONDITION!");
            Console.WriteLine("     Two threads read 0 simultaneously -> both write 1");
            Console.WriteLine("     Result: 1 instead of 2 — data corruption!\n");

            Console.WriteLine("     ConcurrentDictionary:");
            Console.WriteLine("     AddOrUpdate is ATOMIC — no race conditions\n");
        }

        // ── 1. ConcurrentDictionary — Page Visit Counter ─────────
        static void Demo_ConcurrentDictionary()
        {
            Console.WriteLine("=== 1. ConcurrentDictionary — Web Page Visit Counter ===\n");

            ConcurrentDictionary<string, int> visitCounts = new();

            // Simulate multiple threads hitting different pages simultaneously
            string[] pages = { "/home", "/about", "/products", "/contact", "/home",
                           "/products", "/home", "/about", "/home", "/products" };

            // Run all "visits" in parallel — would be unsafe with regular Dictionary
            Parallel.ForEach(pages, page =>
            {
                visitCounts.AddOrUpdate(
                    key: page,
                    addValue: 1,               // first visit
                    updateValueFactory: (k, old) => old + 1  // subsequent visits
                );
            });

            Console.WriteLine("  Page visit counts (after parallel access):");
            foreach (var (page, count) in visitCounts)
                Console.WriteLine($"    {page,-15} -> {count} visits");

            // GetOrAdd — common pattern: get existing or create default
            int blogVisits = visitCounts.GetOrAdd("/blog", 0);
            Console.WriteLine($"\n  /blog visits (GetOrAdd): {blogVisits}");

            // TryGetValue — safe read
            if (visitCounts.TryGetValue("/home", out int homeCount))
                Console.WriteLine($"  /home total : {homeCount}");

            // TryUpdate — only update if current value matches expected
            bool updated = visitCounts.TryUpdate("/about", 999, visitCounts["/about"]);
            Console.WriteLine($"  TryUpdate /about to 999: {updated}");

            // TryRemove — safe remove
            visitCounts.TryRemove("/contact", out int removed);
            Console.WriteLine($"  TryRemove /contact (was {removed})");

            Console.WriteLine($"\n  Final count: {visitCounts.Count} pages tracked\n");
        }

        // ── 2. ConcurrentQueue — Task Dispatcher ─────────────────
        static void Demo_ConcurrentQueue()
        {
            Console.WriteLine("=== 2. ConcurrentQueue<T> — Parallel Task Dispatcher ===\n");

            ConcurrentQueue<string> taskQueue = new();
            ConcurrentBag<string> completed = new();

            // Producer: add tasks
            string[] tasks = { "SendEmail", "GenerateReport", "ProcessPayment",
                           "UpdateInventory", "SendNotification", "BackupData" };

            Console.WriteLine("  Producer adding tasks:");
            foreach (string t in tasks)
            {
                taskQueue.Enqueue(t);
                Console.WriteLine($"     Queued: {t}");
            }

            Console.WriteLine($"\n  Queue size: {taskQueue.Count}");
            Console.WriteLine($"  Peek (next): ");
            if (taskQueue.TryPeek(out string? nextTask))
                Console.WriteLine($"    -> {nextTask}");

            // Multiple consumer threads processing tasks
            Console.WriteLine("\n  3 worker threads processing (FIFO):");
            object consoleLock = new();

            Task[] workers = new Task[3];
            for (int w = 0; w < 3; w++)
            {
                int workerId = w + 1;
                workers[w] = Task.Run(() =>
                {
                    while (taskQueue.TryDequeue(out string? job))
                    {
                        Thread.Sleep(10); // simulate work
                        completed.Add(job!);
                        lock (consoleLock)
                            Console.WriteLine($"     Worker {workerId}: Completed '{job}'");
                    }
                });
            }
            Task.WaitAll(workers);

            Console.WriteLine($"\n  All tasks done! Completed: {completed.Count}/{tasks.Length}\n");
        }

        // ── 3. ConcurrentBag — Parallel Result Collector ─────────
        static void Demo_ConcurrentBag()
        {
            Console.WriteLine("=== 3. ConcurrentBag<T> — Parallel Result Collection ===\n");

            // ConcurrentBag is optimized for scenarios where same thread
            // adds AND removes — like parallel for-each with local results
            ConcurrentBag<(int Input, int Result)> results = new();

            Console.WriteLine("  Computing squares of 1-10 in parallel:");

            Parallel.For(1, 11, i =>
            {
                int square = i * i;
                results.Add((i, square));   // thread-safe add
            });

            // Note: ConcurrentBag doesn't guarantee order!
            Console.WriteLine("  Results (order not guaranteed with parallel):");
            var sorted = new List<(int, int)>(results);
            sorted.Sort((a, b) => a.Item1.CompareTo(b.Item1));
            foreach (var (input, result) in sorted)
                Console.WriteLine($"    {input,2}² = {result,3}");

            // TryTake — remove one item
            if (results.TryTake(out var item))
                Console.WriteLine($"\n  TryTake removed one item: {item.Input}² = {item.Result}");

            Console.WriteLine($"  Remaining in bag: {results.Count}\n");
        }

        // ── 4. ConcurrentStack — Parallel Undo System ────────────
        static void Demo_ConcurrentStack()
        {
            Console.WriteLine("=== 4. ConcurrentStack<T> — Thread-Safe Undo History ===\n");

            ConcurrentStack<string> undoStack = new();

            // Simulate multiple threads making changes
            var actions = new[] { "TypedText", "PastedImage", "FormattedBold",
                              "DeletedParagraph", "AddedLink" };

            Console.WriteLine("  Threads performing actions:");
            Parallel.ForEach(actions, action =>
            {
                undoStack.Push(action);
                Console.WriteLine($"     Pushed: {action}");
            });

            Console.WriteLine($"\n  Stack size: {undoStack.Count}");

            // TryPeek — look at top without removing
            if (undoStack.TryPeek(out string? top))
                Console.WriteLine($"  Top of stack: {top}");

            // TryPop — undo one action
            Console.WriteLine("\n  Undoing actions:");
            for (int i = 0; i < 3; i++)
            {
                if (undoStack.TryPop(out string? undone))
                    Console.WriteLine($"      Undid: {undone}");
            }

            // TryPopRange — pop multiple at once
            string[] batch = new string[undoStack.Count];
            int popped = undoStack.TryPopRange(batch);
            Console.WriteLine($"\n  TryPopRange popped {popped} items: {string.Join(", ", batch[..popped])}");
            Console.WriteLine($"  Stack now empty: {undoStack.IsEmpty}\n");
        }

        // ── 5. BlockingCollection — Producer-Consumer Pipeline ───
        static void Demo_BlockingCollection_ProducerConsumer()
        {
            Console.WriteLine("=== 5. BlockingCollection<T> — Producer-Consumer Pipeline ===\n");

            // BlockingCollection wraps ConcurrentQueue (default) and adds:
            // - Blocking when empty (consumer waits)
            // - Blocking when full (producer waits)
            // - CompleteAdding() signal

            using BlockingCollection<string> pipeline = new(boundedCapacity: 5);

            Console.WriteLine("  Pipeline: Producer -> [BlockingCollection] -> Consumer\n");

            // Producer task
            Task producer = Task.Run(() =>
            {
                string[] emails = {
                    "welcome@user1.com", "invoice@user2.com", "reset@user3.com",
                    "promo@user4.com", "alert@user5.com", "report@user6.com"
                };

                foreach (string email in emails)
                {
                    pipeline.Add(email);          // blocks if capacity reached
                    Console.WriteLine($"   Producer: Queued '{email}'");
                    Thread.Sleep(30);
                }
                pipeline.CompleteAdding();        // signal: no more items
                Console.WriteLine("   Producer: Done adding\n");
            });

            // Consumer task — GetConsumingEnumerable blocks until item available
            Task consumer = Task.Run(() =>
            {
                foreach (string email in pipeline.GetConsumingEnumerable())
                {
                    Thread.Sleep(60);  // simulate slower processing
                    Console.WriteLine($"   Consumer: Sent email to '{email}'");
                }
                Console.WriteLine("   Consumer: All emails sent");
            });

            Task.WaitAll(producer, consumer);

            Console.WriteLine("\n All Concurrent Collections demonstrated!");
            Console.WriteLine("\nSummary:");
            Console.WriteLine("  ConcurrentDictionary  -> thread-safe key-value (page counters, caches)");
            Console.WriteLine("  ConcurrentQueue       -> thread-safe FIFO (job queues)");
            Console.WriteLine("  ConcurrentBag         -> thread-safe unordered (parallel result collection)");
            Console.WriteLine("  ConcurrentStack       -> thread-safe LIFO (undo in multi-user apps)");
            Console.WriteLine("  BlockingCollection    -> producer-consumer pipeline (email queues, logging)");
        }
    }
}
