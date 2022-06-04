``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1586 (21H2)
Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.300
  [Host]     : .NET 5.0.17 (5.0.1722.21314), X64 RyuJIT
  DefaultJob : .NET 5.0.17 (5.0.1722.21314), X64 RyuJIT


```
|      Method |    Mean |    Error |   StdDev |
|------------ |--------:|---------:|---------:|
|    BaseLine | 1.610 s | 0.0316 s | 0.0411 s |
|  FileHelper | 3.019 s | 0.0350 s | 0.0293 s |
|   CsvHelper | 3.826 s | 0.0743 s | 0.1017 s |
| UltraMapper | 1.684 s | 0.0148 s | 0.0124 s |
