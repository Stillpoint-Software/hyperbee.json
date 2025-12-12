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
| Method                  |  Mean      | Error     | StdDev    | Gen0   | Gen1   | Allocated |
|------------------------ | ----------:|----------:|----------:|-------:|-------:|----------:|
|`$..[?(@.price < 10)]`  
| JsonCraft.JsonElement   |   3.049 us | 0.7626 us | 0.0418 us | 0.2899 |      - |   3.59 KB |
| Hyperbee.JsonElement    |   5.400 us | 0.5197 us | 0.0285 us | 1.2894 | 0.0076 |  15.84 KB |
| Hyperbee.JsonNode       |   5.824 us | 1.8214 us | 0.0998 us | 1.4954 | 0.0305 |  18.38 KB |
| JsonCons.JsonElement    |   5.955 us | 2.4616 us | 0.1349 us | 1.0376 | 0.0076 |  12.73 KB |
| Newtonsoft.JObject      |   8.838 us | 1.3040 us | 0.0715 us | 2.1057 | 0.0763 |  25.86 KB |
| JsonEverything.JsonNode |  19.543 us | 9.1216 us | 0.5000 us | 3.9063 | 0.1221 |  48.15 KB |
|                         |            |           |           |        |        |           |
|`$..['bicycle','price']`
| JsonCraft.JsonElement   |   2.451 us | 0.3691 us | 0.0202 us | 0.3242 |      - |   4.01 KB |
| Hyperbee.JsonElement    |   2.689 us | 0.6920 us | 0.0379 us | 0.4158 |      - |   5.12 KB |
| JsonCons.JsonElement    |   3.217 us | 0.4797 us | 0.0263 us | 0.5798 | 0.0038 |   7.13 KB |
| Hyperbee.JsonNode       |   3.435 us | 0.8745 us | 0.0479 us | 0.7210 | 0.0114 |   8.84 KB |
| Newtonsoft.JObject      |   5.548 us | 1.7475 us | 0.0958 us | 1.1826 | 0.0458 |  14.55 KB |
| JsonEverything.JsonNode |  12.999 us | 0.5857 us | 0.0321 us | 2.3193 | 0.0610 |   28.5 KB |
|                         |            |           |           |        |        |           |
|`$..`  
| JsonCraft.JsonElement   |   2.110 us | 0.2534 us | 0.0139 us | 0.2327 |      - |   2.88 KB |
| Hyperbee.JsonElement    |   2.478 us | 0.3201 us | 0.0175 us | 0.5226 | 0.0038 |   6.45 KB |
| Hyperbee.JsonNode       |   3.434 us | 0.6438 us | 0.0353 us | 0.7782 | 0.0153 |   9.54 KB |
| JsonCons.JsonElement    |   3.450 us | 0.5528 us | 0.0303 us | 0.6866 | 0.0038 |   8.45 KB |
| Newtonsoft.JObject      |   5.217 us | 0.4099 us | 0.0225 us | 1.1520 | 0.0458 |  14.19 KB |
| JsonEverything.JsonNode |  17.330 us | 1.5400 us | 0.0844 us | 2.7466 | 0.0610 |  33.97 KB |
|                         |            |           |           |        |        |           |
|`$..author`
| JsonCraft.JsonElement   |   2.352 us | 0.4147 us | 0.0227 us | 0.2327 |      - |   2.88 KB |
| Hyperbee.JsonElement    |   2.359 us | 0.2998 us | 0.0164 us | 0.4158 |      - |    5.1 KB |
| JsonCons.JsonElement    |   2.826 us | 1.7249 us | 0.0945 us | 0.4463 | 0.0038 |   5.47 KB |
| Hyperbee.JsonNode       |   3.290 us | 2.1900 us | 0.1200 us | 0.7019 | 0.0038 |   8.64 KB |
| Newtonsoft.JObject      |   5.246 us | 0.8047 us | 0.0441 us | 1.1520 | 0.0381 |   14.2 KB |
| JsonEverything.JsonNode |  12.429 us | 6.1835 us | 0.3389 us | 2.1057 | 0.0305 |   26.1 KB |
|                         |            |           |           |        |        |           |
|`$..book[?@.isbn]`
| Hyperbee.JsonElement    |   2.883 us | 1.0678 us | 0.0585 us | 0.4997 | 0.0038 |   6.14 KB |
| JsonCons.JsonElement    |   3.362 us | 0.8878 us | 0.0487 us | 0.5836 | 0.0038 |   7.16 KB |
| Hyperbee.JsonNode       |   3.965 us | 3.7942 us | 0.2080 us | 0.7858 | 0.0153 |   9.64 KB |
| JsonEverything.JsonNode |  13.766 us | 1.2953 us | 0.0710 us | 2.4414 | 0.0610 |  29.98 KB |
| JsonCraft.JsonElement   |         NA |        NA |        NA |     NA |     NA |        NA |
| Newtonsoft.JObject      |         NA |        NA |        NA |     NA |     NA |        NA |
|                         |            |           |           |        |        |           |
|`$..book[?@.price == 8.99 && @.category == 'fiction']`
| Hyperbee.JsonElement    |   3.708 us | 1.8637 us | 0.1022 us | 0.6752 | 0.0038 |   8.28 KB |
| Hyperbee.JsonNode       |   4.746 us | 0.9875 us | 0.0541 us | 0.9460 |      - |  11.91 KB |
| JsonCons.JsonElement    |   4.845 us | 1.3375 us | 0.0733 us | 0.6866 |      - |   8.48 KB |
| JsonEverything.JsonNode |  17.256 us | 1.0914 us | 0.0598 us | 3.1738 | 0.0610 |  39.27 KB |
| JsonCraft.JsonElement   |         NA |        NA |        NA |     NA |     NA |        NA |
| Newtonsoft.JObject      |         NA |        NA |        NA |     NA |     NA |        NA |
|                         |            |           |           |        |        |           |
|`$..book[0,1]`
| JsonCraft.JsonElement   |   2.345 us | 0.1935 us | 0.0106 us | 0.2518 |      - |   3.09 KB |
| Hyperbee.JsonElement    |   2.417 us | 0.0175 us | 0.0010 us | 0.4158 |      - |    5.1 KB |
| JsonCons.JsonElement    |   3.005 us | 0.9922 us | 0.0544 us | 0.4959 | 0.0038 |    6.1 KB |
| Hyperbee.JsonNode       |   3.163 us | 1.0620 us | 0.0582 us | 0.7019 |      - |   8.64 KB |
| Newtonsoft.JObject      |   5.089 us | 0.8914 us | 0.0489 us | 1.1749 | 0.0458 |  14.45 KB |
| JsonEverything.JsonNode |  12.339 us | 0.9570 us | 0.0525 us | 2.1362 | 0.0610 |  26.41 KB |
|                         |            |           |           |        |        |           |
|`$..book[0]`
| JsonCraft.JsonElement   |   2.241 us | 0.1554 us | 0.0085 us | 0.2441 |      - |      3 KB |
| Hyperbee.JsonElement    |   2.271 us | 1.0724 us | 0.0588 us | 0.4158 |      - |    5.1 KB |
| JsonCons.JsonElement    |   2.708 us | 0.8340 us | 0.0457 us | 0.4501 |      - |   5.55 KB |
| Hyperbee.JsonNode       |   3.528 us | 0.2841 us | 0.0156 us | 0.7019 | 0.0038 |   8.64 KB |
| Newtonsoft.JObject      |   5.005 us | 0.3486 us | 0.0191 us | 1.1673 | 0.0381 |  14.33 KB |
| JsonEverything.JsonNode |  11.781 us | 1.7173 us | 0.0941 us | 2.1057 | 0.0610 |  26.02 KB |
|                         |            |           |           |        |        |           |
|`$.store..price`
| Hyperbee.JsonElement    |   2.293 us | 0.4706 us | 0.0258 us | 0.3853 |      - |   4.73 KB |
| JsonCraft.JsonElement   |   2.363 us | 1.0577 us | 0.0580 us | 0.2518 |      - |   3.13 KB |
| JsonCons.JsonElement    |   2.720 us | 2.0514 us | 0.1124 us | 0.4539 |      - |   5.57 KB |
| Hyperbee.JsonNode       |   3.181 us | 0.5370 us | 0.0294 us | 0.6828 |      - |   8.38 KB |
| Newtonsoft.JObject      |   5.083 us | 0.4737 us | 0.0260 us | 1.1673 | 0.0381 |  14.34 KB |
| JsonEverything.JsonNode |  13.102 us | 2.1212 us | 0.1163 us | 2.1667 | 0.0610 |  26.63 KB |
|                         |            |           |           |        |        |           |
|`$.store.*`
| JsonCraft.JsonElement   |   1.561 us | 0.1662 us | 0.0091 us | 0.2022 |      - |   2.49 KB |
| Hyperbee.JsonElement    |   1.602 us | 0.6889 us | 0.0378 us | 0.2289 |      - |   2.81 KB |
| JsonCons.JsonElement    |   1.693 us | 0.7460 us | 0.0409 us | 0.2651 |      - |   3.27 KB |
| Hyperbee.JsonNode       |   1.784 us | 0.6002 us | 0.0329 us | 0.2365 | 0.0019 |    2.9 KB |
| JsonEverything.JsonNode |   2.395 us | 0.3007 us | 0.0165 us | 0.3891 |      - |    4.8 KB |
| Newtonsoft.JObject      |   4.664 us | 0.6593 us | 0.0361 us | 1.1749 | 0.0458 |  14.43 KB |
|                         |            |           |           |        |        |           |
|`$.store.bicycle.color`
| Hyperbee.JsonElement    |   1.454 us | 0.6877 us | 0.0377 us | 0.1755 |      - |   2.17 KB |
| JsonCraft.JsonElement   |   1.585 us | 0.7476 us | 0.0410 us | 0.1984 |      - |   2.45 KB |
| Hyperbee.JsonNode       |   1.695 us | 0.7429 us | 0.0407 us | 0.2346 | 0.0019 |   2.88 KB |
| JsonCons.JsonElement    |   1.816 us | 1.7814 us | 0.0976 us | 0.2632 |      - |   3.23 KB |
| JsonEverything.JsonNode |   2.892 us | 0.3155 us | 0.0173 us | 0.4654 | 0.0038 |   5.74 KB |
| Newtonsoft.JObject      |   4.582 us | 0.7337 us | 0.0402 us | 1.1826 | 0.0381 |  14.49 KB |
|                         |            |           |           |        |        |           |
|`$.store.book[-1:]`
| Hyperbee.JsonElement    |   1.579 us | 0.5378 us | 0.0295 us | 0.1945 |      - |   2.41 KB |
| JsonCraft.JsonElement   |   1.622 us | 0.1762 us | 0.0097 us | 0.2098 |      - |   2.58 KB |
| Hyperbee.JsonNode       |   1.809 us | 0.3378 us | 0.0185 us | 0.2422 |      - |   2.97 KB |
| JsonCons.JsonElement    |   1.882 us | 0.5619 us | 0.0308 us | 0.2861 |      - |   3.52 KB |
| JsonEverything.JsonNode |   2.806 us | 0.3959 us | 0.0217 us | 0.4654 | 0.0038 |   5.72 KB |
| Newtonsoft.JObject      |   4.663 us | 0.5645 us | 0.0309 us | 1.1826 | 0.0534 |  14.52 KB |
|                         |            |           |           |        |        |           |
|`$.store.book[:2]`
| Hyperbee.JsonElement    |   1.604 us | 0.5879 us | 0.0322 us | 0.1945 |      - |   2.41 KB |
| JsonCraft.JsonElement   |   1.637 us | 0.0647 us | 0.0035 us | 0.2098 |      - |   2.58 KB |
| Hyperbee.JsonNode       |   1.826 us | 0.1527 us | 0.0084 us | 0.2422 |      - |   2.97 KB |
| JsonCons.JsonElement    |   1.871 us | 0.6744 us | 0.0370 us | 0.2880 |      - |   3.54 KB |
| JsonEverything.JsonNode |   3.068 us | 0.0441 us | 0.0024 us | 0.4883 | 0.0038 |   6.02 KB |
| Newtonsoft.JObject      |   5.069 us | 5.6079 us | 0.3074 us | 1.1826 | 0.0305 |  14.51 KB |
|                         |            |           |           |        |        |           |
|`$.store.book[?(@.author && @.title)]`
| JsonCraft.JsonElement   |   2.007 us | 0.5824 us | 0.0319 us | 0.2670 |      - |    3.3 KB |
| Hyperbee.JsonElement    |   2.387 us | 0.5434 us | 0.0298 us | 0.3395 |      - |   4.18 KB |
| JsonCons.JsonElement    |   2.970 us | 0.5634 us | 0.0309 us | 0.4539 | 0.0038 |   5.58 KB |
| Hyperbee.JsonNode       |   3.636 us | 1.5759 us | 0.0864 us | 0.6561 | 0.0076 |   8.08 KB |
| Newtonsoft.JObject      |   5.449 us | 1.4272 us | 0.0782 us | 1.3199 | 0.0458 |  16.18 KB |
| JsonEverything.JsonNode |   7.147 us | 3.7089 us | 0.2033 us | 1.4648 | 0.0305 |  18.32 KB |
|                         |            |           |           |        |        |           |
|`$.store.book[?(@.category == 'fiction')]`
| JsonCraft.JsonElement   |   2.243 us | 1.6372 us | 0.0897 us | 0.2747 |      - |   3.38 KB |
| JsonCons.JsonElement    |   2.800 us | 0.8079 us | 0.0443 us | 0.4082 | 0.0038 |   5.01 KB |
| Hyperbee.JsonElement    |   2.825 us | 2.4544 us | 0.1345 us | 0.3510 |      - |   4.34 KB |
| Hyperbee.JsonNode       |   3.391 us | 3.0949 us | 0.1696 us | 0.6561 |      - |    8.2 KB |
| Newtonsoft.JObject      |   5.009 us | 0.9350 us | 0.0513 us | 1.2817 | 0.0458 |  15.74 KB |
| JsonEverything.JsonNode |   7.002 us | 0.9434 us | 0.0517 us | 1.3428 | 0.0305 |  16.49 KB |
|                         |            |           |           |        |        |           |
|`$.store.book[?(@.price < 10)].title`
| JsonCraft.JsonElement   |   2.357 us | 1.1758 us | 0.0644 us | 0.2747 |      - |   3.37 KB |
| Hyperbee.JsonElement    |   2.496 us | 1.2422 us | 0.0681 us | 0.3548 |      - |   4.35 KB |
| JsonCons.JsonElement    |   2.954 us | 1.3447 us | 0.0737 us | 0.4196 |      - |   5.18 KB |
| Hyperbee.JsonNode       |   3.466 us | 0.7381 us | 0.0405 us | 0.6561 |      - |   8.09 KB |
| Newtonsoft.JObject      |   5.209 us | 1.2339 us | 0.0676 us | 1.2894 | 0.0534 |  15.89 KB |
| JsonEverything.JsonNode |   7.547 us | 1.3411 us | 0.0735 us | 1.4038 | 0.0305 |  17.38 KB |
|                         |            |           |           |        |        |           |
|`$.store.book[?(@.price > 10 && @.price < 20)]`
| JsonCraft.JsonElement   |   2.798 us | 1.2832 us | 0.0703 us | 0.3090 |      - |   3.82 KB |
| Hyperbee.JsonElement    |   3.067 us | 1.9850 us | 0.1088 us | 0.4349 | 0.0038 |   5.37 KB |
| Hyperbee.JsonNode       |   3.827 us | 2.2293 us | 0.1222 us | 0.7324 |      - |   9.14 KB |
| JsonCons.JsonElement    |   4.398 us | 3.5937 us | 0.1970 us | 0.5035 |      - |   6.23 KB |
| Newtonsoft.JObject      |   5.358 us | 2.0125 us | 0.1103 us | 1.3580 | 0.0534 |  16.69 KB |
| JsonEverything.JsonNode |   9.003 us | 2.3392 us | 0.1282 us | 1.8005 | 0.0305 |  22.27 KB |
|                         |            |           |           |        |        |           |
|`$.store.book[?@.price == 8.99]`
| Hyperbee.JsonElement    |   2.291 us | 1.2891 us | 0.0707 us | 0.3357 |      - |   4.15 KB |
| JsonCons.JsonElement    |   2.826 us | 1.0191 us | 0.0559 us | 0.4044 |      - |   4.97 KB |
| Hyperbee.JsonNode       |   3.630 us | 3.3549 us | 0.1839 us | 0.6409 |      - |   7.89 KB |
| JsonEverything.JsonNode |   6.450 us | 1.0062 us | 0.0552 us | 1.2512 | 0.0305 |  15.47 KB |
| JsonCraft.JsonElement   |         NA |        NA |        NA |     NA |     NA |        NA |
| Newtonsoft.JObject      |         NA |        NA |        NA |     NA |     NA |        NA |
|                         |            |           |           |        |        |           |
|`$.store.book['category','author']`
| JsonCraft.JsonElement   |   1.650 us | 0.2189 us | 0.0120 us | 0.2403 |      - |   2.95 KB |
| JsonCons.JsonElement    |   1.862 us | 0.1098 us | 0.0060 us | 0.2937 | 0.0019 |   3.61 KB |
| Hyperbee.JsonElement    |   1.988 us | 0.8489 us | 0.0465 us | 0.2098 |      - |   2.61 KB |
| JsonEverything.JsonNode |   2.622 us | 0.8004 us | 0.0439 us | 0.4387 | 0.0038 |   5.41 KB |
| Hyperbee.JsonNode       |   3.215 us | 1.1826 us | 0.0648 us | 0.5226 | 0.0076 |   6.42 KB |
| Newtonsoft.JObject      |   4.681 us | 0.5122 us | 0.0281 us | 1.2054 | 0.0534 |  14.85 KB |
|                         |            |           |           |        |        |           |
|`$.store.book[*].author`
| JsonCraft.JsonElement   |   1.729 us | 0.7445 us | 0.0408 us | 0.2136 | 0.0019 |   2.63 KB |
| JsonCons.JsonElement    |   1.909 us | 0.4640 us | 0.0254 us | 0.2861 |      - |   3.55 KB |
| Hyperbee.JsonElement    |   1.993 us | 0.3660 us | 0.0201 us | 0.2480 |      - |   3.05 KB |
| Hyperbee.JsonNode       |   2.885 us | 2.4990 us | 0.1370 us | 0.5493 |      - |   6.83 KB |
| Newtonsoft.JObject      |   4.894 us | 6.5976 us | 0.3616 us | 1.1902 | 0.0534 |  14.64 KB |
| JsonEverything.JsonNode |   5.893 us | 1.3873 us | 0.0760 us | 1.0071 |      - |  12.45 KB |
|                         |            |           |           |        |        |           |
|`$.store.book[*]`
| JsonCraft.JsonElement   |   1.578 us | 0.8516 us | 0.0467 us | 0.1984 |      - |   2.45 KB |
| JsonCons.JsonElement    |   1.692 us | 0.8482 us | 0.0465 us | 0.2728 |      - |   3.35 KB |
| Hyperbee.JsonElement    |   1.738 us | 0.3719 us | 0.0204 us | 0.2155 |      - |   2.65 KB |
| Hyperbee.JsonNode       |   1.923 us | 0.7320 us | 0.0401 us | 0.2556 |      - |   3.17 KB |
| JsonEverything.JsonNode |   3.526 us | 0.0963 us | 0.0053 us | 0.5341 |      - |   6.61 KB |
| Newtonsoft.JObject      |   4.752 us | 0.2905 us | 0.0159 us | 1.1826 | 0.0381 |  14.49 KB |
|                         |            |           |           |        |        |           |
|`$.store.book[0,1]`
| Hyperbee.JsonElement    |   1.560 us | 0.6522 us | 0.0358 us | 0.1945 |      - |   2.41 KB |
| JsonCraft.JsonElement   |   1.675 us | 0.3241 us | 0.0178 us | 0.2136 |      - |   2.64 KB |
| JsonCons.JsonElement    |   1.910 us | 0.3014 us | 0.0165 us | 0.3033 |      - |   3.73 KB |
| Hyperbee.JsonNode       |   1.976 us | 1.1111 us | 0.0609 us | 0.2403 |      - |   2.97 KB |
| JsonEverything.JsonNode |   3.071 us | 0.1558 us | 0.0085 us | 0.4883 |      - |   6.07 KB |
| Newtonsoft.JObject      |   4.870 us | 0.8053 us | 0.0441 us | 1.1902 | 0.0534 |  14.59 KB |
|                         |            |           |           |        |        |           |
|`$.store.book[0].title`
| Hyperbee.JsonElement    |   1.515 us | 0.1457 us | 0.0080 us | 0.1755 |      - |   2.17 KB |
| JsonCraft.JsonElement   |   1.713 us | 1.1282 us | 0.0618 us | 0.2041 |      - |   2.51 KB |
| JsonCons.JsonElement    |   1.869 us | 0.0938 us | 0.0051 us | 0.2689 |      - |    3.3 KB |
| Hyperbee.JsonNode       |   1.945 us | 0.2311 us | 0.0127 us | 0.2937 |      - |    3.6 KB |
| JsonEverything.JsonNode |   3.451 us | 0.4020 us | 0.0220 us | 0.5951 |      - |   7.38 KB |
| Newtonsoft.JObject      |   4.706 us | 1.4286 us | 0.0783 us | 1.1902 | 0.0458 |  14.62 KB |
|                         |            |           |           |        |        |           |
|`$.store.book[0]`
| Hyperbee.JsonElement    |   1.392 us | 0.6219 us | 0.0341 us | 0.1755 |      - |   2.17 KB |
| JsonCraft.JsonElement   |   1.570 us | 0.2170 us | 0.0119 us | 0.1984 |      - |   2.44 KB |
| JsonCons.JsonElement    |   1.783 us | 0.9320 us | 0.0511 us | 0.2613 |      - |   3.21 KB |
| Hyperbee.JsonNode       |   1.870 us | 0.6578 us | 0.0361 us | 0.2308 |      - |   2.83 KB |
| JsonEverything.JsonNode |   2.832 us | 0.6614 us | 0.0363 us | 0.4616 | 0.0038 |   5.68 KB |
| Newtonsoft.JObject      |   4.697 us | 0.9290 us | 0.0509 us | 1.1749 | 0.0381 |  14.48 KB |
|                         |            |           |           |        |        |           |
|`$`
| Hyperbee.JsonElement    |   1.316 us | 0.7199 us | 0.0395 us | 0.1755 |      - |   2.17 KB |
| JsonCraft.JsonElement   |   1.368 us | 0.0907 us | 0.0050 us | 0.1793 |      - |   2.22 KB |
| JsonEverything.JsonNode |   1.438 us | 0.1670 us | 0.0092 us | 0.1526 |      - |   1.88 KB |
| Hyperbee.JsonNode       |   1.440 us | 0.2725 us | 0.0149 us | 0.1411 |      - |   1.75 KB |
| JsonCons.JsonElement    |   1.456 us | 0.1789 us | 0.0098 us | 0.2384 |      - |   2.94 KB |
| Newtonsoft.JObject      |   4.331 us | 1.0036 us | 0.0550 us | 1.1368 | 0.0381 |  13.98 KB |

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

