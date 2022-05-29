``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1586 (21H2)
Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.300
  [Host]   : .NET 5.0.17 (5.0.1722.21314), X64 RyuJIT
  .NET 6.0 : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT


```
|      Method |                Job |            Runtime |     Mean |    Error |   StdDev |
|------------ |------------------- |------------------- |---------:|---------:|---------:|
|  FileHelper |           .NET 6.0 |           .NET 6.0 | 53.07 μs | 0.500 μs | 0.468 μs |
|   CsvHelper |           .NET 6.0 |           .NET 6.0 | 29.40 μs | 0.330 μs | 0.276 μs |
| UltraMapper |           .NET 6.0 |           .NET 6.0 | 29.36 μs | 0.283 μs | 0.265 μs |
|  FileHelper | .NET Framework 4.7 | .NET Framework 4.7 |       NA |       NA |       NA |
|   CsvHelper | .NET Framework 4.7 | .NET Framework 4.7 |       NA |       NA |       NA |
| UltraMapper | .NET Framework 4.7 | .NET Framework 4.7 |       NA |       NA |       NA |

Benchmarks with issues:
  SalesBenchmark.FileHelper: .NET Framework 4.7(Runtime=.NET Framework 4.7)
  SalesBenchmark.CsvHelper: .NET Framework 4.7(Runtime=.NET Framework 4.7)
  SalesBenchmark.UltraMapper: .NET Framework 4.7(Runtime=.NET Framework 4.7)
