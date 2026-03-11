// ============================================================
//  PROJECT 2 — File Handling (System.IO)
//  Student Report Card System
//  Topics: StreamReader, StreamWriter, File.ReadAllText,
//          File & Directory ops, Path, Async file I/O
// ============================================================


using System;
using System.Collections.Generic;
using System.Text;

namespace FileHandling
{
    // ── Student model ─────────────────────────────────────────────
    record Student(string Name, string Subject, double Grade);
    internal class FileHandlingDemo
    {
        // Base directory for all files in this demo
        static readonly string BaseDir = Path.Combine(
            Path.GetTempPath(), "CSharpDemo");

        static async Task Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║  File Handling — Student Report System 📁  ║");
            Console.WriteLine("╚════════════════════════════════════════════╝\n");

            // Setup working directory
            Directory.CreateDirectory(BaseDir);
            Console.WriteLine($"  📂 Working directory: {BaseDir}\n");

            await Demo_WriteFiles();
            await Demo_ReadFiles();
            Demo_StreamReaderWriter();
            Demo_FileOperations();
            Demo_DirectoryOperations();
            Demo_PathOperations();
            await Demo_AsyncFileIO();
            Cleanup();
        }

        // ── 1. Write files — multiple methods ────────────────────
        static async Task Demo_WriteFiles()
        {
            Console.WriteLine("=== 1. Writing Files ===\n");

            // File.WriterAllText - overwrite
            string csvPath = Path.Combine(BaseDir, "students.csv");
            File.WriteAllText(csvPath,
                "Name,Subject,Grade\n" +
                "Alice,Math,92.5\n" +
                "Bob,Science,78.0\n" +
                "Charlie,Math,85.5\n" +
                "Diana,English,91.0\n" +
                "Eve,Science,95.0\n" +
                "Frank,Math,70.0\n");
            Console.WriteLine($"  ✅ WriteAllText   → students.csv ({new FileInfo(csvPath).Length} bytes)");

            // File.WriteAllLines - from array
            string listPath = Path.Combine(BaseDir, "subjects.txt");
            File.WriteAllLines(listPath, new[] { "Math", "Science", "English", "History" });
            Console.WriteLine($"  ✅ WriteAllLines  → subjects.txt");

            // File.AppendAllText - append
            string logPath = Path.Combine(BaseDir, "app.log");
            for (int i = 1; i <= 3; i++)
                File.AppendAllText(logPath, $"[{DateTime.Now:HH:mm:ss}] Event {i} occurred\n");
            Console.WriteLine($"  ✅ AppendAllText  → app.log (3 lines)");

            // Async write
            string asyncPath = Path.Combine(BaseDir, "async_notes.txt");
            await File.WriteAllTextAsync(asyncPath, "This file was written asynchronously!\n");
            await File.AppendAllTextAsync(asyncPath, $"Written at: {DateTime.Now}\n");
            Console.WriteLine($"  ✅ WriteAllTextAsync → async_notes.txt");

            // StreamWriter with append mode
            string detailPath = Path.Combine(BaseDir, "detailed_log.txt");
            using (StreamWriter sw = new(detailPath, append: false))
            {
                sw.WriteLine("=== Detailed Report Log ===");
                sw.WriteLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sw.WriteLine(new string('-', 40));
                sw.WriteLine("Alice     | Math    | A  | 92.5");
                sw.WriteLine("Bob       | Science | C  | 78.0");
                sw.WriteLine("Charlie   | Math    | B  | 85.5");
                sw.Write("EOF");   // Write without newline
            }
            Console.WriteLine($"  ✅ StreamWriter   → detailed_log.txt\n");
        }

