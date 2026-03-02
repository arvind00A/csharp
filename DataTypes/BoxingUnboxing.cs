using System;
using System.Collections.Generic;
using System.Text;


// Boxing
    // Converting value to object (e.g. object obj = 42;).
    // Avoid for performance as it allocates heap memory.


// Unboxing
    // Converting object back to value (e.g. int num = (int)obj;).
    // Requires explicit casting and can throw InvalidCastException if the object is not of the expected type.
namespace DataTypes
{
    internal class BoxingUnboxing
    {
        int data = 42;

        public void BoxingUnboxingTest()
        {
            // Boxing
            object boxedData = data; // value type (int) is boxed to object
            // Unboxing
            int unboxedData = (int)boxedData; // object is unboxed back to int
            Console.WriteLine($"Boxed Data: {boxedData}");
            Console.WriteLine($"Unboxed Data: {unboxedData}");
        }
    }
}
