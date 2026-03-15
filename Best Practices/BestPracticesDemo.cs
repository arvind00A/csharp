// ============================================================
//  PROJECT 1 — C# Best Practices
//  Clean Invoice Processing System
//  Topics: Naming conventions, Null safety, Guard clauses,
//          IDisposable, StringBuilder, performance tips
// ============================================================

using System;
using System.Collections.Generic;
using System.Text;


// ── namespace (file-scoped, C# 10+) ──────────────────────────
namespace Best_Practices;

// ══════════════════════════════════════════════════════════════
//  MODELS — PascalCase, meaningful names, nullable annotations
// ══════════════════════════════════════════════════════════════

public enum InvoiceStatus { Draft, Pending, Paid, Overdue, Cancelled }

public record Address(
    string Street,
    string City,
    string PostalCode,
    string Country = "US");

public record LineItem(
    int ProductId,
    string Description,
    decimal UnitPrice,
    int Quantity)
{
    public decimal Subtotal => UnitPrice * Quantity;
}

public class Invoice
{
    // PascalCase properties
    public int InvoiceId { get; private set; }   // set by repository
    public string InvoiceNumber { get; init; } = "";
    public string CustomerName { get; set; } = "";
    public string? CustomerEmail { get; set; }   // nullable — optional
    public Address? BillingAddress { get; set; }
    public List<LineItem> LineItems { get; init; } = new();
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
    public decimal TaxRate { get; set; } = DefaultTaxRate;

    // Named constants — no magic numbers
    private const decimal DefaultTaxRate = 0.10m;   // 10% VAT
    private const decimal OverdueThresholdDays = 30;

    // Computed properties
    public decimal Subtotal => LineItems.Sum(li => li.Subtotal);
    public decimal TaxAmount => Subtotal * TaxRate;
    public decimal Total => Subtotal + TaxAmount;
    public bool IsOverdue =>
        DueDate.HasValue && DueDate.Value < DateTime.UtcNow && Status != InvoiceStatus.Paid;

    // Internal: allows the repository to assign an ID after creation
    internal void InternalSetId(int id) => InvoiceId = id;
}

// ── DTO — separate from domain model ─────────────────────────
public record CreateInvoiceDto(
    string CustomerName,
    string? CustomerEmail,
    List<LineItem> LineItems,
    decimal TaxRate = 0.10m);

// ══════════════════════════════════════════════════════════════
//  REPOSITORY — IDisposable pattern, dependency injection
// ══════════════════════════════════════════════════════════════

public interface IInvoiceRepository
{
    Invoice? GetById(int id);
    List<Invoice> GetAll();
    List<Invoice> GetByStatus(InvoiceStatus status);
    void Save(Invoice invoice);
    void Delete(int id);
}

// In-memory implementation (no real DB needed for demo)
public class InMemoryInvoiceRepository : IInvoiceRepository
{
    private readonly Dictionary<int, Invoice> _store = new();
    private int _nextId = 1;

    public Invoice? GetById(int id) =>
        _store.TryGetValue(id, out var inv) ? inv : null;

    public List<Invoice> GetAll() => _store.Values.ToList();

    public List<Invoice> GetByStatus(InvoiceStatus status) =>
        _store.Values.Where(i => i.Status == status).ToList();

    public void Save(Invoice invoice)
    {
        // Assign ID if new (Invoice is a class, not a record — use direct property set)
        if (invoice.InvoiceId == 0)
            invoice.InternalSetId(_nextId++);
        _store[invoice.InvoiceId] = invoice;
    }

    public void Delete(int id) => _store.Remove(id);
}

// ══════════════════════════════════════════════════════════════
//  SERVICE — guard clauses, null safety, clean code
// ══════════════════════════════════════════════════════════════

public class InvoiceService
{
    // _camelCase private fields
    private readonly IInvoiceRepository _repository;
    private readonly IInvoiceExporter _exporter;
    private int _invoiceCounter = 1000;

    // Dependency Injection via constructor
    public InvoiceService(IInvoiceRepository repository, IInvoiceExporter exporter)
    {
        // ArgumentNullException guards (C# 10+ style)
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(exporter);
        _repository = repository;
        _exporter = exporter;
    }

