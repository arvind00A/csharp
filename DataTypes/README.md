## Fundamentals

**Date:** March 02, 2026  
**Focus:** Core building blocks of C# programming  
**Environment:** Visual Studio 2026 + .NET 10 (C# 14)

### 1. Variables & Data Types
 
C# is **strongly typed** — every variable must have a declared type at compile time.

**Main Built-in Data Types**

| Category          | Type          | Size (bytes) | Default Value | Example Declaration                     | Common Use Case                     |
|-------------------|---------------|--------------|---------------|-----------------------------------------|-------------------------------------|
| Integral          | `byte`        | 1            | 0             | `byte ageGroup = 25;`                   | Small non-negative numbers          |
| Integral          | `int`         | 4            | 0             | `int score = 0;`                        | General-purpose whole numbers       |
| Integral          | `long`        | 8            | 0L            | `long fileSize = 1024L;`                | Very large numbers                  |
| Floating-point    | `float`       | 4            | 0f            | `float temperature = 36.6f;`            | Graphics / approximate values       |
| Floating-point    | `double`      | 8            | 0.0           | `double pi = 3.14159265359;`            | Scientific & general calculations   |
| High-precision    | `decimal`     | 16           | 0m            | `decimal price = 19.99m;`               | Money, financial calculations       |
| Text              | `string`      | —            | null          | `string name = "Arvind Kumar";`         | Text data (immutable)               |
| Boolean           | `bool`        | 1            | false         | `bool isActive = true;`                 | True/false conditions               |
| Character         | `char`        | 2            | '\0'          | `char grade = 'A';`                     | Single Unicode character            |

**Syntax Examples**

```csharp
// Declaration + Initialization
int age = 28;
string fullName = "Arvind Kumar";
bool isLearning = true;
decimal salary = 75000.50m;
double height = 1.78;

// Declaration only (must assign before use)
int count;
count = 42;

// Constant (compile-time value)
const double Gravity = 9.81;

```
**Best Practices**
* Choose smallest type for memory efficiency.
* Initialize variables early to avoid bugs.
* Use var for readability when type is obvious.
* For large data, prefer value types or spans to reduce GC pressure.
* Test in your console app: Add debug outputs with Console.WriteLine().






