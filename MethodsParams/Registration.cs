using System;
using System.Collections.Generic;
using System.Text;



// Named Arguments

/*
 * -Specify arguments by parameter name (ignores order) when calling a method
 * - Improves readability, especially with multiple parameters
 */
namespace MethodsParams
{
    internal class Registration
    {
        public void RegisterUser (string name, int age, string email = "", bool isPremium = false)
        {
            Console.WriteLine($"{name}, {age}, Premium: {isPremium}, Email: {email}");
        }
    }
}

// Best practices for named arguments
/*
 * User named arguments when:
 * Method has > 3 parameters
 * Many optional parameters
 * Improves readablility (self-documenting code)
 * 
 */