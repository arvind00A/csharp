// ============================================================
//  PROJECT 1 — Non-Generic Collections
//  Legacy Shopping Cart System
//  Topics: ArrayList, Hashtable, Queue, Stack, SortedList
// ============================================================

using System;
using System.Collections;


namespace Collections
{
    internal class NonGenericDemo
    {
        static void Main()
        {
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║  Non-Generic Collections Demo            ║");
            Console.WriteLine("╚══════════════════════════════════════════╝\n");

            DemoArrayList();
            DemoHashtable();
            DemoQueue();
            DemoStack();
            DemoSortedList();
        }

        // ── 1. ArrayList — Shopping Cart ─────────────────────────
        static void DemoArrayList()
        {
            Console.WriteLine("=== 1. ArrayList - Shopping Cart ===\n");

            ArrayList cart = new ArrayList();

            // Add different types (real-world: keep same type!)
            cart.Add("Apple");
            cart.Add("Bread");
            cart.Add("Milk");
            cart.Add("Eggs");
            cart.Add("Butter");

            Console.WriteLine($" Items in cart: {cart.Count}");

            // Access - no type safety, must cast
            string firstItem = (string)cart[0]!;
            Console.WriteLine($" First item: {firstItem}");

            // Insert and Remove
            cart.Insert(2, "Cheese");
            Console.WriteLine($" After Insert at index 2:");
            PrintArrayList(cart);

            cart.Remove("Bread");
            Console.WriteLine($"\n After Removing 'Bread':");
            PrintArrayList(cart);

            // Contains
            Console.WriteLine($"\n Contains 'Milk' : {cart.Contains("Milk")}");
            Console.WriteLine($" Contains 'Bread' : {cart.Contains("Bread")}");

            // Sort and Reverse
            cart.Sort();
            Console.WriteLine("\n Sorted:");
            PrintArrayList(cart);

            cart.Reverse();
            Console.WriteLine("\n Reversed:");
            PrintArrayList(cart);

            // Convert to typed array
            string[] cartArray = (string[])cart.ToArray(typeof(string));
            Console.WriteLine($"\n Converted to string[] - Length: {cartArray.Length}");

            // Clear
            cart.Clear();
            Console.WriteLine($" After Clear - Count: {cart.Count}");

            Console.WriteLine("Note: ArrayList stores object - use List<T> in modern code!\n");

        }

        // ── 2. Hashtable — Product Catalog ───────────────────────
        static void DemoHashtable()
        {
            Console.WriteLine("=== 2. Hashtable - Product Catalog ===\n");
            
            Hashtable catalog = new Hashtable();

            //Add product code -> product name
            catalog.Add("P001", "Laptop");
            catalog.Add("P002", "Keyboard");
            catalog.Add("P003", "Mouse");
            catalog.Add("P004", "Headphones");  // alternative syntax

            Console.WriteLine($" Products in catalog: {catalog.Count}");

            // Rad - must cast!
            string? product = (string?)catalog["P002"];
            Console.WriteLine($" P002 = {product}");

            // Safe read with ContainsKey
            string searchCode = "P003";
            if (catalog.ContainsKey(searchCode))
                Console.WriteLine($" {searchCode} = {catalog[searchCode]}");

            // Check by value
            Console.WriteLine($" Has 'Monitor': {catalog.ContainsValue("Monitor")}");

            // Remove
            catalog.Remove("P004");
            Console.WriteLine($" After remove P004 - Count: {catalog.Count}");

            // Iterate (no guaranteed order!)
            Console.WriteLine("\n All products (unordered):");
            foreach (DictionaryEntry entry in catalog)
                Console.WriteLine($" {entry.Key} -> {entry.Value}");

            Console.WriteLine("\n Note: Use Dictionary<TKey, TValue> in mordern code!\n");
        }

        // ── 3. Queue — Print Job Spooler ─────────────────────────
        static void DemoQueue()
        {
            Console.WriteLine("=== 3. Queue (FIFO) - Print Job Spooler ===\n");

            Queue printQueue = new Queue();

            // Enqueue jobs
            printQueue.Enqueue("Invoice_March.pdf");
            printQueue.Enqueue("Report_Q1.docx");
            printQueue.Enqueue("Photo_Team.jpg");
            printQueue.Enqueue("Slides_Presentation.pptx");

            Console.WriteLine($" Jobs waiting: {printQueue.Count}");
            Console.WriteLine($" Next to print: {printQueue.Peek()}");  // peek without removing

            Console.WriteLine("\n Processing jobs (FIFO order):");
            while (printQueue.Count > 0 )
            {
                string? job = (string?)printQueue.Dequeue();
                Console.WriteLine($" Printing: {job}");
            }

            Console.WriteLine($" Queue empty: {printQueue.Count == 0}\n");
        }

        // ── 4. Stack — Browser History ───────────────────────────
        static void DemoStack()
        {
            Console.WriteLine("=== 4. Stack (LIFO) — Browser History ===\n");

            Stack history = new Stack();

            // Navigate to pages
            string[] pages = { "google.com", "github.com", "docs.microsoft.com", "stackoverflow.com" };
            foreach (string page in pages)
            {
                history.Push(page);
                Console.WriteLine($"  ->  Navigated to: {page}");
            }

            Console.WriteLine($"\n  History depth: {history.Count}");
            Console.WriteLine($"  Current page : {history.Peek()}");

            Console.WriteLine("\n  Pressing Back (LIFO):");
            for (int i = 0; i < 2; i++)
            {
                string? prev = (string?)history.Pop();
                Console.WriteLine($"  <-  Back to: {prev}");
            }

            Console.WriteLine($"\n  Current page now: {history.Peek()}\n");
        }

        // ── 5. SortedList — Leaderboard ──────────────────────────
        static void DemoSortedList()
        {
            Console.WriteLine("=== 5. SortedList — Leaderboard (sorted by name) ===\n");

            SortedList board = new SortedList();

            // Add players (name → score) — inserted out of order
            board.Add("Charlie", 8500);
            board.Add("Alice", 9200);
            board.Add("Eve", 7800);
            board.Add("Bob", 8900);
            board.Add("Diana", 9100);

            Console.WriteLine($"  Players: {board.Count}");
            Console.WriteLine("  Leaderboard (auto-sorted alphabetically by name):");

            for (int i = 0; i < board.Count; i++)
            {
                string name = (string)board.GetKey(i)!;
                int score = (int)board.GetByIndex(i)!;
                Console.WriteLine($"    {i + 1}. {name,-10} {score,6} pts");
            }

            // Access by key
            Console.WriteLine($"\n  Alice's score: {board["Alice"]}");

            // Index of key
            Console.WriteLine($"  Alice's sorted position: {board.IndexOfKey("Alice")}");

            // Add new player
            board.Add("Frank", 9500);
            Console.WriteLine("\n  After adding Frank (auto-sorted):");
            foreach (DictionaryEntry e in board)
                Console.WriteLine($"    {e.Key,-10} {e.Value}");

            Console.WriteLine("\n    Note: Use SortedList<TKey,TValue> in modern code!\n");
            Console.WriteLine(" Non-Generic Collections demo complete!");
        }

        // ── Helper ────────────────────────────────────────────────
        static void PrintArrayList(ArrayList list)
        {
            Console.Write("  [");
            for (int i = 0; i < list.Count; i++)
                Console.Write((i > 0 ? ", " : "") + list[i]);
            Console.WriteLine("]");
        }
    }
}
