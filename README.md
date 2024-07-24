
# Hyperbee.Json

`Hyperbee.Json` is a high-performance JSONPath parser for .NET, that supports both `JsonElement` and `JsonNode`.  
The library is designed to be quick and extensible, allowing support for other JSON document types and functions.

## Features

- **High Performance:** Optimized for performance and efficiency.
- **Supports:** `JsonElement` and `JsonNode`.
- **Extensible:** Easily extended to support additional JSON document types and functions.
- **`IEnumerable` Results:** Deferred execution queries with `IEnumerable`.
- **Conformant:** Adheres to the JSONPath Specification [RFC 9535](https://www.rfc-editor.org/rfc/rfc9535.html). 

## JSONPath RFC

Hyperbee.Json conforms to the RFC, and aims to support the [JSONPath consensus](https://cburgmer.github.io/json-path-comparison) 
when the RFC is unopinionated. When the RFC is unopinionated, and where the consensus is ambiguous or not aligned with our 
performance and usability goals, we may deviate. Our goal is always to provide a robust and performant library while  
strengthening our alignment with the RFC and the community.

## Installation

Install via NuGet:

```bash
dotnet add package Hyperbee.Json
```

## Usage

### Basic Examples

#### Selecting Elements

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

#### Filtering

```csharp

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

## JSONPath Syntax Overview

Here's a quick overview of JSONPath syntax:

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

JSONPath expressions refer to a JSON structure, and JSONPath assumes the name `$` is assigned 
to the root JSON object.

JSONPath expressions can use dot-notation:

    $.store.book[0].title

or bracket-notation:

    $['store']['book'][0]['title']

- JSONPath allows the wildcard symbol `*` for member names and array indices. 
- It borrows the descendant operator `..` from [E4X][e4x]
- It uses the `@` symbol to refer to the current object.
- It uses `?` syntax for filtering.
- It uses the array slice syntax proposal `[start:end:step]` from ECMASCRIPT 4.

Expressions can be used as an alternative to explicit names or indices, as in:

    $.store.book[(length(@)-1)].title

Filter expressions are supported via the syntax `?(<boolean expr>)`, as in:

    $.store.book[?(@.price < 10)].title

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

## Why Choose [Hyperbee.Json](https://github.com/Stillpoint-Software/Hyperbee.Json) ?

- High Performance.
- Supports both `JsonElement`, and `JsonNode`.
- Deferred execution queries with `IEnumerable`.
- Enhanced JsonPath syntax.
- Extendable to support additional JSON document types.
- RFC conforming JSONPath implementation.

## Comparison with Other Libraries

There are excellent libraries available for RFC-9535 .NET JsonPath.

### [JsonPath.Net](https://docs.json-everything.net/path/basics/) Json-Everything

- **Pros:**
  - Comprehensive feature set.
  - Deferred execution queries with `IEnumerable`.
  - Enhanced JsonPath syntax.
  - Strong community support.
  
- **Cons:**
  - No support for `JsonElement`.
  - More memory intensive.
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

 | Method                   |       Mean |       Error |     StdDev |  Allocated
 | :----------------------- | ---------: | ----------: | ---------: | ---------:
 | `$..* First()`
 | Hyperbee_JsonElement     |   2.990 μs |   1.9900 μs |  0.1091 μs |    3.52 KB
 | Hyperbee_JsonNode        |   3.305 μs |   0.4741 μs |  0.0260 μs |    3.09 KB
 | JsonEverything_JsonNode  |   3.511 μs |   4.6418 μs |  0.2544 μs |    3.53 KB
 | JsonCons_JsonElement     |   5.758 μs |   5.6199 μs |  0.3080 μs |    8.48 KB
 | Newtonsoft_JObject       |   9.349 μs |   0.9419 μs |  0.0516 μs |   14.22 KB
 |                          |            |             |            |           
 | `$..*`
 | JsonCons_JsonElement     |   5.758 μs |   5.6199 μs |  0.3080 μs |    8.48 KB
 | Hyperbee_JsonElement     |   2.990 μs |   1.9900 μs |  0.1091 μs |    3.52 KB
 | Hyperbee_JsonNode        |   3.305 μs |   0.4741 μs |  0.0260 μs |    3.09 KB
 | Newtonsoft_JObject       |   9.349 μs |   0.9419 μs |  0.0516 μs |   14.22 KB
 | JsonEverything_JsonNode  |   3.511 μs |   4.6418 μs |  0.2544 μs |    3.53 KB
 |                          |            |             |            |           
 | `$..price`
 | Hyperbee_JsonElement     |   2.990 μs |   1.9900 μs |  0.1091 μs |    3.52 KB
 | JsonCons_JsonElement     |   5.758 μs |   5.6199 μs |  0.3080 μs |    8.48 KB
 | Hyperbee_JsonNode        |   3.305 μs |   0.4741 μs |  0.0260 μs |    3.09 KB
 | Newtonsoft_JObject       |   9.349 μs |   0.9419 μs |  0.0516 μs |   14.22 KB
 | JsonEverything_JsonNode  |   3.511 μs |   4.6418 μs |  0.2544 μs |    3.53 KB
 |                          |            |             |            |           
 | `$.store.book[?(@.price == 8.99)]`
 | Hyperbee_JsonElement     |   2.990 μs |   1.9900 μs |  0.1091 μs |    3.52 KB
 | JsonCons_JsonElement     |   5.758 μs |   5.6199 μs |  0.3080 μs |    8.48 KB
 | Hyperbee_JsonNode        |   3.305 μs |   0.4741 μs |  0.0260 μs |    3.09 KB
 | Newtonsoft_JObject       |   9.349 μs |   0.9419 μs |  0.0516 μs |   14.22 KB
 | JsonEverything_JsonNode  |   3.511 μs |   4.6418 μs |  0.2544 μs |    3.53 KB
 |                          |            |             |            |           
 | `$.store.book[0]`
 | Hyperbee_JsonElement     |   2.990 μs |   1.9900 μs |  0.1091 μs |    3.52 KB
 | JsonCons_JsonElement     |   5.758 μs |   5.6199 μs |  0.3080 μs |    8.48 KB
 | Hyperbee_JsonNode        |   3.305 μs |   0.4741 μs |  0.0260 μs |    3.09 KB
 | JsonEverything_JsonNode  |   3.511 μs |   4.6418 μs |  0.2544 μs |    3.53 KB
 | Newtonsoft_JObject       |   9.349 μs |   0.9419 μs |  0.0516 μs |   14.22 KB

## Additional Documentation

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
