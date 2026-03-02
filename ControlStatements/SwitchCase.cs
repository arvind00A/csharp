using System;
using System.Collections.Generic;
using System.Text;


//switch – case (including break & default)

//Great when you need to compare one value against many constant possibilities.
//Cleaner than many else if chains when checking the same expression.




namespace ControlStatements
{
    internal class SwitchCase
    {
        public void SwitchCaseTest()
        {
            // Example 1: Classic switch with break
            string day = "Monday";

            switch (day)
            {
                case "Saturday":
                case "Sunday":
                    Console.WriteLine("Weekend!");
                    break;

                case "Monday":
                    Console.WriteLine("Start of the week...");
                    break;

                case "Friday":
                    Console.WriteLine("Almost weekend!");
                    break;

                default:
                    Console.WriteLine("Regular weekday.");
                    break;
            }
        }

        public void SwitchExpressionTest()
        {
            // Example 2: switch expression (very clean – C# 8+)
            int score = 85;

            string grade = score switch
            {
                >= 90 => "A",
                >= 80 => "B",
                >= 70 => "C",
                >= 60 => "D",
                _ => "F"          // _ is the discard pattern (like default)
            };

            Console.WriteLine($"Your grade: {grade}");   // Your grade: B
        }

        public void SwitchPatternMatching()
        {
            object value = "Hello";

            string message = value switch
            {
                string s => $"String: {s}",
                int i   => $"Number: {i}",
                null    => $"Nothing here",
                _       => "Unknown type"
            };
            Console.WriteLine(message); // String: Hello
        }
    }
}
