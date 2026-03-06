### Encapsulation

Encapsulation is one of the four pillars of OOP.  
It means bundling data (fields) and methods that operate on that data into a single unit (class), while restricting direct access to some components.  

**Main Goals**  
- Hide internal implementation details  
- Protect data from invalid states  
- Provide controlled access via public interface

**Key Tools in C#**  
- Access modifiers (`private`, `protected`, `public`, etc.)  
- Properties (getters & setters)  
- Private backing fields

**Access Modifiers Quick Reference**

| Modifier             | Accessible from                              | Typical Use Case                     |
|----------------------|----------------------------------------------|--------------------------------------|
| `public`             | Anywhere                                     | Public API, methods for outside use  |
| `private`            | Only inside the same class                   | Internal fields, helper methods      |
| `protected`          | Same class + derived classes                 | Members for inheritance              |
| `internal`           | Same assembly (project)                      | Friend classes in same project       |
| `protected internal` | Derived classes OR same assembly             | Rare, hybrid access                  |


**Properties – The Modern Way**

```csharp
class BankAccount
{
    // Private backing field (encapsulated data)
    private decimal _balance;

    // Full property with validation
    public decimal Balance
    {
        get => _balance;
        private set   // only class can set it
        {
            if (value < 0)
                throw new ArgumentException("Balance cannot be negative");
            _balance = value;
        }
    }

    // Auto-implemented property
    public string AccountHolder { get; set; }

    // Read-only property
    public string AccountInfo => $"Holder: {AccountHolder}, Balance: {_balance:C}";

    public void Deposit(decimal amount)
    {
        if (amount <= 0) throw new ArgumentException("Amount must be positive");
        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0 || amount > Balance)
            throw new ArgumentException("Invalid withdrawal");
        Balance -= amount;
    }
}
```


**Best Practices**
- Never expose public fields → always use properties  
- Use private set for read-only from outside  
- Validate in setters  
- Avoid over-encapsulation (too many private members with no access)



### Inheritance

Inheritance allows a class (derived / child) to inherit members (fields, properties, methods) from another class (base / parent).  
It supports **code reuse** and **"is-a"** relationships.

```csharp
class Animal                // Base class
{
    public string Name { get; set; }
    public virtual void Speak() => Console.WriteLine("...");
}

class Dog : Animal          // Derived class
{
    public override void Speak() => Console.WriteLine("Woof!");
}
```
** Types of Inheritance Supported in C# **

 1. Single
 2. Mutilevel
 3. Hierarchical
 4. Multiple (via interfaces)


** base and this keywords **

- `base` keyword use for parent class constructor and members calls.
- `this` keyword use for current class contructor and members calls.

```csharp
class Employee : Person
{
    public decimal Salary { get; set; }

    public Employee(string name, int age, decimal salary)
        : base(name, age)               // calls base constructor
    {
        this.Salary = salary;           // this = current class
    }

    public override void Introduce()
    {
        base.Introduce();               // calls base method
        Console.WriteLine($"Salary: {Salary:C}");
    }
}
```

**Important Notes**
- C# does not support multiple class inheritance (use interfaces instead)  
- virtual + override enables runtime polymorphism  
- sealed keyword prevents further inheritance



### Abstraction

Abstraction is one of the four pillars of OOP.  
It means hiding complex implementation details and showing only the essential features and behaviors that the user needs to know.  

The user interacts with **what** the object does, not **how** it does it.

**Main Goals**  
- Reduce complexity  
- Increase reusability  
- Allow focus on high-level design

**Two primary mechanisms in C#**  
1. Abstract classes  
2. Interfaces

## 1. Abstract Classes

**Key Characteristics**

- Declared with `abstract` keyword  
- **Cannot be instantiated** (`new AbstractClass()` → compile error)  
- Can contain:
  - abstract members (no implementation – derived classes **must** provide it)  
  - concrete members (with implementation – optional to override)  
  - fields, constructors, properties, events, etc.  
- Used when classes share common code and have an "is-a" relationship

**Syntax Example**

```csharp
// Abstract base class
abstract class Shape
{
    public string Color { get; set; } = "White";

    // Concrete method (shared behavior)
    public void DisplayColor()
    {
        Console.WriteLine($"Color: {Color}");
    }

    // Abstract method – MUST be implemented
    public abstract double CalculateArea();

    // Abstract property
    public abstract string ShapeType { get; }
}

// Concrete derived class
class Circle : Shape
{
    public double Radius { get; set; }

    public override double CalculateArea()
    {
        return Math.PI * Radius * Radius;
    }

    public override string ShapeType => "Circle";
}

// Another derived class
class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }

    public override double CalculateArea()
    {
        return Width * Height;
    }

    public override string ShapeType => "Rectangle";
}
```

## Interface

* Declared with `interface` keyword  
* Pure contract — only declarations (no fields, no constructors)  
* All members are implicitly `public` and `abstract`  
* Supports *multiple inheritance* (a class can implement many interfaces)  
* Since C# 8: default implementations are allowed

```csharp
interface IPrintable
{
    void Print();

    // Default implementation (optional)
    void PrintHeader()
    {
        Console.WriteLine("=== Document ===");
    }
}

interface ISaveable
{
    void Save(string filename);
}

class Report : IPrintable, ISaveable
{
    public string Title { get; set; }

    public void Print()
    {
        PrintHeader(); // using default method
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine("Content here...");
    }

    public void Save(string filename)
    {
        Console.WriteLine($"Saving report as {filename}");
    }
}
```

