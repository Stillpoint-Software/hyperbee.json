
# Hyperbee.Json

`Hyperbee.Json` is a high-performance JSONPath parser for .NET, that supports both `JsonElement` and `JsonNode`.  
The library is designed to be fast and extensible, allowing support for other JSON document types and functions.

## Features

- **High Performance:** Optimized for performance and efficiency.
- **Supports:** `JsonElement` and `JsonNode`.
- **Extensible:** Easily extended to support additional JSON document types and functions.
- **`IEnumerable` Results:** Deferred execution queries with `IEnumerable`.
- **Conformant:** Adheres to the JSONPath Specification [RFC 9535](https://www.rfc-editor.org/rfc/rfc9535.html). 

## JSONPath RFC

Hyperbee.Json conforms to the RFC, and aims to support the [JSONPath consensus](https://cburgmer.github.io/json-path-comparison) 
when the RFC is unopinionated. When the RFC is unopinionated, and where the consensus is ambiguous or not aligned with our 
performance and usability goals, we may deviate. Our goal is to provide a robust and performant library while  
strengthening our alignment with the RFC and the community.

## Installation

Install via NuGet:

```bash
dotnet add package Hyperbee.Json
```

## Usage

### Selecting Elements

```csharp

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

### Selecting Nodes

```csharp

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

## JSONPath Overview

JSONPath operates on JSON documents:

* The special symbol `$` is used to reference the root JSON node. 
* The special symbol `@` is used to reference the current JSON node. 
* Queries can use dot-notation: `$.store.book[0].title`, or bracket-notation: `$['store']['book'][0]['title']` 
* Filters may be used to conditionally include results: `$.store.book[?(@.price < 10)]`

### JSONPath Syntax

| JSONPath                                     | Description                                                
|:---------------------------------------------|:-----------------------------------------------------------
| `$`                                          | Root JSON node                                    
| `@`                                          | Current JSON node                                 
| `.<name>`, `.'<name>'`, or `."<name>"`       | Object member dot operator
| `[<name>]`, or `['<name>']`, or `["<name>"]` | Object member subscript operator
| `[<index]`                                   | Array access operator
| `[,]`                                        | Union operator
| `[start:end:step]`                           | Array slice operator
| `*`, or `[*]`                                | Wildcard 
| `..`                                         | Recursive descent  
| `?<expr>`                                    | Filter selector


### JSONPath Functions

JsonPath expressions support basic method calls.

| Method     | Description                                            | Example                                                
|------------|--------------------------------------------------------|------------------------------------------------
| `length()` | Returns the length of an array or string.              | `$.store.book[?(length(@.title) > 5)]`                
| `count()`  | Returns the count of matching elements.                | `$.store.book[?(count(@.authors) > 1)]`               
| `match()`  | Returns true if a string matches a regular expression. | `$.store.book[?(match(@.title,'.*Century.*'))]`   
| `search()` | Searches for a string within another string.           | `$.store.book[?(search(@.title,'Sword'))]`             
| `value()`  | Accesses the value of a key in the current object.     | `$.store.book[?(value(@.price) < 10)]`                

### JSONPath Extended Syntax

The library extends the JSONPath expression syntax to support additional features.

| Operators           | Description                                   | Example                                                
|---------------------|-----------------------------------------------|------------------------------------------------
| `+` `-` `*` `\` `%` | Basic math operators.                         | `$[?(@.a + @.b == 3)]`                
| `in`                | Tests is a value is in a set.                 | `$[?@.value in ['a', 'b', 'c'] ]`               


### JSONPath Custom Functions

You can extend the supported function set by registering your own functions.

**Example:** Implement a `JsonNode` Path Function:

**Step 1:** Create a custom function that returns the path of a `JsonNode`.

```csharp
public class PathNodeFunction() : ExtensionFunction( PathMethod, CompareConstraint.MustCompare )
{
    public const string Name = "path";
    private static readonly MethodInfo PathMethod = GetMethod<PathNodeFunction>( nameof( Path ) );

    private static ScalarValue<string> Path( IValueType argument )
    {
        return argument.TryGetNode<JsonNode>( out var node ) ? node?.GetPath() : null;
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

## Why Choose Hyperbee.Json?

Hyperbee is fast, lightweight, fully RFC conforming, that supports **both** `JsonElement` and `JsonNode`.

- High Performance.
- Supports both `JsonElement`, and `JsonNode`.
- Deferred execution queries with `IEnumerable`.
- Enhanced JsonPath syntax.
- Extendable to support additional JSON document types.
- RFC conforming JSONPath implementation.

## Comparison with Other Libraries

There are other excellent libraries .NET JsonPath.

### [JsonPath.Net](https://docs.json-everything.net/path/basics/) Json-Everything

- **Pros:**
  - Comprehensive feature set.
  - Deferred execution queries with `IEnumerable`.
  - Enhanced JsonPath syntax.
  - Strong community support.
  
- **Cons:**
  - No support for `JsonElement`.
  - More memory intensive.
  - Not quite as fast as other implementations.
   
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
  - Deferred execution queries with `IEnumerable`.
  - Documentation and examples.
  - Strong community support.

- **Cons:**
  - No support for `JsonElement`, or `JsonNode`.

## Benchmarks

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
 Method                  | Filter                         | Mean      | Error      | StdDev    | Allocated
------------------------ |------------------------------- |-----------|------------|-----------|----------
 Hyperbee_JsonElement    | $..* `First()`                 |  3.105 us |  1.6501 us | 0.0904 us |   3.52 KB
 JsonEverything_JsonNode | $..* `First()`                 |  3.278 us |  3.3157 us | 0.1817 us |   3.53 KB
 Hyperbee_JsonNode       | $..* `First()`                 |  3.302 us |  3.2094 us | 0.1759 us |   3.09 KB
 JsonCons_JsonElement    | $..* `First()`                 |  6.170 us |  4.1597 us | 0.2280 us |   8.48 KB
 Newtonsoft_JObject      | $..* `First()`                 |  8.708 us |  8.7586 us | 0.4801 us |  14.22 KB
                         |                                |           |            |           |          
 JsonCons_JsonElement    | $..*                           |  5.792 us |  6.6920 us | 0.3668 us |   8.45 KB
 Hyperbee_JsonElement    | $..*                           |  7.504 us |  7.6479 us | 0.4192 us |   9.13 KB
 Hyperbee_JsonNode       | $..*                           | 10.320 us |  5.6676 us | 0.3107 us |  10.91 KB
 Newtonsoft_JObject      | $..*                           | 10.862 us |  0.4374 us | 0.0240 us |  14.86 KB
 JsonEverything_JsonNode | $..*                           | 21.914 us | 19.4680 us | 1.0671 us |  36.81 KB
                         |                                |           |            |           |          
 Hyperbee_JsonElement    | $..price                       |  4.557 us |  3.6801 us | 0.2017 us |    4.2 KB
 JsonCons_JsonElement    | $..price                       |  4.989 us |  2.3125 us | 0.1268 us |   5.65 KB
 Hyperbee_JsonNode       | $..price                       |  7.929 us |  0.6128 us | 0.0336 us |   7.48 KB
 Newtonsoft_JObject      | $..price                       | 10.511 us | 11.4901 us | 0.6298 us |   14.4 KB
 JsonEverything_JsonNode | $..price                       | 15.999 us |  0.5210 us | 0.0286 us |  27.63 KB
                         |                                |           |            |           |          
 Hyperbee_JsonElement    | $.store.book[?@.price == 8.99] |  4.221 us |  2.4758 us | 0.1357 us |   5.24 KB
 JsonCons_JsonElement    | $.store.book[?@.price == 8.99] |  5.424 us |  0.3551 us | 0.0195 us |   5.05 KB
 Hyperbee_JsonNode       | $.store.book[?@.price == 8.99] |  7.023 us |  7.0447 us | 0.3861 us |      8 KB
 Newtonsoft_JObject      | $.store.book[?@.price == 8.99] | 10.572 us |  2.4203 us | 0.1327 us |  15.84 KB
 JsonEverything_JsonNode | $.store.book[?@.price == 8.99] | 12.478 us |  0.5762 us | 0.0316 us |  15.85 KB
                         |                                |           |            |           |          
 Hyperbee_JsonElement    | $.store.book[0]                |  2.720 us |  1.9771 us | 0.1084 us |   2.27 KB
 JsonCons_JsonElement    | $.store.book[0]                |  3.266 us |  0.2087 us | 0.0114 us |   3.21 KB
 Hyperbee_JsonNode       | $.store.book[0]                |  3.396 us |  0.5137 us | 0.0282 us |   2.77 KB
 JsonEverything_JsonNode | $.store.book[0]                |  5.088 us |  0.1202 us | 0.0066 us |   5.96 KB
 Newtonsoft_JObject      | $.store.book[0]                |  9.178 us |  9.5618 us | 0.5241 us |  14.56 KB
```

## Additioal Documentation

Additional documentation can be found in the project's `/docs` folder.

## Credits

Hyperbee.Json is built upon the great work of several open-source projects. Special thanks to:

- System.Text.Json team for their work on the `System.Text.Json` library.
- Stefan Goessner for the original [JSONPath implementation](https://goessner.net/articles/JsonPath/).
- Atif Aziz's C# port of Goessner's JSONPath library [.NET JSONPath](https://github.com/atifaziz/JSONPath).  
- Christoph Burgmer [JSONPath consensus effort](https://cburgmer.github.io/json-path-comparison).
- [JSONPath Compliance Test Suite Team](https://github.com/jsonpath-standard/jsonpath-compliance-test-suite).

## Contributing

We welcome contributions! Please see our [Contributing Guide](https://github.com/Stillpoint-Software/.github/blob/main/.github/CONTRIBUTING.md) for more details.
