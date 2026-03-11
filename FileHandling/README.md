### File Handling (System.IO) 🟣

> Reading, writing, and managing files — essential for any real-world C# application.

---

## Table of Contents
- [Overview](#overview)
- [Reading & Writing Files](#reading--writing-files)
  - [High-Level File Methods](#high-level-file-methods)
  - [StreamReader & StreamWriter](#streamreader--streamwriter)
- [File & Directory Operations](#file--directory-operations)
  - [File Class](#file-class)
  - [FileInfo Class](#fileinfo-class)
  - [Directory Class](#directory-class)
  - [Path Class](#path-class)
- [Async File Handling](#async-file-handling)
- [Quick Reference](#quick-reference)

---

## Overview

All file handling lives in the `System.IO` namespace. C# offers three levels of abstraction:

| Approach | Best For | Example |
|---|---|---|
| `File` static methods | Simple one-liners, small files | `File.ReadAllText("f.txt")` |
| `StreamReader` / `StreamWriter` | Large files, fine-grained control | `reader.ReadLine()` |
| Async methods | Web apps, UI apps — non-blocking | `await File.ReadAllTextAsync("f.txt")` |

```csharp
using System.IO;   // add this at the top of every file that uses IO
```

---

## Reading & Writing Files

### High-Level File Methods

These are the simplest way to read and write — perfect for small files.

```csharp
// ── WRITE ─────────────────────────────────────────────────────

// Overwrite entire file (creates if not exists)
File.WriteAllText("notes.txt", "Hello, File!");

// Append to existing file (creates if not exists)
File.AppendAllText("notes.txt", "\nLine 2");

// Write string array — each element on its own line
File.WriteAllLines("list.txt", new[] { "Apple", "Banana", "Cherry" });

// ── READ ──────────────────────────────────────────────────────

// Read entire file as one string
string content = File.ReadAllText("notes.txt");

// Read into string array — one element per line
string[] lines = File.ReadAllLines("list.txt");

// ReadLines — lazy/streaming (memory efficient for large files)
// Reads one line at a time, doesn't load whole file into memory
foreach (string line in File.ReadLines("list.txt"))
    Console.WriteLine(line);
```

> ✅ Use `File.ReadLines()` (not `ReadAllLines`) for large files — it reads lazily and doesn't load the whole file into memory at once.

---

### StreamReader & StreamWriter

Use these when you need fine-grained control or are working with large files.

```csharp
// ── StreamWriter ──────────────────────────────────────────────
// append: false = overwrite (default), append: true = append
using (StreamWriter writer = new StreamWriter("log.txt", append: true))
{
    writer.WriteLine($"[{DateTime.Now:HH:mm:ss}] App started");
    writer.WriteLine($"[{DateTime.Now:HH:mm:ss}] User logged in");
    writer.Write("No newline at the end");
}
// File is automatically closed when 'using' block exits ✅

// ── StreamReader ──────────────────────────────────────────────
using (StreamReader reader = new StreamReader("log.txt"))
{
    // Read line by line
    string? line;
    while ((line = reader.ReadLine()) != null)
        Console.WriteLine(line);

    // OR read entire content at once
    // string all = reader.ReadToEnd();
}

// Modern 'using' syntax (C# 8+) — no curly braces needed
using StreamReader sr = new("data.txt");
string firstLine = sr.ReadLine() ?? "";
// sr is disposed at end of the enclosing method/block
```

**Always wrap file I/O in try-catch:**

```csharp
try
{
    using StreamReader sr = new("missing.txt");
    Console.WriteLine(sr.ReadToEnd());
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"File not found: {ex.FileName}");
}
catch (UnauthorizedAccessException)
{
    Console.WriteLine("No permission to read this file");
}
catch (IOException ex)
{
    Console.WriteLine($"IO error: {ex.Message}");
}
finally
{
    Console.WriteLine("Done attempting to read.");
}
```

> ✅ **Always use `using`** with StreamReader/StreamWriter. It calls `Dispose()` automatically — closing the file even if an exception is thrown. Forgetting to close causes file locks and resource leaks.

---

## File & Directory Operations

### File Class

Static methods for common file operations.

```csharp
using System.IO;

// ── Existence ─────────────────────────────────────────────────
bool exists = File.Exists("data.txt");         // true / false

// ── Create & Delete ───────────────────────────────────────────
File.Create("newfile.txt").Dispose();          // create empty file
File.Delete("newfile.txt");                    // permanent delete (no recycle bin!)

// ── Copy & Move ───────────────────────────────────────────────
File.Copy("source.txt", "backup.txt");                       // copy
File.Copy("source.txt", "backup.txt", overwrite: true);      // copy + overwrite
File.Move("old.txt", "renamed.txt");                         // rename / move

// ── Metadata ──────────────────────────────────────────────────
DateTime created  = File.GetCreationTime("data.txt");
DateTime modified = File.GetLastWriteTime("data.txt");
DateTime accessed = File.GetLastAccessTime("data.txt");
```

---

### FileInfo Class

Object-oriented alternative to `File` — richer properties, better for multiple operations on the same file.

```csharp
FileInfo fi = new FileInfo("data.txt");

Console.WriteLine($"Name          : {fi.Name}");             // data.txt
Console.WriteLine($"Full path     : {fi.FullName}");         // C:\...\data.txt
Console.WriteLine($"Size (bytes)  : {fi.Length}");           // 1024
Console.WriteLine($"Extension     : {fi.Extension}");        // .txt
Console.WriteLine($"Directory     : {fi.DirectoryName}");    // C:\...
Console.WriteLine($"Is read-only  : {fi.IsReadOnly}");       // false
Console.WriteLine($"Created       : {fi.CreationTime}");
Console.WriteLine($"Last modified : {fi.LastWriteTime}");
Console.WriteLine($"Exists        : {fi.Exists}");

// FileInfo also has instance methods
fi.CopyTo("backup.txt", overwrite: true);
fi.MoveTo("renamed.txt");
fi.Delete();
```

---

### Directory Class

```csharp
// ── Create ────────────────────────────────────────────────────
Directory.CreateDirectory("logs");               // create single folder
Directory.CreateDirectory("logs/2026/march");    // creates full nested path ✅

// ── Delete ────────────────────────────────────────────────────
Directory.Delete("temp");                        // delete empty folder
Directory.Delete("temp", recursive: true);       // delete folder + all contents

// ── Move / Rename ─────────────────────────────────────────────
Directory.Move("oldFolder", "newFolder");

// ── Check existence ───────────────────────────────────────────
bool exists = Directory.Exists("logs");

// ── List contents ─────────────────────────────────────────────
string[] files = Directory.GetFiles(".");                    // all files in current dir
string[] txts  = Directory.GetFiles(".", "*.txt");           // filtered by pattern
string[] dirs  = Directory.GetDirectories(".");              // subdirectories only

// Recursive search — finds files in all subdirectories
string[] allCs = Directory.GetFiles(".", "*.cs",
    SearchOption.AllDirectories);

// ── DirectoryInfo — OOP approach ──────────────────────────────
DirectoryInfo di = new DirectoryInfo("logs");
Console.WriteLine($"Name   : {di.Name}");
Console.WriteLine($"Parent : {di.Parent?.Name}");
Console.WriteLine($"Files  : {di.GetFiles("*", SearchOption.AllDirectories).Length}");
```

---

### Path Class

Use `Path` for all path string operations — never concatenate paths manually!

```csharp
// ── Combine — ALWAYS use this instead of string + "\" + string ──
string path = Path.Combine("C:\\Users", "Alice", "docs", "file.txt");
// Result: C:\Users\Alice\docs\file.txt
// Works on Windows (\) AND Linux/Mac (/) automatically ✅

// ── Extract parts ─────────────────────────────────────────────
string full = @"C:\Users\Alice\Documents\report_2026.pdf";

Path.GetFileName(full);                    // report_2026.pdf
Path.GetFileNameWithoutExtension(full);    // report_2026
Path.GetExtension(full);                   // .pdf
Path.GetDirectoryName(full);               // C:\Users\Alice\Documents
Path.IsPathRooted(full);                   // true (absolute path)
Path.GetFullPath("relative.txt");          // converts to absolute path

// ── Special paths ─────────────────────────────────────────────
Path.GetTempPath();           // C:\Users\Alice\AppData\Local\Temp\
Path.GetTempFileName();       // creates a real temp file, returns path
Path.GetRandomFileName();     // random name string (no file created)

// ── Change extension ──────────────────────────────────────────
Path.ChangeExtension("report.txt", ".pdf");   // report.pdf
```

> ⚠️ **Never hardcode path separators!** `"folder" + "\\" + "file.txt"` breaks on Linux/Mac. Always use `Path.Combine("folder", "file.txt")` — it uses the correct separator for the OS automatically.

---

## Async File Handling

File I/O is slow by nature. Async methods release the thread while waiting for the disk — keeping web servers and UI apps responsive.

```csharp
// ── Async Write ───────────────────────────────────────────────
public static async Task WriteAsync()
{
    // Overwrite
    await File.WriteAllTextAsync("output.txt", "Hello async world!");

    // Append
    await File.AppendAllTextAsync("log.txt", $"{DateTime.Now}: Event\n");

    // Write lines array
    await File.WriteAllLinesAsync("items.txt", new[] { "A", "B", "C" });
}

// ── Async Read ────────────────────────────────────────────────
public static async Task ReadAsync()
{
    // Read entire file
    string content = await File.ReadAllTextAsync("output.txt");

    // Read into string array
    string[] lines = await File.ReadAllLinesAsync("items.txt");
}

// ── Async StreamReader (best for large files) ─────────────────
public static async Task ReadLargeFileAsync(string path)
{
    using StreamReader sr = new(path);
    string? line;
    while ((line = await sr.ReadLineAsync()) != null)
    {
        Console.WriteLine(line);
    }
}

// ── Async with exception handling ─────────────────────────────
public static async Task<string> SafeReadAsync(string path)
{
    try
    {
        return await File.ReadAllTextAsync(path);
    }
    catch (FileNotFoundException)
    {
        Console.WriteLine($"File not found: {path}");
        return string.Empty;
    }
    catch (UnauthorizedAccessException)
    {
        Console.WriteLine("No permission to read this file");
        return string.Empty;
    }
    catch (IOException ex)
    {
        Console.WriteLine($"IO error: {ex.Message}");
        return string.Empty;
    }
}

// ── Read multiple files in PARALLEL ──────────────────────────
public static async Task ReadManyAsync()
{
    string[] paths = { "file1.txt", "file2.txt", "file3.txt" };

    // Start all reads at the same time
    Task<string>[] tasks = Array.ConvertAll(paths,
        p => File.ReadAllTextAsync(p));

    // Wait for ALL to complete — runs in parallel!
    string[] contents = await Task.WhenAll(tasks);

    for (int i = 0; i < paths.Length; i++)
        Console.WriteLine($"{paths[i]}: {contents[i].Length} chars");
}
```

> ✅ **When to use async:** Always in ASP.NET Core or UI apps. For simple console scripts or one-time tools, synchronous is fine — the async overhead isn't worth it there.

---

## Quick Reference

```
File Handling (System.IO)
│
├── Read / Write
│   ├── File.WriteAllText      overwrite entire file
│   ├── File.AppendAllText     append to file
│   ├── File.WriteAllLines     write string array
│   ├── File.ReadAllText       entire file as string
│   ├── File.ReadAllLines      string array per line
│   └── File.ReadLines         lazy line-by-line (large files)
│
├── StreamReader / StreamWriter
│   ├── Always use 'using' block — auto-closes file
│   ├── StreamWriter(path, append: true/false)
│   ├── writer.WriteLine() / writer.Write()
│   ├── reader.ReadLine()  → string? (null at EOF)
│   └── reader.ReadToEnd() → entire file as string
│
├── File Class (static)
│   ├── File.Exists / Create / Delete
│   └── File.Copy / Move
│
├── FileInfo Class (instance)
│   └── .Name / .Length / .Extension / .DirectoryName / .IsReadOnly
│
├── Directory Class
│   ├── CreateDirectory (nested paths ✅)
│   ├── Delete(path, recursive: true)
│   ├── GetFiles(".", "*.txt", SearchOption.AllDirectories)
│   └── GetDirectories / Move / Exists
│
├── Path Class
│   ├── Path.Combine()  ← ALWAYS use — never string + "\" + string
│   ├── GetFileName / GetExtension / GetFileNameWithoutExtension
│   ├── GetDirectoryName / GetFullPath
│   └── GetTempPath / GetRandomFileName
│
└── Async
    ├── File.ReadAllTextAsync / WriteAllTextAsync / AppendAllTextAsync
    ├── File.ReadAllLinesAsync / WriteAllLinesAsync
    ├── StreamReader.ReadLineAsync()
    └── Task.WhenAll(tasks)  ← parallel reads
```

```csharp
// One-line cheat sheet
string s    = await File.ReadAllTextAsync("f.txt");
await File.WriteAllTextAsync("f.txt", "content");
await File.AppendAllTextAsync("log.txt", "entry\n");

using StreamWriter sw = new("log.txt", append: true);
using StreamReader sr = new("data.txt");

string path = Path.Combine("folder", "subfolder", "file.txt");  // always! ✅
Directory.CreateDirectory(Path.Combine("logs", "2026"));
```

---

*C# Day 7 (Part 2) — File Handling (System.IO) cheat sheet*