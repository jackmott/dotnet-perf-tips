|                  Method |        Mean |      Error |     StdDev  | Allocated Memory/Op |
|------------------------ |------------:|-----------:|-----------:|--------------------:|
|          LinkedListLinq | 3,264.85 us | 144.130 us | 37.4302 us  |                48 B |
|  TotalLinkedListForEach | 1,887.33 us |  33.430 us |  8.6817 us  |                   - |
|                **ListLinq** | 2,668.39 us |  29.315 us |  7.6130 us  |                40 B |
|  IEnumerableListForEach | 2,081.54 us |  20.511 us |  5.3267 us  |                40 B |
|             ListForEach | 1,345.98 us |  14.756 us |  3.8322 us  |                   - |
|             ListForLoop | 1,011.81 us |  46.382 us | 12.0453 us  |                   - |
|               ArrayLinq | 1,875.12 us |  50.919 us | 13.2235 us  |                32 B |
|            ArrayForeach |   812.65 us |   9.770 us |  2.5373 us  |                   - |
|            ArrayForLoop |   860.92 us |  14.529 us |  3.7733 us  |                   - |
|       ArrayForLoopLocal |   825.88 us |  14.490 us |  3.7631 us  |                   - |
| ArrayForLoopLocalStruct |   246.26 us |   1.196 us |  0.3106 us  |                   - |
|               **ArraySIMD** |    38.64 us |   2.175 us |  0.5650 us  |                   - |
|  enumToStringReflection | 18,569.52 us | 145.193 us |  37.7061 us |        8389356 B |
|      enumToStringSwitch |     84.46 us |  13.887 us |   3.6063 us |        2097220 B |

