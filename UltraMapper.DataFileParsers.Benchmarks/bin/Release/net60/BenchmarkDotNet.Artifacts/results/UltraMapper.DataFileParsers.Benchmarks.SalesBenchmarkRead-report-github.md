```

BenchmarkDotNet v0.13.8, Windows 10 (10.0.19045.3570/22H2/2022Update)
Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK 8.0.100-rc.2.23502.2
  [Host]   : .NET 6.0.23 (6.0.2323.48002), X64 RyuJIT AVX2
  .NET 7.0 : .NET 7.0.12 (7.0.1223.47720), X64 RyuJIT AVX2

Job=.NET 7.0  Runtime=.NET 7.0  

```
| Method      | Mean    | Error    | StdDev   |
|------------ |--------:|---------:|---------:|
| BaseLine    | 2.764 s | 0.0516 s | 0.0483 s |
| FileHelper  | 4.705 s | 0.0136 s | 0.0113 s |
| CsvHelper   | 6.634 s | 0.1231 s | 0.1952 s |
| UltraMapper | 3.110 s | 0.0198 s | 0.0166 s |
