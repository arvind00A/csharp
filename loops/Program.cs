// See https://aka.ms/new-console-template for more information
using loops;

Console.WriteLine("Hello, dev!");

// for loop
Console.WriteLine("for loop");
Forloop loop =  new Forloop();
loop.ForloopTest();

// while loop
Console.WriteLine("\nwhile loop");
Whileloop whileLoop = new Whileloop();
whileLoop.WhileloopTest();
whileLoop.WhileInfiniteLoop();

// do-while loop
Console.WriteLine("\ndo-while loop");
Dowhile dowhile = new Dowhile();
dowhile.DowhileTest();

// foreach loop
Console.WriteLine("\nforeach loop");
Foreachloop foreachloop = new Foreachloop();
foreachloop.ForeachloopTest();