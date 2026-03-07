// ============================================================
//  PROJECT 3 — String Interpolation ($) & StringBuilder
//  Invoice / Receipt Generator
//  Topics: $"" interpolation, format specifiers (:C2 :F1),
//          alignment padding, StringBuilder vs string in loops
// ============================================================

using System;
using System.Text;

class InvoiceGenerator
{
    // Simple product struct (we'll learn classes later!)
    struct Product
    {
        public string Name;
        public int Quantity;
        public double UnitPrice;
        public Product(string name, int qty, double price)
        { Name = name; Quantity = qty; UnitPrice = price; }
        public double Total => Quantity * UnitPrice;
    }

    static void Main()
    {
        Console.WriteLine("╔══════════════════════════════╗");
        Console.WriteLine("║   Invoice Generator 🧾       ║");
        Console.WriteLine("╚══════════════════════════════╝\n");

        // ── Customer info ─────────────────────────────────────
        string customerName = "John Doe";
        string customerEmail = "john@example.com";
        DateTime orderDate = new DateTime(2026, 3, 7);
        int invoiceNumber = 1042;

        // ── Products array ────────────────────────────────────
        Product[] products = {
            new Product("C# Programming Book",  2,  29.99),
            new Product("Mechanical Keyboard",   1, 149.99),
            new Product("USB-C Hub",             3,  24.99),
            new Product("Monitor Stand",         1,  49.99),
            new Product("Notebook (pack of 3)",  2,   8.99),
        };

        double taxRate = 0.08;  // 8%

        // ── 1. String Interpolation: Header ───────────────────
        Console.WriteLine("=== INTERPOLATION EXAMPLES ===\n");

        // Basic variable embedding
        Console.WriteLine($"Customer  : {customerName}");
        Console.WriteLine($"Email     : {customerEmail}");

        // Date format specifier
        Console.WriteLine($"Date      : {orderDate:MMMM dd, yyyy}");        // March 07, 2026
        Console.WriteLine($"Date short: {orderDate:dd/MM/yyyy}");           // 07/03/2026

        // Number padding (right-align invoice number in 6 digits)
        Console.WriteLine($"Invoice # : INV-{invoiceNumber:D6}");           // INV-001042

        // ── 2. Column alignment with interpolation ────────────
        Console.WriteLine("\n=== ALIGNED TABLE (Interpolation) ===");
        Console.WriteLine($"{"Product",-30} {"Qty",4} {"Unit Price",12} {"Total",12}");
        Console.WriteLine(new string('-', 62));

        foreach (Product p in products)
        {
            // ,-30 = left-align in 30 chars    ,4 = right-align in 4    :C2 = currency
            Console.WriteLine($"{p.Name,-30} {p.Quantity,4} {p.UnitPrice,12:C2} {p.Total,12:C2}");
        }

        // ── 3. Compute totals ─────────────────────────────────
        double subtotal = 0;
        foreach (Product p in products) subtotal += p.Total;
        double tax = subtotal * taxRate;
        double total = subtotal + tax;

        Console.WriteLine(new string('-', 62));
        Console.WriteLine($"{"Subtotal",-46} {subtotal,12:C2}");
        Console.WriteLine($"{"Tax (8%)",-46} {tax,12:C2}");
        Console.WriteLine($"{"TOTAL",-46} {total,12:C2}");

        // ── 4. Conditional expression inside interpolation ────
        string paymentStatus = total > 200 ? "⚠️  Large order — requires approval"
                                           : "✅ Auto-approved";
        Console.WriteLine($"\nStatus: {paymentStatus}");

        // ── 5. StringBuilder: build the full receipt text ─────
        Console.WriteLine("\n=== BUILDING RECEIPT WITH StringBuilder ===\n");

        // Why StringBuilder? We're appending many lines in a loop —
        // using string += would create a new string object each time!
        var sb = new StringBuilder();

        sb.AppendLine("╔════════════════════════════════════════════════════════════╗");
        sb.AppendLine($"║{"OFFICIAL INVOICE",36}{"",26}║");
        sb.AppendLine("╠════════════════════════════════════════════════════════════╣");
        sb.AppendLine($"║  Invoice No : INV-{invoiceNumber:D6}{"",37}║");
        sb.AppendLine($"║  Date       : {orderDate:MMMM dd, yyyy}{"",38}║");
        sb.AppendLine($"║  Customer   : {customerName,-45}║");
        sb.AppendLine($"║  Email      : {customerEmail,-45}║");
        sb.AppendLine("╠════════════════════════════════════════════════════════════╣");
        sb.AppendLine($"║  {"ITEM",-28} {"QTY",4} {"UNIT",10} {"TOTAL",10}     ║");
        sb.AppendLine("╠════════════════════════════════════════════════════════════╣");

        // Loop — the PERFECT use case for StringBuilder
        foreach (Product p in products)
        {
            sb.AppendLine($"║  {p.Name,-28} {p.Quantity,4} {p.UnitPrice,10:C2} {p.Total,10:C2}     ║");
        }

        sb.AppendLine("╠════════════════════════════════════════════════════════════╣");
        sb.AppendLine($"║  {"Subtotal",-43} {subtotal,10:C2}     ║");
        sb.AppendLine($"║  {"Tax (8%)",-43} {tax,10:C2}     ║");
        sb.AppendLine($"║  {"TOTAL DUE",-43} {total,10:C2}     ║");
        sb.AppendLine("╠════════════════════════════════════════════════════════════╣");
        sb.AppendLine($"║  Thank you for your business, {customerName}!{"",20}║");
        sb.AppendLine("╚════════════════════════════════════════════════════════════╝");

        // Build the full string ONCE at the end — this is the key benefit!
        string receipt = sb.ToString();
        Console.WriteLine(receipt);

        // ── 6. StringBuilder modification methods ─────────────
        Console.WriteLine("=== StringBuilder Manipulation ===");
        var demo = new StringBuilder("Hello World");
        Console.WriteLine($"Original  : {demo}");

        demo.Replace("World", "C#");
        Console.WriteLine($"Replace   : {demo}");

        demo.Insert(5, " Beautiful");
        Console.WriteLine($"Insert    : {demo}");

        demo.Remove(5, 10);          // remove " Beautiful"
        Console.WriteLine($"Remove    : {demo}");

        Console.WriteLine($"Length    : {demo.Length}");

        demo.Clear();
        Console.WriteLine($"After Clear, Length: {demo.Length}");

        // ── 7. Performance comparison (demonstration) ─────────
        Console.WriteLine("\n=== Performance Demo ===");
        int iterations = 5000;

        // string += in a loop (SLOW)
        var sw1 = System.Diagnostics.Stopwatch.StartNew();
        string slow = "";
        for (int i = 0; i < iterations; i++) slow += "x";
        sw1.Stop();

        // StringBuilder (FAST)
        var sw2 = System.Diagnostics.Stopwatch.StartNew();
        var fast = new StringBuilder();
        for (int i = 0; i < iterations; i++) fast.Append("x");
        string fastResult = fast.ToString();
        sw2.Stop();

        Console.WriteLine($"string +=      ({iterations} iters): {sw1.ElapsedMilliseconds} ms");
        Console.WriteLine($"StringBuilder  ({iterations} iters): {sw2.ElapsedMilliseconds} ms");
        Console.WriteLine("StringBuilder is significantly faster for repeated concatenation!");

        Console.WriteLine("\n✅ Program complete!");
        Console.ReadLine();
    }
}