# JSONPath Syntax Reference

JSONPath is a query language for JSON that allows you to extract specific values from JSON documents. 
This page outlines the syntax and operators supported by `Hyperbee.Json`.

## Basic Syntax and Operators

### Root Node

`$` is used as the root node identifier. $ refers to the entire JSON document, serving as the starting 
point for any JSONPath expression. 

For instance, the expression `$.store.book` would navigate from the root of the JSON document to the 
store object and then to the book array within that object.

### Child Operator

`.` is used to select the child elements of a given node. It helps to navigate through the JSON 
structure by accessing the properties of objects and elements of arrays directly from their parent nodes.

| Expression             | Description                                           
|------------------------|-------------------------------------------------------
| `$.store`              | Selects the `store` child of the root element.        
| `$.store.book`         | Selects the `book` child of the `store` element.      
| `$.store.book[0].title`| Selects the `title` of the first `book` in `store`.  

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
  
  var root = JsonDocument.Parse(json);
  var result = JsonPath.Select(root, "$.store.book");

  Console.WriteLine(result.First()); // Output: "value"
```

### Subscript Operator

`[]` Access Elements by Index.

| Expression            | Description                                           
|-----------------------|-------------------------------------------------------
| `$[0]`                | Selects the first element of the array.               
| `$[-1]`               | Selects the last element of the array.                
| `$[1:3]`              | Selects the second and third elements of the array.   

```csharp
  using Hyperbee.JsonPath;
  using System.Text.Json;

  var json = """
  {
    "store": {
      "book": [
        "value1",
        "value2"
      ]
    }
  }
  """;
  
  var root = JsonDocument.Parse(json);
  var result = JsonPath.Select(root, "$.store.book[0]");

  Console.WriteLine(result.First()); // Output: "value1"
```

### Wildcard

`[*]` Wildcard.

| JSONPath              | Description                                           
|-----------------------|-------------------------------------------------------
| `$.*`                 | Selects all children of the root element.             
| `$..book[*]`          | Selects all `book` elements regardless of their depth.
| `$..*`                | Selects all elements and their children recursively.  

```csharp
  using Hyperbee.JsonPath;
  using System.Text.Json;

  var json = """
  {
    "store": {
      "book": [
        "value1",
        "value2"
      ]
    }
  }
  """;

  var root = JsonDocument.Parse(json);
  var result = JsonPath.Select(root, "$.store.book[*]");

  foreach (var item in result)
  {
      Console.WriteLine(item);
  }
  // Output: "value1"
  // Output: "value2"
```


### Descendant Search

`..` Descendant Search.

| Expression            | Description                                           
|-----------------------|-------------------------------------------------------
| `$..*`                | Selects all elements and their children recursively.  
| `$..author`           | Selects all `author` elements at any depth.    
| `$..store.book`       | Selects all `book` elements under `store` at any depth.

```csharp
  using Hyperbee.JsonPath;
  using System.Text.Json;

  var json = """
  {
    "store": {
      "book": [
        {
          "category": "fiction"
        },
        {
          "category": "science"
        }
      ],
      "bicycle": {
        "category": "road"
      }
    }
  }
  """;

  var root = JsonDocument.Parse(json);
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

`[ , ]` Select multiple items.

| Filter Description                          | JSONPath Expression                             | Description                                                               
|---------------------------------------------|-------------------------------------------------|---------------------------------------------------------------------------
| Selecting Multiple Keys                     | `$[?(@.key1 == value1), (@.key2 == value2)]`    | Selects elements where `key1` is `value1` or `key2` is `value2`.           
| Selecting Multiple Elements by Index        | `$[0, 2, 4]`                                    | Selects elements at index 0, 2, and 4.                                     
| Selecting Multiple Nested Keys              | `$..['key1', 'key2']`                           | Selects all elements that have `key1` or `key2` at any level. 
| Selecting Elements with Multiple Conditions | `$[?(@.key1 == value1) || ?(@.key2 == value2)]` | Selects elements where either `key1` is `value1` or `key2` is `value2`.    

```csharp
  using Hyperbee.JsonPath;
  using System.Text.Json;

  var json = """
  {
    "store": {
      "book": [
        "value1",
        "value2",
        "value3"
      ]
    }
  }
  """;

  var root = JsonDocument.Parse(json);
  var result = JsonPath.Select(root, "$.store.book[0,2]");

  foreach (var item in result)
  {
      Console.WriteLine(item);
  }
  // Output: "value1"
  // Output: "value3"
```

### Slices

The syntax for a slice is: `[start:end:step]`

Each component is optional.

- `start`: The beginning index of the slice (inclusive). Defaults to 0 if omitted and `step` is positive, or the end of the sequence if `step` is negative.
- `stop`: The end index of the slice (exclusive). Defaults to the length of the sequence if omitted and `step` is positive, or the start of the sequence if `step` is negative.
- `step`: The interval between indices. Defaults to 1 if omitted.

### Slice Expression Description

| Slice Expression      | Description                                            
|-----------------------|--------------------------------------------------------
| `[start:stop]`        | Elements from `start` to `stop-1`                      
| `[start:]`            | Elements from `start` to the end of the sequence       
| `[:stop]`             | Elements from the start of the sequence to `stop-1`    
| `[:]`                 | All elements                                           
| `[start:stop:step]`   | Elements from `start` to `stop-1` with a step of `step`

### Examples

