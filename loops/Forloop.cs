using System;
using System.Collections.Generic;
using System.Text;


// for loop — Best when you know how many times to repeat (or use an index/counter)

/*
 * for (initialization; condition; increment/decrement)
 * {
 *      // body runs while condition is true
 *      
 * }
 * 
 * 1. initializer - executed once (usually declare & set counter)
 * 2. condition - checked before each iteration, if false loop ends
 * 3. iterator — executed after each body run (usually increment/decrement)
 * 
 */

namespace loops
{
    internal class Forloop
    {
        public void ForloopTest()
        {
            // print 1-10
            for (int i = 1; i <= 10; i++)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();

            // sum of first 5 numbers (with index usage)
            int sum = 0;
            for (int i = 1; i <= 5; i++)
            {
                sum += i; // sum = sum + i
            }
            Console.WriteLine($"sum: {sum}");

            // Loop backwards
            for (int i = 10;  i >= 1;  i--)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();

            // complex for loop
            for (int i = 0, j = 100; i < 5; i++, j -= 10)
            {
                Console.WriteLine($"i={i}, j={j}");
            }
        }
    }
}
