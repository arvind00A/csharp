using System;
using System.Collections.Generic;
using System.Text;

// while loop - Repeat while a condition is true (check before entering)
// Use when you don't know exactly how many iterations you'll need (e.g., reading input until valid).

/*
 * while (condition){
 *   body runs while condition is true
 * }
 */


namespace loops
{
    internal class Whileloop
    {
        public void WhileloopTest()
        {
            // Exmaple - Keep asking until correct password

            string password = "";
            int attempts = 0;

            while (password != "secret" && attempts < 3)
            {
                Console.Write("Enter password: ");
                password = Console.ReadLine() ?? "";
                attempts++;

                if (password != "secret")
                    Console.WriteLine("Incorrect password. Try again.");
            }

            if (password == "secret")
                Console.WriteLine("Access granted!");
            else
                Console.WriteLine("Too many attempts.");
        }

        public void WhileInfiniteLoop()
        {
            while (true)             // runs forever unless we break
            {
                Console.WriteLine("Type 'exit' to quit: ");
                string input = Console.ReadLine() ?? "";

                if (input?.ToLower() == "exit")
                    break;

                Console.WriteLine($"You typed: {input}");
            }
        }
    }
}
