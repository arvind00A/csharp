using System;
using System.Collections.Generic;
using System.Text;

namespace MethodsParams
{
    // Examples with different return types
    internal class Calculator
    {
        // 1. void - no return value
        public void PrintWelcome(string name)
        {
            Console.WriteLine($"Welcome, {name}");
            // no return needed
        }


        // 2. int return
        public int Add(int a, int b)
        {
            return a + b;
        }


        // 3. string return
        public string GetGreeting(bool isMorning)
        {
            return isMorning ? "Good morning!" : "Good evening!";
        }


        // 4. array return (int[])
        public int[] GetEvenNumbers (int max)
        {
            int[] evens = new int[max / 2];
            for (int i = 0, j = 0; i < max; i += 2)
            {
                evens[j++] = i;
            }
            return evens;
        }
    }
}
