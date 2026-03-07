## Strings

### String Operations

Strings in C# are **immutable** — every operation returns a *new* string.

**Substring:**

```csharp
string text = "Hello, World!";

string s1 = text.Substring(7);       // "World!"
string s2 = text.Substring(0, 5);    // "Hello"

// C# 8+ range syntax
string s3 = text[7..];               // "World!"
string s4 = text[..5];               // "Hello"
```

**Replace, Trim, Case:**

```csharp
string s = "I love cats and cats are great";

string r = s.Replace("cats", "dogs");
// "I love dogs and dogs are great"

string trimmed = "  hello  ".Trim();  // "hello"
string upper   = s.ToUpper();
string lower   = s.ToLower();
```

**Search methods:**

```csharp
string sentence = "The quick brown fox";

int pos  = sentence.IndexOf("quick");       // 4
int last = sentence.LastIndexOf("o");       // 17
bool has = sentence.Contains("fox");        // true
bool sw  = sentence.StartsWith("The");      // true
bool ew  = sentence.EndsWith("fox");        // true

string[] words = sentence.Split(' ');       // ["The","quick","brown","fox"]
string joined  = string.Join("-", words);   // "The-quick-brown-fox"
```

---

### String Interpolation ($)

Prefix a string with `$` to embed variables and expressions directly inside `{}`.

**Basic usage:**

```csharp
string name = "Alice";
int age = 30;

// Old way (concatenation)
string old = "Hello, " + name + "! You are " + age + " years old.";

// New way (interpolation) ✅
string msg = $"Hello, {name}! You are {age} years old.";

// With expressions
string info = $"Next year you'll be {age + 1}.";

// With method calls
string shout = $"HELLO, {name.ToUpper()}!";  // "HELLO, ALICE!"
```

**Format specifiers:**

```csharp
double price = 1234.5678;
DateTime now = DateTime.Now;

Console.WriteLine($"Price: {price:C2}");          // $1,234.57
Console.WriteLine($"Value: {price:F2}");          // 1234.57
Console.WriteLine($"Today: {now:dd/MM/yyyy}");    // e.g. 07/03/2026

// Alignment / padding
Console.WriteLine($"|{name,10}|");   // |     Alice|  (right-align in 10 chars)
Console.WriteLine($"|{name,-10}|");  // |Alice     |  (left-align)
```

---

### String vs StringBuilder

**The problem with `string` in loops:**

```csharp
// BAD — creates a new string object every iteration 🐢
string result = "";
for (int i = 0; i < 10000; i++)
    result += $"Line {i}\n";   // 10,000 allocations!
```

**`StringBuilder` — the solution:**

```csharp
using System.Text;

// GOOD — single mutable buffer 🚀
var sb = new StringBuilder();

for (int i = 0; i < 10000; i++)
    sb.AppendLine($"Line {i}");

string result = sb.ToString();  // Build the string once at the end

// Other StringBuilder methods
sb.Insert(0, "START\n");    // Insert at position
sb.Replace("foo", "bar");   // Replace in buffer
sb.Remove(0, 6);            // Remove characters
sb.Clear();                 // Empty the buffer
Console.WriteLine(sb.Length); // Current character count
```

**When to use each:**

| | `string` | `StringBuilder` |
|---|---|---|
| Immutable | ✅ Yes | ❌ No (mutable) |
| Good for | Reading, searching, comparing | Building in loops, many modifications |
| Performance | Slow in loops | Fast for building |
| Namespace | `System` | `System.Text` |

---