        // ── 2. Read files — multiple methods ─────────────────────
        static async Task Demo_ReadFiles()
        {
            Console.WriteLine("=== 2. Reading Files ===\n");

            string csvPath = Path.Combine(BaseDir, "students.csv");

            // ReadAllText - entire file as one string
            string allText = File.ReadAllText(csvPath);
            Console.WriteLine($"  ReadAllText ({allText.Length} chars)");
            Console.WriteLine("  " + allText[..60] + "...\n");

            // ReadAllLines — line array
            string[] lines = File.ReadAllLines(csvPath);
            Console.WriteLine($"  ReadAllLines ({lines.Length} lines including header):");
            foreach (string line in lines[1..])     // skip header
                Console.WriteLine($"    {line}");

            // ReadLines - lazy (efficient for large files)
            Console.WriteLine("\n ReadLines (lazy, memory efficient):");
            int lineNum = 0;
            foreach (string line in File.ReadAllLines(csvPath))
            {
                if (lineNum++ == 0) continue;   // skip header
                string[] parts = line.Split(',');
                if (parts.Length == 3)
                    Console.WriteLine($"    {parts[0],-12} {parts[1], -10} Grade: {parts[2]}");
            }

            // Async read
            string asyncPath = Path.Combine(BaseDir, "async_notes.txt");
            string asyncContent = await File.ReadAllTextAsync(asyncPath);
            Console.WriteLine($"\n  ReadAllTextAsync:\n   {asyncContent.Trim()}");

            // StreamReader - line by line
            Console.WriteLine("\n  StreamReader (detailed_log.txt):");
            string detailPath = Path.Combine(BaseDir, "detailed_log.txt");
            using (StreamReader sr = new(detailPath))
            {
                string? line;
                while ((line = sr.ReadLine()) != null)
                    Console.WriteLine($"   {line}");
            }
            Console.WriteLine();
        }

        // ── 3. StreamReader + StreamWriter: CSV processor ────────
        static void Demo_StreamReaderWriter()
        {
            Console.WriteLine("=== 3. StreamReader & StreamWriter — CSV Processor ===\n");

            string inputPath = Path.Combine(BaseDir, "students.csv");
            string outputPath = Path.Combine(BaseDir, "report.txt");
            string errorPath = Path.Combine(BaseDir, "errors.log");

            List<Student> students = new();
            int errors = 0;

            // Read CSV with StreamReader
            try
            {
                using StreamReader sr = new(inputPath);
                string? line;
                bool isHeader = true;

                while ((line = sr.ReadLine()) != null)
                {
                    if (isHeader) { isHeader = false; continue; }
                    string[] parts = line.Split(',');

                    if (parts.Length != 3)
                    {
                        File.AppendAllText(errorPath, $"Bad line: {line}\n");
                        errors++;
                        continue;
                    }

                    if (!double.TryParse(parts[2], out double grade))
                    {
                        File.AppendAllText(errorPath, $"Bad grade '{parts[2]}' for {parts[0]}\n");
                        errors++;
                        continue;
                    }

                    students.Add(new Student(parts[0].Trim(), parts[1].Trim(), grade));
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"  ❌ Input file not found: {ex.FileName}");
                return;
            }
            catch (IOException ex)
            {
                Console.WriteLine($"  ❌ IO Error reading CSV: {ex.Message}");
                return;
            }

            Console.WriteLine($"  ✅ Parsed {students.Count} students ({errors} errors)");

            // Write report with StreamWriter
            try
            {
                using StreamWriter sw = new(outputPath);
                sw.WriteLine("╔══════════════════════════════════════════╗");
                sw.WriteLine("║           STUDENT REPORT CARD             ║");
                sw.WriteLine("╠══════════════════════════════════════════╣");
                sw.WriteLine($"║  Generated: {DateTime.Now:yyyy-MM-dd HH:mm}              ║");
                sw.WriteLine("╚══════════════════════════════════════════╝");
                sw.WriteLine();
                sw.WriteLine($"  {"Name",-12} {"Subject",-10} {"Grade",6}  {"Letter",6}  {"Status"}");
                sw.WriteLine(new string('-', 55));

                double total = 0;
                foreach (Student s in students)
                {
                    string letter = s.Grade >= 90 ? "A" :
                                    s.Grade >= 80 ? "B" :
                                    s.Grade >= 70 ? "C" :
                                    s.Grade >= 60 ? "D" : "F";
                    string status = s.Grade >= 70 ? "PASS" : "FAIL";
                    sw.WriteLine($"  {s.Name,-12} {s.Subject,-10} {s.Grade,6:F1}  {letter,6}  {status}");
                    total += s.Grade;
                }

                sw.WriteLine(new string('-', 55));
                sw.WriteLine($"  Class Average: {total / students.Count:F2}");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("  ❌ No permission to write report");
                return;
            }
            catch (IOException ex)
            {
                Console.WriteLine($"  ❌ IO Error writing report: {ex.Message}");
                return;
            }
            finally
            {
                // Always runs — announce completion
                Console.WriteLine("  🔍 [finally] StreamWriter processing complete");
            }

            // Display the report
            Console.WriteLine($"\n  📋 Report (report.txt):");
            foreach (string line in File.ReadLines(outputPath))
                Console.WriteLine($"  {line}");
            Console.WriteLine();
        }

