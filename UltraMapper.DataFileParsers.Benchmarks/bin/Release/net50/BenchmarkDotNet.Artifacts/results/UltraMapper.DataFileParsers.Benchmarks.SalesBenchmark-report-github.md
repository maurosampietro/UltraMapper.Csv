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
|  FileHelper | 3.183 s | 0.0613 s | 0.0681 s |
|   CsvHelper | 4.124 s | 0.0800 s | 0.1197 s |
| UltraMapper | 3.132 s | 0.0597 s | 0.0639 s |