    // ── Create ────────────────────────────────────────────────
    public Invoice Create(CreateInvoiceDto dto)
    {
        // Guard clauses — validate first, happy path at bottom
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentException.ThrowIfNullOrWhiteSpace(dto.CustomerName);
        if (dto.LineItems is null || dto.LineItems.Count == 0)
            throw new ArgumentException("Invoice must have at least one line item", nameof(dto));
        if (dto.TaxRate is < 0 or > 1)
            throw new ArgumentOutOfRangeException(nameof(dto.TaxRate), "Tax rate must be 0–1");

        // Validate each line item
        foreach (var item in dto.LineItems)
        {
            if (item.UnitPrice < 0) throw new ArgumentException($"UnitPrice cannot be negative for '{item.Description}'");
            if (item.Quantity < 1) throw new ArgumentException($"Quantity must be ≥ 1 for '{item.Description}'");
        }

        // Happy path
        var invoice = new Invoice
        {
            InvoiceNumber = GenerateInvoiceNumber(),
            CustomerName = dto.CustomerName,
            CustomerEmail = dto.CustomerEmail,
            LineItems = dto.LineItems,
            TaxRate = dto.TaxRate,
            DueDate = DateTime.UtcNow.AddDays(30),
            Status = InvoiceStatus.Pending
        };

        _repository.Save(invoice);
        Console.WriteLine($"  ✅ Created: {invoice.InvoiceNumber} for {invoice.CustomerName}");
        return invoice;
    }

    // ── Mark as Paid ──────────────────────────────────────────
    public void MarkAsPaid(int invoiceId)
    {
        var invoice = _repository.GetById(invoiceId)
            ?? throw new InvalidOperationException($"Invoice #{invoiceId} not found");

        if (invoice.Status == InvoiceStatus.Paid)
            throw new InvalidOperationException($"Invoice {invoice.InvoiceNumber} is already paid");

        if (invoice.Status == InvoiceStatus.Cancelled)
            throw new InvalidOperationException($"Cannot pay a cancelled invoice");

        invoice.Status = InvoiceStatus.Paid;   // direct mutation — Invoice is a class
        _repository.Save(invoice);
        Console.WriteLine($"  💰 Paid: {invoice.InvoiceNumber} ({invoice.Total:C2})");
    }

    // ── Get report ────────────────────────────────────────────
    public string GenerateReport()
    {
        var invoices = _repository.GetAll();
        // StringBuilder for string building — NOT += in loops
        return _exporter.Export(invoices);
    }

    // ── Get overdue ───────────────────────────────────────────
    public List<Invoice> GetOverdue()
    {
        var all = _repository.GetAll();
        var overdue = all.Where(i => i.IsOverdue).ToList();

        // Mark them as overdue in storage
        foreach (var inv in overdue)
        {
            inv.Status = InvoiceStatus.Overdue;   // direct mutation — Invoice is a class
            _repository.Save(inv);
        }

        return overdue;
    }

    // Named constant instead of magic string
    private string GenerateInvoiceNumber() => $"INV-{DateTime.UtcNow:yyyyMM}-{++_invoiceCounter}";
}

// ══════════════════════════════════════════════════════════════
//  EXPORTER — IDisposable pattern, StringBuilder
// ══════════════════════════════════════════════════════════════

public interface IInvoiceExporter
{
    string Export(List<Invoice> invoices);
}

// ── Text exporter using StringBuilder (best practice for building strings)
public class TextInvoiceExporter : IInvoiceExporter
{
    public string Export(List<Invoice> invoices)
    {
        // ✅ StringBuilder — NOT string += in loop
        var sb = new StringBuilder();
        sb.AppendLine("═══════════════════════════════════════════");
        sb.AppendLine("           INVOICE REPORT");
        sb.AppendLine($"           Generated: {DateTime.Now:yyyy-MM-dd HH:mm}");
        sb.AppendLine("═══════════════════════════════════════════");

        // Group by status
        var byStatus = invoices
            .GroupBy(i => i.Status)
            .OrderBy(g => g.Key);

        foreach (var group in byStatus)
        {
            sb.AppendLine($"\n  ── {group.Key} ({group.Count()}) ──");
            foreach (var inv in group.OrderByDescending(i => i.Total))
            {
                // Null-conditional operator for optional fields
                string email = inv.CustomerEmail is not null
                    ? $" <{inv.CustomerEmail}>" : "";
                string due = inv.DueDate?.ToString("MMM dd") ?? "—";
                sb.AppendLine($"  {inv.InvoiceNumber,-20} {inv.CustomerName,-20}{email}");
                sb.AppendLine($"  {"",20} Total: {inv.Total,10:C2}  Due: {due}");
            }
        }

        // Summary using named constants would go here
        decimal totalPaid = invoices.Where(i => i.Status == InvoiceStatus.Paid).Sum(i => i.Total);
        decimal totalPending = invoices.Where(i => i.Status == InvoiceStatus.Pending).Sum(i => i.Total);
        decimal totalOverdue = invoices.Where(i => i.Status == InvoiceStatus.Overdue).Sum(i => i.Total);

        sb.AppendLine("\n═══════════════════════════════════════════");
        sb.AppendLine($"  Paid:     {totalPaid,10:C2}");
        sb.AppendLine($"  Pending:  {totalPending,10:C2}");
        sb.AppendLine($"  Overdue:  {totalOverdue,10:C2}");
        sb.AppendLine($"  TOTAL:    {invoices.Sum(i => i.Total),10:C2}");
        sb.AppendLine("═══════════════════════════════════════════");

        return sb.ToString();
    }
}

