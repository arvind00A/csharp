using System;
using System.Collections.Generic;
using System.Text;

// do-while loop — Run at least once, then check condition
    //Like while, but the body executes before the first condition check.

/*
 *  do {
 *      // body runs at least once, then repeats while condition is true
 *      
 *  } while (condition);
 *  
 */

namespace loops
{
    internal class Dowhile
    {

       public void DowhileTest()
        {
            string choice;
            do
            {
                Console.WriteLine("\n--- Menu ---");
                Console.WriteLine("1. Say Hello");
                Console.WriteLine("2. Show Time");
                Console.WriteLine("Q. Quite");

                Console.WriteLine("Choose: ");
                choice = Console.ReadLine()?.ToUpper() ?? "";

                if (choice == "1")
                    Console.WriteLine("Hello, Developers");
                else if (choice == "2")
                    Console.WriteLine($"Current time: {DateTime.Now}");
                // else if Q → loop will end after check
            } while (choice != "Q");

            Console.WriteLine("Goodbye!");
        }
    }
}
