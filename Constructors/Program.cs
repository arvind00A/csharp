// See https://aka.ms/new-console-template for more information
using Constructors;


// Using default constructor
Car defaultCar = new Car();
defaultCar.ShowDetails();

// Parameterized constructor
 Car premiumCar = new Car("Luxury Sedan", 2024, "Black");
premiumCar.ShowDetails();


// Copy constructor
Car copiedCar = new Car(premiumCar);
copiedCar.ShowDetails();

Console.WriteLine($"Company total sales count: {Car.TotalCarsSold}");
