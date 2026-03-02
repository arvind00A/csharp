using System;
using System.Collections.Generic;
using System.Text;


// Arithmetic Operators (+, -, *, /, %)
// These perform mathematical operations on numberic types (int, double, decimal, float, long, etc.).

namespace Operators
{
    internal class Arithmatic
    {
        int x = 17;
        int y = 5;

        public void  ArithmaticTest()
        {
            Console.WriteLine();
            Console.WriteLine($"x + y = {x + y}"); // Addition
            Console.WriteLine($"x - y = {x - y}"); // Subtraction
            Console.WriteLine($"x * y = {x * y}"); // Multiplication
            Console.WriteLine($"x / y = {x / y}"); // Division (integer division in this case)
            Console.WriteLine($"x % y = {x % y}"); // Modulus (remainder of division)

            double z = 17.0;
            Console.WriteLine(z / y);   // 3.4 (floating-point division)

            string greeting = "Hello" + ", " + "Arvind";
            Console.WriteLine(greeting);  // Hello, Arvind (string concatenation using + operator)

            
            //  Compound Assignment Operators (+=, -=, *=, /=, %=)
            int score = 100;
            score += 20;   // same as score = score + 20;
            score -= 15;
            score *= 2;
            score /= 5;
            Console.WriteLine(score);  // → let's calculate: 100+20=120, 120-15=105, 105*2=210, 210/5=42


            decimal price = 19.99m;
            decimal tax = 0.18m;
            decimal total = price * (1 + tax);
            Console.WriteLine(total);  // 23.588  (exact)
        }

        
    }
}
