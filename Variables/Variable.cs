using System;
using System.Collections.Generic;
using System.Text;

// Variable
/*
 * A variable is a named storage location in memory that can hold a value.
 * It's like a labeled box-you declared it, assign a value, and use it in your code.
 * 
*/

namespace Basic_Syntex
{
    internal class Variable
    {
        // Constants
        const double Pi = 3.14159; // Use const for values that never change


        // Read-Only
        readonly string CurrentDate; // readonly field - can be set in constructor or at declaration

        public Variable()
        {
            CurrentDate = DateTime.Now.ToString("yyyy-MM-dd"); // Set in constructor, can't be changed later
        }

        static void Main(string[] args)
        {
            // Declaration
            int age; // declares an integer variable named age


            // Initialization
            age = 25; // Assign an initial value. You can do this during declaration or later.


            // Assignment
            age = 30; // Use = to set or change the value. C# supports compound assignments like +=, -=, etc.


            // Scope
            //Variables are only accessible within their block (e.g., inside a method, loop, or class).
            //Local variables (in methods) aren't initialized by default—using them without a value causes a compile error.
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine(i);
            }
            //Console.WriteLine(i); // This will cause an error because i is out of scope here.



            //Naming Conventions
            //Use camelCase for local variables (e.g., userName), PascalCase for constants (e.g., MaxUsers).
            //Avoid starting with numbers or using reserved keywords like int
            string userName = "Arvind";

        }

    }
}
