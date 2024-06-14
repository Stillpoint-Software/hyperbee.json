# JSONPath

JSON Path is a query language for JSON documents inspired by XPath. JSONPath defines 
a string syntax for selecting and extracting JSON values from within a given JSON document.

This library is a C# implementation of JSONPath for .NET `System.Text.Json` and `System.Text.Json.Nodes`. 

The implementation

* Works natively with both `JsonDocument` (`JsonElement`) and `JsonNode`
* Can be extended to support other JSON models
* Aligns with the draft JSONPath Specification RFC 9535 
  * [Working Draft](https://github.com/ietf-wg-jsonpath/draft-ietf-jsonpath-base).
  * [Editor Copy](https://ietf-wg-jsonpath.github.io/draft-ietf-jsonpath-base/draft-ietf-jsonpath-base.html)
* Functions according to the emerging consensus of use based on the majority of existing 
  implementations; except through concious exception or deference to the RFC.
  * [Parser Comparison Results](https://cburgmer.github.io/json-path-comparison)
  * [Parser Comparison GitHub](https://github.com/cburgmer/json-path-comparison/tree/master)


## JSONPath Syntax

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

Below is a complete overview and a side-by-side comparison of the JSONPath
syntax elements with its XPath counterparts:

| XPath     | JSONPath           | Description                                                
|:----------|:-------------------|:-----------------------------------------------------------
| `/`       | `$`                | The root object/element                                    
| `.`       | `@`                | The current object/element                                 
| `/`       | `.` or `[]`        | Child operator                                             
| `..`      | n/a                | Parent operator                                            
| `//`      | `..`               | Recursive descent. JSONPath borrows this syntax from E4X.  
| `*`       | `*`                | Wildcard. All objects/elements regardless their names.     
| `@`       | n/a                | Attribute access. JSON structures don't have attributes.   
| `[]`      | `[]`               | Subscript operator. XPath uses it to iterate over element collections and for [predicates][xpath-predicates]. In Javascript and JSON it is the native array operator. 
| `\|`      | `[,]`              | Union operator in XPath results in a combination of node sets. JSONPath allows alternate names or array indices as a set.
| n/a       | `[start:end:step]` | Array slice operator borrowed from ES4.
| `[]`      | `?()`              | Applies a filter (script) expression.
| n/a       | `()`               | Script expression, using the underlying script engine.
| `()`      | n/a                | Grouping in XPath

### Examples

Given a simple JSON structure that represents a bookstore:

```json
{ "store": {
    "book": [
        { "category": "reference",
        "author": "Nigel Rees",
        "title": "Sayings of the Century",
        "price": 8.95
        },
        { "category": "fiction",
        "author": "Evelyn Waugh",
        "title": "Sword of Honour",
        "price": 12.99
        },
        { "category": "fiction",
        "author": "Herman Melville",
        "title": "Moby Dick",
        "isbn": "0-553-21311-3",
        "price": 8.99
        },
        { "category": "fiction",
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

| XPath                 | JSONPath                  | Result                                 | Notes
|:----------------------|:--------------------------|:---------------------------------------|:------
|`/store/book/author`   | `$.store.book[*].author`  | The authors of all books in the store 
|`//author`             | `$..author`               | All authors                            
|`/store/*`             | `$.store.*`               | All things in store, which are some books and a red bicycle 
|`/store//price`        | `$.store..price`          | The price of everything in the store
|`//book[3]`            | `$..book[2]`              | The third book
|`//book[last()]`       | `$..book[(@.length-1)]<br>$..book[-1:]`  | The last book in order
|`//book[position()<3]` | `$..book[0,1]`<br>`$..book[:2]`| The first two books
|`//book/*[self::category|self::author]` or `//book/(category,author)` in XPath 2.0 | `$..book[category,author]` | The categories and authors of all books 
|`//book[isbn]`         | `$..book[?(@.isbn)]`      | Filter all books with `isbn` number
|`//book[price<10]`     | `$..book[?(@.price<10)]`  | Filter all books cheapier than 10
|`//*[price>19]/..`     | `$..[?(@.price>19)]`      | Categories with things more expensive than 19 | Parent (caret) not present in original spec
|`//*`                  | `$..*`                    | All elements in XML document; all members of JSON structure 
|`/store/book/[position()!=1]` | `$.store.book[?(@path !== "$[\'store\'][\'book\'][0]")]` | All books besides that at the path pointing to the first | `@path` not present in original spec

## Code examples
A couple of trivial code examples. Review the tests for detailed examples.

**Example 1** Select the last element of an array.
```csharp
const string json = @"
[
  ""first"",
  ""second"",
  ""third""
]";

var document = JsonDocument.Parse( json );
var match = document.Select( "$[-1:]" ).Single();

Assert.IsTrue( match.Value.GetString() == "third" );
```

**Example 2** Select all elemets that have a `key` property with a value less than 42. 
This example leverages bracket expressions using the default `Expression` jsonpath filter evaluator.

```csharp
const string json = @"
[
  { ""key"": 0}, 
  { ""key"": 42}, 
  { ""key"": -1}, 
  { ""key"": 41}, 
  { ""key"": 43}, 
  { ""key"": 42.0001}, 
  { ""key"": 41.9999}, 
  { ""key"": 100}, 
  { ""some"": ""value""}
]";

var document = JsonDocument.Parse( json );
var matches = document.Select( "$[?(@.key<42)]" );

// outputs 0 -1 41 41.9999

foreach( var element in matches )
{
    Console.WriteLine( document.RootElement.GetDouble() );
};

```
## Helper Classes

In addition to JSONPath processing, a few additional helper classes are provided to support dynamic property access,
property diving, and element comparisons.

### Dynamic Object Serialization

Basic support is provided for serializing to and from dynamic objects through the use of a custom `JsonConverter`.
The `DynamicJsonConverter` converter class is useful for simple scenareos. It is intended as a simple helper for basic use cases only.

#### DynamicJsonConverter

```csharp
var serializerOptions = new JsonSerializerOptions
{
    Converters = {new DynamicJsonConverter()}
};

// jsonInput is a string containing the bookstore json from the previous examples
var jobject = JsonSerializer.Deserialize<dynamic>( jsonInput, serializerOptions);

Assert.IsTrue( jobject.store.bicycle.color == "red" );

var jsonOutput = JsonSerializer.Serialize<dynamic>( jobject, serializerOptions ) as string;

Assert.IsTrue( jsonInput == jsonOutput );
```

##### Enum handling

When deserializing, the converter will treat enumerations as strings. You can override this behavior by setting 
the `TryReadValueHandler` on the converter. This handler will allow you to intercept and convert string and
numeric values during the deserialization process.

### Equality Helpers

| Method                             | Description
|:-----------------------------------|:-----------
| `JsonElement.DeepEquals`           | Performs a deep equals comparison 
| `JsonElementEqualityDeepComparer`  | A deep equals equality comparer

### Property Diving

| Method                             | Description
|:-----------------------------------|:-----------
| `JsonElement.GetPropertyFromKey`   | Dives for properties using absolute bracket location keys like `$['store']['book'][2]['author']`

### JsonElement Helpers

| Method                             | Description
|:-----------------------------------|:-----------
| `JsonPathBuilder`                  | Returns the JsonPath location string for a given element

## Acknowlegements

This project builds on the work of:

* [Stefan G&ouml;ssner - Original JSONPath specification dated 2007-02-21](http://goessner.net/articles/JsonPath/#e2)  
* [Atif Aziz - .NET JSONPath](https://github.com/atifaziz/JSONPath)  
* [Christoph Burgmer - Parser Consensus tests](https://cburgmer.github.io/json-path-comparison)
