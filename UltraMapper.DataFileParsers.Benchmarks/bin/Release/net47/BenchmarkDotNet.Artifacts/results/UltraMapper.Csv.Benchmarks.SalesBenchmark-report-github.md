``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19044
Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
  [Host]   : .NET Framework 4.8 (4.8.4470.0), X64 RyuJIT
  .NET 4.7 : .NET Framework 4.8 (4.8.4470.0), X64 RyuJIT

Job=.NET 4.7  Runtime=.NET 4.7  

```
|      Method |     Mean |    Error |   StdDev |
|------------ |---------:|---------:|---------:|
|  FileHelper | 89.11 μs | 0.390 μs | 0.346 μs |
|   CsvHelper | 34.69 μs | 0.116 μs | 0.102 μs |
| UltraMapper | 34.84 μs | 0.264 μs | 0.234 μs |
