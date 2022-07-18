using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System;

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
