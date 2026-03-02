using System;
using System.Collections.Generic;
using System.Text;



// Type Conversions
//1 Implicit Conversions: Automatic conversions that the compiler performs when there is no risk of data loss (e.g., int to long).
//2 Explicit Conversions (Casting): Manual conversions that require a cast operator, used when there is a potential for data loss (e.g., long to int).
//Use Convert.ToX() or TryParse() for strings (e.g., int.TryParse("123", out int result)) to safely convert strings to numeric types without throwing exceptions on failure.
namespace DataTypes
{
    internal class TypeConversions
    {
        public void TypeConversionsTest() 
        { 
            // Implicit Conversion
            int smallNumber = 42;
            long largeNumber = smallNumber; // Implicit conversion from int to long
            Console.WriteLine($"int to long data: {largeNumber}");

            // Explicit Conversion (Casting)
            long bigNumber = 10000000000;
            int smallerNumber = (int)bigNumber; // Explicit conversion from long to int (potential data loss)
            Console.WriteLine($"long to int data: {smallerNumber}");

            // String Conversion using Convert.ToX()
            string numberString = "123";
            int convertedNumber = Convert.ToInt32(numberString); // Converts string to int
            Console.WriteLine($"string to int data: {convertedNumber}");

            // String Conversion using TryParse()
            string invalidNumberString = "abc";
            if (int.TryParse(invalidNumberString, out int result))
            {
                Console.WriteLine($"Parsed number: {result}");
            }
            else
            {
                Console.WriteLine($"Failed to parse '{invalidNumberString}' as an integer.");
            }
        }
    }
}