        // ── 4. File operations ────────────────────────────────────
        static void Demo_FileOperations()
        {
            Console.WriteLine("=== 4. File Operations ===\n");

            string srcPath = Path.Combine(BaseDir, "students.csv");
            string bakPath = Path.Combine(BaseDir, "students_backup.csv");
            string movePath = Path.Combine(BaseDir, "students_archive.csv");

            // Exists
            Console.WriteLine($"  students.csv exists: {File.Exists(srcPath)}");
            Console.WriteLine($"  missing.txt  exists: {File.Exists(Path.Combine(BaseDir, "missing.txt"))}");

            // Copy
            File.Copy(srcPath, bakPath, overwrite: true);
            Console.WriteLine($"  ✅ Copied to students_backup.csv");

            // FileInfo
            FileInfo fi = new(srcPath);
            Console.WriteLine($"\n  FileInfo for students.csv:");
            Console.WriteLine($"    Name      : {fi.Name}");
            Console.WriteLine($"    Size      : {fi.Length} bytes");
            Console.WriteLine($"    Extension : {fi.Extension}");
            Console.WriteLine($"    Created   : {fi.CreationTime:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"    Modified  : {fi.LastWriteTime:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"    ReadOnly  : {fi.IsReadOnly}");
            Console.WriteLine($"    Directory : {fi.DirectoryName}");

            // Create and delete
            string tempFile = Path.Combine(BaseDir, "temp_delete_me.txt");
            File.WriteAllText(tempFile, "temporary");
            Console.WriteLine($"\n  ✅ Created temp file");
            File.Delete(tempFile);
            Console.WriteLine($"  ✅ Deleted temp file");
            Console.WriteLine($"  Still exists: {File.Exists(tempFile)}\n");
        }

        // ── 5. Directory operations ───────────────────────────────
        static void Demo_DirectoryOperations()
        {
            Console.WriteLine("=== 5. Directory Operations ===\n");

            string reportsDir = Path.Combine(BaseDir, "reports", "2026", "march");
            string archiveDir = Path.Combine(BaseDir, "archive");

            // CreateDirectory — creates full nested path
            Directory.CreateDirectory(reportsDir);
            Console.WriteLine($"  ✅ Created nested: reports/2026/march");

            // Write some test files into it
            File.WriteAllText(Path.Combine(reportsDir, "week1.txt"), "Week 1 data");
            File.WriteAllText(Path.Combine(reportsDir, "week2.txt"), "Week 2 data");
            File.WriteAllText(Path.Combine(reportsDir, "notes.md"), "# Notes");

            // GetFiles
            string[] txtFiles = Directory.GetFiles(reportsDir, "*.txt");
            Console.WriteLine($"\n  .txt files in reports/2026/march:");
            foreach (string f in txtFiles)
                Console.WriteLine($"    📄 {Path.GetFileName(f)}");

            // GetFiles recursive
            string[] allTxt = Directory.GetFiles(BaseDir, "*.txt",
                SearchOption.AllDirectories);
            Console.WriteLine($"\n  All .txt files (recursive): {allTxt.Length} found");

            // GetDirectories
            string[] subdirs = Directory.GetDirectories(BaseDir);
            Console.WriteLine($"\n  Subdirectories of BaseDir:");
            foreach (string d in subdirs)
                Console.WriteLine($"    📁 {Path.GetFileName(d)}");

            // DirectoryInfo
            DirectoryInfo di = new(BaseDir);
            Console.WriteLine($"\n  DirectoryInfo:");
            Console.WriteLine($"    Name   : {di.Name}");
            Console.WriteLine($"    Parent : {di.Parent?.Name}");
            Console.WriteLine($"    Files  : {di.GetFiles("*", SearchOption.AllDirectories).Length}");

            // Move directory
            Directory.CreateDirectory(archiveDir);
            string archiveSub = Path.Combine(archiveDir, "march_backup");
            if (!Directory.Exists(archiveSub))
                Directory.Move(reportsDir, archiveSub);
            Console.WriteLine($"\n  ✅ Moved reports/2026/march → archive/march_backup");
            Console.WriteLine($"  Original exists: {Directory.Exists(reportsDir)}\n");
        }

        // ── 6. Path operations ────────────────────────────────────
        static void Demo_PathOperations()
        {
            Console.WriteLine("=== 6. Path Operations ===\n");

            string fullPath = @"C:\Users\Alice\Documents\report_2026.pdf";

            Console.WriteLine($"  Full path      : {fullPath}");
            Console.WriteLine($"  GetFileName    : {Path.GetFileName(fullPath)}");
            Console.WriteLine($"  GetExtension   : {Path.GetExtension(fullPath)}");
            Console.WriteLine($"  WithoutExt     : {Path.GetFileNameWithoutExtension(fullPath)}");
            Console.WriteLine($"  GetDirectory   : {Path.GetDirectoryName(fullPath)}");
            Console.WriteLine($"  IsPathRooted   : {Path.IsPathRooted(fullPath)}");

            // Path.Combine — ALWAYS use this instead of string concatenation!
            string combined = Path.Combine("C:\\Users", "Alice", "Documents", "file.txt");
            Console.WriteLine($"\n  Path.Combine   : {combined}");

            // Would be wrong with manual concatenation on non-Windows:
            // "C:\\Users" + "\\" + "Alice" — hardcoded separator!

            // Special paths
            Console.WriteLine($"\n  GetTempPath    : {Path.GetTempPath()}");
            Console.WriteLine($"  GetRandomFile  : {Path.GetRandomFileName()}");
            Console.WriteLine($"  GetFullPath    : {Path.GetFullPath("relative.txt")}");

            // Change extension
            string newPath = Path.ChangeExtension(fullPath, ".txt");
            Console.WriteLine($"\n  ChangeExtension: {newPath}");

            // Combine works cross-platform (\ on Windows, / on Linux/Mac)
            string xplat = Path.Combine(BaseDir, "output", "result.csv");
            Console.WriteLine($"  Cross-platform : {xplat}\n");
        }

        // ── 7. Async File I/O ─────────────────────────────────────
        static async Task Demo_AsyncFileIO()
        {
            Console.WriteLine("=== 7. Async File I/O ===\n");

            // Async write
            string asyncPath = Path.Combine(BaseDir, "async_output.txt");
            await File.WriteAllTextAsync(asyncPath,
                "Line 1: Written asynchronously\n" +
                "Line 2: Thread was free while disk wrote\n");
            Console.WriteLine("  ✅ WriteAllTextAsync complete");

            // Async append
            for (int i = 1; i <= 5; i++)
                await File.AppendAllTextAsync(asyncPath,
                    $"Log entry {i}: {DateTime.Now:HH:mm:ss.fff}\n");
            Console.WriteLine("  ✅ AppendAllTextAsync (5 entries)");

            // Async read
            string content = await File.ReadAllTextAsync(asyncPath);
            Console.WriteLine($"\n  ReadAllTextAsync ({content.Length} chars):");
            foreach (string line in content.Split('\n', StringSplitOptions.RemoveEmptyEntries))
                Console.WriteLine($"    {line}");

            // Async ReadAllLines
            string[] lines = await File.ReadAllLinesAsync(asyncPath);
            Console.WriteLine($"\n  ReadAllLinesAsync → {lines.Length} lines");

            // Async StreamReader for large file
            Console.WriteLine("\n  Async StreamReader:");
            using (StreamReader sr = new(asyncPath))
            {
                string? line;
                int count = 0;
                while ((line = await sr.ReadLineAsync()) != null)
                {
                    count++;
                    if (count <= 3)
                        Console.WriteLine($"    Line {count}: {line}");
                }
                Console.WriteLine($"    ... and {count - 3} more");
            }

            // Parallel async reads — 3 files read simultaneously!
            Console.WriteLine("\n  Parallel async reads (Task.WhenAll):");
            string[] paths = {
                Path.Combine(BaseDir, "students.csv"),
                Path.Combine(BaseDir, "app.log"),
                Path.Combine(BaseDir, "async_output.txt")
            };

            Task<string>[] readTasks = Array.ConvertAll(paths,
                p => File.ReadAllTextAsync(p));

            string[] contents = await Task.WhenAll(readTasks);

            for (int i = 0; i < paths.Length; i++)
                Console.WriteLine($"    {Path.GetFileName(paths[i])}: {contents[i].Length} chars");

            Console.WriteLine("\n  ✅ All files read in parallel!\n");
        }

        // ── Cleanup ───────────────────────────────────────────────
        static void Cleanup()
        {
            Console.WriteLine("=== Cleanup ===\n");
            try
            {
                Directory.Delete(BaseDir, recursive: true);
                Console.WriteLine($"  ✅ Cleaned up demo directory");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"  ⚠️  Could not clean up: {ex.Message}");
            }
            Console.WriteLine("\n✅ File Handling demo complete!");
        }
    }
}
