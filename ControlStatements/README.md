### Control Statements – if-else & switch

**if – else if – else**

**Definition**  
Executes code blocks conditionally based on boolean expressions.

**Syntax**

```csharp
if (condition1)
{
    // runs if condition1 == true
}
else if (condition2)
{
    // runs if condition1 == false AND condition2 == true
}
else
{
    // runs if all previous conditions are false
}
 ```
 
 **Best practice**
1. Avoid deep nesting -> prefer early return or guard clauses
