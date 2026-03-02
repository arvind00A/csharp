using System;
using System.Collections.Generic;
using System.Text;


// if – else if – else

// The most common decision-making statement.
// Executes a block of code only if a condition is true.


namespace ControlStatements
{
    internal class IfElse
    {
        int temperature = 28;

        public void IfElseTest()
        {
            if (temperature > 30)
            {
                Console.WriteLine("It's a hot day.");
            }
            else if (temperature >= 20 && temperature <= 30)
            {
                Console.WriteLine("It's a warm day.");
            }
            else
            {
                Console.WriteLine("It's a cold day.");
            }
        }

        //Nested if (common pattern)
        public void NestedIfElseTest()
        {
            int age = 17;
            bool hasParentConsent = true;

            if (age >= 18)
            {
                Console.WriteLine("You can register.");
            }
            else
            {
                if (hasParentConsent)
                {
                    Console.WriteLine("You can register with parent consent.");
                }
                else
                {
                    Console.WriteLine("Sorry, too young and no consent.");
                }
            }
        }
    }
}
