# Welcome to Hyperbee Json

Hyperbee Json is a high-performance JSON library for .NET, providing robust support for JSONPath, JsonPointer, JsonPatch, and JsonDiff. 
This library is optimized for speed and low memory allocations, and it adheres to relevant RFCs to ensure reliable and predictable behavior.

Unlike other libraries that support only `JsonElement` or `JsonNode`, Hyperbee.Json supports **both** types, and can be easily extended to 
support additional document types and functions.

## Features

- **High Performance:** Optimized for performance and efficiency.
- **Low Memory Allocations:** Designed to minimize memory usage.
- **Comprehensive JSON Support:** Supports JSONPath, JsonPointer, JsonPatch, and JsonDiff.
- **Conformance:** Adheres to the JSONPath Specification [RFC 9535](https://www.rfc-editor.org/rfc/rfc9535.html), JSONPointer [RFC 6901](https://www.rfc-editor.org/rfc/rfc6901.html), and JsonPatch [RFC 6902](https://www.rfc-editor.org/rfc/rfc6902.html).
- **Supports both `JsonElement` and `JsonNode`:** Works seamlessly with both JSON document types.

## JSONPath

JSONPath is a query language for JSON, allowing you to navigate and extract data from JSON documents using a set of path expressions. 
Hyperbee.Json's JSONPath implementation is designed for optimal performance, ensuring low memory allocations and fast query execution. 
It fully conforms to [RFC 9535](https://www.rfc-editor.org/rfc/rfc9535.html).

[Read more about JsonPath](https://stillpoint-software.github.io/hyperbee.json/jsonpath/jsonpath.html)

## JSONPointer

JSONPointer is a syntax for identifying a specific value within a JSON document. It is simple and easy to use, making it an excellent 
choice for pinpointing exact values. Hyperbee.Json's JsonPointer implementation adheres to [RFC 6901](https://www.rfc-editor.org/rfc/rfc6901.html).

[Read more about JsonPointer](https://stillpoint-software.github.io/hyperbee.json/jsonpointer.html)

## JSONPatch

JSONPatch is a format for describing changes to a JSON document. It allows you to apply partial modifications to JSON data efficiently. 
Hyperbee.Json supports JsonPatch as defined in [RFC 6902](https://www.rfc-editor.org/rfc/rfc6902.html).

[Read more about JsonPatch](https://stillpoint-software.github.io/hyperbee.json/jsonpatch.html)

## JSONDiff

JSONDiff allows you to compute the difference between two JSON documents, which is useful for versioning and synchronization. 
Hyperbee.Json's implementation is optimized for performance and low memory usage, adhering to the standards set in [RFC 6902](https://www.rfc-editor.org/rfc/rfc6902.html).

[Read more about JsonDiff](https://stillpoint-software.github.io/hyperbee.json/jsonpatch.html#jsondiff)

## Getting Started

To get started with Hyperbee.Json, refer to the [documentation](https://stillpoint-software.github.io/hyperbee.json) for detailed instructions and examples. 

You can intall the library via NuGet:

```bash
dotnet add package Hyperbee.Json
```

## Benchmarks

Here is a performance comparison of various JSONPath queries on the standard book store document.

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


| Method                  |  Mean      | Error      | StdDev    | Gen0   | Gen1   | Allocated |
|------------------------ | ----------:|-----------:|----------:|-------:|-------:|----------:|
| `$..[?(@.price < 10)]`
| JsonCraft.JsonElement   |   2.957 us |  1.1397 us | 0.0625 us | 0.2899 |      - |   3.59 KB |
| Hyperbee.JsonElement    |   5.221 us |  0.8505 us | 0.0466 us | 1.2894 | 0.0076 |  15.84 KB |
| JsonCons.JsonElement    |   5.518 us |  1.3367 us | 0.0733 us | 1.0376 | 0.0076 |  12.73 KB |
| Hyperbee.JsonNode       |   5.915 us |  0.8714 us | 0.0478 us | 1.4954 | 0.0153 |  18.38 KB |
| Newtonsoft.JObject      |   8.009 us |  1.7612 us | 0.0965 us | 2.1057 | 0.0763 |  25.86 KB |
| JsonEverything.JsonNode |  17.850 us | 12.0238 us | 0.6591 us | 3.9063 | 0.1221 |  48.15 KB |
|                         |            |            |           |        |        |           |
| `$..['bicycle','price']`  
| JsonCraft.JsonElement   |   2.309 us |  0.2498 us | 0.0137 us | 0.3242 |      - |   4.01 KB |
| Hyperbee.JsonElement    |   2.701 us |  1.0824 us | 0.0593 us | 0.4158 |      - |   5.12 KB |
| JsonCons.JsonElement    |   3.058 us |  0.3820 us | 0.0209 us | 0.5760 | 0.0038 |   7.09 KB |
| Hyperbee.JsonNode       |   3.626 us |  1.3209 us | 0.0724 us | 0.7210 | 0.0114 |   8.84 KB |
| Newtonsoft.JObject      |   4.990 us |  1.5961 us | 0.0875 us | 1.1826 | 0.0458 |  14.55 KB |
| JsonEverything.JsonNode |  12.183 us |  2.7932 us | 0.1531 us | 2.3193 | 0.0610 |   28.5 KB |
|                         |            |            |           |        |        |           |
| `$..*` 
| JsonCraft.JsonElement   |   2.023 us |  0.4743 us | 0.0260 us | 0.2327 |      - |   2.88 KB |
| Hyperbee.JsonElement    |   2.390 us |  0.7132 us | 0.0391 us | 0.5226 | 0.0038 |   6.45 KB |
| JsonCons.JsonElement    |   3.239 us |  0.8771 us | 0.0481 us | 0.6866 | 0.0038 |   8.45 KB |
| Hyperbee.JsonNode       |   3.864 us |  2.6714 us | 0.1464 us | 0.7629 |      - |   9.54 KB |
| Newtonsoft.JObject      |   4.864 us |  0.7073 us | 0.0388 us | 1.1520 | 0.0458 |  14.19 KB |
| JsonEverything.JsonNode |  17.367 us | 10.7988 us | 0.5919 us | 2.7466 | 0.0610 |  33.97 KB |
|                         |            |            |           |        |        |           |
|`$..author`
| JsonCraft.JsonElement   |   2.071 us |  0.8571 us | 0.0470 us | 0.2327 |      - |   2.88 KB |
| Hyperbee.JsonElement    |   2.275 us |  0.5597 us | 0.0307 us | 0.4158 |      - |    5.1 KB |
| JsonCons.JsonElement    |   2.572 us |  0.2856 us | 0.0157 us | 0.4463 | 0.0038 |   5.47 KB |
| Hyperbee.JsonNode       |   3.226 us |  0.6372 us | 0.0349 us | 0.7019 |      - |   8.64 KB |
| Newtonsoft.JObject      |   4.793 us |  3.7445 us | 0.2053 us | 1.1520 | 0.0381 |   14.2 KB |
| JsonEverything.JsonNode |  11.786 us |  2.5761 us | 0.1412 us | 2.0752 |      - |   26.1 KB |
|                         |            |            |           |        |        |           |
| `$..book[?@.isbn]`
| Hyperbee.JsonElement    |   2.814 us |  0.7079 us | 0.0388 us | 0.4997 | 0.0038 |   6.14 KB |
| JsonCons.JsonElement    |   3.348 us |  0.4197 us | 0.0230 us | 0.5836 | 0.0038 |   7.16 KB |
| Hyperbee.JsonNode       |   3.587 us |  1.1927 us | 0.0654 us | 0.7629 |      - |   9.64 KB |
| JsonEverything.JsonNode |  12.860 us |  6.6350 us | 0.3637 us | 2.4414 | 0.0610 |  29.98 KB |
| JsonCraft.JsonElement   |         NA |         NA |        NA |     NA |     NA |        NA |
| Newtonsoft.JObject      |         NA |         NA |        NA |     NA |     NA |        NA |
|                         |            |            |           |        |        |           |
| `$..book[?@.price == 8.99 && @.category == 'fiction']`
| Hyperbee.JsonElement    |   3.599 us |  1.1407 us | 0.0625 us | 0.6752 | 0.0038 |   8.28 KB |
| Hyperbee.JsonNode       |   4.593 us |  2.6977 us | 0.1479 us | 0.9460 |      - |  11.91 KB |
| JsonCons.JsonElement    |   4.684 us |  3.2355 us | 0.1774 us | 0.6866 |      - |   8.48 KB |
| JsonEverything.JsonNode |  17.000 us | 14.9488 us | 0.8194 us | 3.1738 |      - |  39.52 KB |
| JsonCraft.JsonElement   |         NA |         NA |        NA |     NA |     NA |        NA |
| Newtonsoft.JObject      |         NA |         NA |        NA |     NA |     NA |        NA |
|                         |            |            |           |        |        |           |
| $..book[0,1] 
| JsonCraft.JsonElement   |   2.184 us |  1.1145 us | 0.0611 us | 0.2518 |      - |   3.09 KB |
| Hyperbee.JsonElement    |   2.316 us |  0.6324 us | 0.0347 us | 0.4158 |      - |    5.1 KB |
| JsonCons.JsonElement    |   2.973 us |  3.2806 us | 0.1798 us | 0.4921 | 0.0038 |   6.06 KB |
| Hyperbee.JsonNode       |   3.363 us |  2.4201 us | 0.1327 us | 0.7019 | 0.0038 |   8.64 KB |
| Newtonsoft.JObject      |   4.709 us |  1.1244 us | 0.0616 us | 1.1749 | 0.0458 |  14.45 KB |
| JsonEverything.JsonNode |  11.585 us |  1.1525 us | 0.0632 us | 2.1362 | 0.0610 |  26.41 KB |
|                         |            |            |           |        |        |           |
| `$..book[0]`
| JsonCraft.JsonElement   |   2.097 us |  0.7390 us | 0.0405 us | 0.2441 |      - |      3 KB |
| Hyperbee.JsonElement    |   2.417 us |  0.4037 us | 0.0221 us | 0.4158 |      - |    5.1 KB |
| JsonCons.JsonElement    |   2.767 us |  1.0765 us | 0.0590 us | 0.4539 | 0.0038 |   5.59 KB |
| Hyperbee.JsonNode       |   3.439 us |  2.6110 us | 0.1431 us | 0.7019 |      - |   8.64 KB |
| Newtonsoft.JObject      |   4.730 us |  0.4614 us | 0.0253 us | 1.1673 | 0.0381 |  14.33 KB |
| JsonEverything.JsonNode |  11.404 us |  1.6551 us | 0.0907 us | 2.0752 | 0.0610 |  26.02 KB |
|                         |            |            |           |        |        |           |
| `$.store..price`
| Hyperbee.JsonElement    |   2.174 us |  0.2046 us | 0.0112 us | 0.3853 |      - |   4.73 KB |
| JsonCraft.JsonElement   |   2.174 us |  0.9541 us | 0.0523 us | 0.2518 |      - |   3.13 KB |
| JsonCons.JsonElement    |   2.657 us |  1.2199 us | 0.0669 us | 0.4539 |      - |   5.57 KB |
| Hyperbee.JsonNode       |   3.219 us |  1.6130 us | 0.0884 us | 0.6828 |      - |   8.38 KB |
| Newtonsoft.JObject      |   4.751 us |  0.3461 us | 0.0190 us | 1.1673 | 0.0381 |  14.34 KB |
| JsonEverything.JsonNode |  12.312 us |  4.6283 us | 0.2537 us | 2.1362 | 0.0610 |  26.63 KB |
|                         |            |            |           |        |        |           |
| `$.store.*`
| JsonCraft.JsonElement   |   1.415 us |  0.5540 us | 0.0304 us | 0.2022 |      - |   2.49 KB |
| Hyperbee.JsonElement    |   1.564 us |  0.4252 us | 0.0233 us | 0.2289 |      - |   2.81 KB |
| JsonCons.JsonElement    |   1.699 us |  0.5680 us | 0.0311 us | 0.2651 |      - |   3.27 KB |
| Hyperbee.JsonNode       |   1.762 us |  0.0234 us | 0.0013 us | 0.2365 | 0.0019 |    2.9 KB |
| JsonEverything.JsonNode |   2.264 us |  0.3401 us | 0.0186 us | 0.3891 | 0.0038 |    4.8 KB |
| Newtonsoft.JObject      |   4.142 us |  0.4524 us | 0.0248 us | 1.1749 | 0.0458 |  14.43 KB |
|                         |            |            |           |        |        |           |
| `$.store.bicycle.color`
| Hyperbee.JsonElement    |   1.415 us |  0.3898 us | 0.0214 us | 0.1755 |      - |   2.17 KB |
| JsonCraft.JsonElement   |   1.532 us |  0.2165 us | 0.0119 us | 0.1984 |      - |   2.45 KB |
| JsonCons.JsonElement    |   1.671 us |  0.2610 us | 0.0143 us | 0.2632 |      - |   3.23 KB |
| Hyperbee.JsonNode       |   1.706 us |  0.4849 us | 0.0266 us | 0.2346 |      - |   2.88 KB |
| JsonEverything.JsonNode |   2.629 us |  1.6792 us | 0.0920 us | 0.4654 | 0.0038 |   5.74 KB |
| Newtonsoft.JObject      |   4.336 us |  1.2294 us | 0.0674 us | 1.1826 | 0.0381 |  14.49 KB |
|                         |            |            |           |        |        |           |
| `$.store.book[-1:]`
| JsonCraft.JsonElement   |   1.479 us |  0.1113 us | 0.0061 us | 0.2098 |      - |   2.58 KB |
| Hyperbee.JsonElement    |   1.515 us |  0.3284 us | 0.0180 us | 0.1945 |      - |   2.41 KB |
| JsonCons.JsonElement    |   1.813 us |  0.0749 us | 0.0041 us | 0.2861 | 0.0019 |   3.52 KB |
| Hyperbee.JsonNode       |   1.821 us |  0.1880 us | 0.0103 us | 0.2422 |      - |   2.97 KB |
| JsonEverything.JsonNode |   2.686 us |  0.5270 us | 0.0289 us | 0.4654 | 0.0038 |   5.72 KB |
| Newtonsoft.JObject      |   4.390 us |  0.6167 us | 0.0338 us | 1.1826 | 0.0534 |  14.52 KB |
|                         |            |            |           |        |        |           |
| `$.store.book[:2]`
| JsonCraft.JsonElement   |   1.547 us |  0.3302 us | 0.0181 us | 0.2098 |      - |   2.58 KB |
| Hyperbee.JsonElement    |   1.554 us |  1.0503 us | 0.0576 us | 0.1945 |      - |   2.41 KB |
| JsonCons.JsonElement    |   1.867 us |  0.9304 us | 0.0510 us | 0.2880 |      - |   3.54 KB |
| Hyperbee.JsonNode       |   1.869 us |  0.2955 us | 0.0162 us | 0.2289 |      - |   2.97 KB |
| JsonEverything.JsonNode |   3.112 us |  1.4792 us | 0.0811 us | 0.4883 |      - |   6.02 KB |
| Newtonsoft.JObject      |   4.344 us |  4.3641 us | 0.2392 us | 1.1826 | 0.0305 |  14.51 KB |
|                         |            |            |           |        |        |           |
| `$.store.book[?(@.author && @.title)]`
| JsonCraft.JsonElement   |   1.924 us |  0.1844 us | 0.0101 us | 0.2689 | 0.0019 |    3.3 KB |
| Hyperbee.JsonElement    |   2.331 us |  0.8201 us | 0.0450 us | 0.3395 |      - |   4.18 KB |
| JsonCons.JsonElement    |   2.776 us |  0.5326 us | 0.0292 us | 0.4539 | 0.0038 |   5.58 KB |
| Hyperbee.JsonNode       |   3.377 us |  0.4035 us | 0.0221 us | 0.6561 | 0.0076 |   8.08 KB |
| Newtonsoft.JObject      |   4.779 us |  2.3452 us | 0.1285 us | 1.3199 | 0.0458 |  16.18 KB |
| JsonEverything.JsonNode |   6.403 us |  1.9211 us | 0.1053 us | 1.4648 | 0.0305 |  18.32 KB |
|                         |            |            |           |        |        |           |
| `$.store.book[?(@.category == 'fiction')]`
| JsonCraft.JsonElement   |   2.038 us |  0.5141 us | 0.0282 us | 0.2747 |      - |   3.38 KB |
| Hyperbee.JsonElement    |   2.317 us |  0.2970 us | 0.0163 us | 0.3510 |      - |   4.34 KB |
| JsonCons.JsonElement    |   2.643 us |  0.8319 us | 0.0456 us | 0.4082 | 0.0038 |   5.01 KB |
| Hyperbee.JsonNode       |   3.393 us |  0.3912 us | 0.0214 us | 0.6561 |      - |    8.2 KB |
| Newtonsoft.JObject      |   4.663 us |  1.3455 us | 0.0737 us | 1.2817 | 0.0458 |  15.74 KB |
| JsonEverything.JsonNode |   6.502 us |  4.8220 us | 0.2643 us | 1.3428 | 0.0305 |  16.49 KB |
|                         |            |            |           |        |        |           |
| `$.store.book[?(@.price < 10)].title` 
| JsonCraft.JsonElement   |   2.349 us |  0.1454 us | 0.0080 us | 0.2747 |      - |   3.37 KB |
| Hyperbee.JsonElement    |   2.379 us |  0.2508 us | 0.0137 us | 0.3548 |      - |   4.35 KB |
| JsonCons.JsonElement    |   3.008 us |  1.7216 us | 0.0944 us | 0.4196 |      - |   5.18 KB |
| Hyperbee.JsonNode       |   3.453 us |  0.6436 us | 0.0353 us | 0.6561 |      - |   8.09 KB |
| Newtonsoft.JObject      |   4.762 us |  1.8678 us | 0.1024 us | 1.2894 | 0.0534 |  15.89 KB |
| JsonEverything.JsonNode |   6.891 us |  1.2944 us | 0.0709 us | 1.4114 | 0.0381 |  17.38 KB |
|                         |            |            |           |        |        |           |
| `$.store.book[?(@.price > 10 && @.price < 20)]`
| JsonCraft.JsonElement   |   2.637 us |  0.3234 us | 0.0177 us | 0.3090 |      - |   3.82 KB |
| Hyperbee.JsonElement    |   2.730 us |  0.9871 us | 0.0541 us | 0.4349 | 0.0038 |   5.37 KB |
| JsonCons.JsonElement    |   3.726 us |  0.1036 us | 0.0057 us | 0.5074 | 0.0038 |   6.23 KB |
| Hyperbee.JsonNode       |   4.091 us |  0.3535 us | 0.0194 us | 0.7324 |      - |   9.14 KB |
| Newtonsoft.JObject      |   5.167 us |  2.3846 us | 0.1307 us | 1.3580 | 0.0534 |  16.69 KB |
| JsonEverything.JsonNode |   8.309 us |  1.7991 us | 0.0986 us | 1.7700 |      - |  22.02 KB |
|                         |            |            |           |        |        |           |
| `$.store.book[?@.price == 8.99]`
| Hyperbee.JsonElement    |   2.249 us |  0.4713 us | 0.0258 us | 0.3357 |      - |   4.15 KB |
| JsonCons.JsonElement    |   2.763 us |  1.0265 us | 0.0563 us | 0.4044 |      - |   4.97 KB |
| Hyperbee.JsonNode       |   3.416 us |  2.2807 us | 0.1250 us | 0.6409 |      - |   7.89 KB |
| JsonEverything.JsonNode |   6.221 us |  5.2637 us | 0.2885 us | 1.2512 | 0.0305 |  15.47 KB |
| JsonCraft.JsonElement   |         NA |         NA |        NA |     NA |     NA |        NA |
| Newtonsoft.JObject      |         NA |         NA |        NA |     NA |     NA |        NA |
|                         |            |            |           |        |        |           |
| `$.store.book['category','author']`   
| JsonCraft.JsonElement   |   1.575 us |  0.1797 us | 0.0099 us | 0.2403 |      - |   2.95 KB |
| Hyperbee.JsonElement    |   1.931 us |  0.2480 us | 0.0136 us | 0.2117 |      - |   2.61 KB |
| JsonCons.JsonElement    |   1.955 us |  0.6808 us | 0.0373 us | 0.2937 | 0.0019 |   3.61 KB |
| JsonEverything.JsonNode |   2.794 us |  4.9383 us | 0.2707 us | 0.4272 |      - |   5.41 KB |
| Hyperbee.JsonNode       |   2.973 us |  1.8205 us | 0.0998 us | 0.5188 |      - |   6.42 KB |
| Newtonsoft.JObject      |   4.169 us |  1.0726 us | 0.0588 us | 1.2054 | 0.0534 |  14.85 KB |
|                         |            |            |           |        |        |           |
| `$.store.book[*].author` 
| JsonCraft.JsonElement   |   1.718 us |  0.1185 us | 0.0065 us | 0.2136 | 0.0019 |   2.63 KB |
| Hyperbee.JsonElement    |   1.910 us |  0.2440 us | 0.0134 us | 0.2480 |      - |   3.05 KB |
| JsonCons.JsonElement    |   1.972 us |  1.2539 us | 0.0687 us | 0.2861 |      - |   3.55 KB |
| Hyperbee.JsonNode       |   2.945 us |  0.7789 us | 0.0427 us | 0.5569 | 0.0076 |   6.83 KB |
| Newtonsoft.JObject      |   4.415 us |  1.5198 us | 0.0833 us | 1.1902 | 0.0534 |  14.64 KB |
| JsonEverything.JsonNode |   5.322 us |  0.6684 us | 0.0366 us | 1.0071 |      - |  12.45 KB |
|                         |            |            |           |        |        |           |
| `$.store.book[*]`  
| JsonCraft.JsonElement   |   1.428 us |  0.1454 us | 0.0080 us | 0.1984 |      - |   2.45 KB |
| Hyperbee.JsonElement    |   1.707 us |  0.3096 us | 0.0170 us | 0.2155 |      - |   2.65 KB |
| JsonCons.JsonElement    |   1.725 us |  1.0244 us | 0.0562 us | 0.2728 |      - |   3.35 KB |
| Hyperbee.JsonNode       |   1.948 us |  0.1280 us | 0.0070 us | 0.2575 |      - |   3.17 KB |
| JsonEverything.JsonNode |   3.429 us |  3.3142 us | 0.1817 us | 0.5379 | 0.0038 |   6.61 KB |
| Newtonsoft.JObject      |   4.222 us |  0.6171 us | 0.0338 us | 1.1826 | 0.0381 |  14.49 KB |
|                         |            |            |           |        |        |           |
| `$.store.book[0,1]`
| Hyperbee.JsonElement    |   1.542 us |  0.2708 us | 0.0148 us | 0.1945 |      - |   2.41 KB |
| JsonCraft.JsonElement   |   1.570 us |  1.4388 us | 0.0789 us | 0.2136 |      - |   2.64 KB |
| Hyperbee.JsonNode       |   1.862 us |  0.2584 us | 0.0142 us | 0.2403 |      - |   2.97 KB |
| JsonCons.JsonElement    |   1.922 us |  0.6784 us | 0.0372 us | 0.3014 |      - |   3.73 KB |
| JsonEverything.JsonNode |   3.016 us |  0.8618 us | 0.0472 us | 0.4883 |      - |   6.07 KB |
| Newtonsoft.JObject      |   4.287 us |  1.0252 us | 0.0562 us | 1.1902 | 0.0534 |  14.59 KB |
|                         |            |            |           |        |        |           |
| `$.store.book[0].title` 
| Hyperbee.JsonElement    |   1.457 us |  0.3260 us | 0.0179 us | 0.1755 |      - |   2.17 KB |
| JsonCraft.JsonElement   |   1.768 us |  1.8446 us | 0.1011 us | 0.2041 |      - |   2.51 KB |
| JsonCons.JsonElement    |   1.808 us |  0.5749 us | 0.0315 us | 0.2689 |      - |    3.3 KB |
| Hyperbee.JsonNode       |   1.926 us |  0.6973 us | 0.0382 us | 0.2937 |      - |   3.63 KB |
| JsonEverything.JsonNode |   3.203 us |  0.4522 us | 0.0248 us | 0.5951 |      - |   7.38 KB |
| Newtonsoft.JObject      |   4.559 us |  3.8857 us | 0.2130 us | 1.1902 | 0.0458 |  14.62 KB |
|                         |            |           |            |        |        |           | 
| `$.store.book[0]`                                     
| Hyperbee.JsonElement    |   1.349 us |  0.5027 us | 0.0276 us | 0.1755 |      - |   2.17 KB |
| JsonCraft.JsonElement   |   1.493 us |  0.5471 us | 0.0300 us | 0.1984 |      - |   2.44 KB |
| Hyperbee.JsonNode       |   1.657 us |  0.3633 us | 0.0199 us | 0.2327 |      - |   2.86 KB |
| JsonCons.JsonElement    |   1.733 us |  0.3863 us | 0.0212 us | 0.2613 |      - |   3.21 KB |
| JsonEverything.JsonNode |   2.670 us |  0.6862 us | 0.0376 us | 0.4616 | 0.0038 |   5.68 KB |
| Newtonsoft.JObject      |   4.155 us |  1.3012 us | 0.0713 us | 1.1749 | 0.0381 |  14.48 KB |
|                         |            |            |           |        |        |           |
| `$`
| JsonCraft.JsonElement   |   1.299 us |  0.3820 us | 0.0209 us | 0.1793 |      - |   2.22 KB |
| Hyperbee.JsonElement    |   1.312 us |  0.2933 us | 0.0161 us | 0.1755 |      - |   2.17 KB |
| Hyperbee.JsonNode       |   1.322 us |  0.3885 us | 0.0213 us | 0.1411 |      - |   1.75 KB |
| JsonEverything.JsonNode |   1.361 us |  0.2123 us | 0.0116 us | 0.1526 |      - |   1.88 KB |
| JsonCons.JsonElement    |   1.477 us |  0.6836 us | 0.0375 us | 0.2384 |      - |   2.94 KB |
| Newtonsoft.JObject      |   3.864 us |  0.4926 us | 0.0270 us | 1.1368 | 0.0381 |  13.98 KB |

## Credits

Hyperbee.Json is built upon the great work of several open-source projects. Special thanks to:

- System.Text.Json team for their work on the `System.Text.Json` library.
- Stefan Gössner for the original [JSONPath implementation](https://goessner.net/articles/JsonPath/).
- Atif Aziz's C# port of Goessner's JSONPath library [.NET JSONPath](https://github.com/atifaziz/JSONPath).  
- [JSONPath Compliance Test Suite Team](https://github.com/jsonpath-standard/jsonpath-compliance-test-suite).
- Christoph Burgmer [JSONPath consensus effort](https://cburgmer.github.io/json-path-comparison).
- [Just The Docs](https://github.com/just-the-docs/just-the-docs) for the documentation theme.

## Contributing

We welcome contributions! Please see our [Contributing Guide](https://github.com/Stillpoint-Software/.github/blob/main/.github/CONTRIBUTING.md) for more details.


# Status

| Branch     | Action                                                                                                                                                                                                                      |
|------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `develop`  | [![Build status](https://github.com/Stillpoint-Software/Hyperbee.Json/actions/workflows/pack_publish.yml/badge.svg?branch=develop)](https://github.com/Stillpoint-Software/Hyperbee.Json/actions/workflows/pack_publish.yml)  |
| `main`     | [![Build status](https://github.com/Stillpoint-Software/Hyperbee.Json/actions/workflows/pack_publish.yml/badge.svg)](https://github.com/Stillpoint-Software/Hyperbee.Json/actions/workflows/pack_publish.yml)                 |

# Help
 See [Todo](https://github.com/Stillpoint-Software/Hyperbee.Json/blob/main/docs/todo.md)

[![Hyperbee.Json](https://github.com/Stillpoint-Software/Hyperbee.Json/blob/main/assets/hyperbee.svg?raw=true)](https://github.com/Stillpoint-Software/Hyperbee.Json)