// ── File exporter — IDisposable via using ─────────────────────
public class FileInvoiceExporter : IInvoiceExporter, IDisposable
{
    private readonly string _outputPath;
    private bool _disposed = false;

    public FileInvoiceExporter(string outputPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(outputPath);
        _outputPath = outputPath;
    }

    public string Export(List<Invoice> invoices)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(FileInvoiceExporter));

        var content = new TextInvoiceExporter().Export(invoices);
        // ✅ using ensures StreamWriter is always closed
        using var writer = new StreamWriter(_outputPath, append: false);
        writer.Write(content);
        Console.WriteLine($"  📄 Report written to: {_outputPath}");
        return content;
    }

    // IDisposable pattern
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        // FileInvoiceExporter holds no open handles, but demonstrates the pattern
        Console.WriteLine("  🗑️  FileInvoiceExporter disposed");
        _disposed = true;
    }
}

// ══════════════════════════════════════════════════════════════
//  DEMO
// ══════════════════════════════════════════════════════════════


internal class BestPracticesDemo
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║  Best Practices — Invoice System 📋      ║");
        Console.WriteLine("╚══════════════════════════════════════════╝\n");

        // Dependency injection — passing implementations via interface
        var repository = new InMemoryInvoiceRepository();
        var exporter = new TextInvoiceExporter();
        var service = new InvoiceService(repository, exporter);

        Console.WriteLine("=== Creating Invoices ===\n");
        var inv1 = service.Create(new CreateInvoiceDto(
            "Alice Technologies",
            "alice@techco.com",
            new() {
                new(101, "Cloud Hosting (1yr)", 299.00m, 1),
                new(202, "SSL Certificate",      49.00m, 2),
            }
        ));

        var inv2 = service.Create(new CreateInvoiceDto(
            "Bob's Bakery",
            null,   // nullable — email is optional
            new() {
                new(301, "POS Software License",  149.99m, 1),
                new(302, "Support Package",        79.99m, 12),
            }
        ));

        var inv3 = service.Create(new CreateInvoiceDto(
            "Charlie Consulting",
            "charlie@consult.io",
            new() {
                new(401, "Strategy Workshop",  1500.00m, 2),
                new(402, "Report Writing",      800.00m, 3),
            }
        ));

        var inv4 = service.Create(new CreateInvoiceDto(
            "Diana Design",
            "d@design.co",
            new() {
                new(501, "Brand Identity Package", 2200.00m, 1),
            }
        ));

        Console.WriteLine("\n=== Paying Invoices ===\n");
        service.MarkAsPaid(inv1.InvoiceId);
        service.MarkAsPaid(inv3.InvoiceId);

        Console.WriteLine("\n=== Guard Clause Demo ===\n");
        try
        {
            service.Create(new CreateInvoiceDto("", null, new()));
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"  ❌ Caught: {ex.Message}");
        }

        try
        {
            service.MarkAsPaid(inv1.InvoiceId);   // already paid
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  ❌ Caught: {ex.Message}");
        }

        // Null-safe operations
        Console.WriteLine("\n=== Null Safety Demo ===\n");
        var allInvoices = repository.GetAll();
        foreach (var inv in allInvoices)
        {
            string emailDisplay = inv.CustomerEmail ?? "(no email)";      // ??
            int? emailLen = inv.CustomerEmail?.Length;              // ?.
            string city = inv.BillingAddress?.City ?? "Unknown";  // ?. + ??
            Console.WriteLine($"  {inv.CustomerName,-25} email={emailDisplay,-25} city={city}");
        }

        Console.WriteLine("\n=== Report ===\n");
        string report = service.GenerateReport();
        Console.WriteLine(report);

        // IDisposable demo
        Console.WriteLine("=== IDisposable Demo ===\n");
        using (var fileExporter = new FileInvoiceExporter(
            Path.Combine(Path.GetTempPath(), "invoices.txt")))
        {
            var svc2 = new InvoiceService(repository, fileExporter);
            svc2.GenerateReport();
        }   // FileInvoiceExporter.Dispose() called here automatically

        Console.WriteLine("\n✅ Best Practices demo complete!");
    }
}