# Abstract Class vs Interface in C#

**Quick Overview**  
Both **abstract classes** and **interfaces** are used to achieve **abstraction** in C#, but they serve different purposes and have different capabilities.

This table shows the key differences (updated for C# 14 / .NET 10 in 2026).

| Feature                              | Abstract Class                                      | Interface                                          | Winner / When to Choose                          |
|--------------------------------------|-----------------------------------------------------|----------------------------------------------------|--------------------------------------------------|
| Keyword                              | `abstract class`                                    | `interface`                                        | —                                                |
| Can be instantiated?                 | No                                                  | No                                                 | —                                                |
| Can have fields / instance variables | Yes                                                 | No (only constants since C# 8)                     | Abstract Class                                   |
| Can have constructors                | Yes                                                 | No                                                 | Abstract Class                                   |
| Can have concrete methods            | Yes (fully implemented methods)                     | Yes (default methods since C# 8)                   | Abstract Class (more flexible)                   |
| Can have private/protected members   | Yes                                                 | No (all members are public)                        | Abstract Class                                   |
| Can have properties / indexers       | Yes                                                 | Yes (since C# 8 – can have get/set)                | Both                                             |
| Can have events                      | Yes                                                 | Yes                                                | Both                                             |
| Inheritance / Implementation         | Single inheritance only (`: BaseClass`)             | Multiple inheritance allowed (`: I1, I2, I3`)      | **Interface** (multiple)                         |
| Access modifiers on members          | Can be `public`, `private`, `protected`, `internal` | All members are implicitly `public`                | Abstract Class (more control)                    |
| When to use                          | Shared implementation + "is-a" relationship         | Define capability/contract + "can-do" behavior     | —                                                |
| Best for                             | Base classes with common code/logic                 | Microservices contracts, DI, plugins, capabilities | —                                                |
| Typical real-world examples          | `Stream`, `ControllerBase`, `Animal` base           | `IEnumerable<T>`, `IDisposable`, `IComparable<T>`  | —                                                |
| Default method support               | Yes (normal methods)                                | Yes (since C# 8 – `void Log() { ... }`)            | Both (modern C#)                                 |
| Can have static members              | Yes                                                 | Yes (since C# 8 – static fields/methods)           | Both                                             |
| Performance (tiny difference)        | Slightly faster (virtual dispatch + concrete code)  | Slightly slower (interface dispatch)               | Abstract Class (negligible)                      |

### Summary – Choose Based on Purpose

| Goal / Need                                  | Recommended Choice          | Reason                                                                 |
|----------------------------------------------|-----------------------------|------------------------------------------------------------------------|
| Need shared code / fields / constructors     | Abstract Class              | Interfaces cannot provide implementation or state                      |
| Need multiple inheritance                    | Interface                   | C# classes can only inherit from one base class                        |
| Define pure contract / behavior              | Interface                   | Cleaner, no implementation baggage                                     |
| Partial implementation + extension points    | Abstract Class              | Can provide default behavior while forcing overrides                   |
| Dependency Injection / loose coupling        | Interface                   | Most DI containers work best with interfaces                           |
| "is-a" relationship (Dog is an Animal)       | Abstract Class              | Natural inheritance hierarchy                                          |
| "can-do" capability (Printable, Flyable)     | Interface                   | Multiple abilities can be added                                        |



### Polymorphism

Polymorphism means "one name, many forms".  
The same method call can behave differently depending on the actual object type.

**Two Types in C#**

1. **Compile-time (Static) Polymorphism**  
   - Method overloading  
   - Operator overloading

```csharp
class MathOperations
{
    public int Add(int a, int b) => a + b;
    public double Add(double a, double b) => a + b;
    public string Add(string a, string b) => a + b;
}
```

2. **Runtime (Dynamic) Polymorphism**
    - Method overriding using virtual + override

```csharp
class Animal
{
    public virtual void Speak() => Console.WriteLine("Some sound...");
}

class Cat : Animal
{
    public override void Speak() => Console.WriteLine("Meow!");
}

class Dog : Animal
{
    public override void Speak() => Console.WriteLine("Woof!");
}

// Polymorphism in action
Animal[] animals = { new Cat(), new Dog(), new Animal() };
foreach (var animal in animals)
{
    animal.Speak();     // Meow! / Woof! / Some sound...
}
```

**Key Points**
- Polymorphism works through *base class reference* pointing to *derived class object* .
- `virtual` in base, `override` in derived.  
- `new` keyword hides (not overrides) – avoid for polymorphism.




### Indexers 

Indexers allow objects to be indexed like arrays using `[]` syntax.  
They make classes behave like collections.


```csharp
class Scoreboard
{
    private int[] scores = new int[10];

    public int this[int index]
    {
        get => scores[index];
        set => scores[index] = value;
    }

    // Overloaded indexer
    public string this[string playerName]
    {
        get => $"Score for {playerName} not tracked"; // dummy
    }
}

// Usage
var board = new Scoreboard();
board[0] = 85;
Console.WriteLine(board[0]);          // 85
```