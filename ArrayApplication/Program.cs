// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, dev!");
Console.WriteLine("Here learning array!");

// Declare and allocate
int[] numbers = new int[5];     // [0, 0, 0, 0, 0]

// Declare with values (array initializer)
int[] scores = { 90, 85, 78, 92, 88 };


// Access by index (zero-based)
Console.WriteLine(scores[0]);   // 90
Console.WriteLine(scores[4]);   // 88


// Modify an element
scores[2] = 95;
Console.WriteLine(scores[2]);   // 95


string[] fruits = { "Apple", "Banana", "Cherry" };

// for loop (when you need the index)
for (int i = 0; i < fruits.Length; i++)
    Console.WriteLine($"[{i}] {fruits[i]}");

// foreach loop (simpler, read-only)
foreach (string fruit in fruits)
    Console.WriteLine(fruit);