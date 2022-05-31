``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1586 (21H2)
Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.300
  [Host]     : .NET 5.0.17 (5.0.1722.21314), X64 RyuJIT
  DefaultJob : .NET 5.0.17 (5.0.1722.21314), X64 RyuJIT


```
|      Method |    Mean |    Error |   StdDev |
|------------ |--------:|---------:|---------:|
|  FileHelper | 3.093 s | 0.0582 s | 0.0544 s |
|   CsvHelper | 3.836 s | 0.0693 s | 0.0615 s |
| UltraMapper | 2.998 s | 0.0586 s | 0.0548 s |
