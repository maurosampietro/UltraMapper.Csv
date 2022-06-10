``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.17763.2928 (1809/October2018Update/Redstone5)
Intel Core i7-8565U CPU 1.80GHz (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.300
  [Host]   : .NET 5.0.17 (5.0.1722.21314), X64 RyuJIT
  .NET 5.0 : .NET 5.0.17 (5.0.1722.21314), X64 RyuJIT

Job=.NET 5.0  Runtime=.NET 5.0  

```
|      Method | Mean | Error |
|------------ |-----:|------:|
|    BaseLine |   NA |    NA |
|   CsvHelper |   NA |    NA |
| UltraMapper |   NA |    NA |

Benchmarks with issues:
  SalesBenchmarkReadMultichar.BaseLine: .NET 5.0(Runtime=.NET 5.0)
  SalesBenchmarkReadMultichar.CsvHelper: .NET 5.0(Runtime=.NET 5.0)
  SalesBenchmarkReadMultichar.UltraMapper: .NET 5.0(Runtime=.NET 5.0)
