using System.Collections.Generic;

namespace UltraMapper.DataFileParsers.Benchmarks
{
    public interface ICsvBenchmark<T>
    {
        IEnumerable<T> ReadRecords( string fileLocation );
        void WriteRecords( IEnumerable<T> records );
    }
}
