// ============================================================
//  PROJECT 2 — Asynchronous Programming
//  Async News Feed Aggregator
//  Topics: async/await, Task.WhenAll, Exception handling in
//          async, IAsyncEnumerable, CancellationToken
// ============================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Asyncprogramming
{
    // ── Models ────────────────────────────────────────────────────
    record NewsArticle(int Id, string Title, string Source,
        string Category, DateTime PublishedAt, int WordCount);

    record FeedResult(string Source, List<NewsArticle> Articles,
        bool Success, string? Error = null, long FetchMs = 0);

    // ── Simulated news sources (each has different latency) ───────
    static class NewsSources
    {
        private static readonly Random _rng = new();

        // Simulates an HTTP fetch with variable latency
        public static async Task<List<NewsArticle>> FetchAsync(
            string source, int latencyMs, bool canFail = false,
            CancellationToken ct = default)
        {
            await Task.Delay(latencyMs, ct);   // simulate network I/O

            // Simulate occasional failures
            if (canFail && _rng.Next(0, 5) == 0)
                throw new HttpRequestException($"Connection to {source} timed out");

            // Generate fake articles
            string[] categories = { "Tech", "Finance", "Sports", "Science", "World" };
            var articles = new List<NewsArticle>();
            int count = _rng.Next(3, 8);
            for (int i = 1; i <= count; i++)
            {
                articles.Add(new NewsArticle(
                    Id: _rng.Next(1000, 9999),
                    Title: $"[{source}] Article #{i}: {categories[i % categories.Length]} Update",
                    Source: source,
                    Category: categories[i % categories.Length],
                    PublishedAt: DateTime.Now.AddMinutes(-_rng.Next(0, 120)),
                    WordCount: _rng.Next(200, 1500)
                ));
            }
            return articles;
        }

        // Async stream — simulates a real-time news ticker
        public static async IAsyncEnumerable<NewsArticle> StreamLiveAsync(
            string source, int count,
            [EnumeratorCancellation] CancellationToken ct = default)
        {
            var rng = new Random();
            string[] categories = { "Breaking", "Live", "Update", "Flash" };

            for (int i = 1; i <= count; i++)
            {
                await Task.Delay(rng.Next(100, 400), ct);  // variable delay between articles
                yield return new NewsArticle(
                    Id: rng.Next(10000, 99999),
                    Title: $"[LIVE] {source} — {categories[rng.Next(categories.Length)]} #{i}",
                    Source: source,
                    Category: "Live",
                    PublishedAt: DateTime.Now,
                    WordCount: rng.Next(50, 300)
                );
            }
        }
    }

    internal class AsyncDemo
    {
        static async Task Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║  Async Programming — News Aggregator 📰  ║");
            Console.WriteLine("╚══════════════════════════════════════════╝\n");

            await Demo1_SequentialVsParallel();
            await Demo2_ExceptionHandling();
            await Demo3_WhenAnyTimeout();
            await Demo4_AsyncStreams();
            await Demo5_RetryPattern();
        }

        // ── 1. Sequential vs Parallel fetching ───────────────────
        static async Task Demo1_SequentialVsParallel()
        {
            Console.WriteLine("=== 1. Sequential vs Parallel Fetching ===\n");

            var sources = new[] { "BBC", "Reuters", "AP News", "CNN", "The Guardian" };
            var latency = new[] { 400, 600, 300, 500, 350 };

            // SEQUENTIAL — each fetch waits for the previous
            var sw = Stopwatch.StartNew();
            var seqArticles = new List<NewsArticle>();
            foreach (var (source, ms) in sources.Zip(latency))
            {
                var articles = await NewsSources.FetchAsync(source, ms);
                seqArticles.AddRange(articles);
                Console.Write($"  {source} ");
            }
            sw.Stop();
            Console.WriteLine($"\n  ⏱️  Sequential: {sw.ElapsedMilliseconds}ms for {seqArticles.Count} articles\n");

            // PARALLEL — all fetches start simultaneously
            sw.Restart();
            var tasks = sources.Zip(latency)
                .Select(pair => FetchWithMetadataAsync(pair.First, pair.Second))
                .ToList();

            var results = await Task.WhenAll(tasks);
            sw.Stop();

            int totalArticles = results.Sum(r => r.Articles.Count);
            Console.WriteLine($"  ⚡ Parallel:    {sw.ElapsedMilliseconds}ms for {totalArticles} articles");
            Console.WriteLine($"  🚀 Speedup:     ~{latency.Sum() / sw.ElapsedMilliseconds}x faster\n");

            foreach (var r in results.OrderBy(r => r.FetchMs))
                Console.WriteLine($"    {r.Source,-15} {r.Articles.Count} articles in {r.FetchMs}ms");
            Console.WriteLine();
        }

        static async Task<FeedResult> FetchWithMetadataAsync(string source, int latencyMs)
        {
            var sw = Stopwatch.StartNew();
            Console.WriteLine($"  → Fetching {source}...");
            var articles = await NewsSources.FetchAsync(source, latencyMs);
            sw.Stop();
            Console.WriteLine($"  ← {source}: {articles.Count} articles in {sw.ElapsedMilliseconds}ms");
            return new FeedResult(source, articles, true, FetchMs: sw.ElapsedMilliseconds);
        }

        // ── 2. Exception handling in async ───────────────────────
        static async Task Demo2_ExceptionHandling()
        {
            Console.WriteLine("=== 2. Exception Handling in Async ===\n");

            var sources = new[]
            {
            ("TechCrunch", 200, true),    // (source, latency, canFail)
            ("HackerNews", 300, true),
            ("Wired",      250, true),
            ("Ars Technica",180, false),
        };

            // ── Handle each individually ──────────────────────────
            Console.WriteLine("  Per-source error handling:");
            var safeResults = await Task.WhenAll(sources.Select(async s =>
            {
                try
                {
                    var articles = await NewsSources.FetchAsync(s.Item1, s.Item2, s.Item3);
                    return new FeedResult(s.Item1, articles, true);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"  ⚠️  {s.Item1} failed: {ex.Message}");
                    return new FeedResult(s.Item1, new(), false, ex.Message);
                }
            }));

            int ok = safeResults.Count(r => r.Success);
            int failed = safeResults.Count(r => !r.Success);
            Console.WriteLine($"\n  Results: {ok} succeeded, {failed} failed");
            Console.WriteLine($"  Total articles from successful sources: " +
                              $"{safeResults.Where(r => r.Success).Sum(r => r.Articles.Count)}\n");

            // ── WhenAll with AggregateException ───────────────────
            Console.WriteLine("  AggregateException from Task.WhenAll:");
            var failingTasks = new[]
            {
            Task.Run(async () => { await Task.Delay(50);
                throw new InvalidOperationException("Source A error"); return 0; }),
            Task.Run(async () => { await Task.Delay(80);
                throw new TimeoutException("Source B timed out"); return 0; }),
            Task.Run(async () => { await Task.Delay(30); return 42; }),
        };

            Task<int[]> combined = Task.WhenAll(failingTasks);
            try
            {
                await combined;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  First exception: {ex.GetType().Name}: {ex.Message}");
                // Get ALL exceptions
                if (combined.Exception?.InnerExceptions is { } all)
                    foreach (var inner in all)
                        Console.WriteLine($"    → {inner.GetType().Name}: {inner.Message}");
            }
            Console.WriteLine();
        }

        // ── 3. WhenAny + timeout pattern ─────────────────────────
        static async Task Demo3_WhenAnyTimeout()
        {
            Console.WriteLine("=== 3. WhenAny + Timeout Pattern ===\n");

            var slowSources = new[]
            {
            ("Slow Feed 1", 800),
            ("Fast Feed",   150),
            ("Slow Feed 2", 600),
        };

            Console.WriteLine("  Racing sources — first wins:");
            var raceTasks = slowSources
                .Select(s => Task.Run(async () =>
                {
                    var articles = await NewsSources.FetchAsync(s.Item1, s.Item2);
                    Console.WriteLine($"  🏁 {s.Item1} finished ({s.Item2}ms)");
                    return (s.Item1, articles);
                }))
                .ToList();

            var winner = await Task.WhenAny(raceTasks);
            var (winnerName, winnerArticles) = await winner;
            Console.WriteLine($"  🥇 First result from: {winnerName} ({winnerArticles.Count} articles)");

            // Timeout pattern
            Console.WriteLine("\n  Timeout pattern (2s limit on slow source):");
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            try
            {
                var slowTask = NewsSources.FetchAsync("Very Slow API", 3000, ct: cts.Token);
                var timeoutTask = Task.Delay(Timeout.Infinite, cts.Token);

                var first = await Task.WhenAny(slowTask, timeoutTask);
                if (first == slowTask)
                    Console.WriteLine($"  ✅ Got articles before timeout");
                else
                    Console.WriteLine($"  ⏱️  Timed out — using cached data");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"  ⏱️  Request cancelled (timed out)");
            }
            Console.WriteLine();
        }

        // ── 4. Async Streams ─────────────────────────────────────
        static async Task Demo4_AsyncStreams()
        {
            Console.WriteLine("=== 4. Async Streams (IAsyncEnumerable) ===\n");

            Console.WriteLine("  Live news ticker from 2 sources simultaneously:\n");

            using var cts = new CancellationTokenSource();

            // Stream from one source
            int articleCount = 0;
            try
            {
                await foreach (var article in
                    NewsSources.StreamLiveAsync("Reuters Live", 6, cts.Token))
                {
                    articleCount++;
                    string age = (DateTime.Now - article.PublishedAt).TotalSeconds < 1
                        ? "just now"
                        : $"{(DateTime.Now - article.PublishedAt).TotalSeconds:F0}s ago";

                    Console.WriteLine($"  📡 [{age,-8}] {article.Title}");

                    // Stop after 4 articles
                    if (articleCount >= 4)
                    {
                        Console.WriteLine("  ⛔ Stopping stream after 4 articles...");
                        cts.Cancel();
                        break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Expected — we cancelled
            }

            // Paginated fetch as stream
            Console.WriteLine("\n  Streaming paginated results:");
            await foreach (var article in StreamPaginatedAsync(maxArticles: 5))
                Console.WriteLine($"  📄 [{article.Source,-15}] {article.Title[..Math.Min(50, article.Title.Length)]}...");

            Console.WriteLine();
        }

        // Stream that fetches multiple pages internally
        static async IAsyncEnumerable<NewsArticle> StreamPaginatedAsync(
            int maxArticles = 10,
            [EnumeratorCancellation] CancellationToken ct = default)
        {
            int emitted = 0;
            string[] pages = { "TechCrunch", "Wired", "Ars Technica" };

            foreach (var source in pages)
            {
                if (ct.IsCancellationRequested || emitted >= maxArticles) yield break;

                var batch = await NewsSources.FetchAsync(source, 100, ct: ct);
                foreach (var article in batch)
                {
                    if (emitted >= maxArticles) yield break;
                    yield return article;
                    emitted++;
                }
            }
        }

        // ── 5. Retry pattern ─────────────────────────────────────
        static async Task Demo5_RetryPattern()
        {
            Console.WriteLine("=== 5. Retry with Exponential Backoff ===\n");

            async Task<List<NewsArticle>> FetchWithRetry(
                string source, int maxRetries = 3)
            {
                for (int attempt = 1; attempt <= maxRetries; attempt++)
                {
                    try
                    {
                        Console.WriteLine($"  Attempt {attempt}/{maxRetries}: {source}");
                        // canFail=true — will randomly fail
                        return await NewsSources.FetchAsync(source, 100, canFail: true);
                    }
                    catch (HttpRequestException ex) when (attempt < maxRetries)
                    {
                        int delay = attempt * 200;   // exponential backoff
                        Console.WriteLine($"  ⚠️  Failed: {ex.Message}. Retrying in {delay}ms...");
                        await Task.Delay(delay);
                    }
                }
                // Final attempt — let exception propagate
                return await NewsSources.FetchAsync(source, 100, canFail: true);
            }

            try
            {
                var articles = await FetchWithRetry("Unreliable Source");
                Console.WriteLine($"  ✅ Eventually succeeded: {articles.Count} articles");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"  ❌ All retries exhausted: {ex.Message}");
            }

            Console.WriteLine("\n✅ Async Programming demo complete!");
        }
    }
}
