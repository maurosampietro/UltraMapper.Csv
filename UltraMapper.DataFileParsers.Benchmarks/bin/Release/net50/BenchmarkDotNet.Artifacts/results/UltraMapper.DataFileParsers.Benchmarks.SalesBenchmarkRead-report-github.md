``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.17763.2928 (1809/October2018Update/Redstone5)
Intel Core i7-8565U CPU 1.80GHz (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.300
  [Host]     : .NET 5.0.17 (5.0.1722.21314), X64 RyuJIT
  DefaultJob : .NET 5.0.17 (5.0.1722.21314), X64 RyuJIT


```
|      Method |     Mean |    Error |   StdDev |
|------------ |---------:|---------:|---------:|
|    BaseLine |  4.961 s | 0.1401 s | 0.4043 s |
|  FileHelper | 10.555 s | 0.3188 s | 0.9301 s |
|   CsvHelper | 13.676 s | 0.3145 s | 0.9023 s |
| UltraMapper |  5.826 s | 0.1123 s | 0.1460 s |
