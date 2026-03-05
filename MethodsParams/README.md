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

 **Pass by Reference (ref)**:
 
* Passes memory address (reference)
* Changes inside method affect original variable
* Variable must be initialized before passing

```csharp
void Increment(ref int x)
{
    x++; // This changes the original variable
}
```

 **out parameter**:

- Like ref, but:
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

**Optional Parameters**:

* Parameters with default values
* Must appear *after* all required parameters
* Caller can omit them, and default value will be used
```csharp
void SendEmail(string to, string subject = "No subject", string body = "Hello!")
{
    // code to send email
}

SendEmail("exmple@gmail.com"); // Uses default subject and body
SendEmail("team@gmail.com", "Meeting Reminder");    // overrides default subject, uses default body
```
### Rules for Parameters:
 * 1. Optional parameters must be defined after all required parameters.
 * 2. Optional parameters must have default values.
 * 3. When calling a method, you can omit optional parameters, and their default values will be used.
 * 4. If you provide a value for an optional parameter, it will override the default value.
 * 5. Optional parameters can be of any type (e.g., string, int, bool, etc.).
 * 6. You can have multiple optional parameters in a method, but they must all come after required parameters.


 ** Named arguments **:
 
 * Specify arguments by parameter name (ignores order) when calling a method.
 * Improves readability, especially with multiple parameters.

 ```csharp
 void RegisterUser(string name, int age, string email = "", bool isPremium = false)
 {
     Console.WriteLine($"Name: {name}, Age: {age}, Premium: {isPremium}, Email: {email}");
 }

 RegisterUser("Arvind", 23, "arvind@gmial.com", true); // Positional arguments
 RegisterUser(name: "Arvind", age: 23, isPremium: true); // Named arguments, email uses default value
 RegisterUser("Arvind", 23, email: "arvind@gmail.com"); // Mix of positional and named arguments, isPremium uses default value
 ```

 ** Best practices **:
 * Use named arguments when:
    - Method has >3 parameters.
    - Many optional
    - Improves readability (self-documenting code).
 * Avoid mixing positional and named arguments in the same method call for clarity.


 