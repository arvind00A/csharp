// ============================================================
//  PROJECT 2 — Generic Collections
//  School Management System
//  Topics: List<T>, Dictionary<K,V>, Queue<T>, Stack<T>,
//          HashSet<T>, LinkedList<T>, SortedList<K,V>
// ============================================================

using System;
using System.Collections.Generic;

namespace Collections
{
    // ── Model ─────────────────────────────────────────────────────
    class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Subject { get; set; } = "";
        public double Grade { get; set; }

        public override string ToString() =>
            $"[{Id:D3}] {Name,-12} | {Subject,-10} | Grade: {Grade:F1}";
    }


    internal class SchoolSystem
    {
        static void Main()
        {
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║  Generic Collections — School System     ║");
            Console.WriteLine("╚══════════════════════════════════════════╝\n");

            DemoListT();
            DemoDictionary();
            DemoQueueAndStack();
            DemoHashSet();
            DemoLinkedList();
            DemoSortedList();
        }

        // ── 1. List<T> — Student Roster ──────────────────────────
        static void DemoListT()
        {
            Console.WriteLine("=== 1. List<T> - Student Roster ===\n");
            List<Student> students = new()
            {
                new Student { Id=1, Name="Alice",   Subject="Math",    Grade=92.5 },
                new Student { Id=2, Name="Bob",     Subject="Science", Grade=78.0 },
                new Student { Id=3, Name="Charlie", Subject="Math",    Grade=85.5 },
                new Student { Id=4, Name="Diana",   Subject="English", Grade=91.0 },
                new Student { Id=5, Name="Eve",     Subject="Science", Grade=95.0 },
            };

            Console.WriteLine($" Total students: {students.Count}");

            // Access by index - type-safe, no cast!
            Console.WriteLine($" First: {students[0].Name}");
            Console.WriteLine($" Last : {students[^1].Name}");

            // Add more students
            students.Add(new Student { Id = 6, Name = "Frank", Subject = "Math", Grade = 70.0 });
            students.AddRange(new[]
            {
                new Student { Id=7, Name="Grace", Subject="English", Grade=88.0},
                new Student { Id=8, Name="Henry", Subject="Science", Grade=82.5}
            });

            // Find - single match
            Student? topStudent = students.Find(s => s.Grade >= 95.0);
            Console.WriteLine($"\n Top student (>=95): {topStudent?.Name}");

            // FindAll - multiple matches
            List<Student> mathStudents = students.FindAll(s => s.Subject == "Math");
            Console.WriteLine($"\n Math students ({mathStudents.Count}):");
            mathStudents.ForEach(s => Console.WriteLine($"    {s}"));

            // Sort by grade descending
            students.Sort((a, b) => b.Grade.CompareTo(a.Grade));
            Console.WriteLine("\n All students ranked by grade:");
            for (int i = 0; i < students.Count; i++)
                Console.WriteLine($"   #{i+1} {students[i]}");

            // Remove failing students (below 75)
            int removed = students.RemoveAll(s => s.Grade < 75.0);
            Console.WriteLine($"\n  Removed {removed} students with grade < 75");
            Console.WriteLine($"  Remaining students: {students.Count}");

            // Average grade
            double sum = 0;
            students.ForEach(s => sum += s.Grade);
            Console.WriteLine($"  Class average: {sum / students.Count:F1}\n");
        }

        // ── 2. Dictionary<K,V> — Subject → Students ──────────────
        static void DemoDictionary()
        {
            Console.WriteLine("=== 2. Dictionary<K,V> — Subject Enrollment ===\n");

            // Key = subject name, Value = list of student names
            Dictionary<string, List<string>> enrollment = new()
            {
                { "Math",    new List<string> { "Alice", "Charlie", "Frank" } },
                { "Science", new List<string> { "Bob", "Eve", "Henry" } },
                { "English", new List<string> { "Diana", "Grace" } },
            };

            // Print enrollment
            Console.WriteLine("  Enrollment:");
            foreach (var (subject, roster) in enrollment)
                Console.WriteLine($"    {subject,-10}: {string.Join(", ", roster)}");

            // Add a student to a subject
            enrollment["Math"].Add("Zara");
            Console.WriteLine($"\n  Math after adding Zara: {string.Join(", ", enrollment["Math"])}");

            // Add new subject
            enrollment["History"] = new List<string> { "Alice", "Bob" };

            // Safe lookup with TryGetValue
            string searchSubject = "Physics";
            if (enrollment.TryGetValue(searchSubject, out var physicsStudents))
                Console.WriteLine($"  {searchSubject}: {string.Join(", ", physicsStudents)}");
            else
                Console.WriteLine($"  '{searchSubject}' subject not found");

            // Word frequency counter — classic Dictionary use case
            Console.WriteLine("\n  Word Frequency Counter:");
            string sentence = "the cat sat on the mat the cat";
            Dictionary<string, int> freq = new();
            foreach (string word in sentence.Split(' '))
                freq[word] = freq.GetValueOrDefault(word, 0) + 1;

            foreach (var (word, count) in freq)
                Console.WriteLine($"    '{word}' appears {count}x");

            Console.WriteLine();
        }

        // ── 3. Queue<T> + Stack<T> ───────────────────────────────
        static void DemoQueueAndStack()
        {
            Console.WriteLine("=== 3. Queue<T> — Exam Queue + Stack<T> — Undo System ===\n");

            // Queue: students waiting for exam seat assignment
            Queue<string> examQueue = new();
            string[] arrivals = { "Alice", "Bob", "Charlie", "Diana", "Eve" };
            foreach (string s in arrivals)
            {
                examQueue.Enqueue(s);
                Console.WriteLine($"   {s} joined the exam queue");
            }

            Console.WriteLine($"\n  Queue size: {examQueue.Count}");
            Console.WriteLine($"  Next up   : {examQueue.Peek()}");

            Console.WriteLine("\n  Assigning exam seats (FIFO):");
            int seatNumber = 1;
            while (examQueue.Count > 0)
            {
                string student = examQueue.Dequeue();
                Console.WriteLine($"   Seat {seatNumber++} -> {student}");
            }

            // Stack: grade entry undo system
            Console.WriteLine("\n  Grade Entry System with Undo (Stack):");
            Stack<(string Name, double Grade)> history = new();

            void EnterGrade(string name, double grade)
            {
                history.Push((name, grade));
                Console.WriteLine($"    Entered: {name} = {grade}");
            }

            void Undo()
            {
                if (history.Count == 0) { Console.WriteLine("  Nothing to undo"); return; }
                var (name, grade) = history.Pop();
                Console.WriteLine($"  -  Undid  : {name} = {grade}");
            }

            EnterGrade("Alice", 92.0);
            EnterGrade("Bob", 78.5);
            EnterGrade("Charlie", 200.0);  // mistake!
            Undo();                         // undo Charlie's wrong grade
            EnterGrade("Charlie", 88.0);   // re-enter correctly

            Console.WriteLine($"\n  Final grade stack ({history.Count} entries):");
            foreach (var (name, grade) in history)   // Stack iterates top to bottom
                Console.WriteLine($"    {name}: {grade}");

            Console.WriteLine();

        }

        // ── 4. HashSet<T> — Unique Tags ──────────────────────────
        static void DemoHashSet()
        {
            Console.WriteLine("=== 4. HashSet<T> — Unique Course Tags ===\n");

            HashSet<string> cseTags = new() { "programming", "algorithms", "data-structures", "oop" };
            HashSet<string> dataTags = new() { "statistics", "algorithms", "data-structures", "ml" };
            HashSet<string> designTags = new() { "ux", "graphics", "typography", "design" };

            Console.WriteLine($"  CSE tags   : {string.Join(", ", cseTags)}");
            Console.WriteLine($"  Data tags  : {string.Join(", ", dataTags)}");
            Console.WriteLine($"  Design tags: {string.Join(", ", designTags)}");

            // Duplicate ignored
            bool added = cseTags.Add("programming");
            Console.WriteLine($"\n  Adding 'programming' again -> success: {added} (duplicate ignored)");

            // Set operations
            HashSet<string> common = new(cseTags);
            common.IntersectWith(dataTags);
            Console.WriteLine($"\n  Common tags (CSE ∩ Data): {string.Join(", ", common)}");

            HashSet<string> allTech = new(cseTags);
            allTech.UnionWith(dataTags);
            Console.WriteLine($"  All tech tags (CSE ∪ Data): {string.Join(", ", allTech)}");

            HashSet<string> cseOnly = new(cseTags);
            cseOnly.ExceptWith(dataTags);
            Console.WriteLine($"  CSE-only tags (CSE - Data): {string.Join(", ", cseOnly)}");

            // Fast membership check O(1)
            Console.WriteLine($"\n  CSE has 'ml'          : {cseTags.Contains("ml")}");
            Console.WriteLine($"  Design has 'graphics'  : {designTags.Contains("graphics")}");

            // Remove duplicates from a list using HashSet
            List<int> withDuplicates = new() { 1, 2, 3, 2, 4, 3, 5, 1 };
            HashSet<int> unique = new(withDuplicates);
            Console.WriteLine($"\n  List with dupes : [{string.Join(", ", withDuplicates)}]");
            Console.WriteLine($"  After HashSet    : [{string.Join(", ", unique)}]");

            Console.WriteLine();
        }

        // ── 5. LinkedList<T> — Playlist ──────────────────────────
        static void DemoLinkedList()
        {
            Console.WriteLine("=== 5. LinkedList<T> — Music Playlist ===\n");

            LinkedList<string> playlist = new();
            playlist.AddLast("Song A");
            playlist.AddLast("Song B");
            playlist.AddLast("Song C");
            playlist.AddLast("Song D");

            PrintLinkedList(playlist);

            // Add at front
            playlist.AddFirst("Intro Track");
            Console.WriteLine("  After AddFirst('Intro Track'):");
            PrintLinkedList(playlist);

            // Find and insert after
            LinkedListNode<string>? nodeB = playlist.Find("Song B");
            if (nodeB != null)
            {
                playlist.AddAfter(nodeB, "Bonus Track");
                Console.WriteLine("  After AddAfter('Song B', 'Bonus Track'):");
                PrintLinkedList(playlist);
            }

            // Remove from middle — O(1) once we have the node
            playlist.Remove("Song C");
            Console.WriteLine("  After Remove('Song C'):");
            PrintLinkedList(playlist);

            Console.WriteLine($"  First: {playlist.First?.Value}");
            Console.WriteLine($"  Last : {playlist.Last?.Value}");
            Console.WriteLine($"  Count: {playlist.Count}");

            // Navigate with nodes
            Console.WriteLine("\n  Forward traversal:");
            for (var node = playlist.First; node != null; node = node.Next)
                Console.Write($"  {node.Value} -> ");
            Console.WriteLine("END");

            Console.WriteLine("\n  Backward traversal (doubly-linked!):");
            for (var node = playlist.Last; node != null; node = node.Previous)
                Console.Write($"  {node.Value} -> ");
            Console.WriteLine("START\n");
        }

        // ── 6. SortedList<K,V> — Leaderboard ─────────────────────
        static void DemoSortedList()
        {
            Console.WriteLine("=== 6. SortedList<K,V> — Auto-Sorted Leaderboard ===\n");

            // Keys are int scores (sorted ascending) — we'll reverse for display
            SortedList<string, double> gpas = new()
            {
                { "Charlie", 3.5 },
                { "Alice",   3.9 },   // out of order
                { "Eve",     3.7 },
                { "Bob",     3.2 },
                { "Diana",   3.8 },
            };

            Console.WriteLine("  GPA list (auto-sorted A->Z by name):");
            for (int i = 0; i < gpas.Count; i++)
                Console.WriteLine($"    {gpas.Keys[i],-12} GPA: {gpas.Values[i]:F2}");

            // Safe access
            if (gpas.TryGetValue("Alice", out double aliceGpa))
                Console.WriteLine($"\n  Alice's GPA: {aliceGpa:F2}");

            // Find highest GPA manually
            double maxGpa = 0;
            string topName = "";
            foreach (var (name, gpa) in gpas)
                if (gpa > maxGpa) { maxGpa = gpa; topName = name; }

            Console.WriteLine($"  Highest GPA: {topName} ({maxGpa:F2})");

            gpas.Add("Frank", 3.6);
            Console.WriteLine($"\n  After adding Frank — Count: {gpas.Count}");
            Console.WriteLine($"  Frank's sorted index: {gpas.IndexOfKey("Frank")}");

            Console.WriteLine("\n All Generic Collections demonstrated!");
        }

        static void PrintLinkedList(LinkedList<string> ll)
        {
            Console.Write("  Playlist: ");
            foreach (string s in ll) Console.Write($"[{s}] -> ");
            Console.WriteLine($"null (Count: {ll.Count})");
        }
    }
}
