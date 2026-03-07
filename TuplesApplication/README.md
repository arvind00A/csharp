## Tuples

A **tuple** groups multiple values without creating a dedicated class.

**Basic tuple syntax:**

```csharp
// Positional (Item1, Item2...)
(string, int) person = ("Alice", 30);
Console.WriteLine(person.Item1);  // "Alice"
Console.WriteLine(person.Item2);  // 30

// Named tuple fields (recommended) ✅
(string Name, int Age) user = ("Bob", 25);
Console.WriteLine(user.Name);  // "Bob"
Console.WriteLine(user.Age);   // 25
```

**Returning multiple values from a method:**

```csharp
static (int Min, int Max, double Avg) GetStats(int[] nums)
{
    return (nums.Min(), nums.Max(), nums.Average());
}

int[] data = { 3, 7, 1, 9, 4 };
var stats = GetStats(data);

Console.WriteLine($"Min: {stats.Min}, Max: {stats.Max}, Avg: {stats.Avg:F1}");
// Min: 1, Max: 9, Avg: 4.8
```

**Deconstruction — extract into separate variables:**

```csharp
var (min, max, avg) = GetStats(data);
Console.WriteLine($"Min={min} Max={max}");

// Discard values you don't need with _
var (_, max2, _) = GetStats(data);
```

**When to use what:**

| Use `Tuple` when... | Use a `class`/`struct` when... |
|---|---|
| Returning 2–3 values from a method | Complex data with behavior/methods |
| Temporary grouping of data | Reused across multiple places in the codebase |
| Quick key-value pairs | Needs XML documentation or serialization |

---