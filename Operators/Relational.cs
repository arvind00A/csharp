using System;
using System.Collections.Generic;
using System.Text;

// Relational / Comparison Operators (==, !=, <, >, <=, >=)
// These compare two values and return a bool.


namespace Operators
{
    internal class Relational
    {
        int x = 10;
        int y = 20;

        public void RelationalTest()
        {
            Console.WriteLine(x == y);  // false
            Console.WriteLine(x != y);  // true
            Console.WriteLine(x < y);   // true
            Console.WriteLine(x >= 10); // true


            string name1 = "Arvind";
            string name2 = "arvind";

            Console.WriteLine(name1 == name2);  // false (case-sensitive comparison)
            Console.WriteLine(name1.Equals(name2, StringComparison.OrdinalIgnoreCase)); // true (case-insensitive)


            int age = 25;
            // Useful pattern:
            if (age >= 18 && age <= 65)
            {
                Console.WriteLine("Working age");
            }
        }
    }
}
