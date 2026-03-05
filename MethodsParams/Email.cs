using System;
using System.Collections.Generic;
using System.Text;

// Optional parameters
/*
 * - Parameters with default values
 * - Must appear after required parameters
 * - Can be omitted when calling the method
 */

namespace MethodsParams
{
    internal class Email
    {
        public void SendEmail (string to, string subject = "No subject", string body = "Hello!") 
        {
            Console.WriteLine($"To: {to}  |  Subject: {subject}  |  Body: {body}");
        }
    }
}


// Rules
/*
 * 1. Optional parameters must be defined after all required parameters.
 * 2. Optional parameters must have default values.
 * 3. When calling a method, you can omit optional parameters, and their default values will be used.
 * 4. If you provide a value for an optional parameter, it will override the default value.
 * 5. Optional parameters can be of any type (e.g., string, int, bool, etc.).
 * 6. You can have multiple optional parameters in a method, but they must all come after required parameters.
 */
