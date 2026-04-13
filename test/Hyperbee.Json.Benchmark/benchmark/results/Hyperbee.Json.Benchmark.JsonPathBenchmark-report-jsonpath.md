```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8117/25H2/2025Update/HudsonValley2)
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.104
  [Host]  : .NET 10.0.4 (10.0.4, 10.0.426.12010), X64 RyuJIT x86-64-v3
  .NET 10 : .NET 10.0.4 (10.0.4, 10.0.426.12010), X64 RyuJIT x86-64-v3


 | Method                   |          Mean |         Error |        StdDev |  Allocated
 | :----------------------- | ------------: | ------------: | ------------: | ---------:
 | `$..[?(@.price < 10)]`
 | Hyperbee.JsonElement     |   6,589.72 ns |  13,724.37 ns |    752.279 ns |    13408 B
 | Hyperbee.JsonNode        |   9,511.05 ns |  14,918.98 ns |    817.759 ns |    18112 B
 | JsonCons.JsonElement     |   9,617.04 ns |   8,150.52 ns |    446.757 ns |    13032 B
 | Newtonsoft.JObject       |  13,136.51 ns |  10,565.73 ns |    579.143 ns |    26480 B
 | JsonEverything.JsonNode  |  24,738.29 ns |  26,520.10 ns |  1,453.656 ns |    49304 B
 |                          |               |               |               |           
 | `$..['bicycle','price']`
 | Hyperbee.JsonElement     |   2,304.00 ns |   4,789.71 ns |    262.540 ns |     3072 B
 | JsonCons.JsonElement     |   5,411.72 ns |   2,044.44 ns |    112.063 ns |     7304 B
 | Hyperbee.JsonNode        |   5,959.43 ns |   6,388.91 ns |    350.197 ns |     9056 B
 | Newtonsoft.JObject       |   8,732.70 ns |   7,717.27 ns |    423.010 ns |    14904 B
 | JsonEverything.JsonNode  |  16,081.80 ns |  32,632.20 ns |  1,788.681 ns |    29184 B
 |                          |               |               |               |           
 | `$..*`
 | Hyperbee.JsonElement     |   1,530.38 ns |     904.54 ns |     49.581 ns |     4432 B
 | JsonCons.JsonElement     |   5,529.29 ns |   5,095.25 ns |    279.288 ns |     8648 B
 | Hyperbee.JsonNode        |   6,375.70 ns |  13,107.14 ns |    718.447 ns |     9768 B
 | Newtonsoft.JObject       |   8,316.71 ns |  13,631.95 ns |    747.213 ns |    14528 B
 | JsonEverything.JsonNode  |  17,290.19 ns |  11,706.56 ns |    641.676 ns |    34784 B
 |                          |               |               |               |           
 | `$..author`
 | Hyperbee.JsonElement     |   1,624.67 ns |     304.86 ns |     16.711 ns |     3056 B
 | JsonCons.JsonElement     |   4,369.83 ns |   3,368.40 ns |    184.633 ns |     5640 B
 | Hyperbee.JsonNode        |   5,546.52 ns |   2,128.96 ns |    116.695 ns |     8848 B
 | Newtonsoft.JObject       |   8,346.02 ns |  12,029.11 ns |    659.356 ns |    14544 B
 | JsonEverything.JsonNode  |  13,113.01 ns |   6,545.11 ns |    358.760 ns |    26728 B
 |                          |               |               |               |           
 | `$..book[?@.isbn]`
 | Hyperbee.JsonElement     |   2,535.29 ns |   5,230.41 ns |    286.697 ns |     4024 B
 | JsonCons.JsonElement     |   5,479.99 ns |   5,162.81 ns |    282.991 ns |     7336 B
 | Hyperbee.JsonNode        |   6,270.33 ns |     952.73 ns |     52.222 ns |     9776 B
 | JsonEverything.JsonNode  |  15,563.95 ns |  10,045.42 ns |    550.624 ns |    30696 B
 | Newtonsoft.JObject       |            NA |            NA |            NA |         NA
 |                          |               |               |               |           
 | `$..book[?@.price == 8.99 && @.category == 'fiction']`
 | Hyperbee.JsonElement     |   3,454.45 ns |   2,551.12 ns |    139.835 ns |     6120 B
 | Hyperbee.JsonNode        |   7,541.74 ns |  10,405.88 ns |    570.381 ns |    12000 B
 | JsonCons.JsonElement     |   8,161.76 ns |   6,143.02 ns |    336.720 ns |     8640 B
 | JsonEverything.JsonNode  |  22,294.77 ns |  27,986.15 ns |  1,534.016 ns |    40472 B
 | Newtonsoft.JObject       |            NA |            NA |            NA |         NA
 |                          |               |               |               |           
 | `$..book[0,1]`
 | Hyperbee.JsonElement     |   1,907.37 ns |   4,405.90 ns |    241.502 ns |     3056 B
 | JsonCons.JsonElement     |   5,556.87 ns |  13,635.13 ns |    747.387 ns |     6248 B
 | Hyperbee.JsonNode        |   6,020.31 ns |   4,143.39 ns |    227.113 ns |     8848 B
 | Newtonsoft.JObject       |   8,536.98 ns |   6,526.24 ns |    357.725 ns |    14792 B
 | JsonEverything.JsonNode  |  13,979.45 ns |  19,281.48 ns |  1,056.883 ns |    27048 B
 |                          |               |               |               |           
 | `$.store..price`
 | Hyperbee.JsonElement     |   1,369.37 ns |   1,313.98 ns |     72.024 ns |     2680 B
 | JsonCons.JsonElement     |   4,589.72 ns |   4,689.36 ns |    257.040 ns |     5704 B
 | Hyperbee.JsonNode        |   5,263.10 ns |   4,128.49 ns |    226.296 ns |     8576 B
 | Newtonsoft.JObject       |   8,093.64 ns |   2,103.71 ns |    115.311 ns |    14680 B
 | JsonEverything.JsonNode  |  12,969.09 ns |  11,670.40 ns |    639.694 ns |    27272 B
 |                          |               |               |               |           
 | `$.store.* #First()`
 | Hyperbee.JsonElement     |     423.70 ns |     346.12 ns |     18.972 ns |      752 B
 | JsonCons.JsonElement     |   2,575.12 ns |     900.27 ns |     49.347 ns |     3384 B
 | Hyperbee.JsonNode        |   2,999.84 ns |   3,209.10 ns |    175.902 ns |     2944 B
 | JsonEverything.JsonNode  |   3,612.82 ns |   4,544.38 ns |    249.093 ns |     4648 B
 | Newtonsoft.JObject       |   9,036.39 ns |  25,870.81 ns |  1,418.066 ns |    14816 B
 |                          |               |               |               |           
 | `$.store.*`
 | Hyperbee.JsonElement     |     437.72 ns |     309.52 ns |     16.966 ns |      712 B
 | JsonCons.JsonElement     |   2,932.44 ns |   3,752.08 ns |    205.664 ns |     3344 B
 | Hyperbee.JsonNode        |   3,011.14 ns |   3,030.26 ns |    166.099 ns |     2968 B
 | JsonEverything.JsonNode  |   3,630.15 ns |   1,683.62 ns |     92.285 ns |     4912 B
 | Newtonsoft.JObject       |   7,320.97 ns |   4,230.57 ns |    231.892 ns |    14776 B
 |                          |               |               |               |           
 | `$.store.bicycle.color`
 | Hyperbee.JsonElement     |     167.21 ns |     121.79 ns |      6.676 ns |       80 B
 | JsonCons.JsonElement     |   2,815.20 ns |   1,361.83 ns |     74.646 ns |     3304 B
 | Hyperbee.JsonNode        |   2,820.77 ns |   2,055.87 ns |    112.689 ns |     2952 B
 | JsonEverything.JsonNode  |   4,157.10 ns |   4,348.01 ns |    238.329 ns |     5880 B
 | Newtonsoft.JObject       |   7,383.76 ns |  12,169.49 ns |    667.051 ns |    14840 B
 |                          |               |               |               |           
 | `$.store.book[-1:]`
 | Hyperbee.JsonElement     |     343.95 ns |      50.68 ns |      2.778 ns |      296 B
 | Hyperbee.JsonNode        |   2,970.09 ns |   1,506.62 ns |     82.583 ns |     3040 B
 | JsonCons.JsonElement     |   3,292.51 ns |   9,107.18 ns |    499.195 ns |     3600 B
 | JsonEverything.JsonNode  |   4,538.91 ns |  10,360.90 ns |    567.916 ns |     5856 B
 | Newtonsoft.JObject       |   7,135.67 ns |   1,812.73 ns |     99.362 ns |    14864 B
 |                          |               |               |               |           
 | `$.store.book[:2]`
 | Hyperbee.JsonElement     |     370.32 ns |     121.84 ns |      6.678 ns |      296 B
 | JsonCons.JsonElement     |   3,038.17 ns |   3,501.46 ns |    191.927 ns |     3624 B
 | Hyperbee.JsonNode        |   3,140.74 ns |   1,032.27 ns |     56.582 ns |     3040 B
 | JsonEverything.JsonNode  |   4,369.53 ns |   3,976.08 ns |    217.942 ns |     6168 B
 | Newtonsoft.JObject       |   7,437.74 ns |   1,115.70 ns |     61.155 ns |    14856 B
 |                          |               |               |               |           
 | `$.store.book[?(!@.isbn)]`
 | Hyperbee.JsonElement     |     958.88 ns |     496.08 ns |     27.192 ns |     1264 B
 | JsonCons.JsonElement     |   4,178.60 ns |   7,835.08 ns |    429.467 ns |     4992 B
 | Hyperbee.JsonNode        |   4,944.55 ns |   1,868.34 ns |    102.410 ns |     7232 B
 | JsonEverything.JsonNode  |   7,989.53 ns |   8,356.64 ns |    458.056 ns |    13288 B
 | Newtonsoft.JObject       |            NA |            NA |            NA |         NA
 |                          |               |               |               |           
 | `$.store.book[?(@.author && @.title)]`
 | Hyperbee.JsonElement     |   1,410.90 ns |     957.34 ns |     52.475 ns |     1920 B
 | JsonCons.JsonElement     |   4,748.35 ns |   7,493.38 ns |    410.738 ns |     5712 B
 | Hyperbee.JsonNode        |   5,390.33 ns |   4,810.77 ns |    263.694 ns |     8016 B
 | Newtonsoft.JObject       |   8,234.02 ns |   3,888.73 ns |    213.154 ns |    16568 B
 | JsonEverything.JsonNode  |   9,568.68 ns |   6,937.42 ns |    380.263 ns |    18760 B
 |                          |               |               |               |           
 | `$.store.book[?(@.category == 'fiction')]`
 | Hyperbee.JsonElement     |   1,465.47 ns |     869.91 ns |     47.682 ns |     2176 B
 | JsonCons.JsonElement     |   4,028.25 ns |     620.64 ns |     34.020 ns |     5128 B
 | Hyperbee.JsonNode        |   5,794.61 ns |   8,453.28 ns |    463.353 ns |     8240 B
 | Newtonsoft.JObject       |   7,887.90 ns |   6,908.96 ns |    378.704 ns |    16120 B
 | JsonEverything.JsonNode  |  10,979.35 ns |  23,237.47 ns |  1,273.724 ns |    16888 B
 |                          |               |               |               |           
 | `$.store.book[?(@.price < 10 || @.category == 'fiction')]`
 | Hyperbee.JsonElement     |   2,129.32 ns |     786.36 ns |     43.103 ns |     3328 B
 | JsonCons.JsonElement     |   6,005.82 ns |   5,040.32 ns |    276.277 ns |     6336 B
 | Hyperbee.JsonNode        |   6,864.07 ns |  18,400.68 ns |  1,008.603 ns |     9424 B
 | Newtonsoft.JObject       |   8,303.30 ns |   5,123.69 ns |    280.847 ns |    17080 B
 | JsonEverything.JsonNode  |  14,107.82 ns |  23,780.71 ns |  1,303.501 ns |    24032 B
 |                          |               |               |               |           
 | `$.store.book[?(@.price < 10)].title`
 | Hyperbee.JsonElement     |   1,535.42 ns |     802.38 ns |     43.981 ns |     2192 B
 | JsonCons.JsonElement     |   4,700.93 ns |   3,133.72 ns |    171.770 ns |     5304 B
 | Hyperbee.JsonNode        |   5,842.73 ns |   6,854.44 ns |    375.715 ns |     8128 B
 | Newtonsoft.JObject       |   7,890.74 ns |   2,902.39 ns |    159.090 ns |    16272 B
 | JsonEverything.JsonNode  |  13,046.54 ns |  43,355.56 ns |  2,376.465 ns |    17792 B
 |                          |               |               |               |           
 | `$.store.book[?(@.price == 8.99)]`
 | Hyperbee.JsonElement     |   1,299.44 ns |     507.79 ns |     27.834 ns |     1984 B
 | JsonCons.JsonElement     |   4,567.67 ns |     592.57 ns |     32.481 ns |     5176 B
 | Hyperbee.JsonNode        |   5,711.16 ns |   6,198.26 ns |    339.748 ns |     7920 B
 | Newtonsoft.JObject       |   8,145.15 ns |   4,697.48 ns |    257.485 ns |    16128 B
 | JsonEverything.JsonNode  |  11,231.53 ns |  31,597.52 ns |  1,731.967 ns |    15840 B
 |                          |               |               |               |           
 | `$.store.book[?(@.price > 10 && @.price < 20)]`
 | Hyperbee.JsonElement     |   2,045.34 ns |   1,340.62 ns |     73.484 ns |     3136 B
 | JsonCons.JsonElement     |   6,306.07 ns |   3,978.97 ns |    218.101 ns |     6384 B
 | Hyperbee.JsonNode        |   6,413.33 ns |   2,512.74 ns |    137.732 ns |     9104 B
 | Newtonsoft.JObject       |   8,905.79 ns |  15,385.03 ns |    843.305 ns |    17088 B
 | JsonEverything.JsonNode  |  13,237.72 ns |  19,293.59 ns |  1,057.547 ns |    22800 B
 |                          |               |               |               |           
 | `$.store.book[?(length(@.title) > 10)]`
 | Hyperbee.JsonElement     |   1,260.51 ns |   1,058.61 ns |     58.026 ns |     1960 B
 | Hyperbee.JsonNode        |   6,070.88 ns |   6,545.07 ns |    358.757 ns |     8024 B
 | JsonCons.JsonElement     |   6,540.23 ns |   4,686.75 ns |    256.897 ns |     8720 B
 | JsonEverything.JsonNode  |  13,521.50 ns |   5,069.92 ns |    277.899 ns |    19984 B
 | Newtonsoft.JObject       |            NA |            NA |            NA |         NA
 |                          |               |               |               |           
 | `$.store.book['category','author']`
 | Hyperbee.JsonElement     |   1,058.06 ns |     153.34 ns |      8.405 ns |      504 B
 | JsonCons.JsonElement     |   3,061.84 ns |     557.06 ns |     30.535 ns |     3696 B
 | JsonEverything.JsonNode  |   3,790.12 ns |     294.78 ns |     16.158 ns |     5536 B
 | Hyperbee.JsonNode        |   4,973.44 ns |   2,966.40 ns |    162.598 ns |     6576 B
 | Newtonsoft.JObject       |   7,556.08 ns |   7,502.20 ns |    411.221 ns |    15208 B
 |                          |               |               |               |           
 | `$.store.book[*].author`
 | Hyperbee.JsonElement     |     951.62 ns |     871.96 ns |     47.795 ns |      960 B
 | JsonCons.JsonElement     |   3,105.79 ns |     581.63 ns |     31.881 ns |     3632 B
 | Hyperbee.JsonNode        |   5,296.73 ns |   4,366.35 ns |    239.334 ns |     6992 B
 | JsonEverything.JsonNode  |   7,448.66 ns |   9,304.40 ns |    510.006 ns |    12752 B
 | Newtonsoft.JObject       |   8,009.46 ns |  12,538.51 ns |    687.278 ns |    14992 B
 |                          |               |               |               |           
 | `$.store.book[*]`
 | Hyperbee.JsonElement     |     517.63 ns |      84.36 ns |      4.624 ns |      544 B
 | JsonCons.JsonElement     |   2,862.62 ns |     943.54 ns |     51.719 ns |     3432 B
 | Hyperbee.JsonNode        |   3,313.42 ns |   6,493.13 ns |    355.911 ns |     3248 B
 | JsonEverything.JsonNode  |   4,318.65 ns |   1,033.19 ns |     56.633 ns |     6768 B
 | Newtonsoft.JObject       |   7,629.11 ns |  10,180.63 ns |    558.035 ns |    14840 B
 |                          |               |               |               |           
 | `$.store.book[0,1]`
 | Hyperbee.JsonElement     |     422.06 ns |   1,219.38 ns |     66.838 ns |      296 B
 | Hyperbee.JsonNode        |   2,946.07 ns |   2,157.91 ns |    118.282 ns |     3040 B
 | JsonCons.JsonElement     |   3,125.25 ns |   1,412.06 ns |     77.400 ns |     3816 B
 | JsonEverything.JsonNode  |   4,583.33 ns |   8,253.72 ns |    452.414 ns |     6216 B
 | Newtonsoft.JObject       |   7,398.30 ns |   2,928.94 ns |    160.545 ns |    14944 B
 |                          |               |               |               |           
 | `$.store.book[0:3:2]`
 | Hyperbee.JsonElement     |     425.82 ns |     533.81 ns |     29.260 ns |      296 B
 | Hyperbee.JsonNode        |   3,170.87 ns |   4,319.07 ns |    236.743 ns |     3040 B
 | JsonCons.JsonElement     |   3,250.95 ns |   3,702.78 ns |    202.962 ns |     3672 B
 | JsonEverything.JsonNode  |   4,544.01 ns |   1,791.86 ns |     98.218 ns |     6168 B
 | Newtonsoft.JObject       |   7,223.98 ns |     448.85 ns |     24.603 ns |    14904 B
 |                          |               |               |               |           
 | `$.store.book[0].title`
 | Hyperbee.JsonElement     |     213.07 ns |     222.27 ns |     12.183 ns |       80 B
 | JsonCons.JsonElement     |   2,870.69 ns |     619.44 ns |     33.954 ns |     3384 B
 | Hyperbee.JsonNode        |   3,157.28 ns |   2,152.20 ns |    117.970 ns |     3720 B
 | JsonEverything.JsonNode  |   5,104.60 ns |   2,695.82 ns |    147.767 ns |     7552 B
 | Newtonsoft.JObject       |   7,862.10 ns |   9,601.61 ns |    526.297 ns |    14968 B
 |                          |               |               |               |           
 | `$.store.book[0]`
 | Hyperbee.JsonElement     |     171.25 ns |     332.05 ns |     18.201 ns |       80 B
 | JsonCons.JsonElement     |   2,867.70 ns |   3,098.16 ns |    169.821 ns |     3288 B
 | Hyperbee.JsonNode        |   3,051.39 ns |   4,436.76 ns |    243.194 ns |     2928 B
 | JsonEverything.JsonNode  |   3,961.99 ns |   1,245.96 ns |     68.295 ns |     5816 B
 | Newtonsoft.JObject       |   7,577.69 ns |   8,275.01 ns |    453.581 ns |    14824 B
 |                          |               |               |               |           
 | `$`
 | Hyperbee.JsonElement     |      29.76 ns |      10.00 ns |      0.548 ns |       56 B
 | JsonEverything.JsonNode  |   2,361.50 ns |   2,021.81 ns |    110.822 ns |     1928 B
 | Hyperbee.JsonNode        |   2,428.05 ns |   1,414.84 ns |     77.552 ns |     1792 B
 | JsonCons.JsonElement     |   2,747.57 ns |   4,158.88 ns |    227.962 ns |     3008 B
 | Newtonsoft.JObject       |   7,584.68 ns |  12,872.74 ns |    705.598 ns |    14312 B

Benchmarks with issues:
  JsonPathBenchmark.Newtonsoft.JObject: .NET 10(Runtime=.NET 10.0, IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$..book[?@.isbn]]
  JsonPathBenchmark.Newtonsoft.JObject: .NET 10(Runtime=.NET 10.0, IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$..book[?@.price == 8.99 && @.category == 'fiction']]
  JsonPathBenchmark.Newtonsoft.JObject: .NET 10(Runtime=.NET 10.0, IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$.store.book[?(!@.isbn)]]
  JsonPathBenchmark.Newtonsoft.JObject: .NET 10(Runtime=.NET 10.0, IterationCount=3, LaunchCount=1, WarmupCount=3) [Filter=$.store.book[?(length(@.title) > 10)]]
```
