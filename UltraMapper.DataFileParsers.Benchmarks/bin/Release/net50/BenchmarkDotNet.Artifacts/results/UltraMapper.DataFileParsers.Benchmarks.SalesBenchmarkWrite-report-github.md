``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1586 (21H2)
Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.300
  [Host]   : .NET 5.0.17 (5.0.1722.21314), X64 RyuJIT
  .NET 5.0 : .NET 5.0.17 (5.0.1722.21314), X64 RyuJIT

Job=.NET 5.0  Runtime=.NET 5.0  

```
|      Method |    Mean |    Error |   StdDev |  Median |
|------------ |--------:|---------:|---------:|--------:|
|  FileHelper | 2.203 s | 0.0419 s | 0.0498 s | 2.180 s |
|   CsvHelper | 4.162 s | 0.0811 s | 0.1355 s | 4.233 s |
| UltraMapper | 1.644 s | 0.0329 s | 0.0937 s | 1.621 s |
