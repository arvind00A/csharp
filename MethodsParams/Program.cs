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



// Opetional parameters
Console.WriteLine("\nOptional parameters");
Email gmail = new Email();
gmail.SendEmail("arvind@gmail.com");
gmail.SendEmail("team@gmail.com", "Meeting Reminder");



// Named arguments
Console.WriteLine("\nNamed arguments");
Registration reg = new Registration();

// Normal (positional) arguments
reg.RegisterUser("Arvind", 28, "arvind@gmail.com", true);

// Named (order doesn't matter, skip optionals)
reg.RegisterUser(name: "Arvind", age: 28, isPremium: true);

// Mix positional and named (named must come after positional)
reg.RegisterUser("Arvind", 28, email: "arvind@work.com");


// Mini-Project : Methods and Parameters
Person person = new Person("Arvind", 28);

person.CelebrateBirthday();   // positional + defaults
person.CelebrateBirthday(message: "Cheers to another year!", extraGifts: 2);   // named + overrides

person.GetDetails(out string info, out bool adult);
Console.WriteLine(info);
Console.WriteLine($"Adult? {adult}");