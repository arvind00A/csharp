using System;
using System.Collections.Generic;
using System.Text;

// foreach loop — Best for iterating over collections (arrays, lists, strings, etc.)
    // Simplest & safest when you just need each element (no index needed).

/*
 * foreach (type variable in collection)
 * {
 *      // body runs for each element in collection
 * }
 * 
 * 1. variable - represents the current element (read-only)
 * 2. collection - any enumerable collection (array, list, string, etc.)
 * 
 * foreach loops are read-only and do not allow modifying the collection directly.
 */


namespace loops
{
    internal class Foreachloop
    {
        public void ForeachloopTest()
        {
            // Array
            int[] numbers = { 10, 20, 30, 40, 50 };

            foreach (int num in numbers)
            {
                Console.WriteLine(num + " ");   // 10 20 30 40 50
            }
            Console.WriteLine();


            // String (char by char)
            string name = "Arvind";

            foreach (char letter in name)
            {
                Console.Write(letter + " "); // A r v i n d 
            }
            Console.WriteLine();


            // List<string>
            List<string> fruits = new List<string> { "Apple", "Banana", "Cherry", "Date" };

            foreach (string fruit in fruits)
            {
                Console.WriteLine($"I like {fruit}");
            }
        }
    }
}
