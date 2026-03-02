using System;
using System.Collections.Generic;
using System.Text;

// Value Types (Stored on Stack)

// These hold the actual data.
// They're efficient for small, frequent operations but can't be null (unless using nullable versions like int?).



namespace DataTypes
{
    public class ValueTypes
    {
        // Integral

        // 1.1 byte size(1 bytes) Range~(0 to 255)   -  Unsigned
            // use case : Storing small positive numbers, such as ages or counts.
        byte age = 25;

        // 1.2 sbyte size(1 bytes) Range~(-128 to 127)   -  Signed
            // use case : Storing small negative and positive numbers, such as temperature changes or small financial transactions.
        sbyte temperatureChange = -5;

        // 1.3 short size(2 bytes) Range~(-32,768 to 32,767)   -  Signed
            // use case : Storing small integers that can be negative, such as temperatures or small counts.
        short countryCode = 44;

        // 1.4 ushort size(2 bytes) Range~(0 to 65,535)   -  Unsigned
            // use case : Storing small positive integers that can be larger than byte, such as population counts or product IDs.
        ushort populationCount = 10000;

        // 1.5 int size(4 bytes) Range~(-2,147,483,648 to 2,147,483,647) or (-2B to 2B) or (-2^31 to 2^31-1)   -  Signed
            // use case : Storing larger integers that can be negative, such as financial transactions or user IDs.
        int userId = 123456;

        // 1.6 uint size(4 bytes) Range~(0 to 4,294,967,295 or 4B)   -  Unsigned
            // use case : Storing larger positive integers that can be larger than int, such as large counts or identifiers.
        uint largeCount = 3000000000;

        // 1.7 long size(8 bytes) Range~(-9,223,372,036,854,775,808 to 9,223,372,036,854,775,807) or (-9E18 to 9E18)   -  Signed
         // use case : Storing very large integers that can be negative, such as astronomical data or large financial transactions.
        long bigNumber = 5000000000000;

        // 1.8 ulong size(8 bytes) Range~(0 to 18,446,744,073,709,551,615 or 18E18) or (0 to 18E18)   -  Unsigned
            // use case : Storing very large positive integers that can be larger than long, such as large counts or identifiers in big data applications.
        ulong veryBigNumber = 10000000000000000000;
        public void IntegralTest()
        {
            Console.WriteLine($"Age: {age}");
            Console.WriteLine($"Temprature: " + temperatureChange);
            Console.WriteLine($"Code: {countryCode}");
            Console.WriteLine($"Population:  {populationCount}");
            Console.WriteLine($"userId: {userId}");
            Console.WriteLine($"largeCount: {largeCount}");
            Console.WriteLine($"bigNumber: {bigNumber}");
            Console.WriteLine($"veryBigNumber: {veryBigNumber}");
        }



        // Floating-Point

        // 2.1 float size(4 bytes) Range~(±1.5e-45 to ±3.4e38)[7 digits precision]   -  Single precision
            // use case : Storing decimal numbers with moderate precision, such as measurements or graphics coordinates.
        float PI = 3.14f; // suffix 'f' is required to indicate a float literal

        // 2.2 double size(8 bytes) Range~(±5.0e-324 to ±1.7e308)[15-16 digits precision]   -  Double precision
            // use case : Storing decimal numbers with high precision, such as scientific calculations or financial data.
        double gravity = 9.80665; // double literals are the default for decimal numbers, so no suffix is needed

        // 2.3 decimal size(16 bytes) Range~(±1.0e-28 to ±7.9e28)[28-29 digits precision]   -  High precision decimal
            // use case : Storing decimal numbers with very high precision, such as financial calculations or currency values.
        decimal price = 19.99m; // suffix 'm' is required to indicate a decimal literal


        public void FloatingPointTest()
        {
            Console.WriteLine($"PI: {PI}");
            Console.WriteLine($"Gravity: {gravity}");
            Console.WriteLine($"Price: {price}");
        }


        // Boolean

        // 3.1 bool size(1 byte) Range~(true or false)   -  Boolean
            // use case : Storing true/false values, such as flags, conditions, or binary states.
        bool isActive = true;

        public void BooleanTest()
        {
            if (isActive)
            {
                Console.WriteLine("The system is active.");
            }
            else
            {
                Console.WriteLine("The system is inactive.");
            }
        }


        // Character

        // 4.1 char size(2 bytes) Range~(U+0000 to U+FFFF)   -  Unicode character
            // use case : Storing single characters, such as letters, digits, or symbols.
        char grade = 'A'; // default value '\u0000' (null character) to '\uffff'

        public void CharaterTest()
        {
            Console.WriteLine($"grade: {grade}");
        }

        // 5.1 struct size(varies) Range~(varies)   -  User-defined value type
            // use case : Storing small, immutable data structures, such as coordinates, points, or simple records.
        MyStruct student = new MyStruct(1, "Alice");  // Local variable -> stored on stack

        public void StructTest()
        {
            Console.WriteLine($"Student ID: {student.Id}, Name: {student.Name}");
        }


        // 6.1 enums size(4 bytes by default) Range~(varies based on underlying type)   -  User-defined value type
            // use case : Storing a set of named constants, such as days of the week, states, or categories.
        Colors favoriteColor = Colors.Blue;  // Local variable -> stored on stack

        public void EnumTest() 
        {
            Console.WriteLine($"Favorite Color: {favoriteColor}");
        }


        // Nullable
            // Wraps any value type to allow null
        int? OptionalPinCode = null;

        public void NullableTest()
        {
            Console.WriteLine($"Optional PinCode: {OptionalPinCode}");
        }

    }


    // User-Defined Value Type (Struct) 
        // Structs are value types that can contain multiple fields and methods. They're useful for representing small, immutable data structures.
        // Usecase: Lightweight objects without inheritance, like coordinates.
    // this is value type because it's a struct, and it will be stored on the stack when instantiated.
    struct MyStruct
    {
        public int Id;
        public string Name;
        public MyStruct(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }


    // User-Defined Value Type (Enum)
        // Enums are value types that represent a set of named constants. They're useful for improving code readability and maintainability.
        // Usecase: Representing a fixed set of related constants, like days of the week.
    
    enum Colors
    {
        Red,    // 0
        Green,  // 1
        Blue    // 2
    }
}

// Note: 
// Signed Types
/* 
 * Can represent both positive and negative numbers.
 *  Example: 
 *      sbyte (-128 to 127)
 *      short (-32,768 to 32,767)
 *      int (-2,147,483,648 to 2,147,483,647)
 *      long (-9,223,372,036,854,775,808 to 9,223,372,036,854,775,807)
 *      
 *  Usage: When you need to store values that may go below zero (like temperatures, blances, or differences).
 */

//Unsigned Types
/*
 * Can represent only non-negative numbers (zero and positive).
 *  Example:
 *      byte (0 to 255)
 *      ushort (0 to 65,535)
 *      uint (0 to 4,294,967,295)
 *      ulong (0 to 18,446,744,073,709,551,615)
 *      
 *  Usage: When you need to store values that will never be negative (like age, IDs, or file sizes).
 */
