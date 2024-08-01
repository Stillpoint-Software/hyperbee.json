---
layout: default
title: Functions
nav_order: 3
---

# JSONPath Functions

JsonPath expressions support basic method calls.

| Method     | Description                                            | Example                                                
|------------|--------------------------------------------------------|------------------------------------------------
| `length()` | Returns the length of an array or string.              | `$.store.book[?(length(@.title) > 5)]`                
| `count()`  | Returns the count of matching elements.                | `$.store.book[?(count(@.authors) > 1)]`               
| `match()`  | Returns true if a string matches a regular expression. | `$.store.book[?(match(@.title,'.*Century.*'))]`   
| `search()` | Searches for a string within another string.           | `$.store.book[?(search(@.title,'Sword'))]`             
| `value()`  | Accesses the value of a key in the current object.     | `$.store.book[?(value(@.price) < 10)]`                

## JSONPath Extensions Functions

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

