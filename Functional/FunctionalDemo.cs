// ============================================================
//  PROJECT 1 — Functional Programming
//  E-Commerce Analytics Engine
//  Topics: Extension Methods, Lambda, LINQ, Pattern Matching,
//          Pure Functions, Higher-Order Functions
// ============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Functional
{
    // ── Models ────────────────────────────────────────────────────
    public record Product(int Id, string Name, string Category, decimal Price, int Stock);
    public record Order(int Id, string Customer, List<(Product Product, int Qty)> Items,
        DateTime Date, string Status);


    // ── Extension Methods ─────────────────────────────────────────
    public static class ProductExtensions
    {
        public static bool IsInStock(this Product p)     => p.Stock > 0;
        public static bool IsLowStock(this Product p)    => p.Stock > 0 && p.Stock < 5;
        public static bool IsPremium(this Product p)     => p.Price > 500m;
        public static string StockStatus(this Product p) =>
            p.Stock == 0 ? "OUT OF STOCK" :
            p.Stock < 5 ? $"LOW ({p.Stock})" :
                           $"OK  ({p.Stock})";

    }

    public static class OrderExtensions
    {
        public static decimal Total(this Order o) =>
            o.Items.Sum(i => i.Product.Price * i.Qty);

        public static bool IsComplete(this Order o) => o.Status == "Completed";
        public static bool IsPending(this Order o) => o.Status == "Pending";
        public static bool IsHighValue(this Order o) => o.Total() > 500m;
        public static int ItemCount(this Order o) => o.Items.Sum(i => i.Qty);

    }

    public static class DecimalExtensions
    {
        public static string AsCurrency(this decimal d) => $"{d:C2}";
        public static string AsPercent(this decimal d) => $"{d:F1}%";
    }

    // ── Pure Functions (no side effects, same input = same output) ──
    public static class PureFunctions
    {
        public static decimal CalcDiscount(decimal price, int qty) =>
            qty >= 10 ? price * 0.15m :
            qty >= 5 ? price * 0.10m :
            qty >= 3 ? price * 0.05m : 0m;

        public static string ClassifyOrder(decimal total) => total switch
        {
            > 1000m => "Platinum",
            > 500m => "Gold",
            > 200m => "Silver",
            _ => "Bronze"
        };

        public static decimal ApplyTax(decimal amount, string region) => region switch
        {
            "US-CA" => amount * 1.0725m,
            "US-NY" => amount * 1.08m,
            "UK" => amount * 1.20m,
            "EU" => amount * 1.19m,
            _ => amount * 1.05m
        };
    }


    // ── Higher-Order Functions ────────────────────────────────────
    public static class Analytics
    {
        // Takes a function parameter
        public static Dictionary<string, decimal> GroupAndSum<T>(
            IEnumerable<T> items,
            Func<T, string> groupKey,
            Func<T, decimal> valueSelector) =>
            items
                .GroupBy(groupKey)
                .ToDictionary(g => g.Key, g => g.Sum(valueSelector));

        // Returns a function (factory)
        public static Func<Order, bool> OrderFilter(
            decimal? minTotal = null,
            string? status = null,
            string? customer = null) =>
            o => (minTotal == null || o.Total() >= minTotal)
              && (status == null || o.Status == status)
              && (customer == null || o.Customer == customer);

        // Function composition
        public static Func<T, R2> Compose<T, R1, R2>(
            Func<T, R1> first,
            Func<R1, R2> second) => x => second(first(x));
    }

    // ── Main Demo ─────────────────────────────────────────────────
    internal class FunctionalDemo
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║  Functional Programming — E-Commerce 🛒  ║");
            Console.WriteLine("╚══════════════════════════════════════════╝\n");

            // Sample data
            var products = new List<Product>
            {
                new(1, "Laptop Pro",    "Electronics", 1299m, 8),
                new(2, "Wireless Mouse","Electronics",   49m, 23),
                new(3, "USB Hub",       "Electronics",   29m, 4),  // low stock
                new(4, "Standing Desk", "Furniture",    799m, 0),  // out of stock
                new(5, "Ergonomic Chair","Furniture",   449m, 6),
                new(6, "Notebook",      "Stationery",    12m, 50),
                new(7, "Mechanical KB", "Electronics",  149m, 11),
                new(8, "Monitor 27\"",  "Electronics",  399m, 3),  // low stock
            };

            var orders = new List<Order>
            {
                new(1001, "Alice",   new() { (products[0],1),(products[1],2) }, DateTime.Now.AddDays(-5),  "Completed"),
                new(1002, "Bob",     new() { (products[4],1),(products[5],3) }, DateTime.Now.AddDays(-3),  "Completed"),
                new(1003, "Alice",   new() { (products[6],1),(products[7],1) }, DateTime.Now.AddDays(-1),  "Pending"),
                new(1004, "Charlie", new() { (products[1],5),(products[2],2) }, DateTime.Now.AddDays(-7),  "Completed"),
                new(1005, "Diana",   new() { (products[0],2),(products[6],1) }, DateTime.Now,              "Processing"),
                new(1006, "Bob",     new() { (products[3],1) },                 DateTime.Now.AddDays(-10), "Cancelled"),
                new(1007, "Eve",     new() { (products[5],10),(products[1],1)}, DateTime.Now.AddDays(-2),  "Completed"),
            };

            // ── 1. Extension Methods ──────────────────────────────
            Console.WriteLine("=== 1. Extension Methods ===\n");
            Console.WriteLine("  Product stock status:");
            foreach (var p in products)
                Console.WriteLine($"    {p.Name,-20} {p.StockStatus(),-14} {(p.IsPremium() ? "💎 Premium" : "")}");

            Console.WriteLine($"\n  High-value orders:");
            foreach (var o in orders.Where(o => o.IsHighValue()))
                Console.WriteLine($"    #{o.Id} {o.Customer,-10} {o.Total().AsCurrency()}  [{o.Status}]");

            
            // ── 2. Lambda Expressions ─────────────────────────────
            Console.WriteLine("\n=== 2. Lambda Expressions ===\n");

            Func<decimal, decimal, decimal> applyDiscount = (price, pct) => price * (1 - pct);
            Func<string, string> tagBadge = name => $"[{name.ToUpper()}]";
            Predicate<Product> needsReorder = p => p.Stock < 5;

            Console.WriteLine("  Discount calculator:");
            Console.WriteLine($"    Laptop   at 15% off: {applyDiscount(1299m, 0.15m).AsCurrency()}");
            Console.WriteLine($"    Monitor  at 10% off: {applyDiscount(399m, 0.10m).AsCurrency()}");

            Console.WriteLine("\n  Products needing reorder:");
            products.FindAll(needsReorder)
                    .ForEach(p => Console.WriteLine($"    {tagBadge("REORDER")} {p.Name} (stock: {p.Stock})"));


            // ── 3. LINQ ───────────────────────────────────────────
            Console.WriteLine("\n=== 3. LINQ ===\n");

            // Top 3 most expensive in-stock products
            var top3 = products
                .Where(p => p.IsInStock())
                .OrderByDescending(p => p.Price)
                .Take(3)
                .Select(p => $"  {p.Name,-22} {p.Price.AsCurrency()}");

            Console.WriteLine("  Top 3 most expensive (in stock):");
            foreach (var item in top3) Console.WriteLine(item);

            // Revenue by category
            Console.WriteLine("\n  Revenue by category:");
            var revByCat = orders
                .Where(o => o.IsComplete())
                .SelectMany(o => o.Items)
                .GroupBy(i => i.Product.Category)
                .Select(g => new {
                    Category = g.Key,
                    Revenue = g.Sum(i => i.Product.Price * i.Qty),
                    Units = g.Sum(i => i.Qty)
                })
                .OrderByDescending(x => x.Revenue);

            foreach (var r in revByCat)
                Console.WriteLine($"    {r.Category,-14} Revenue: {r.Revenue.AsCurrency()}  Units: {r.Units}");

            // Customer stats
            Console.WriteLine("\n  Customer spending (completed orders):");
            var custStats = orders
                .Where(o => o.IsComplete())
                .GroupBy(o => o.Customer)
                .Select(g => new {
                    Customer = g.Key,
                    Orders = g.Count(),
                    Total = g.Sum(o => o.Total()),
                    Avg = g.Average(o => (double)o.Total())
                })
                .OrderByDescending(x => x.Total);

            foreach (var c in custStats)
                Console.WriteLine($"    {c.Customer,-10} Orders: {c.Orders}  Total: {c.Total.AsCurrency()}  Avg: {((decimal)c.Avg).AsCurrency()}");


            // ── 4. Pattern Matching ───────────────────────────────
            Console.WriteLine("\n=== 4. Pattern Matching ===\n");

            Console.WriteLine("  Order tier classification:");
            foreach (var o in orders.Where(o => o.IsComplete()))
            {
                string tier = PureFunctions.ClassifyOrder(o.Total());
                string icon = tier switch
                {
                    "Platinum" => "🏆",
                    "Gold" => "🥇",
                    "Silver" => "🥈",
                    _ => "🥉"
                };
                Console.WriteLine($"    {icon} #{o.Id} {o.Customer,-10} {o.Total().AsCurrency(),-10} → {tier}");
            }

            // Property pattern on orders
            Console.WriteLine("\n  Order priority (property pattern):");
            foreach (var o in orders)
            {
                string priority = o switch
                {
                    { Status: "Pending", } when o.IsHighValue() => "🔴 HIGH",
                    { Status: "Pending" } => "🟡 NORMAL",
                    { Status: "Processing" } => "🔵 ACTIVE",
                    { Status: "Completed" } => "✅ DONE",
                    { Status: "Cancelled" } => "❌ CANCELLED",
                    _ => "❓ UNKNOWN"
                };
                Console.WriteLine($"    {priority} — #{o.Id} {o.Customer}");
            }


            // ── 5. Higher-Order Functions ─────────────────────────
            Console.WriteLine("\n=== 5. Higher-Order Functions ===\n");

            // GroupAndSum
            var revenueMap = Analytics.GroupAndSum(
                orders.Where(o => o.IsComplete()).SelectMany(o => o.Items),
                i => i.Product.Category,
                i => i.Product.Price * i.Qty);

            Console.WriteLine("  GroupAndSum (category → revenue):");
            foreach (var (cat, rev) in revenueMap.OrderByDescending(kv => kv.Value))
                Console.WriteLine($"    {cat,-14}: {rev.AsCurrency()}");

            // OrderFilter factory
            var aliceCompleted = Analytics.OrderFilter(status: "Completed", customer: "Alice");
            Console.WriteLine($"\n  Alice's completed orders:");
            orders.Where(aliceCompleted)
                  .ToList()
                  .ForEach(o => Console.WriteLine($"    #{o.Id} {o.Date:MM-dd} {o.Total().AsCurrency()}"));

            // Function composition
            Func<Order, decimal> getTotal = o => o.Total();
            Func<decimal, string> classifyFn = PureFunctions.ClassifyOrder;
            Func<Order, string> getTier = Analytics.Compose(getTotal, classifyFn);

            Console.WriteLine("\n  Composed function (Order → Tier):");
            orders.Where(o => o.IsComplete())
                  .ToList()
                  .ForEach(o => Console.WriteLine($"    #{o.Id} → {getTier(o)}"));

            // Pure function demo
            Console.WriteLine("\n=== 6. Pure Functions ===\n");
            Console.WriteLine("  Discount table:");
            foreach (int qty in new[] { 1, 3, 5, 10 })
            {
                decimal discount = PureFunctions.CalcDiscount(100m, qty);
                Console.WriteLine($"    Qty {qty,2}: discount = {discount.AsCurrency()} off {100m.AsCurrency()}");
            }

            Console.WriteLine("\n  Tax by region:");
            decimal basePrice = 500m;
            foreach (string region in new[] { "US-CA", "US-NY", "UK", "EU", "AU" })
            {
                decimal taxed = PureFunctions.ApplyTax(basePrice, region);
                Console.WriteLine($"    {region,-6}: {basePrice.AsCurrency()} → {taxed.AsCurrency()}");
            }

            Console.WriteLine("\n✅ Functional Programming demo complete!");
            Console.ReadLine();
        }

    }
}
