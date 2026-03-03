### Methods and Parameters

**Methods**
A method is a block of code that performs a task.
-  It can:
	* Return nothing (void)
    * Return a value (int, string, bool, array, custom class, etc.)

```csharp
[access-modifier] [static?] returnType MethodName(parameters)
{
    // code
    return value;   // required unless void
}
```

**Parameter Passing**
C# passes parameters by value by default. 
But you can change behavior with ref and out.

- **Pass by Value (default)**: 

A copy of the variable is passed. 
* Copy of the value is sent
* Changes inside method do not affect original variable


```csharp
void Increment(int x)
{
    x++; // This does not affect the original variable
}
```

- **Pass by Reference (ref)**:
 
* Passes memory address (reference)
* Changes inside method affect original variable
* Variable must be initialized before passing

```csharp
void Increment(ref int x)
{
    x++; // This changes the original variable
}
```

- **out parameter**:

* Like ref, but:
    * Method must assign a value to the out parameter
    * Caller does not need to initialize the variable
    * Often used to return multiple values

```csharp
void GetValues(out int a, out int b)
{
    a = 10; // Must assign a value
    b = 20; // Must assign a value
}
```





