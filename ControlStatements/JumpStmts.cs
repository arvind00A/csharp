using System;
using System.Collections.Generic;
using System.Text;


// Jump Statements: break, continue, return
    //These change the normal flow of loops or methods.

// break - exits the nearest enclosing loop or switch statement.

// continue - skips the current iteration of a loop and moves to the next iteration.

// return - exits the current method and optionally returns a value to the caller.

namespace ControlStatements
{
    internal class JumpStmts
    {
        public void BreakContinueTest()
        {
            for (int i = 1; i <= 10; i++)
            {
                if (i == 5)
                {
                    break; // exits the loop when i is 5
                }
                Console.WriteLine(i); // prints 1, 2, 3, 4
            }
            for (int j = 1; j <= 10; j++)
            {
                if (j % 2 == 0)
                {
                    continue; // skips even numbers
                }
                Console.WriteLine(j); // prints odd numbers: 1, 3, 5, 7, 9
            }

            Console.WriteLine(CheckPassword("weak"));      // Too short
            Console.WriteLine(CheckPassword("GoodPass123"));// Strong password!
        }

        // return example (early exit)
        string CheckPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return "Password cannot be empty";

            if (password.Length < 8)
                return "Too short";

            if (!password.Any(char.IsUpper))
                return "Needs at least one uppercase letter";

            return "Strong password!";
        }
    }
}
