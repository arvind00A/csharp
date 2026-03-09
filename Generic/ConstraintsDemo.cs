// ============================================================
//  PROJECT 3 — Generic Constraints
//  Smart Calculator & Animal Shelter
//  Topics: where T : class, where T : struct,
//          where T : new(), where T : IInterface,
//          where T : IComparable<T>, multiple constraints
// ============================================================

using System;
using System.Collections.Generic;





namespace Generic
{
    // ════════════════════════════════════════════════════════════
    // PART A — Numeric utilities using IComparable<T> + IConvertible
    // ════════════════════════════════════════════════════════════
    static class MathUtils
    {
        // ── where T : IComparable<T> ─────────────────────────────
        // We can compare values because T guarantees CompareTo()

        public static T Max<T>(T a, T b) where T : IComparable<T>
            => a.CompareTo(b) >= 0 ? a : b;

        public static T Min<T>(T a, T b) where T : IComparable<T>
            => a.CompareTo(b) <= 0 ? a : b;

        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0) return min;
            if (value.CompareTo(max) > 0) return max;
            return value;
        }

        // Find max in an entire array
        public static T MaxInArray<T>(T[] array) where T : IComparable<T>
        {
            T result = array[0];
            for (int i = 1; i < array.Length; i++)
                if (array[i].CompareTo(result) > 0) result = array[i];
            return result;
        }

        // Sort array using bubble sort
        public static void BubbleSort<T>(T[] array) where T : IComparable<T>
        {
            for (int i = 0; i < array.Length - 1; i++)
                for (int j = 0; j < array.Length - 1 - i; j++)
                    if (array[j].CompareTo(array[j + 1]) > 0)
                    {
                        T temp = array[j]; array[j] = array[j + 1]; array[j + 1] = temp;
                    }
        }
    }

    // ════════════════════════════════════════════════════════════
    // PART B — class + new() constraint
    // ════════════════════════════════════════════════════════════

    // Factory that can create instances of any class with new()
    class ObjectFactory<T> where T : class, new()
    {
        private List<T> _created = new();

        public T Create()
        {
            T obj = new T();   // only works because of new() constraint
            _created.Add(obj);
            Console.WriteLine($"   Created new {typeof(T).Name}");
            return obj;
        }

        public int TotalCreated => _created.Count;
    }


    // ════════════════════════════════════════════════════════════
    // PART C — Interface constraint — Animal Shelter
    // ════════════════════════════════════════════════════════════

    interface IAnimal
    {
        string Name { get; }
        string Sound { get; }
        int Age { get; }
        void Speak();
    }

    class Dog : IAnimal
    {
        public string Name { get; set; } = "Dog";
        public string Sound { get; } = "Woof";
        public int Age { get; set; } = 1;
        public void Speak() => Console.WriteLine($"  {Name} says: {Sound}!");
        public Dog() { }
        public Dog(string name, int age) { Name = name; Age = age; }
    }

    class Cat : IAnimal
    {
        public string Name { get; set; } = "Cat";
        public string Sound { get; } = "Meow";
        public int Age { get; set; } = 1;
        public void Speak() => Console.WriteLine($"  {Name} says: {Sound}!");
        public Cat() { }
        public Cat(string name, int age) { Name = name; Age = age; }
    }

    // Shelter: T must be a class, implement IAnimal, have no-arg constructor
    class AnimalShelter<T> where T : class, IAnimal, new()
    {
        private List<T> _animals = new();

        public void Admit(T animal)
        {
            _animals.Add(animal);
            Console.WriteLine($"   Admitted: {animal.Name} (Age {animal.Age})");
        }

        // Works because T implements IAnimal → we know .Speak() exists
        public void RollCall()
        {
            Console.WriteLine($"\n   {typeof(T).Name} Roll Call ({_animals.Count} animals):");
            foreach (var a in _animals) a.Speak();
        }

        // Find oldest animal
        public T? FindOldest()
        {
            if (_animals.Count == 0) return null;
            T oldest = _animals[0];
            foreach (var a in _animals)
                if (a.Age > oldest.Age) oldest = a;
            return oldest;
        }

        // Create a "blank" animal using new() constraint
        public T CreateBlank()
        {
            T blank = new T();
            Console.WriteLine($"   Created blank {typeof(T).Name}");
            return blank;
        }

        public int Count => _animals.Count;
    }

    // ════════════════════════════════════════════════════════════
    // PART D — struct constraint
    // ════════════════════════════════════════════════════════════

    // Works ONLY with value types (int, double, bool, DateTime...)
    // Useful for nullable wrapping
    class ValueHolder<T> where T : struct
    {
        private T? _value = null;

        public void Set(T value) { _value = value; }
        public void Clear() { _value = null; }

        public bool HasValue => _value.HasValue;
        public T Value => _value ?? throw new InvalidOperationException("No value set.");

        public T GetOrDefault(T defaultValue) => _value ?? defaultValue;

        public override string ToString() =>
            HasValue ? $"ValueHolder({_value})" : "ValueHolder(empty)";
    }


    // ════════════════════════════════════════════════════════════
    // MAIN
    // ════════════════════════════════════════════════════════════
    internal class ConstraintsDemo
    {
        static void Main()
        {
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║  Generic Constraints Demo                ║");
            Console.WriteLine("╚══════════════════════════════════════════╝\n");

            // ── PART A: IComparable<T> constraint ────────────────
            Console.WriteLine("=== A. where T : IComparable<T> ===\n");

            Console.WriteLine("  Max(10, 25)           = " + MathUtils.Max(10, 25));
            Console.WriteLine("  Max(\"apple\", \"mango\") = " + MathUtils.Max("apple", "mango"));
            Console.WriteLine("  Min(3.14, 2.71)       = " + MathUtils.Min(3.14, 2.71));

            Console.WriteLine("\n  Clamp tests:");
            Console.WriteLine($"    Clamp(15,  1, 10)  = {MathUtils.Clamp(15, 1, 10)}  (over max)");
            Console.WriteLine($"    Clamp(-5,  0, 10)  = {MathUtils.Clamp(-5, 0, 10)}  (under min)");
            Console.WriteLine($"    Clamp( 7,  1, 10)  = {MathUtils.Clamp(7, 1, 10)}  (in range)");

            int[] scores = { 55, 92, 78, 64, 88, 73, 99, 41 };
            double[] temps = { 36.6, 37.2, 38.1, 36.9, 37.8 };
            string[] names = { "Charlie", "Alice", "Eve", "Bob", "Diana" };

            Console.WriteLine($"\n  Max score : {MathUtils.MaxInArray(scores)}");
            Console.WriteLine($"  Max temp  : {MathUtils.MaxInArray(temps)}°C");
            Console.WriteLine($"  Max name  : {MathUtils.MaxInArray(names)} (alphabetically last)");

            Console.WriteLine("\n  BubbleSort<T>:");
            MathUtils.BubbleSort(scores);
            Console.Write("    Sorted scores: "); foreach (int s in scores) Console.Write($"{s} "); Console.WriteLine();

            MathUtils.BubbleSort(names);
            Console.Write("    Sorted names:  "); foreach (string n in names) Console.Write($"{n} "); Console.WriteLine();

            // ── PART B: class + new() constraint ─────────────────
            Console.WriteLine("\n=== B. where T : class, new() ===\n");

            var dogFactory = new ObjectFactory<Dog>();
            var d1 = dogFactory.Create();
            var d2 = dogFactory.Create();
            var d3 = dogFactory.Create();
            Console.WriteLine($"  Total dogs created: {dogFactory.TotalCreated}");

            var catFactory = new ObjectFactory<Cat>();
            catFactory.Create();
            catFactory.Create();
            Console.WriteLine($"  Total cats created: {catFactory.TotalCreated}");

            // ── PART C: Interface constraint ──────────────────────
            Console.WriteLine("\n=== C. where T : class, IAnimal, new() ===\n");

            Console.WriteLine("   Dog Shelter:");
            var dogShelter = new AnimalShelter<Dog>();
            dogShelter.Admit(new Dog("Rex", 3));
            dogShelter.Admit(new Dog("Buddy", 7));
            dogShelter.Admit(new Dog("Max", 1));
            dogShelter.Admit(new Dog("Daisy", 5));
            dogShelter.RollCall();
            var oldestDog = dogShelter.FindOldest();
            Console.WriteLine($"\n   Oldest dog: {oldestDog?.Name} (Age {oldestDog?.Age})");

            Console.WriteLine("\n   Cat Shelter:");
            var catShelter = new AnimalShelter<Cat>();
            catShelter.Admit(new Cat("Whiskers", 2));
            catShelter.Admit(new Cat("Luna", 6));
            catShelter.Admit(new Cat("Mittens", 4));
            catShelter.RollCall();

            catShelter.CreateBlank();  // uses new() constraint internally

            // ── PART D: struct constraint ─────────────────────────
            Console.WriteLine("\n=== D. where T : struct (value types only) ===\n");

            var intHolder = new ValueHolder<int>();
            Console.WriteLine($"  {intHolder}");         // empty
            intHolder.Set(42);
            Console.WriteLine($"  {intHolder}");         // has value
            Console.WriteLine($"  Value: {intHolder.Value}");
            intHolder.Clear();
            Console.WriteLine($"  GetOrDefault: {intHolder.GetOrDefault(-1)}");

            var tempHolder = new ValueHolder<double>();
            tempHolder.Set(98.6);
            Console.WriteLine($"\n  {tempHolder}");

            // This would be a COMPILE ERROR (string is not a struct):
            // var strHolder = new ValueHolder<string>();  // ❌

            Console.WriteLine("\n All constraint types demonstrated:");
            Console.WriteLine("    where T : IComparable<T>       — compare, sort, clamp");
            Console.WriteLine("    where T : class, new()         — create instances, null-safe");
            Console.WriteLine("    where T : class, IAnimal, new() — multiple constraints");
            Console.WriteLine("    where T : struct               — value types only");
        }
    }
}
