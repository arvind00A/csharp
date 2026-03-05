using System;
using System.Collections.Generic;
using System.Text;

namespace Constructors
{
    internal class Car
    {
        // Instance fields
        public string Model;
        public int Year;
        public string Color;
        public readonly string Vin;     // readonly field

        // Static field
        public static int TotalCarsSold = 0;
        public static decimal DiscountRate = 0.10m; // 10% discount


        // Static constructor
        static Car()
        {
            Console.WriteLine("[Static] Car class initialized. Welcome to the dealership!");
        }

        // Default constructor
        public Car()
        {
            Model = "Basic Model";
            Year = DateTime.Now.Year;
            Color = "White";
            Vin = "DEFAULT-VIN-" + Guid.NewGuid().ToString().Substring(0, 8);
            TotalCarsSold++;
        }

        // Parameterized constructor (overloading)
        public Car(string model, int year, string color)
        {
            Model = model;
            Year = year;
            Color = color;
            Vin = "VIN-" + Guid.NewGuid().ToString().Substring(0, 8);
            TotalCarsSold++;
        }


        // Copy Constructor
        public Car (Car existing)
        {
            Model = existing.Model;
            Year = existing.Year;
            Color = existing.Color;
            Vin = "COPY-" + existing.Vin; // Generate a new VIN based on the existing one
            TotalCarsSold++;
            Console.WriteLine("-> Copy of car created");
        }

        public void ShowDetails()
        {
            decimal price = 500000m; // Base price
            decimal finalPrice = price * (1 - DiscountRate); // Apply discount

            Console.WriteLine($"Model: {Model}, Year: {Year}, Color: {Color}");
            Console.WriteLine($"VIN: {Vin}");
            Console.WriteLine($"Final Price (after {DiscountRate*100}% discount): ₹{finalPrice:N0}");
            Console.WriteLine($"Total cars sold so far: {TotalCarsSold}");
            Console.WriteLine("--------------------------------------\n");
        }
    }
}
