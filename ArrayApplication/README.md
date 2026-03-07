
## Table of Contents
- [Arrays](#arrays)
  - [Single-Dimensional Arrays](#single-dimensional-arrays)
  - [Array Methods](#array-methods)
  - [Using the Array Class](#using-the-array-class)
  - [Multi-Dimensional & Jagged Arrays](#multi-dimensional--jagged-arrays)

---

## Arrays

### Single-Dimensional Arrays

An array stores a **fixed number of elements** of the **same type** in contiguous memory. Elements are accessed via a zero-based index.

```csharp
// Declare and allocate
int[] numbers = new int[5];           // [0, 0, 0, 0, 0]

// Declare with initializer
int[] scores = { 90, 85, 78, 92, 88 };

// Access by index (zero-based)
Console.WriteLine(scores[0]);   // 90
Console.WriteLine(scores[4]);   // 88

// Modify an element
scores[2] = 95;
```

**Iterating over arrays:**

```csharp
string[] fruits = { "Apple", "Banana", "Cherry" };

// for loop — when you need the index
for (int i = 0; i < fruits.Length; i++)
    Console.WriteLine($"[{i}] {fruits[i]}");

// foreach loop — simpler, read-only
foreach (string fruit in fruits)
    Console.WriteLine(fruit);
```

> 💡 **Key Point:** Arrays are **fixed in size**. Once created, you can't add or remove elements. Use `List<T>` for a resizable collection.

---

### Array Methods

**Length & properties:**

```csharp
int[] nums = { 4, 2, 8, 1, 9, 3 };

Console.WriteLine(nums.Length);       // 6
Console.WriteLine(nums.Rank);         // 1 (number of dimensions)
Console.WriteLine(nums.GetLength(0)); // 6
```

**Sort & Reverse:**

```csharp
int[] nums = { 4, 2, 8, 1, 9, 3 };

Array.Sort(nums);
// [1, 2, 3, 4, 8, 9]

Array.Reverse(nums);
// [9, 8, 4, 3, 2, 1]

Console.WriteLine(string.Join(", ", nums));
// Output: 9, 8, 4, 3, 2, 1
```

**Other useful methods:**

| Method | Description |
|--------|-------------|
| `Array.IndexOf()` | Find index of a value (-1 if not found) |
| `Array.Copy()` | Copy elements to another array (partial or full) |
| `Array.Clear()` | Reset elements to default (0 for int, null for string) |
| `Array.Exists()` | Check if any element matches a predicate |
| `Array.Find()` | Get first matching element |

```csharp
string[] names = { "Alice", "Bob", "Charlie" };

int idx     = Array.IndexOf(names, "Bob");                      // 1
bool hasLong = Array.Exists(names, n => n.Length > 4);          // true
string found = Array.Find(names, n => n.StartsWith("C"));       // "Charlie"
```

---

### Using the Array Class

The static `Array` class (from `System`) provides powerful utilities for arrays.

**Array.Copy & Array.Clone:**

```csharp
int[] original = { 1, 2, 3, 4, 5 };

// Clone: full copy
int[] clone = (int[])original.Clone();

// Array.Copy: partial copy
int[] dest = new int[3];
Array.Copy(original, 1, dest, 0, 3);
// dest = [2, 3, 4]  (start at index 1, copy 3 items)
```

**Array.BinarySearch:**

For **sorted** arrays, binary search is O(log n) vs O(n) for linear search.

```csharp
int[] sorted = { 10, 20, 30, 40, 50 };

int idx = Array.BinarySearch(sorted, 30);
Console.WriteLine(idx);  // 2

// Negative result means not found
int notFound = Array.BinarySearch(sorted, 25);
Console.WriteLine(notFound);  // negative value
```

> ⚠️ Always call `Array.Sort()` **before** `Array.BinarySearch()`. Binary search only works on sorted arrays.

---

### Multi-Dimensional & Jagged Arrays

**2D Array (Matrix):**

```csharp
// Declare a 3×3 matrix
int[,] matrix = {
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 }
};

Console.WriteLine(matrix[1, 2]);  // 6  (row 1, col 2)

// Iterate with nested loops
for (int r = 0; r < matrix.GetLength(0); r++)
    for (int c = 0; c < matrix.GetLength(1); c++)
        Console.Write(matrix[r, c] + " ");
```

**Jagged Array (array of arrays):**

Each row can have a **different length**.

```csharp
int[][] jagged = new int[3][];
jagged[0] = new int[] { 1, 2 };
jagged[1] = new int[] { 3, 4, 5, 6 };
jagged[2] = new int[] { 7 };

Console.WriteLine(jagged[1][2]);  // 5
```

| | Multi-Dimensional `[,]` | Jagged `[][]` |
|---|---|---|
| Shape | Fixed rectangular | Variable row lengths |
| Memory | Contiguous (faster) | Separate arrays per row |
| Use case | Matrices, grids | Triangle tables, sparse data |

---
