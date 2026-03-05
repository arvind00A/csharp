### Fields (Data Members)

*Fields* are variables declared directly in a class or struct.
They store the data or state of an objects (instance fields) or the class itself (static fields).

** Types of Fields**
*1. Instance field*
*2. Static field*
*3. Readonly field*

**1. Instance field**

Instance fields are associated with an instance of a class. Each object created from the class has its own copy of the instance fields.
```csharp
class Person
{
	public string Name; // Instance field
	public int Age; // Instance field
}
```

*When to use instance fields?*
- Object-specific data: When you want to store data that is specific to each instance of a class, such as a person's name or age.


**2. Static field**

Shared across all instances of a class, static fields belong to the class itself rather than any particular object. They are accessed using the class name.
```csharp
class MathConstants
{
	public static double Pi = 3.14159; // Static field
}
```

*When to use static fields?*
- Shared data: When you want to store data that is common to all instances of a class, such as mathematical constants or configuration settings.


**3. Readonly field**

Can be set only in declaration or constructor, and cannot be modified afterward. This makes them ideal for values that should remain constant after initialization.
```csharp
class Circle
{
	public readonly double Radius; // Readonly field
	public Circle(double radius)
	{
		Radius = radius; // Assigning value in constructor
	}
}
```

*When to use readonly fields?*
- Immutable after creation: When you want to ensure that a field's value cannot be changed after it has been initialized, such as the radius of a circle or a person's birthdate.