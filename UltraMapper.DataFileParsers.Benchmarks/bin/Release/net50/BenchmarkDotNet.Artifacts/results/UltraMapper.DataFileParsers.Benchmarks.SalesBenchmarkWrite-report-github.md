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
|  FileHelper | 2.421 s | 0.0196 s | 0.0183 s |
|   CsvHelper | 4.429 s | 0.0876 s | 0.1043 s |
| UltraMapper | 1.431 s | 0.0384 s | 0.1121 s |
