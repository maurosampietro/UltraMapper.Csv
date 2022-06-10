using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using UltraMapper.Csv;
using UltraMapper.Csv.FileFormats.Delimited;
using UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.SalesExample;
using UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.SalesExample.MultiCharCsvDelimiter;
using UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.SalesExample.SingleCharCsvDelimiter;

namespace UltraMapper.DataFileParsers.Benchmarks
{    
    public class Program
    {
        public static void Main( string[] args )
        {
            var summary = BenchmarkRunner.Run( typeof( Program ).Assembly, new DebugInProcessConfig() );

            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
        }
    }
}
