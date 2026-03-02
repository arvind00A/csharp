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



// Reference Types
Console.WriteLine("\nReference Types");
ReferenceTypes referenceTypes = new ReferenceTypes();
referenceTypes.ReferenceTypeTest();

Console.WriteLine("\nArray");
referenceTypes.ArrayTest();

Console.WriteLine("\nRecord");
referenceTypes.RecordTest();


// Boxing and Unboxing
Console.WriteLine("\nBoxing and Unboxing");
BoxingUnboxing boxingUnboxing = new BoxingUnboxing();
boxingUnboxing.BoxingUnboxingTest();


// Type Conversions
TypeConversions typeConversions = new TypeConversions();
typeConversions.TypeConversionsTest();



// var keyword ~ (still strongly-typed, not dynamic) - Compiler infers type based on assigned value, but the variable is still statically typed.
Console.WriteLine("\nVar Keyword");
var x = 10; // Compiler infers type as int
Console.WriteLine($"x: {x}");


// Nullables and Null Safety
    //In C# 8+, enable nullable reference types (#nullable enable) to warn about potential nullls.
    // Use ?? (null-coalescing operator) to provide default values for nulls (e.g., string name = inputName ?? "Default Name";).
    //       or use ?. (null-conditional operator) to safely access members of potentially null objects (e.g., int? length = name?.Length;).
string inputName = Console.ReadLine();
string name = inputName ?? "Default Name"; // Null-coalescing operator
Console.WriteLine($"\nname: {name}");

int? length = name?.Length; // Null-conditional operator
Console.WriteLine($"Name-length: {length}");