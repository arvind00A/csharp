### Serialization 🟣

> Binary Serialization · JSON Serialization · XML Serialization

---

## Table of Contents
- [What is Serialization?](#what-is-serialization)
- [JSON Serialization (System.Text.Json)](#json-serialization-systemtextjson)
- [JSON with Newtonsoft.Json](#json-with-newtonsoftjson)
- [XML Serialization](#xml-serialization)
- [Binary Serialization](#binary-serialization)
- [Format Comparison](#format-comparison)
- [Quick Reference](#quick-reference)

---

## What is Serialization?

**Serialization** = converting an object to a storable/transmittable format (string, bytes).
**Deserialization** = converting that format back to an object.

```
C# Object  ──serialize──►  JSON / XML / Binary  ──►  File / DB / Network
C# Object  ◄─deserialize─  JSON / XML / Binary  ◄──  File / DB / Network
```

---

## JSON Serialization (System.Text.Json)

Built-in since .NET 3. Fast, low-allocation, and no extra packages needed.

### Attributes

```csharp
using System.Text.Json;
using System.Text.Json.Serialization;

public class Person
{
    public string Name  { get; set; } = "";
    public int    Age   { get; set; }
    public string Email { get; set; } = "";

    [JsonIgnore]                         // exclude from JSON output
    public string Password { get; set; } = "";

    [JsonPropertyName("first_name")]     // custom JSON key name
    public string FirstName { get; set; } = "";

    [JsonInclude]                        // include even if not public set
    public string Internal { get; } = "read-only";

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Status Status { get; set; }   // serialize enum as string, not int
}

public enum Status { Active, Inactive, Suspended }
```

### Serialize (object → JSON string)

```csharp
var person = new Person { Name = "Alice", Age = 30, Email = "alice@ex.com" };

// Basic — compact output
string json = JsonSerializer.Serialize(person);
// {"Name":"Alice","Age":30,"Email":"alice@ex.com","first_name":""}

// With options
var opts = new JsonSerializerOptions
{
    WriteIndented          = true,                        // pretty print
    PropertyNamingPolicy   = JsonNamingPolicy.CamelCase,  // Name → name
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,  // skip nulls
    PropertyNameCaseInsensitive = true,                   // case-insensitive deserialize
    Converters = { new JsonStringEnumConverter() }        // enum → string
};

string pretty = JsonSerializer.Serialize(person, opts);
/* Output:
{
  "name": "Alice",
  "age": 30,
  "email": "alice@ex.com"
}
*/

// Collections
var list = new List<Person> { person };
string jsonArr = JsonSerializer.Serialize(list, opts);
```

### Deserialize (JSON string → object)

```csharp
string src = """{"Name":"Bob","Age":25,"Email":"bob@ex.com"}""";

// Basic
Person? bob = JsonSerializer.Deserialize<Person>(src);
Console.WriteLine(bob?.Name);   // Bob

// With options (case-insensitive key matching)
Person? bob2 = JsonSerializer.Deserialize<Person>(src, opts);

// List
string listJson = """[{"Name":"Alice","Age":30},{"Name":"Bob","Age":25}]""";
List<Person>? people = JsonSerializer.Deserialize<List<Person>>(listJson);

// Dictionary
string dictJson = """{"key1":"value1","key2":"value2"}""";
Dictionary<string, string>? dict =
    JsonSerializer.Deserialize<Dictionary<string, string>>(dictJson);
```

### Async Serialize / Deserialize (for large data / streams)

```csharp
// Write to file asynchronously
await using var writeStream = File.OpenWrite("data.json");
await JsonSerializer.SerializeAsync(writeStream, person, opts);

// Read from file asynchronously
await using var readStream = File.OpenRead("data.json");
Person? loaded = await JsonSerializer.DeserializeAsync<Person>(readStream, opts);

// HTTP response body
using HttpClient client = new();
var stream = await client.GetStreamAsync("https://api.example.com/users");
var users = await JsonSerializer.DeserializeAsync<List<Person>>(stream, opts);
```

---

## JSON with Newtonsoft.Json

The classic, battle-tested library. Install via NuGet: `Newtonsoft.Json`.

```csharp
using Newtonsoft.Json;

// ── Attributes ────────────────────────────────────────────────
public class Product
{
    public string Name    { get; set; } = "";

    [JsonIgnore]                           // exclude from output
    public string Secret  { get; set; } = "";

    [JsonProperty("product_price")]        // custom key
    public decimal Price  { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Category { get; set; }
}

// ── Serialize ─────────────────────────────────────────────────
var product = new Product { Name = "Laptop", Price = 999m };
string json  = JsonConvert.SerializeObject(product);
string pretty = JsonConvert.SerializeObject(product, Formatting.Indented);

// ── Deserialize ───────────────────────────────────────────────
string src     = """{"Name":"Phone","product_price":599}""";
Product? phone = JsonConvert.DeserializeObject<Product>(src);

List<Product>? products =
    JsonConvert.DeserializeObject<List<Product>>("""[...]""");

// ── JsonSerializerSettings ────────────────────────────────────
var settings = new JsonSerializerSettings
{
    NullValueHandling     = NullValueHandling.Ignore,
    DefaultValueHandling  = DefaultValueHandling.Ignore,
    Formatting            = Formatting.Indented,
    ContractResolver      = new CamelCasePropertyNamesContractResolver()
};
string result = JsonConvert.SerializeObject(product, settings);
```

| Feature | System.Text.Json | Newtonsoft.Json |
|---|---|---|
| Performance | ⚡ Faster, low alloc | Slower |
| Built-in | ✅ .NET 3+ (no NuGet) | NuGet required |
| Default key case | Exact match | Case-insensitive |
| Flexibility | Stricter | More features |
| Legacy support | ❌ Less | ✅ More |

> 💡 Prefer **System.Text.Json** for new projects. Use **Newtonsoft.Json** when you need advanced features (custom converters, `$type` polymorphism, etc.) or when working with existing codebases.

---

## XML Serialization

Used in legacy systems, SOAP web services, and Office file formats.

```csharp
using System.Xml.Serialization;
using System.IO;

// ── Attributes ────────────────────────────────────────────────
[XmlRoot("employee")]                    // root element name in XML
public class Employee
{
    [XmlAttribute]                       // becomes attribute: <employee Id="1">
    public int Id       { get; set; }

    [XmlElement("full_name")]            // custom element name
    public string Name  { get; set; } = "";

    [XmlIgnore]                          // exclude entirely
    public string Secret { get; set; } = "";

    [XmlArray("skills")]                 // wrapping array element
    [XmlArrayItem("skill")]              // each item element name
    public List<string> Skills { get; set; } = new();
}

// ── Serialize to file ─────────────────────────────────────────
XmlSerializer xs = new(typeof(Employee));

var emp = new Employee
{
    Id = 1, Name = "Alice",
    Skills = new() { "C#", "SQL", "Docker" }
};

using (var sw = new StreamWriter("employee.xml"))
    xs.Serialize(sw, emp);

/* Result:
<?xml version="1.0" encoding="utf-8"?>
<employee Id="1">
  <full_name>Alice</full_name>
  <skills>
    <skill>C#</skill>
    <skill>SQL</skill>
    <skill>Docker</skill>
  </skills>
</employee>
*/

// ── Deserialize from file ─────────────────────────────────────
using (var sr = new StreamReader("employee.xml"))
{
    Employee? loaded = (Employee?)xs.Deserialize(sr);
    Console.WriteLine(loaded?.Name);   // Alice
}

// ── Serialize a list (wrap in a container class) ──────────────
[XmlRoot("employees")]
public class EmployeeList
{
    [XmlElement("employee")]
    public List<Employee> Items { get; set; } = new();
}

XmlSerializer listXs = new(typeof(EmployeeList));
using (var sw = new StreamWriter("employees.xml"))
    listXs.Serialize(sw, new EmployeeList { Items = new() { emp } });

// ── Serialize to string ───────────────────────────────────────
using var ms = new MemoryStream();
xs.Serialize(ms, emp);
string xmlStr = System.Text.Encoding.UTF8.GetString(ms.ToArray());
```

> ⚠️ XmlSerializer requires a **public parameterless constructor** on the class. All serialized properties must be **public** with both getter and setter.

---

## Binary Serialization

Smallest file size, fastest read/write. Not human-readable. Must read in the same order as written.

```csharp
using System.IO;

// ── BinaryWriter — write primitives ──────────────────────────
using (var fs = File.Create("data.bin"))
using (var bw = new BinaryWriter(fs))
{
    bw.Write(42);            // int     — 4 bytes
    bw.Write("Alice");       // string  — length-prefixed UTF-8
    bw.Write(3.14);          // double  — 8 bytes
    bw.Write(true);          // bool    — 1 byte
    bw.Write(99.99m);        // decimal — 16 bytes
    bw.Write('A');           // char    — 2 bytes
}
// Much smaller than JSON/XML equivalent

// ── BinaryReader — read in SAME ORDER ────────────────────────
using (var fs = File.OpenRead("data.bin"))
using (var br = new BinaryReader(fs))
{
    int     id     = br.ReadInt32();
    string  name   = br.ReadString();
    double  pi     = br.ReadDouble();
    bool    active = br.ReadBoolean();
    decimal price  = br.ReadDecimal();
    char    grade  = br.ReadChar();

    Console.WriteLine($"id={id}, name={name}, pi={pi}");
}

// ── Write/read byte arrays ────────────────────────────────────
using (var fs = File.Create("raw.bin"))
using (var bw = new BinaryWriter(fs))
{
    byte[] data = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F };
    bw.Write(data.Length);   // write length first
    bw.Write(data);          // then data
}

using (var fs = File.OpenRead("raw.bin"))
using (var br = new BinaryReader(fs))
{
    int    len  = br.ReadInt32();
    byte[] data = br.ReadBytes(len);
}

// ── Modern alternatives (NuGet packages) ─────────────────────
// MessagePack — fast binary, cross-platform, schema-based
// [MessagePackObject]
// [Key(0)] public string Name { get; set; }
// byte[] bin = MessagePackSerializer.Serialize(obj);
// var obj2   = MessagePackSerializer.Deserialize<T>(bin);

// MemoryPack — fastest .NET binary serializer (2023+)
// [MemoryPackable]
// partial class MyClass { [MemoryPackOrder(0)] public int Id { get; set; } }
```

---

## Format Comparison

| Format | Human Readable | File Size | Speed | Use Case |
|---|---|---|---|---|
| JSON (STJ) | ✅ Yes | Medium | ⚡ Fast | APIs, config, modern apps |
| JSON (Newtonsoft) | ✅ Yes | Medium | Moderate | Legacy, advanced features |
| XML | ✅ Yes | Large (verbose) | Slow | SOAP, Office, legacy |
| Binary (BinaryWriter) | ❌ No | Smallest | Fastest | Games, IoT, files |
| MessagePack | ❌ No | Small | Very fast | Microservices, caching |

```csharp
// Same data — size comparison
// JSON:         {"Name":"Alice","Age":30}         → ~24 bytes
// XML:          <person><Name>Alice</Name>...     → ~50+ bytes
// Binary:       Alice + 4 bytes int               → ~10 bytes
```

---

## Quick Reference

```
Serialization
│
├── JSON — System.Text.Json
│   ├── JsonSerializer.Serialize(obj, opts)       → string
│   ├── JsonSerializer.Deserialize<T>(json, opts) → T?
│   ├── await JsonSerializer.SerializeAsync(stream, obj)
│   ├── [JsonIgnore]         → exclude property
│   ├── [JsonPropertyName]   → rename property
│   └── JsonNamingPolicy.CamelCase → camelCase output
│
├── JSON — Newtonsoft.Json
│   ├── JsonConvert.SerializeObject(obj)           → string
│   ├── JsonConvert.DeserializeObject<T>(json)     → T?
│   ├── [JsonIgnore] / [JsonProperty("key")]
│   └── Formatting.Indented → pretty print
│
├── XML — XmlSerializer
│   ├── new XmlSerializer(typeof(T))
│   ├── xs.Serialize(streamWriter, obj)
│   ├── xs.Deserialize(streamReader) → object?
│   ├── [XmlRoot("name")]     → root element
│   ├── [XmlElement("name")]  → child element
│   ├── [XmlAttribute]        → XML attribute
│   └── [XmlIgnore]           → exclude
│
└── Binary — BinaryWriter / BinaryReader
    ├── new BinaryWriter(fileStream)
    ├── bw.Write(int/string/double/bool/...)
    ├── new BinaryReader(fileStream)
    ├── br.ReadInt32() / ReadString() / ReadDouble()
    └── ⚠️ MUST read in same order as written!
```

```csharp
// One-line cheat sheet
string  json  = JsonSerializer.Serialize(obj, opts);
T?      obj   = JsonSerializer.Deserialize<T>(json, opts);
xs.Serialize(writer, obj);        // XML
T? obj2 = (T?)xs.Deserialize(reader); // XML
bw.Write(42); bw.Write("name");   // Binary write
int n = br.ReadInt32();           // Binary read (same order!)
```

---