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

| Method                  | Mean      | Error     | StdDev    | Gen0   | Gen1   | Allocated |
|------------------------ |----------:|----------:|----------:|-------:|-------:|----------:|
|`$..[?(@.price < 10)]`  
| JsonCraft.JsonElement   |  3.841 us | 0.3111 us | 0.0171 us | 0.2899 |      - |   3.59 KB |
| JsonCons.JsonElement    |  7.459 us | 0.6412 us | 0.0351 us | 1.0376 | 0.0076 |  12.77 KB |
| Hyperbee.JsonElement    |  8.284 us | 2.5497 us | 0.1398 us | 1.6785 | 0.0153 |  20.73 KB |
| Hyperbee.JsonNode       | 11.086 us | 8.0907 us | 0.4435 us | 1.8921 |      - |  23.79 KB |
| Newtonsoft.JObject      | 12.153 us | 2.4881 us | 0.1364 us | 2.1057 | 0.0763 |  25.86 KB |
| JsonEverything.JsonNode | 23.541 us | 1.3946 us | 0.0764 us | 3.9063 | 0.1221 |  48.15 KB |
|                         |           |           |           |        |        |           |
|`$..['bicycle','price']`   
| JsonCraft.JsonElement   |  3.136 us | 0.2760 us | 0.0151 us | 0.3242 |      - |   4.01 KB |
| Hyperbee.JsonElement    |  3.578 us | 0.4623 us | 0.0253 us | 0.4349 | 0.0038 |   5.37 KB |
| JsonCons.JsonElement    |  3.948 us | 0.6099 us | 0.0334 us | 0.5798 |      - |   7.18 KB |
| Hyperbee.JsonNode       |  5.181 us | 1.8016 us | 0.0988 us | 0.7477 | 0.0076 |   9.23 KB |
| Newtonsoft.JObject      |  7.823 us | 0.8017 us | 0.0439 us | 1.1749 | 0.0458 |  14.55 KB |
| JsonEverything.JsonNode | 16.753 us | 1.5507 us | 0.0850 us | 2.3193 | 0.0610 |   28.5 KB |
|         
|`$..`                    |           |           |           |        |        |           |
| JsonCraft.JsonElement   |  2.497 us | 0.0903 us | 0.0049 us | 0.2327 |      - |   2.88 KB |
| Hyperbee.JsonElement    |  3.299 us | 0.8178 us | 0.0448 us | 0.5302 | 0.0038 |   6.51 KB |
| JsonCons.JsonElement    |  4.176 us | 0.5887 us | 0.0323 us | 0.6866 |      - |   8.49 KB |
| Hyperbee.JsonNode       |  5.330 us | 0.7684 us | 0.0421 us | 0.8392 |      - |   10.3 KB |
| Newtonsoft.JObject      |  7.784 us | 0.5303 us | 0.0291 us | 1.1444 | 0.0458 |  14.19 KB |
| JsonEverything.JsonNode | 21.226 us | 1.0905 us | 0.0598 us | 2.7466 | 0.0610 |  33.97 KB |
|                         |           |           |           |        |        |           |
|`$..author`
| JsonCraft.JsonElement   |  2.904 us | 4.2915 us | 0.2352 us | 0.2327 |      - |   2.88 KB |
| Hyperbee.JsonElement    |  3.068 us | 0.3672 us | 0.0201 us | 0.4196 | 0.0038 |   5.16 KB |
| JsonCons.JsonElement    |  3.381 us | 0.6294 us | 0.0345 us | 0.4501 |      - |   5.55 KB |
| Hyperbee.JsonNode       |  5.030 us | 0.6100 us | 0.0334 us | 0.7324 | 0.0076 |   9.02 KB |
| Newtonsoft.JObject      |  7.469 us | 1.7563 us | 0.0963 us | 1.1444 | 0.0305 |   14.2 KB |
| JsonEverything.JsonNode | 15.752 us | 3.8966 us | 0.2136 us | 2.1057 | 0.0305 |   26.1 KB |
|                         |           |           |           |        |        |           |
|`$..book[?@.isbn]`
| Hyperbee.JsonElement    |  3.913 us | 1.5846 us | 0.0869 us | 0.5493 |      - |    6.8 KB |
| JsonCons.JsonElement    |  4.359 us | 2.1655 us | 0.1187 us | 0.5875 |      - |   7.21 KB |
| Hyperbee.JsonNode       |  5.335 us | 0.7057 us | 0.0387 us | 0.8621 | 0.0153 |  10.62 KB |
| JsonEverything.JsonNode | 17.200 us | 1.9836 us | 0.1087 us | 2.4414 | 0.0610 |  29.98 KB |
| JsonCraft.JsonElement   |        NA |        NA |        NA |     NA |     NA |        NA |
| Newtonsoft.JObject      |        NA |        NA |        NA |     NA |     NA |        NA |
|                         |           |           |           |        |        |           |
|`$..book[?@.price == 8.99 && @.category == 'fiction']`
| Hyperbee.JsonElement    |  5.135 us | 1.0302 us | 0.0565 us | 0.7706 |      - |   9.47 KB |
| JsonCons.JsonElement    |  6.105 us | 0.6309 us | 0.0346 us | 0.6943 | 0.0076 |   8.52 KB |
| Hyperbee.JsonNode       |  7.002 us | 3.0008 us | 0.1645 us | 1.0910 | 0.0229 |  13.41 KB |
| JsonEverything.JsonNode | 21.845 us | 3.9271 us | 0.2153 us | 3.2043 | 0.0916 |  39.52 KB |
| JsonCraft.JsonElement   |        NA |        NA |        NA |     NA |     NA |        NA |
| Newtonsoft.JObject      |        NA |        NA |        NA |     NA |     NA |        NA |
|                         |           |           |           |        |        |           |
`$..book[0,1]`
| JsonCraft.JsonElement   |  2.936 us | 0.6578 us | 0.0361 us | 0.2518 |      - |   3.09 KB |
| Hyperbee.JsonElement    |  3.157 us | 0.8302 us | 0.0455 us | 0.4196 | 0.0038 |   5.16 KB |
| JsonCons.JsonElement    |  3.691 us | 0.1430 us | 0.0078 us | 0.4997 |      - |   6.15 KB |
| Hyperbee.JsonNode       |  4.972 us | 0.6586 us | 0.0361 us | 0.7324 | 0.0076 |   9.02 KB |
| Newtonsoft.JObject      |  7.441 us | 0.5961 us | 0.0327 us | 1.1749 | 0.0458 |  14.45 KB |
| JsonEverything.JsonNode | 15.523 us | 5.4908 us | 0.3010 us | 2.1362 | 0.0610 |  26.41 KB |
|                         |           |           |           |        |        |           |
|`$..book[0]`
| JsonCraft.JsonElement   |  2.825 us | 0.0934 us | 0.0051 us | 0.2441 |      - |      3 KB |
| Hyperbee.JsonElement    |  3.114 us | 0.4106 us | 0.0225 us | 0.4196 | 0.0038 |   5.16 KB |
| JsonCons.JsonElement    |  3.451 us | 0.2921 us | 0.0160 us | 0.4578 | 0.0038 |   5.63 KB |
| Hyperbee.JsonNode       |  4.691 us | 1.4048 us | 0.0770 us | 0.7324 | 0.0076 |   9.02 KB |
| Newtonsoft.JObject      |  7.720 us | 0.8748 us | 0.0480 us | 1.1597 | 0.0458 |  14.33 KB |
| JsonEverything.JsonNode | 15.314 us | 3.3459 us | 0.1834 us | 2.1210 | 0.0610 |  26.02 KB |
|                         |           |           |           |        |        |           |
|`$.store..price`
| JsonCraft.JsonElement   |  2.912 us | 1.3083 us | 0.0717 us | 0.2518 |      - |   3.13 KB |
| Hyperbee.JsonElement    |  2.993 us | 0.4534 us | 0.0249 us | 0.3891 | 0.0038 |    4.8 KB |
| JsonCons.JsonElement    |  3.446 us | 0.9305 us | 0.0510 us | 0.4578 |      - |   5.62 KB |
| Hyperbee.JsonNode       |  4.721 us | 0.9516 us | 0.0522 us | 0.7095 | 0.0076 |    8.7 KB |
| Newtonsoft.JObject      |  7.723 us | 1.1978 us | 0.0657 us | 1.1597 | 0.0305 |  14.34 KB |
| JsonEverything.JsonNode | 15.966 us | 1.1138 us | 0.0610 us | 2.1362 | 0.0610 |  26.63 KB |
|                         |           |           |           |        |        |           |
|`$.store.*`
| JsonCraft.JsonElement   |  1.882 us | 0.5586 us | 0.0306 us | 0.2022 |      - |   2.49 KB |
| JsonCons.JsonElement    |  2.151 us | 0.1067 us | 0.0058 us | 0.2670 |      - |   3.31 KB |
| Hyperbee.JsonElement    |  2.167 us | 0.6457 us | 0.0354 us | 0.2327 |      - |   2.88 KB |
| Hyperbee.JsonNode       |  2.435 us | 0.9690 us | 0.0531 us | 0.2403 |      - |   2.95 KB |
| JsonEverything.JsonNode |  3.047 us | 0.4674 us | 0.0256 us | 0.3891 |      - |    4.8 KB |
| Newtonsoft.JObject      |  6.576 us | 0.8290 us | 0.0454 us | 1.1749 | 0.0458 |  14.43 KB |
|                         |           |           |           |        |        |           |
|`$.store.bicycle.color`
| Hyperbee.JsonElement    |  1.920 us | 0.6179 us | 0.0339 us | 0.1869 |      - |    2.3 KB |
| JsonCraft.JsonElement   |  2.032 us | 0.7770 us | 0.0426 us | 0.2022 |      - |   2.49 KB |
| JsonCons.JsonElement    |  2.216 us | 0.1473 us | 0.0081 us | 0.2670 |      - |   3.27 KB |
| Hyperbee.JsonNode       |  2.383 us | 1.2149 us | 0.0666 us | 0.2327 |      - |   2.88 KB |
| JsonEverything.JsonNode |  3.556 us | 0.5152 us | 0.0282 us | 0.4654 | 0.0038 |   5.74 KB |
| Newtonsoft.JObject      |  6.700 us | 1.6404 us | 0.0899 us | 1.1826 | 0.0381 |  14.49 KB |
|                         |           |           |           |        |        |           |
|`$.store.book[-1:]`
| JsonCraft.JsonElement   |  2.004 us | 0.3158 us | 0.0173 us | 0.2098 |      - |   2.58 KB |
| Hyperbee.JsonElement    |  2.087 us | 0.1721 us | 0.0094 us | 0.1984 |      - |   2.47 KB |
| JsonCons.JsonElement    |  2.399 us | 0.3533 us | 0.0194 us | 0.2899 |      - |   3.56 KB |
| Hyperbee.JsonNode       |  2.467 us | 0.9814 us | 0.0538 us | 0.2403 |      - |   2.97 KB |
| JsonEverything.JsonNode |  3.679 us | 0.4354 us | 0.0239 us | 0.4654 | 0.0038 |   5.72 KB |
| Newtonsoft.JObject      |  6.472 us | 1.5636 us | 0.0857 us | 1.1826 | 0.0534 |  14.52 KB |
|                         |           |           |           |        |        |           |
|`$.store.book[:2]`
| JsonCraft.JsonElement   |  2.010 us | 0.1463 us | 0.0080 us | 0.2098 |      - |   2.58 KB |
| Hyperbee.JsonElement    |  2.128 us | 0.4884 us | 0.0268 us | 0.1984 |      - |   2.47 KB |
| JsonCons.JsonElement    |  2.382 us | 0.0585 us | 0.0032 us | 0.2899 |      - |   3.59 KB |
| Hyperbee.JsonNode       |  2.450 us | 0.1728 us | 0.0095 us | 0.2403 |      - |   2.97 KB |
| JsonEverything.JsonNode |  4.019 us | 2.1096 us | 0.1156 us | 0.4883 | 0.0038 |   6.02 KB |
| Newtonsoft.JObject      |  6.899 us | 0.6775 us | 0.0371 us | 1.1826 | 0.0305 |  14.51 KB |
|                         |           |           |           |        |        |           |
|`$.store.book[?(@.author && @.title)]`
| JsonCraft.JsonElement   |  2.567 us | 0.2239 us | 0.0123 us | 0.2670 |      - |    3.3 KB |
| Hyperbee.JsonElement    |  3.362 us | 0.2369 us | 0.0130 us | 0.4501 |      - |   5.52 KB |
| JsonCons.JsonElement    |  3.805 us | 0.8769 us | 0.0481 us | 0.4578 | 0.0038 |   5.63 KB |
| Hyperbee.JsonNode       |  5.128 us | 1.1406 us | 0.0625 us | 0.7477 | 0.0076 |   9.23 KB |
| Newtonsoft.JObject      |  7.514 us | 1.6892 us | 0.0926 us | 1.3199 | 0.0458 |  16.18 KB |
| JsonEverything.JsonNode |  9.261 us | 3.4741 us | 0.1904 us | 1.4801 | 0.0305 |  18.32 KB |
|                         |           |           |           |        |        |           |
|`$.store.book[?(@.category == 'fiction')]`
| JsonCraft.JsonElement   |  2.734 us | 0.3242 us | 0.0178 us | 0.2747 |      - |   3.38 KB |
| Hyperbee.JsonElement    |  3.315 us | 0.3024 us | 0.0166 us | 0.4120 |      - |   5.09 KB |
| JsonCons.JsonElement    |  3.426 us | 1.0773 us | 0.0590 us | 0.4120 |      - |   5.05 KB |
| Hyperbee.JsonNode       |  5.003 us | 0.8363 us | 0.0458 us | 0.7248 | 0.0153 |   8.89 KB |
| Newtonsoft.JObject      |  7.213 us | 1.1931 us | 0.0654 us | 1.2817 | 0.0458 |  15.74 KB |
| JsonEverything.JsonNode |  8.898 us | 1.8821 us | 0.1032 us | 1.3428 | 0.0305 |  16.49 KB |
|                         |           |           |           |        |        |           |
|`$.store.book[?(@.price < 10)].title`
| JsonCraft.JsonElement   |  3.108 us | 1.8864 us | 0.1034 us | 0.2747 |      - |   3.37 KB |
| Hyperbee.JsonElement    |  3.353 us | 0.4814 us | 0.0264 us | 0.4158 |      - |    5.1 KB |
| JsonCons.JsonElement    |  4.008 us | 0.5005 us | 0.0274 us | 0.4272 |      - |   5.27 KB |
| Hyperbee.JsonNode       |  5.285 us | 0.7662 us | 0.0420 us | 0.7019 |      - |   8.78 KB |
| Newtonsoft.JObject      |  7.687 us | 1.0056 us | 0.0551 us | 1.2817 | 0.0458 |  15.89 KB |
| JsonEverything.JsonNode |  9.513 us | 6.3528 us | 0.3482 us | 1.4038 |      - |  17.38 KB |
|                         |           |           |           |        |        |           |
|`$.store.book[?(@.price > 10 && @.price < 20)]`
| JsonCraft.JsonElement   |  3.475 us | 0.0797 us | 0.0044 us | 0.3090 |      - |   3.82 KB |
| Hyperbee.JsonElement    |  4.010 us | 0.6169 us | 0.0338 us | 0.5341 |      - |   6.55 KB |
| JsonCons.JsonElement    |  5.102 us | 1.1449 us | 0.0628 us | 0.5112 |      - |   6.28 KB |
| Hyperbee.JsonNode       |  6.086 us | 1.3252 us | 0.0726 us | 0.8316 | 0.0153 |  10.27 KB |
| Newtonsoft.JObject      |  8.050 us | 0.6497 us | 0.0356 us | 1.3580 | 0.0458 |  16.69 KB |
| JsonEverything.JsonNode | 11.612 us | 0.2688 us | 0.0147 us | 1.8158 | 0.0458 |  22.27 KB |
|                         |           |           |           |        |        |           |
|`$.store.book[?@.price == 8.99]`
| Hyperbee.JsonElement    |  3.162 us | 0.9502 us | 0.0521 us | 0.3967 |      - |    4.9 KB |
| JsonCons.JsonElement    |  3.822 us | 0.0869 us | 0.0048 us | 0.4044 |      - |   5.02 KB |
| Hyperbee.JsonNode       |  4.889 us | 1.5106 us | 0.0828 us | 0.6943 | 0.0076 |   8.58 KB |
| JsonEverything.JsonNode |  8.310 us | 2.0101 us | 0.1102 us | 1.2512 | 0.0305 |  15.47 KB |
| JsonCraft.JsonElement   |        NA |        NA |        NA |     NA |     NA |        NA |
| Newtonsoft.JObject      |        NA |        NA |        NA |     NA |     NA |        NA |
|                         |           |           |           |        |        |           |
|`$.store.book['category','author']`
| JsonCraft.JsonElement   |  2.084 us | 0.4663 us | 0.0256 us | 0.2403 |      - |   2.95 KB |
| JsonCons.JsonElement    |  2.480 us | 0.0770 us | 0.0042 us | 0.2937 |      - |   3.61 KB |
| Hyperbee.JsonElement    |  2.569 us | 1.1453 us | 0.0628 us | 0.2174 |      - |   2.67 KB |
| JsonEverything.JsonNode |  3.309 us | 0.2755 us | 0.0151 us | 0.4387 | 0.0038 |   5.41 KB |
| Hyperbee.JsonNode       |  4.142 us | 1.4670 us | 0.0804 us | 0.5188 | 0.0076 |   6.42 KB |
| Newtonsoft.JObject      |  6.737 us | 0.5860 us | 0.0321 us | 1.2054 | 0.0534 |  14.85 KB |
|                         |           |           |           |        |        |           |
|`$.store.book[*].author`
| JsonCraft.JsonElement   |  2.256 us | 1.4487 us | 0.0794 us | 0.2136 |      - |   2.63 KB |
| Hyperbee.JsonElement    |  2.469 us | 0.7492 us | 0.0411 us | 0.2518 |      - |   3.12 KB |
| JsonCons.JsonElement    |  2.496 us | 0.3427 us | 0.0188 us | 0.2899 |      - |   3.59 KB |
| Hyperbee.JsonNode       |  4.088 us | 0.0348 us | 0.0019 us | 0.5569 | 0.0076 |   6.83 KB |
| Newtonsoft.JObject      |  7.169 us | 1.8980 us | 0.1040 us | 1.1902 | 0.0534 |  14.64 KB |
| JsonEverything.JsonNode |  7.511 us | 0.2592 us | 0.0142 us | 1.0071 | 0.0153 |  12.45 KB |
|                         |           |           |           |        |        |           |
|`$.store.book[*]`
| JsonCraft.JsonElement   |  2.048 us | 1.8693 us | 0.1025 us | 0.2022 |      - |   2.48 KB |
| Hyperbee.JsonElement    |  2.179 us | 0.6664 us | 0.0365 us | 0.2213 |      - |   2.71 KB |
| JsonCons.JsonElement    |  2.246 us | 0.1811 us | 0.0099 us | 0.2747 |      - |    3.4 KB |
| Hyperbee.JsonNode       |  2.672 us | 0.2099 us | 0.0115 us | 0.2556 |      - |   3.17 KB |
| JsonEverything.JsonNode |  4.315 us | 0.0253 us | 0.0014 us | 0.5341 |      - |   6.61 KB |
| Newtonsoft.JObject      |  6.632 us | 1.6876 us | 0.0925 us | 1.1826 | 0.0381 |  14.49 KB |
|                         |           |           |           |        |        |           |
|`$.store.book[0,1]`
| Hyperbee.JsonElement    |  2.049 us | 0.0126 us | 0.0007 us | 0.1984 |      - |   2.47 KB |
| JsonCraft.JsonElement   |  2.056 us | 0.2169 us | 0.0119 us | 0.2136 |      - |   2.64 KB |
| Hyperbee.JsonNode       |  2.435 us | 0.5524 us | 0.0303 us | 0.2403 |      - |   2.97 KB |
| JsonCons.JsonElement    |  2.554 us | 1.3052 us | 0.0715 us | 0.3052 |      - |   3.77 KB |
| JsonEverything.JsonNode |  3.987 us | 0.4523 us | 0.0248 us | 0.4883 |      - |   6.07 KB |
| Newtonsoft.JObject      |  6.648 us | 0.7321 us | 0.0401 us | 1.1902 | 0.0534 |  14.59 KB |
|                         |           |           |           |        |        |           |
|`$.store.book[0].title`
| Hyperbee.JsonElement    |  1.897 us | 0.1620 us | 0.0089 us | 0.1831 |      - |   2.27 KB |
| JsonCraft.JsonElement   |  2.055 us | 0.2876 us | 0.0158 us | 0.2060 |      - |   2.55 KB |
| JsonCons.JsonElement    |  2.318 us | 0.3233 us | 0.0177 us | 0.2708 |      - |   3.35 KB |
| Hyperbee.JsonNode       |  2.575 us | 0.1096 us | 0.0060 us | 0.2937 |      - |   3.63 KB |
| JsonEverything.JsonNode |  4.534 us | 1.5492 us | 0.0849 us | 0.5951 | 0.0076 |   7.38 KB |
| Newtonsoft.JObject      |  6.734 us | 2.0745 us | 0.1137 us | 1.1902 | 0.0458 |  14.62 KB |
|                         |           |           |           |        |        |           |
|`$.store.book[0]`
| Hyperbee.JsonElement    |  1.931 us | 0.1500 us | 0.0082 us | 0.1831 |      - |   2.27 KB |
| JsonCraft.JsonElement   |  1.996 us | 0.1804 us | 0.0099 us | 0.1984 |      - |   2.48 KB |
| Hyperbee.JsonNode       |  2.204 us | 0.1747 us | 0.0096 us | 0.2327 |      - |   2.86 KB |
| JsonCons.JsonElement    |  2.221 us | 0.1388 us | 0.0076 us | 0.2632 |      - |   3.26 KB |
| JsonEverything.JsonNode |  3.592 us | 0.3654 us | 0.0200 us | 0.4616 | 0.0038 |   5.68 KB |
| Newtonsoft.JObject      |  6.892 us | 2.7132 us | 0.1487 us | 1.1749 | 0.0381 |  14.48 KB |
|                         |           |           |           |        |        |           |
|`$`
| JsonCraft.JsonElement   |  1.732 us | 0.0529 us | 0.0029 us | 0.1831 |      - |   2.26 KB |
| Hyperbee.JsonElement    |  1.751 us | 0.0628 us | 0.0034 us | 0.1850 |      - |   2.27 KB |
| JsonEverything.JsonNode |  1.767 us | 0.0675 us | 0.0037 us | 0.1526 |      - |   1.88 KB |
| Hyperbee.JsonNode       |  1.790 us | 0.1222 us | 0.0067 us | 0.1450 |      - |   1.78 KB |
| JsonCons.JsonElement    |  1.929 us | 0.1539 us | 0.0084 us | 0.2422 |      - |   2.98 KB |
| Newtonsoft.JObject      |  6.587 us | 0.7309 us | 0.0401 us | 1.1368 | 0.0381 |  14.01 KB |

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