// ============================================================
//  PROJECT 2 — Generic Methods
//  Universal Array & Collection Toolkit
//  Topics: public void Method<T>(), type inference,
//          Swap<T>, FindIndex<T>, Fill<T>, PrintAll<T>
// ============================================================

using System;
using System.Collections.Generic;

namespace Generic
{
    static class GenericToolkit
    {
        // ── 1. Print any array ────────────────────────────────────
        public static void PrintArray<T>(T[] array, string label = "Array")
        {
            Console.Write($"  {label,-15}: [ ");
            foreach (var item in array) Console.Write($"{item} ");
            Console.WriteLine("]");
        }

        // ── 2. Swap two values of any type ────────────────────────
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a; a = b; b = temp;
        }

        // ── 3. Find index of a value ──────────────────────────────
        public static int FindIndex<T>(T[] array, T target)
        {
            for (int i = 0; i < array.Length; i++)
                if (array[i]!.Equals(target)) return i;
            return -1;
        }

        // ── 4. Fill array with a value ────────────────────────────
        public static void Fill<T>(T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++) array[i] = value;
        }

        // ── 5. Reverse any array in-place ─────────────────────────
        public static void Reverse<T>(T[] array)
        {
            int left = 0, right = array.Length - 1;
            while (left < right)
            {
                Swap(ref array[left], ref array[right]);
                left++; right--;
            }
        }

        // ── 6. Count how many elements match a condition ──────────
        public static int CountWhere<T>(T[] array, Func<T, bool> predicate)
        {
            int count = 0;
            foreach (T item in array) if (predicate(item)) count++;
            return count;
        }

        // ── 7. Get all elements matching a condition ──────────────
        public static T[] Filter<T>(T[] array, Func<T, bool> predicate)
        {
            // Count first, then fill (no LINQ)
            int count = CountWhere(array, predicate);
            T[] result = new T[count];
            int idx = 0;
            foreach (T item in array)
                if (predicate(item)) result[idx++] = item;
            return result;
        }

        // ── 8. Transform each element (like Select/Map) ───────────
        public static TOut[] Transform<TIn, TOut>(TIn[] array, Func<TIn, TOut> transform)
        {
            TOut[] result = new TOut[array.Length];
            for (int i = 0; i < array.Length; i++)
                result[i] = transform(array[i]);
            return result;
        }

        // ── 9. Get the minimum (requires IComparable) ─────────────
        public static T Min<T>(T[] array) where T : IComparable<T>
        {
            T min = array[0];
            for (int i = 1; i < array.Length; i++)
                if (array[i].CompareTo(min) < 0) min = array[i];
            return min;
        }

        // ── 10. Get the maximum ────────────────────────────────────
        public static T Max<T>(T[] array) where T : IComparable<T>
        {
            T max = array[0];
            for (int i = 1; i < array.Length; i++)
                if (array[i].CompareTo(max) > 0) max = array[i];
            return max;
        }

        // ── 11. Check if all elements satisfy a condition ─────────
        public static bool All<T>(T[] array, Func<T, bool> predicate)
        {
            foreach (T item in array)
                if (!predicate(item)) return false;
            return true;
        }

        // ── 12. Check if any element satisfies a condition ────────
        public static bool Any<T>(T[] array, Func<T, bool> predicate)
        {
            foreach (T item in array)
                if (predicate(item)) return true;
            return false;
        }
    }

    // ─────────────────────────────────────────────────────────────
    internal class GenericMethodsDemo
    {
        static void Main()
        {
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║  Generic Methods — Array Toolkit Demo    ║");
            Console.WriteLine("╚══════════════════════════════════════════╝\n");

            // ── 1. PrintArray<T> with type inference ──────────────
            Console.WriteLine("=== 1. PrintArray<T> — type inference ===");
            int[] nums = { 5, 3, 8, 1, 9, 2, 7, 4, 6 };
            string[] fruits = { "Apple", "Banana", "Cherry", "Date", "Elderberry" };
            double[] prices = { 9.99, 24.99, 4.49, 149.00, 0.99 };
            bool[] flags = { true, false, true, true, false };

            GenericToolkit.PrintArray(nums, "Integers");
            GenericToolkit.PrintArray(fruits, "Fruits");
            GenericToolkit.PrintArray(prices, "Prices");
            GenericToolkit.PrintArray(flags, "Flags");

            // ── 2. Swap<T> ────────────────────────────────────────
            Console.WriteLine("\n=== 2. Swap<T> ===");
            int a = 100, b = 200;
            Console.WriteLine($"  Before: a={a}, b={b}");
            GenericToolkit.Swap(ref a, ref b);
            Console.WriteLine($"  After:  a={a}, b={b}");

            string s1 = "Hello", s2 = "World";
            Console.WriteLine($"  Before: s1={s1}, s2={s2}");
            GenericToolkit.Swap(ref s1, ref s2);
            Console.WriteLine($"  After:  s1={s1}, s2={s2}");

            // ── 3. FindIndex<T> ───────────────────────────────────
            Console.WriteLine("\n=== 3. FindIndex<T> ===");
            Console.WriteLine($"  Index of 9 in nums   : {GenericToolkit.FindIndex(nums, 9)}");
            Console.WriteLine($"  Index of 'Cherry'    : {GenericToolkit.FindIndex(fruits, "Cherry")}");
            Console.WriteLine($"  Index of 99 (missing): {GenericToolkit.FindIndex(nums, 99)}");

            // ── 4. Reverse<T> ─────────────────────────────────────
            Console.WriteLine("\n=== 4. Reverse<T> ===");
            int[] revTest = { 1, 2, 3, 4, 5 };
            GenericToolkit.PrintArray(revTest, "Before");
            GenericToolkit.Reverse(revTest);
            GenericToolkit.PrintArray(revTest, "After Reverse");

            // ── 5. Fill<T> ────────────────────────────────────────
            Console.WriteLine("\n=== 5. Fill<T> ===");
            int[] zeros = new int[5];
            string[] greets = new string[4];
            GenericToolkit.Fill(zeros, 0);
            GenericToolkit.Fill(greets, "Hello");
            GenericToolkit.PrintArray(zeros, "Zeros");
            GenericToolkit.PrintArray(greets, "Hellos");

            // ── 6. Min<T> and Max<T> ──────────────────────────────
            Console.WriteLine("\n=== 6. Min<T> and Max<T> ===");
            Console.WriteLine($"  Min of nums   : {GenericToolkit.Min(nums)}");
            Console.WriteLine($"  Max of nums   : {GenericToolkit.Max(nums)}");
            Console.WriteLine($"  Min of fruits : {GenericToolkit.Min(fruits)}");  // alphabetical
            Console.WriteLine($"  Max of prices : {GenericToolkit.Max(prices)}");

            // ── 7. CountWhere<T> + Filter<T> ──────────────────────
            Console.WriteLine("\n=== 7. CountWhere<T> + Filter<T> ===");
            int countOver5 = GenericToolkit.CountWhere(nums, n => n > 5);
            Console.WriteLine($"  Numbers > 5: {countOver5}");

            int[] bigNums = GenericToolkit.Filter(nums, n => n > 5);
            GenericToolkit.PrintArray(bigNums, "Nums > 5");

            string[] longFruits = GenericToolkit.Filter(fruits, f => f.Length > 5);
            GenericToolkit.PrintArray(longFruits, "Long fruits");

            // ── 8. Transform<TIn, TOut> ───────────────────────────
            Console.WriteLine("\n=== 8. Transform<TIn, TOut> ===");

            // int[] → string[] (convert each number to a label)
            string[] labels = GenericToolkit.Transform(nums, n => $"Item#{n}");
            GenericToolkit.PrintArray(labels, "Labels");

            // double[] → string[] (format as currency)
            string[] priceLabels = GenericToolkit.Transform(prices, p => $"${p:F2}");
            GenericToolkit.PrintArray(priceLabels, "Prices $");

            // string[] → int[] (get lengths)
            int[] lengths = GenericToolkit.Transform(fruits, f => f.Length);
            GenericToolkit.PrintArray(lengths, "Fruit lengths");

            // ── 9. All<T> and Any<T> ──────────────────────────────
            Console.WriteLine("\n=== 9. All<T> and Any<T> ===");
            Console.WriteLine($"  All nums positive   : {GenericToolkit.All(nums, n => n > 0)}");
            Console.WriteLine($"  All nums > 5        : {GenericToolkit.All(nums, n => n > 5)}");
            Console.WriteLine($"  Any num > 8         : {GenericToolkit.Any(nums, n => n > 8)}");
            Console.WriteLine($"  Any fruit starts 'Z': {GenericToolkit.Any(fruits, f => f.StartsWith("Z"))}");

            // ── 10. Chaining operations ───────────────────────────
            Console.WriteLine("\n=== 10. Chaining (Filter → Transform → Print) ===");
            // Get numbers > 3, square them, print
            int[] over3 = GenericToolkit.Filter(nums, n => n > 3);
            int[] squared = GenericToolkit.Transform(over3, n => n * n);
            GenericToolkit.PrintArray(over3, "Nums > 3");
            GenericToolkit.PrintArray(squared, "Squared");

            Console.WriteLine("\n All methods are generic — they work for int, string, double, bool, and any other type!");
        }
    }
}
