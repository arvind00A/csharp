using System;
using System.Collections.Generic;
using System.Text;

// Logical Operators (&&, ||, !)
// These are used to combine or invert boolean expressions. They operate on boolean values (true/false) and return a boolean result.
namespace Operators
{
    internal class Logical
    {
        public void LogicalTest()
        {
            bool isAdult = true;
            bool hasLicense = false;

            if (isAdult && hasLicense)
            {
                Console.WriteLine("Can drive!");
            }
            else
            {
                Console.WriteLine("Cannot drive.");   // this prints
            }

            bool isWeekend = true;
            bool isHoliday = false;

            if (isWeekend || isHoliday)
            {
                Console.WriteLine("Relax time!");     // this prints
            }

            bool isRaining = false;
            if (!isRaining)
            {
                Console.WriteLine("Go outside!");     // this prints
            }



            string? username = null;

            // Safe: && stops if left is false → doesn't try to access .Length
            if (username != null && username.Length > 3)
            {
                Console.WriteLine("Long name!");
            }
            else
            {
                Console.WriteLine("Short or no name.");
            }
        }
    }
}
