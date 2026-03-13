// ============================================================
//  PROJECT 1 — Multithreading
//  Parallel Image Processing Pipeline
//  Topics: Thread Class, Task Class, Task.WhenAll,
//          CancellationToken, Parallel.ForEach, Interlocked
// ============================================================


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Multithreading
{
    // ── Simulated "image" data ────────────────────────────────────
    record ImageJob(int Id, string FileName, int Width, int Height, string Filter);

    record ProcessedImage(int Id, string FileName, string Result,
        long ProcessingMs, int ThreadId);

    // ── Image processor (simulates CPU-bound work) ────────────────
    static class ImageProcessor
    {
        // Pure function — safe to run in parallel (no shared state)
        public static ProcessedImage Process(ImageJob job, CancellationToken ct = default)
        {
            var sw = Stopwatch.StartNew();

            // Simulate CPU-intensive processing
            int workMs = job.Filter switch
            {
                "blur" => 200,
                "sharpen" => 150,
                "grayscale" => 80,
                "resize" => 300,
                "watermark" => 120,
                _ => 100
            };

            ct.ThrowIfCancellationRequested();

            // Simulate work in chunks so cancellation can be checked
            for (int chunk = 0; chunk < 4; chunk++)
            {
                Thread.Sleep(workMs / 4);
                ct.ThrowIfCancellationRequested();
            }

            sw.Stop();
            string result = $"{job.Filter.ToUpper()} applied to {job.Width}x{job.Height}";
            return new ProcessedImage(job.Id, job.FileName, result,
                sw.ElapsedMilliseconds, Thread.CurrentThread.ManagedThreadId);
        }
    }
    internal class MultithreadingDemo
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║  Multithreading — Image Processor 🖼️     ║");
            Console.WriteLine("╚══════════════════════════════════════════╝\n");

            // Sample jobs
            var jobs = new List<ImageJob>
            {
                new(1,  "photo_001.jpg", 1920, 1080, "blur"),
                new(2,  "photo_002.jpg", 3840, 2160, "sharpen"),
                new(3,  "photo_003.jpg", 1280,  720, "grayscale"),
                new(4,  "photo_004.jpg", 2560, 1440, "resize"),
                new(5,  "photo_005.jpg", 1920, 1080, "watermark"),
                new(6,  "photo_006.jpg", 1920, 1080, "blur"),
                new(7,  "photo_007.jpg", 3840, 2160, "grayscale"),
                new(8,  "photo_008.jpg", 1280,  720, "sharpen"),
                new(9,  "photo_009.jpg", 2560, 1440, "blur"),
                new(10, "photo_010.jpg", 1920, 1080, "resize"),
            };

            Demo1_ThreadClass(jobs.Take(3).ToList());
            Demo2_TaskClass(jobs.Take(4).ToList()).GetAwaiter().GetResult();
            Demo3_ParallelForEach(jobs);
            Demo4_CancellationToken(jobs).GetAwaiter().GetResult();
            Demo5_Interlocked(jobs);
        }

        // ── 1. Thread Class ───────────────────────────────────────
        static void Demo1_ThreadClass(List<ImageJob> jobs)
        {
            Console.WriteLine("=== 1. Thread Class ===\n");

            // Create one thread per job manually
            Thread[] threads = new Thread[jobs.Count];
            var results = new ProcessedImage?[jobs.Count];

            var sw = Stopwatch.StartNew();

            for (int i = 0; i < jobs.Count; i++)
            {
                int idx = i;                    // capture loop variable
                var job = jobs[idx];

                threads[idx] = new Thread(() =>
                {
                    Console.WriteLine($"  [Thread {Thread.CurrentThread.ManagedThreadId:00}] " +
                                      $"Starting: {job.FileName}");
                    results[idx] = ImageProcessor.Process(job);
                    Console.WriteLine($"  [Thread {Thread.CurrentThread.ManagedThreadId:00}] " +
                                      $"Done:     {job.FileName} ({results[idx]!.ProcessingMs}ms)");
                });

                threads[idx].Name = $"ImageThread-{idx + 1}";
                threads[idx].IsBackground = true;
                threads[idx].Start();
            }

            // Wait for all threads to finish
            foreach (var t in threads) t.Join();
            sw.Stop();

            Console.WriteLine($"\n  All {jobs.Count} images processed in {sw.ElapsedMilliseconds}ms");
            Console.WriteLine($"  Results:");
            foreach (var r in results)
                Console.WriteLine($"    #{r!.Id} {r.FileName}: {r.Result}");
            Console.WriteLine();
        }

        // ── 2. Task Class ────────────────────────────────────────
        static async Task Demo2_TaskClass(List<ImageJob> jobs)
        {
            Console.WriteLine("=== 2. Task Class ===\n");

            var sw = Stopwatch.StartNew();

            // Start all tasks at once (parallel!)
            Task<ProcessedImage>[] tasks = jobs
                .Select(job => Task.Run(() =>
                {
                    Console.WriteLine($"  [TaskPool] Starting: {job.FileName}");
                    var result = ImageProcessor.Process(job);
                    Console.WriteLine($"  [TaskPool] Done:     {job.FileName} ({result.ProcessingMs}ms)");
                    return result;
                }))
                .ToArray();

            // Wait for ALL to complete
            ProcessedImage[] results = await Task.WhenAll(tasks);
            sw.Stop();

            Console.WriteLine($"\n  All {jobs.Count} images processed in {sw.ElapsedMilliseconds}ms " +
                              $"(parallel, not {results.Sum(r => r.ProcessingMs)}ms sequential)");

            // WhenAny — first completed
            var priorityJobs = jobs.Take(3).ToList();
            var priorityTasks = priorityJobs.Select(j => Task.Run(() => ImageProcessor.Process(j))).ToList();
            var firstDone = await Task.WhenAny(priorityTasks);
            var first = await firstDone;
            Console.WriteLine($"\n  First completed: {first.FileName} ({first.ProcessingMs}ms)\n");
        }

        // ── 3. Parallel.ForEach ───────────────────────────────────
        static void Demo3_ParallelForEach(List<ImageJob> jobs)
        {
            Console.WriteLine("=== 3. Parallel.ForEach ===\n");

            var results = new ConcurrentBag<ProcessedImage>();
            int processed = 0;
            var sw = Stopwatch.StartNew();

            // Parallel.ForEach distributes across all CPU cores
            Parallel.ForEach(
                jobs,
                new ParallelOptions { MaxDegreeOfParallelism = 4 },
                job =>
                {
                    var result = ImageProcessor.Process(job);
                    results.Add(result);
                    int count = Interlocked.Increment(ref processed);  // thread-safe!
                    Console.WriteLine($"  [{count,2}/{jobs.Count}] {job.FileName} " +
                                      $"on thread {result.ThreadId:00} — {result.ProcessingMs}ms");
                });

            sw.Stop();

            // Stats
            var grouped = results.GroupBy(r => r.ThreadId).OrderBy(g => g.Key);
            Console.WriteLine($"\n  Parallel completed {jobs.Count} images in {sw.ElapsedMilliseconds}ms");
            Console.WriteLine($"  Thread distribution:");
            foreach (var g in grouped)
                Console.WriteLine($"    Thread {g.Key:00}: {g.Count()} images");

            // PLINQ version
            Console.WriteLine("\n  PLINQ version:");
            sw.Restart();
            var plinqResults = jobs
                .AsParallel()
                .WithDegreeOfParallelism(4)
                .Select(job => ImageProcessor.Process(job))
                .ToList();
            sw.Stop();
            Console.WriteLine($"  PLINQ: {plinqResults.Count} images in {sw.ElapsedMilliseconds}ms\n");
        }

        // ── 4. CancellationToken ──────────────────────────────────
        static async Task Demo4_CancellationToken(List<ImageJob> jobs)
        {
            Console.WriteLine("=== 4. CancellationToken ===\n");

            using var cts = new CancellationTokenSource();

            // Cancel after 400ms
            _ = Task.Run(async () =>
            {
                await Task.Delay(400);
                Console.WriteLine("  ⏱️  400ms passed — cancelling batch...");
                cts.Cancel();
            });

            var sw = Stopwatch.StartNew();
            int completed = 0, cancelled = 0;

            var tasks = jobs.Select(job => Task.Run(() =>
            {
                try
                {
                    var result = ImageProcessor.Process(job, cts.Token);
                    Interlocked.Increment(ref completed);
                    Console.WriteLine($"  ✅ {job.FileName} done ({result.ProcessingMs}ms)");
                    return result;
                }
                catch (OperationCanceledException)
                {
                    Interlocked.Increment(ref cancelled);
                    Console.WriteLine($"  ❌ {job.FileName} cancelled");
                    return (ProcessedImage?)null;
                }
            })).ToList();

            await Task.WhenAll(tasks);
            sw.Stop();

            Console.WriteLine($"\n  Completed: {completed} | Cancelled: {cancelled} | " +
                              $"Total: {sw.ElapsedMilliseconds}ms\n");
        }

        // ── 5. Interlocked — thread-safe counters ─────────────────
        static void Demo5_Interlocked(List<ImageJob> jobs)
        {
            Console.WriteLine("=== 5. Interlocked vs lock ===\n");

            // Method 1: lock (safe but adds overhead)
            int counterLock = 0;
            object lockObj = new();
            var sw = Stopwatch.StartNew();

            Parallel.For(0, 10000, _ =>
            {
                lock (lockObj) counterLock++;
            });
            sw.Stop();
            Console.WriteLine($"  lock:        counter={counterLock,6}  time={sw.ElapsedMilliseconds}ms");

            // Method 2: Interlocked (atomic, faster)
            int counterAtomic = 0;
            sw.Restart();

            Parallel.For(0, 10000, _ =>
            {
                Interlocked.Increment(ref counterAtomic);
            });
            sw.Stop();
            Console.WriteLine($"  Interlocked: counter={counterAtomic,6}  time={sw.ElapsedMilliseconds}ms");

            // Stats accumulation with Interlocked
            long totalPixels = 0;
            Parallel.ForEach(jobs, job =>
            {
                long pixels = (long)job.Width * job.Height;
                Interlocked.Add(ref totalPixels, pixels);
            });
            Console.WriteLine($"\n  Total pixels across all images: {totalPixels:N0}");
            Console.WriteLine("\n✅ Multithreading demo complete!");
        }
    }
}
