``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1586 (21H2)
Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.300
  [Host]   : .NET 5.0.17 (5.0.1722.21314), X64 RyuJIT
  .NET 5.0 : .NET 5.0.17 (5.0.1722.21314), X64 RyuJIT

Job=.NET 5.0  Runtime=.NET 5.0  

```
|      Method |    Mean |    Error |   StdDev |
|------------ |--------:|---------:|---------:|
|    BaseLine | 2.573 s | 0.0233 s | 0.0206 s |
|   CsvHelper | 4.869 s | 0.0573 s | 0.0536 s |
| UltraMapper | 2.705 s | 0.0471 s | 0.0441 s |
