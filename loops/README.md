### Jump Statements – break, continue, return

**Definition**  
Keywords that alter the normal flow of execution in loops or methods.

| Statement  | Where used         | Effect                                              | Typical Use Case                          |
|------------|--------------------|-----------------------------------------------------|-------------------------------------------|
| `break`    | loops, switch      | Immediately exits the nearest loop or switch        | Stop searching once item is found         |
| `continue` | loops only         | Skips rest of current iteration → next iteration    | Skip invalid / unwanted items             |
| `return`   | methods            | Exits the method (can return a value)               | Early exit on error or when done          |

**Examples**

```csharp
// break
for (int i = 1; i <= 20; i++)
{
    if (i % 7 == 0) { Console.WriteLine(i); break; }
}

// continue
for (int i = 1; i <= 10; i++)
{
    if (i % 2 == 0) continue;
    Console.Write(i + " ");           // prints odds only
}

// return in method
string Validate(string input)
{
    if (string.IsNullOrEmpty(input)) return "Empty!";
    return "Valid";
}
