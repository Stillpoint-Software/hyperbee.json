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