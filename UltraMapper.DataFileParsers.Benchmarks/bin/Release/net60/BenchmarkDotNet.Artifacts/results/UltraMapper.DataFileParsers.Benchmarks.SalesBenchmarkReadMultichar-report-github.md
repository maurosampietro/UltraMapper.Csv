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
| BaseLine    | 6.039 s | 0.1129 s | 0.1546 s |
| CsvHelper   | 6.328 s | 0.0323 s | 0.0270 s |
| UltraMapper | 6.070 s | 0.0810 s | 0.0758 s |
