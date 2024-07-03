## Additional Classes

In addition to JSONPath, a few additional classes are provided to support pointer-style
property diving, element comparisons, and dynamic property access.

### Property Diving

Property diving acts similarly to JSON Pointer; it expects an absolute  path that returns a single element.
Unlike JSON Pointer, property diving notation expects a singular JSON Path. 

| Method                             | Description
|:-----------------------------------|:-----------
| `JsonElement.FromJsonPathPointer`  | Dives for properties using absolute locations like `$.store.book[2].author`

The syntax supports absolute (normalized) paths; dotted notation, quoted names, and simple bracketed array accessors only.
The intention is to return a single element by literal path.

Json path style '$', wildcard '*', '..', and '[a,b]' multi-result selector notations and filters are **not** supported.

```
Examples of valid path syntax:

    prop1.prop2
    prop1[0]
    'prop.2'
    prop1[0].prop2
    prop1['prop.2']
    prop1.'prop.2'[0].prop3
```

### JsonElement Path

Unlike `JsonNode`, `JsonElement` does not have a `Path` property. `JsonPathBuilder` will find the path
for a given `JsonElement`.

| Method                     | Description
|:---------------------------|:-----------
| `JsonPathBuilder.GetPath` | Returns the JsonPath location string for a given element

### Equality Helpers

| Method                             | Description
|:-----------------------------------|:-----------
| `JsonElement.DeepEquals`           | Performs a deep equals comparison on two `JsonElements`
| `JsonElementDeepEqualityComparer`  | A deep equals equality comparer that compares two `JsonElements`

### Dynamic Object Serialization

Basic support is provided for serializing to and from dynamic objects through the use of a custom `JsonConverter`.
The `DynamicJsonConverter` converter class is useful for simple scenareos. It is intended as a simple helper for 
basic use cases only.

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
