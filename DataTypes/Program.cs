// See https://aka.ms/new-console-template for more information
using DataTypes;

Console.WriteLine("Hello, Dev!");

Console.WriteLine("We are leaning about data types in C#.");
Console.WriteLine();

// Data Types:
/*
 * C# data types are divided into buit-in (primitive) and user-defined.
 * Primitives are simple, while user-defined (like classes) allow complex structures.
 */


ValueTypes valueTypes = new ValueTypes();

Console.WriteLine("Integral");
valueTypes.IntegralTest();

Console.WriteLine("\nFloating-Point");
valueTypes.FloatingPointTest();


Console.WriteLine($"\nBoolean");
valueTypes.BooleanTest();


Console.WriteLine($"\nCharacter");
valueTypes.CharaterTest();


Console.WriteLine($"\nStucts");
valueTypes.StructTest();


Console.WriteLine($"\nEnums");
valueTypes.EnumTest();

Console.WriteLine($"\nNullable");
valueTypes.NullableTest();