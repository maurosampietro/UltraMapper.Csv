```

BenchmarkDotNet v0.13.8, Windows 10 (10.0.19045.3448/22H2/2022Update)
Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK 7.0.302
  [Host]     : .NET 6.0.16 (6.0.1623.17311), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.16 (6.0.1623.17311), X64 RyuJIT AVX2


```
| Method      | Mean    | Error    | StdDev   |
|------------ |--------:|---------:|---------:|
| FileHelper  | 3.964 s | 0.0358 s | 0.0299 s |
| CsvHelper   | 7.386 s | 0.1107 s | 0.1036 s |
| UltraMapper | 2.253 s | 0.0430 s | 0.0478 s |
