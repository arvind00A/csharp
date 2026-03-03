// See https://aka.ms/new-console-template for more information
using MethodsParams;

Console.WriteLine("Hello, dev!");

Calculator calc = new Calculator();

calc.PrintWelcome("Arvind");    // Welcome, Arvind

int sum = calc.Add(5, 7);
Console.WriteLine($"Sum: {sum}");   // Sum: 12

string msg = calc.GetGreeting(true);
Console.WriteLine(msg);   // Good morning

int[] evens = calc.GetEvenNumbers(10);
Console.WriteLine("Evens: " + string.Join(", ", evens));



// Passing parameters

Console.WriteLine("\nPassing parameters");
int value = 5;
ParamenterPassing param =  new ParamenterPassing();
param.Increment(value);
Console.WriteLine("After Increment (by value): " + value); // Output: 5

param.IncrementRef(ref value);
Console.WriteLine("After IncrementRef (by reference): " + value); // Output: 6

int num1 = 20;
int num2 = 10;

int total;
int difference;

// Calling method with out parameters
param.Calculate(num1, num2, out total, out difference);

Console.WriteLine("Total: " + total);
Console.WriteLine("Difference: " + difference);
