using System;
using System.Collections.Generic;
using System.Text;

// Reference Types (Stored on Heap)

// These hold a reference(pointer) to the data.
// They can be null and support inheritance/polymorphism.

namespace DataTypes
{
    internal class ReferenceTypes
    {
        // string: A sequence of characters, used for text. It's immutable, meaning once created, it cannot be changed.
        // use case : Storing and manipulating text, such as names, messages, or any textual data.
        // Text handling. Use @ for verbatim: @"C:\path".
        string name = "Arvind"; // default value null



        // object: The base type from which all other types derive. It can hold any data type, but requires casting to access specific members.
        // use case : Storing any type of data when the specific type is not known at compile time, such as in collections or for polymorphic behavior.
        object data = 42; // default value null



        // dynamic: A type that bypasses compile-time type checking. It allows for more flexible code but can lead to runtime errors if not used carefully.
        // use case : Interacting with dynamic languages, COM objects, or when you want to defer type checking until runtime.
        // Interop with dynamic languages like JavaScript.
        dynamic dynamicData = "Hello"; // default value null


        public void ReferenceTypeTest()
        {
            Console.WriteLine($"Name: {name}");
            Console.WriteLine($"Object Data: {data}");
            Console.WriteLine($"Dynamic Data: {dynamicData}");

        }


        // Arrays: Fixed-size collections of elements of the same type. They are reference types and can hold any data type, including other arrays.
        // use case : Storing collections of data when the size is known and fixed, such as a list of scores or a grid of values.
        int[] numbers = { 1, 2, 3 }; // default value null
        int[] numbs = new int[] { 1, 2, 3, 4, 5 };
        int[] items = new int[5];

        public void ArrayTest()
        {
            Console.WriteLine("Numbers: " + string.Join(", ", numbers));
            Console.WriteLine("Numbs: " + string.Join(", ", numbs));
            Console.WriteLine("Items: " + string.Join(", ", items));
        }


        // Classes: User-defined reference types that can contain fields, properties, methods, and events. They support inheritance and polymorphism.
        // Use case : Representing complex data structures and behaviors, such as entities in a business domain, UI components, or any object-oriented design.
        /*
         * class Person {
         *      public string Name;
         * }
         */


        // Interface: Defines a contract that classes can implement. It specifies a set of methods and properties that implementing classes must provide, but does not contain any implementation itself.
        // Use case : Defining a common set of behaviors for unrelated classes, enabling polymorphism and decoupling code from specific implementations.
        /*
         * interface IShape {
         *      public double Area();
         * }
         */


        // Delegates: Reference types that represent references to methods. They are used to pass methods as arguments, define callback methods, and implement event handling.
        // Use case : Implementing event-driven programming, callbacks, and defining custom method signatures for flexible code execution.
        /*
         * delegate int MathOperation(int x, int y);
         * 
         */


        // Records: Introduced in C# 9.0, records are reference types that provide built-in functionality for immutability and value-based equality.
        // They are ideal for representing data models and DTOs (Data Transfer Objects).
        record Point(int X, int Y); // default value null

        public void RecordTest()
        {
            Point p1 = new Point(1, 2);
            Point p2 = new Point(1, 2);
            Console.WriteLine($"Point 1: {p1}");
            Console.WriteLine($"Point 2: {p2}");
            Console.WriteLine($"Are points equal? {p1 == p2}"); // True due to value-based equality


            p1 = new Point(10, 20);
            p2 = p1 with { Y = 30 }; // Using 'with' expression to create a new record with modified properties
            Console.WriteLine(p1 == p2); // False (value equality checks)
        }
    }
}