| Example       | Description                            
|---------------|----------------------------------------
| `$[-1]`       | Last element                              
| `$[-2:]`      | Last two elements                         
| `$[:-2]`      | All elements except the last two          
| `$[::-1]`     | All elements, in reverse                  
| `$[1::-1]`    | First two elements, in reverse            
| `$[:-3:-1]`   | Last two elements, in reverse             
| `$[-3::-1]`   | All elements except the last two, in reverse


```csharp
  using Hyperbee.JsonPath;
  using System.Text.Json;

  var json = """
  {
    "store": {
      "book": [
        "value1",
        "value2",
        "value3",
        "value4"
      ]
    }
  }
  """;

  var root = JsonDocument.Parse(json);
  var result = JsonPath.Select(root, "$.store.book[0:3:2]");

  foreach (var item in result)
  {
      Console.WriteLine(item);
  }
  // Output: "value1"
  // Output: "value3"
```

## Filters

### Filter Expressions

`?` Filter elements based on an expression.

## JSONPath Filters

JSONPath filters allow you to query and manipulate JSON data structures by specifying conditions within square brackets `[]`. These filters enable you to select elements based on various criteria.

### Basic Syntax

The general syntax for a JSONPath filter is:

`$[?(@.key operator value)]`

- `@`: Refers to the current element being processed.
- `key`: The key within the JSON objects to apply the filter on.
- `operator`: The comparison operator (e.g., `==`, `!=`, `>`, `<`, `>=`, `<=`, `in`, `nin`).
- `value`: The value to compare the key against.

### Comparison Operators

| Operator | Description                         
|----------|-------------------------------------
| `==`     | Equal to                            
| `!=`     | Not equal to                        
| `>`      | Greater than                        
| `<`      | Less than                           
| `>=`     | Greater than or equal to            
| `<=`     | Less than or equal to               

### Examples

| Filter Operator         | Expression               | Description                                                  
|-------------------------|--------------------------|---------------------------
| Equality                | `$[?(@.age == 30)]`      | Selects elements where the `age` key is equal to 30.         
| Inequality              | `$[?(@.name != "John")]` | Selects elements where the `name` key is not "John".         
| Greater Than            | `$[?(@.price > 20)]`     | Selects elements where the `price` key is greater than 20.   
| Less Than               | `$[?(@.quantity < 5)]`   | Selects elements where the `quantity` key is less than 5.    


### Combining Filters

Filters can be combined using logical operators `&&` (and) and `||` (or).

| Filter Operator      | Expression                             | Description                                                                          
|----------------------|----------------------------------------|---------------------------
| Logical AND          | `$[?(@.price > 20 && @.quantity < 5)]` | Selects elements where the `price` key is greater than 20 and the `quantity` key is less than 5. 
| Logical OR           | `$[?(@.name == "John" || @.age > 30)]` | Selects elements where the `name` key is "John" or the `age` key is greater than 30. 

```csharp
  using Hyperbee.JsonPath;
  using System.Text.Json;

  var json = """
  {
    "store": {
      "book": [
        {
          "price": 10
        },
        {
          "price": 15
        }
      ]
    }
  }
  """;

  var root = JsonDocument.Parse(json);
  var result = JsonPath.Select(root, "$.store.book[?@.price > 10]");

  foreach (var item in result)
  {
      Console.WriteLine(item);
  }
  // Output: { "price": 15 }
```

### Current Node

`@` Current Node.

| Expression            | Description                                           
|-----------------------|---------------------------------------------
| `$[?(@.price < 10)]`  | Selects elements with `price` less than 10.           
| `$[?(@.name)]`        | Selects elements that have a `name` key.              
| `$[?(@.age > 25)]`    | Selects elements with `age` greater than 25.          

```csharp
  using Hyperbee.JsonPath;
  using System.Text.Json;

  var json = """
  {
    "store": {
      "book": [
        {
          "price": 10
        },
        {
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
      Console.WriteLine(item);
  }
  // Output: { "price": 15 }
```

## More Examples

### JSON Sample Document 1
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

#### Select all books:
   ```csharp
   using Hyperbee.JsonPath;
   using System.Text.Json;

   var root = JsonDocument.Parse(json);
   var result = JsonPath.Select(root, "$.store.book[*]");

   foreach (var item in result)
   {
       Console.WriteLine(item);
   }
   // Output: { "category": "fiction", "price": 10 }
   // Output: { "category": "science", "price": 15 }
   ```

#### Select all categories:
   ```csharp
   using Hyperbee.JsonPath;
   using System.Text.Json;

   var root = JsonDocument.Parse(json);
   var result = JsonPath.Select(root, "$.store.book[*].category");

   foreach (var item in result)
   {
       Console.WriteLine(item);
   }
   // Output: "fiction"
   // Output: "science"
   ```

#### Select books with price greater than 10:
   ```csharp
   using Hyperbee.JsonPath;
   using System.Text.Json;

   var root = JsonDocument.Parse(json);
   var result = JsonPath.Select(root, "$.store.book[?@.price > 10]");

   foreach (var item in result)
   {
       Console.WriteLine(item);
   }
   // Output: { "category": "science", "price": 15 }
   ```

### JSON Sample Document 2
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

#### Select all book titles:
   ```csharp
   using Hyperbee.JsonPath;
   using System.Text.Json;

   var root = JsonDocument.Parse(json);
   var result = JsonPath.Select(root, "$.library.books[*].title");

   foreach (var item in result)
   {
       Console.WriteLine(item);
   }
   // Output: "Book 1"
   // Output: "Book 2"
   ```

#### Select all authors:
   ```csharp
   using Hyperbee.JsonPath;
   using System.Text.Json;

   var root = JsonDocument.Parse(json);
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
