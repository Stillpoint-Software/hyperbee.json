
# Hyperbee.Json

`Hyperbee.Json` is a high-performance JSONPath parser for .NET, that supports both `JsonElement` and `JsonNode`.  
The library is designed to be quick and extensible, allowing support for other JSON document types and functions.

## Features

- **High Performance:** Optimized for performance and efficiency.
- **Supports:** `JsonElement` and `JsonNode`.
- **Extensible:** Easily extended to support additional JSON document types and functions.
- **`IEnumerable` Results:** Deferred execution queries with `IEnumerable`.
- **Conformant:** Adheres to the JSONPath Specification [RFC 9535](https://www.rfc-editor.org/rfc/rfc9535.html). 

## JSONPath Consensus

Hyperbee.Json aims to follow the RFC and to support the [JSONPath consensus](https://cburgmer.github.io/json-path-comparison) 
when the RFC is unopinionated. When the RFC is unopinionated and where the consensus is ambiguous or not aligned with our 
performance and usability goals, we may deviate. Our goal is always to provide a robust and performant library while  
strengthening our alignment with the RFC.

## Installation

Install via NuGet:

```bash
dotnet add package Hyperbee.Json
```

## Usage

### Basic Examples

#### Selecting a Single Element

```csharp
using Hyperbee.JsonPath;
using System.Text.Json;

var json = """
{ 
  "store": { 
    "book": [ 
      { "category": "fiction" }, 
      { "category": "science" } 
    ] 
  } 
}
""";

var root = JsonDocument.Parse(json);
var result = JsonPath.Select(root, "$.store.book[0].category");

Console.WriteLine(result.First()); // Output: "fiction"
```

#### Selecting Multiple Elements

```csharp
using Hyperbee.JsonPath;
using System.Text.Json;

var json = """
{ 
  "store": { 
    "book": [
      { "category": "fiction" }, 
      { "category": "science" } 
    ] 
  } 
}
""";

var root = JsonDocument.Parse(json);
var result = JsonPath.Select(root, "$.store.book[*].category");

foreach (var item in result)
{
    Console.WriteLine(item); // Output: "fiction" and "science"
}
```

#### Filtering

```csharp
using Hyperbee.JsonPath;
using System.Text.Json;

var json = """
{ 
  "store": { 
    "book": [
      { 
        "category": "fiction",
        "price": 10  
      }, 
      { 
        "category": "science",
        "price": 15  
      } 
    ] 
  } 
}
""";

var root = JsonDocument.Parse(json);
var result = JsonPath.Select(root, "$.store.book[?(@.price > 10)]");

foreach (var item in result)
{
    Console.WriteLine(item); // Output: { "category": "science", "price": 15 }
}
```

#### Working with (JsonElement, Path) pairs
```csharp
using Hyperbee.JsonPath;
using System.Text.Json;

var json = """
{ 
  "store": { 
    "book": [
      { "category": "fiction" }, 
      { "category": "science" } 
    ] 
  } 
}
""";

var root = JsonDocument.Parse(json);
var result = JsonPath.SelectPath(root, "$.store.book[0].category");

var (node, path) = result.First();

Console.WriteLine(node); // Output: "fiction"
Console.WriteLine(path); // Output: "$.store.book[0].category
```

#### Working with JsonNode

```csharp
using Hyperbee.JsonPath;
using System.Text.Json.Nodes;

var json = """
{ 
  "store": { 
    "book": [
      { "category": "fiction" }, 
      { "category": "science" } 
    ] 
  } 
}
""";

var root = JsonNode.Parse(json);
var result = JsonPath.Select(root, "$.store.book[0].category");

Console.WriteLine(result.First()); // Output: "fiction"
```

## JSONPath Syntax Reference

Here's a quick reference for JSONPath syntax:

| JSONPath                                     | Description                                                
|:---------------------------------------------|:-----------------------------------------------------------
| `$`                                          | Root node                                    
| `@`                                          | Current node                                 
| `.<name>`, `.'<name>'`, or `."<name>"`       | Object member dot operator
| `[<name>]`, or `['<name>']`, or `["<name>"]` | Object member subscript operator
| `[<index]`                                   | Array access operator
| `[,]`                                        | Union operator
| `[start:end:step]`                           | Array slice operator
| `*`, or `[*]`                                | Wildcard 
| `..`                                         | Recursive descent  
| `?<expr>`                                    | Filter selector

JSONPath expressions refer to a JSON structure in the same way as XPath expressions 
are used in combination with an XML document. JSONPath assumes the name `$` is assigned 
to the root level object.

JSONPath expressions can use dot-notation:

    $.store.book[0].title

or bracket-notation:

    $['store']['book'][0]['title']

JSONPath allows the wildcard symbol `*` for member names and array indices. It
borrows the descendant operator `..` from [E4X][e4x], and the array slice
syntax proposal `[start:end:step]` from ECMASCRIPT 4.

Expressions can be used as an alternative to explicit names or indices, as in:

    $.store.book[(@.length-1)].title

using the symbol `@` for the current object. Filter expressions are supported via
the syntax `?(<boolean expr>)`, as in:

    $.store.book[?(@.price < 10)].title

### JSONPath Methods

JsonPath expressions support basic methods calls.

| Method     | Description                                            | Example                                                
|------------|--------------------------------------------------------|------------------------------------------------
| `length()` | Returns the length of an array or string.              | `$.store.book[?(length(@.title) > 5)]`                
| `count()`  | Returns the count of matching elements.                | `$.store.book[?(count(@.authors) > 1)]`               
| `match()`  | Returns true if a string matches a regular expression. | `$.store.book[?(match(@.title, '.*Century.*'))]`   
| `search()` | Searches for a string within another string.           | `$.store.book[?(search(@.title, 'Sword'))]`             
| `value()`  | Accesses the value of a key in the current object.     | `$.store.book[?(value(@.price) < 10)]`                


You can extend the supported function set by registering your own functions.

#### Example: `JsonNode` Path Function

**Step 1:** Create a custom function that returns the path of a `JsonNode`.

```csharp
public class PathNodeFunction() : FilterExtensionFunction( argumentCount: 1 )
{
    public const string Name = "path";
    private static readonly Expression PathExpression = Expression.Constant( (Func<IEnumerable<JsonNode>, string>) Path );

    protected override Expression GetExtensionExpression( Expression[] arguments )
    {
        return Expression.Invoke( PathExpression, arguments[0] );
    }

    public static string Path( IEnumerable<JsonNode> nodes )
    {
        var node = nodes.FirstOrDefault();
        return node?.GetPath();
    }
}
```

**Step 2:** Register your custom function.

```csharp
JsonTypeDescriptorRegistry.GetDescriptor<JsonNode>().Functions
    .Register( PathNodeFunction.Name, () => new PathNodeFunction() );
```

**Step 3:** Use your custom function in a JSONPath query.

```csharp
var results = source.Select( "$..[?path(@) == '$.store.book[2].title']" );
```

## Comparison with Other Libraries

There are excellent libraries available for RFC-9535 .NET JsonPath.

### [JsonPath.Net](https://docs.json-everything.net/path/basics/) Json-Everything

- **Pros:**
  - Extensive JSON ecosystem.
  - Comprehensive feature set.
  - Deferred execution queries with `IEnumerable`.
  - Strong community support.

- **Cons:**
  - No support for `JsonElement`.
  - Not quite as fast as other `System.Text.Json` implementations.
   
### [JsonCons.NET](https://danielaparker.github.io/JsonCons.Net/articles/JsonPath/JsonConsJsonPath.html)

- **Pros:**
  - High performance.
  - Enhanced JsonPath syntax.

- **Cons:**
  - No support for `JsonNode`.
  - Does not return an `IEnumerable` result (no defered query execution).
  
### [Json.NET](https://www.newtonsoft.com/json) Newtonsoft

- **Pros:**
  - Comprehensive feature set.
  - Documentation and examples.
  - Strong community support.
  - Level 2 .NET Foundation Project.

- **Cons:**
  - No support for `JsonElement`, or `JsonNode`.

### Why Choose [Hyperbee.Json](https://github.com/Stillpoint-Software/Hyperbee.Json) ?

- High Performance.
- Supports both `JsonElement`, and `JsonNode`.
- Deferred execution queries with `IEnumerable`.
- Extendable to support additional JSON document types and functions.
- Consensus focused JSONPath implementation.

- ## Benchmarks

Here is a performance comparison of various queries on the standard book store document.

```json
{
  "store": {
    "book": [
      {
        "category": "reference",
        "author": "Nigel Rees",
        "title": "Sayings of the Century",
        "price": 8.95
      },
      {
        "category": "fiction",
        "author": "Evelyn Waugh",
        "title": "Sword of Honour",
        "price": 12.99
      },
      {
        "category": "fiction",
        "author": "Herman Melville",
        "title": "Moby Dick",
        "isbn": "0-553-21311-3",
        "price": 8.99
      },
      {
        "category": "fiction",
        "author": "J. R. R. Tolkien",
        "title": "The Lord of the Rings",
        "isbn": "0-395-19395-8",
        "price": 22.99
      }
    ],
    "bicycle": {
      "color": "red",
      "price": 19.95
    }
  }
}
```

```
| Method                  | Filter                           | Mean      | Error     | StdDev    | Allocated 
|:----------------------- |:-------------------------------- |:--------- |:--------- |:--------- |:---------
| Hyperbee_JsonElement    | $..* `First()`                   |  3.026 us | 0.3647 us | 0.0200 us |   4.22 KB 
| JsonEverything_JsonNode | $..* `First()`                   |  3.170 us | 0.3034 us | 0.0166 us |   3.53 KB 
| Hyperbee_JsonNode       | $..* `First()`                   |  3.275 us | 1.7533 us | 0.0961 us |   3.37 KB 
| JsonCons_JsonElement    | $..* `First()`                   |  5.699 us | 0.2191 us | 0.0120 us |   8.48 KB 
| Newtonsoft_JObject      | $..* `First()`                   |  8.671 us | 1.7810 us | 0.0976 us |  14.22 KB 
|                         |                                  |           |           |           |           
| JsonCons_JsonElement    | $..*                             |  5.772 us | 3.8960 us | 0.2136 us |   8.45 KB 
| Hyperbee_JsonElement    | $..*                             |  8.179 us | 4.9380 us | 0.2707 us |  11.02 KB 
| Newtonsoft_JObject      | $..*                             |  9.867 us | 0.9006 us | 0.0494 us |  14.86 KB 
| Hyperbee_JsonNode       | $..*                             | 10.188 us | 2.0528 us | 0.1125 us |  10.83 KB 
| JsonEverything_JsonNode | $..*                             | 21.124 us | 5.1117 us | 0.2802 us |  36.81 KB 
|                         |                                  |           |           |           |           
| Hyperbee_JsonElement    | $..price                         |  4.867 us | 0.1883 us | 0.0103 us |   6.37 KB 
| JsonCons_JsonElement    | $..price                         |  4.924 us | 1.5997 us | 0.0877 us |   5.65 KB 
| Hyperbee_JsonNode       | $..price                         |  7.827 us | 5.0475 us | 0.2767 us |   8.77 KB 
| Newtonsoft_JObject      | $..price                         |  9.442 us | 1.0020 us | 0.0549 us |   14.4 KB 
| JsonEverything_JsonNode | $..price                         | 15.865 us | 2.1515 us | 0.1179 us |  27.63 KB 
|                         |                                  |           |           |           |           
| Hyperbee_JsonElement    | $.store.book[?(@.price == 8.99)] |  4.550 us | 1.0340 us | 0.0567 us |   9.08 KB 
| JsonCons_JsonElement    | $.store.book[?(@.price == 8.99)] |  5.341 us | 1.0738 us | 0.0589 us |   5.05 KB 
| Hyperbee_JsonNode       | $.store.book[?(@.price == 8.99)] |  7.341 us | 3.6147 us | 0.1981 us |  10.63 KB 
| Newtonsoft_JObject      | $.store.book[?(@.price == 8.99)] |  9.621 us | 5.1553 us | 0.2826 us |  15.84 KB 
| JsonEverything_JsonNode | $.store.book[?(@.price == 8.99)] | 11.789 us | 5.2457 us | 0.2875 us |  15.85 KB 
|                         |                                  |           |           |           |           
| Hyperbee_JsonElement    | $.store.book[0]                  |  2.896 us | 0.1069 us | 0.0059 us |   3.41 KB 
| JsonCons_JsonElement    | $.store.book[0]                  |  2.967 us | 0.1084 us | 0.0059 us |   3.21 KB 
| Hyperbee_JsonNode       | $.store.book[0]                  |  3.352 us | 0.1778 us | 0.0097 us |   3.58 KB 
| JsonEverything_JsonNode | $.store.book[0]                  |  4.779 us | 2.9031 us | 0.1591 us |   5.96 KB 
| Newtonsoft_JObject      | $.store.book[0]                  |  8.714 us | 2.5518 us | 0.1399 us |  14.56 KB 
```
```
```

## Additional Documentation

Additional documentation can be found in the project's `/docs` folder.

## Credits

Hyperbee.Json is built upon the great work of several open-source projects. Special thanks to:

- Stefan Goessner for the original [JSONPath implementation](https://goessner.net/articles/JsonPath/).
- System.Text.Json team for their work on the `System.Text.Json` library.
- Atif Aziz's C# port of Goessner's JSONPath library [.NET JSONPath](https://github.com/atifaziz/JSONPath).  
- Christoph Burgmer [JSONPath consensus effort](https://cburgmer.github.io/json-path-comparison).

## Contributing

We welcome contributions! Please see our [Contributing Guide](https://github.com/Stillpoint-Software/.github/blob/main/.github/CONTRIBUTING.md) for more details.
