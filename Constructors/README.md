### Constructors

 A *constructor* is a special method that is *automatically called* when an object of a class is created (using new).

 Its job is to initialize the object's state (fields/properties).
    - * Name = same as class name
    - No return type (not even void)
    - Can be overloaded (multiple constructors with different parameters)
    - Called only once per object lifetime


** Default Constructor **

Constructor with no parameters.

-* If you don't write any constructor, C# automatically provides an empty default constructor.

```csharp
class Car
{
    // Default constructor (explicit version)
    public car()
    {
        Console.WriteLine("Default constructor called");
        Model = "Unknown";
        Year = 2000;
    }

    public string Model { get; set; }
    public int Year { get; set; }
}
```

** Parameterized Constructor **
 Constructor that accepts *parameters* to initialize fields with meaningful values at creation time.

 ```csharp
 class Car
 {
    public string Model { get; set; }
    public int Year { get; set; }

    // Parameterized constructor
    public Car(string model, int year)
    {
        Model = model;
        Year = year;
        Console.WriteLine($"Parameterized constructor: {Model} ({Year})");
    }
}

```


** Constructor Overloading **
 A class can have multiple constructors with different parameter lists (number, type, or order).
 ```csharp
 class Car
 {
    public string Model { get; set; }
    public int Year { get; set; }
    public string Color { get; set; }

    // Overload 1: no params
    public Car()
    {
        Model = "Unknown";
        Year = 2000;
        Color = "White";
    }

    // Overload 2: model + year
    public Car(string model, int year)
    {
        Model = model;
        Year = year;
        Color = "Silver"; // default color
    }

    // Overload 3: full info
    public Car(string model, int year, string color)
    {
        Model = model;
        Year = year;
        Color = color;
    }
}
```

** Constructor Chaining **
 One constructor can call another constructor in the same class using the `this` keyword to avoid code duplication.
 ```csharp
 class Car
 {
    public string Model { get; set; }
    public int Year { get; set; }
    public string Color { get; set; }

    // Default constructor
    public Car() : this("Unknown", 2000, "White") // calls the full constructor
    {
    }

    // Parameterized constructor (model + year)
    public Car(string model, int year) : this(model, year, "Silver") // calls the full constructor
    {
    }

    // Full parameterized constructor
    public Car(string model, int year, string color)
    {
        Model = model;
        Year = year;
        Color = color;
    }
}
```

** Static Constructor **
 *Static Constructor*
    - Runs *once* per class (before any instance is created or static members is accessed)
    - No parameters, no access modifier (implicitly private)
    - Used to initialize *static fields* or perform one-time setup

```csharp
class Car
{
    public static int TotalCars { get; private set; }
    // Static constructor
    static Car()
    {
        TotalCars = 0;
        Console.WriteLine("Static constructor called");
    }
    public Car()
    {
        TotalCars++;
    }
}
```

** Copy Constructor **
 A constructor that creates a new object as a copy of an existing object.
 
 *Note: C# does not have built-in support for copy constructors like C++, but you can implement it manually.*

```csharp
class Car
{
    public string Model { get; set; }
    public int Year { get; set; }

    // Copy constructor
    public Car(Car other)
    {
        Model = other.Model;
        Year = other.Year;
    }
}
```
