# JSONPath Syntax Reference

JSONPath is a query language for JSON, similar to XPath for XML. It allows you to extract specific values from JSON documents. 
This page outlines the syntax and operators supported by Hyperbee.Json.

## Basic Syntax

### Root Node

`$` : Refers to the root object or array.

### Child Operator

`.` : Access a child element.
```csharp
  using Hyperbee.JsonPath;
  using System.Text.Json;

  var json = """
  { 
    "store": { 
      "book": "value" 
    } 
  }
  """;
  
  var root = JsonDocument.Parse(json).RootElement;
  var result = JsonPath.Select(root, "$.store.book");

  Console.WriteLine(result.First()); // Output: "value"
```

### Subscript Operator

`[]` : Access elements by index or key.
```csharp
  using Hyperbee.JsonPath;
  using System.Text.Json;

  var json = "{ \"store\": { \"book\": [\"value1\", \"value2\"] } }";
  
  var root = JsonDocument.Parse(json).RootElement;
  var result = JsonPath.Select(root, "$.store.book[0]");

  Console.WriteLine(result.First()); // Output: "value1"
```

### Wildcard

`[*]` : Wildcard for arrays or objects.
```csharp
  using Hyperbee.JsonPath;
  using System.Text.Json;

  var json = "{ \"store\": { \"book\": [\"value1\", \"value2\"] } }";

  var root = JsonDocument.Parse(json).RootElement;
  var result = JsonPath.Select(root, "$.store.book[*]");

  foreach (var item in result)
  {
      Console.WriteLine(item);
  }
  // Output: "value1"
  // Output: "value2"
```

## Filters

### Filter Expressions

`?()` : Filters elements based on a predicate.
```csharp
  using Hyperbee.JsonPath;
  using System.Text.Json;

  var json = "{ \"store\": { \"book\": [{ \"price\": 10 }, { \"price\": 15 }] } }";

  var root = JsonDocument.Parse(json).RootElement;
  var result = JsonPath.Select(root, "$.store.book[?(@.price > 10)]");

  foreach (var item in result)
  {
      Console.WriteLine(item);
  }
  // Output: { "price": 15 }
```

### Current Node

`@` : Represents the current node being processed in filters.
```csharp
  using Hyperbee.JsonPath;
  using System.Text.Json;

  var json = "{ \"store\": { \"book\": [{ \"price\": 10 }, { \"price\": 15 }] } }";

  var root = JsonDocument.Parse(json).RootElement;
  var result = JsonPath.Select(root, "$.store.book[?(@.price > 10)]");

  foreach (var item in result)
  {
      Console.WriteLine(item);
  }
  // Output: { "price": 15 }
```

## Operators

### Recursive Descent

`..` : Recursively search for matching elements.
```csharp
  using Hyperbee.JsonPath;
  using System.Text.Json;

  var json = "{ \"store\": { \"book\": [{\"category\": \"fiction\"}, {\"category\": \"science\"}], \"bicycle\": {\"category\": \"road\"} } }";

  var root = JsonDocument.Parse(json).RootElement;
  var result = JsonPath.Select(root, "$..category");

  foreach (var item in result)
  {
      Console.WriteLine(item);
  }
  // Output: "fiction"
  // Output: "science"
  // Output: "road"
```

### Union

`[ , ]` : Select multiple items.
```csharp
  using Hyperbee.JsonPath;
  using System.Text.Json;

  var json = "{ \"store\": { \"book\": [\"value1\", \"value2\", \"value3\"] } }";

  var root = JsonDocument.Parse(json).RootElement;
  var result = JsonPath.Select(root, "$.store.book[0,2]");

  foreach (var item in result)
  {
      Console.WriteLine(item);
  }
  // Output: "value1"
  // Output: "value3"
```

### Slices

`[start:end:step]` : Python-like array slicing.
```csharp
  using Hyperbee.JsonPath;
  using System.Text.Json;

  var json = "{ \"store\": { \"book\": [\"value1\", \"value2\", \"value3\", \"value4\"] } }";

  var root = JsonDocument.Parse(json).RootElement;
  var result = JsonPath.Select(root, "$.store.book[0:3:2]");

  foreach (var item in result)
  {
      Console.WriteLine(item);
  }
  // Output: "value1"
  // Output: "value3"
```

## Examples

### Simple JSON Structure

#### JSON
```json
{
  "store": {
    "book": [
      { "category": "fiction", "price": 10 },
      { "category": "science", "price": 15 }
    ]
  }
}
```

#### JSONPath Expressions

1. Select all books:
   ```csharp
   using Hyperbee.JsonPath;
   using System.Text.Json;

   var root = JsonDocument.Parse(json).RootElement;
   var result = JsonPath.Select(root, "$.store.book[*]");

   foreach (var item in result)
   {
       Console.WriteLine(item);
   }
   // Output: { "category": "fiction", "price": 10 }
   // Output: { "category": "science", "price": 15 }
   ```

2. Select all categories:
   ```csharp
   using Hyperbee.JsonPath;
   using System.Text.Json;

   var root = JsonDocument.Parse(json).RootElement;
   var result = JsonPath.Select(root, "$.store.book[*].category");

   foreach (var item in result)
   {
       Console.WriteLine(item);
   }
   // Output: "fiction"
   // Output: "science"
   ```

3. Select books with price greater than 10:
   ```csharp
   using Hyperbee.JsonPath;
   using System.Text.Json;

   var root = JsonDocument.Parse(json).RootElement;
   var result = JsonPath.Select(root, "$.store.book[?(@.price > 10)]");

   foreach (var item in result)
   {
       Console.WriteLine(item);
   }
   // Output: { "category": "science", "price": 15 }
   ```

### Nested JSON Structure

#### JSON
```json
{
  "library": {
    "books": [
      { "title": "Book 1", "details": { "author": "Author 1" } },
      { "title": "Book 2", "details": { "author": "Author 2" } }
    ]
  }
}
```

#### JSONPath Expressions

1. Select all book titles:
   ```csharp
   using Hyperbee.JsonPath;
   using System.Text.Json;

   var root = JsonDocument.Parse(json).RootElement;
   var result = JsonPath.Select(root, "$.library.books[*].title");

   foreach (var item in result)
   {
       Console.WriteLine(item);
   }
   // Output: "Book 1"
   // Output: "Book 2"
   ```

2. Select all authors:
   ```csharp
   using Hyperbee.JsonPath;
   using System.Text.Json;

   var root = JsonDocument.Parse(json).RootElement;
   var result = JsonPath.Select(root, "$.library.books[*].details.author");

   foreach (var item in result)
   {
       Console.WriteLine(item);
   }
   // Output: "Author 1"
   // Output: "Author 2"
   ```

## Additional Resources

- Stefan Goessner for the [original JSONPath implementation](https://goessner.net/articles/JsonPath/).
- JSONPath Specification [RFC 9535](https://www.rfc-editor.org/rfc/rfc9535.html). 
- Christoph Burgmer [JSONPath consensus effort](https://cburgmer.github.io/json-path-comparison)
