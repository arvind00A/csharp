// ============================================================
//  PROJECT 1 — Generic Classes
//  Generic Repository (Mini In-Memory Database)
//  Topics: class MyClass<T>, multiple type params,
//          generic Stack, Box, Pair classes
// ============================================================

namespace Generic
{
    // ── Shared model classes ──────────────────────────────────────
    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public double Price { get; set; }

        public override string ToString() => $"Product[{Id}] {Name} - ${Price:F2}";
    }

    class Customer
    {
        public int    Id    { get; set; }
        public string Name  { get; set; } = "";
        public string Email { get; set; } = "";
        public override string ToString() => $"Customer[{Id}] {Name} {Email}";
    }

    // ── 1. Generic Box class ──────────────────────────────────────
    class Box<T>
    {
        public T Value { get; set; }

        public Box(T value) { Value = value; }

        public void Display()
        {
            Console.WriteLine($" Box<{typeof(T).Name}> = {Value}");
        }
    }

    // ── 2. Generic Pair class (two type parameters) ──────────────────────────────────────
    class Pair<TFirst, TSecond>
    {
        public TFirst First { get; }
        public TSecond Second { get; }

        public Pair(TFirst first, TSecond second)
        {
            First = first;  
            Second = second;
        }


        public override string ToString() => $"({First}, {Second})";
    }

    // ── 3. Generic Stack ──────────────────────────────────────
    class GenericStack<T>
    {
        private T[] _data = new T[100];
        private int _top = -1;

        public void Push(T item)
        {
            if (_top >= _data.Length - 1)
                throw new InvalidOperationException("Stack full");
            _data[++_top] = item;
            Console.WriteLine($"  Pushed: {item}");
        }

        public T Pop()
        {
            if (IsEmpty) throw new InvalidOperationException("Stack empty");
            T item = _data[_top--];
            Console.WriteLine($"  Popped: {item}");
            return item;
        }

        public T Peek() => IsEmpty ? throw new InvalidOperationException() : _data[_top];

        public bool IsEmpty => _top == -1;
        public int Count => _top + 1;

        public void PrintAll()
        {
            Console.WriteLine(" Stack (top->bottom)");
            for (int i = _top; i >= 0; i--)
                Console.WriteLine($"{_data[i]}");
            Console.WriteLine();
        }
    }

    // ── 4. Generic Repository - the main project ──────────────────────────────────────
    // A simple in-memory CRUD store for any type
    class Repository<T> where T : class
    {
        private List<T> _store = new List<T>();
        private string  _name  = typeof(T).Name;

        public void Add(T item)
        {
            _store.Add(item);
            Console.WriteLine($" Added to {_name} repo: {item}");
        }

        public bool Remove(T item)
        {
            bool removed = _store.Remove(item);
            Console.WriteLine(removed
                ? $" Removed from {_name} repo: {item}"
                : $" Item not found in {_name} repo");
            return removed;
        }

        public T? Find(Func<T, bool> predicate)
        {
            foreach (T item in _store)
                if (predicate(item)) return item;
            return null;
        }

        public List<T> Filter(Func<T, bool> predicate)
        {
            List<T> results = new List<T>();
            
            foreach (T item in _store)
                if (predicate(item)) 
                    results.Add(item);

            return results;
        }

        public void PrintAll()
        {
            Console.WriteLine($"\n {_name} Repository ({_store.Count} items):");
            if (_store.Count == 0)
            {
                Console.WriteLine(" (empty)");
                return;
            }

            foreach (T item in _store)
                Console.WriteLine($"  • {item}");
        }

        public int Count => _store.Count;
    }

    // ── Main Program ──────────────────────────────────────────────
    internal class GenericClassesDemo
    {
        static void Main()
        {
            Console.WriteLine("╔═══════════════════════════════════════╗");
            Console.WriteLine("║  Generic Classes — Repository Demo    ║");
            Console.WriteLine("╚═══════════════════════════════════════╝\n");

            // ── Demo 1: Box<T> ────────────────────────────────────
            Console.WriteLine("=== 1. Box<T> - works with any type ===");
            new Box<int>(42).Display();
            new Box<string>("Hello Generics!").Display();
            new Box<double>(3.14159).Display();
            new Box<bool>(true).Display();

            // ── Demo 2: Pair<TFirst, TSecond> ─────────────────────
            Console.WriteLine("\n=== 2. Pair<TFirst, Tsecond> ===");
            var nameAge  = new Pair<string, int>("Alice", 30);
            var coords   = new Pair<double, double>(51.5074, -0.1278);
            var keyValue = new Pair<string, bool>("IsAdmin", true);

            Console.WriteLine($"  Name & Age  : {nameAge}");
            Console.WriteLine($"  Coordinates : {coords}");
            Console.WriteLine($"  Key-Value   : {keyValue}");


            // ── Demo 3: Generics Stack ─────────────────────
            Console.WriteLine("\n=== 3. GenericStack<T> ===");

            Console.WriteLine("n -- Ineger Stack --");
            var intStack = new GenericStack<int>();
            intStack.Push(10);
            intStack.Push(20);
            intStack.Push(30);
            intStack.PrintAll();
            intStack.Pop();
            intStack.PrintAll();

            Console.WriteLine("\n -- String Stack (browser history simulation) --");
            var history = new GenericStack<string>();
            history.Push("https://google.com");
            history.Push("https://github.com");
            history.Push("https://docs.microsoft.com");
            Console.WriteLine($" Current page: {history.Peek()}");
            history.Pop();  // go back
            Console.WriteLine(" After back: {history.Peek()}");

            // ── Demo 4: Repository<T> - the main project ─────────────────────
            Console.WriteLine("\n=== 4. Repository<T> - Product CRUD ===");

            var productRepo = new Repository<Product>();

            var p1 = new Product { Id = 1, Name = "Laptop",   Price = 999.99 };
            var p2 = new Product { Id = 2, Name = "Mouse",    Price = 29.99  };
            var p3 = new Product { Id = 3, Name = "Keyboard", Price = 79.99 };
            var p4 = new Product { Id = 4, Name = "Monitor", Price = 349.99 };

            productRepo.Add(p1);
            productRepo.Add(p2);
            productRepo.Add(p3);
            productRepo.Add(p4);

            productRepo.PrintAll();

            // Find single item
            Console.WriteLine("\n Find product with Id = 2:");
            var found = productRepo.Find(p => p.Id == 2);
            Console.WriteLine($" Found: {found}");

            // Filter items
            Console.WriteLine($"\n Products under $100:");
            var cheap = productRepo.Filter(p => p.Price < 100);
            foreach (var p in cheap)
                Console.WriteLine($"   • {p}");

            // Remove item
            Console.WriteLine();
            productRepo.Remove(p2);
            productRepo.PrintAll();

            // ── Demo 5: Same Repository<T> with Customers ──────────
            Console.WriteLine("\n=== 5. Repository<T> — Customer CRUD ===");

            var customerRepo = new Repository<Customer>();
            customerRepo.Add(new Customer { Id = 1, Name = "Alice",   Email = "alice@example.com" });
            customerRepo.Add(new Customer { Id = 2, Name = "Bob",     Email = "bob@example.com" });
            customerRepo.Add(new Customer { Id = 3, Name = "Charlie", Email = "charlie@example.com" });

            customerRepo.PrintAll();

            Console.WriteLine("\n  Find customer named 'Bob':");
            var bob = customerRepo.Find(c => c.Name == "Bob");
            Console.WriteLine($"  Found: {bob}");

            Console.WriteLine("\n Program complete! Notice we used ONE Repository<T> class for BOTH Products and Customers.");
        }
    }
}
