using FileHelpers;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.SalesExample.SingleCharCsvDelimiter
{
    public class SingleCharFileHelpersSalesTest : ICsvBenchmark<SaleRecord>
    {
        public IEnumerable<SaleRecord> ReadRecords( string fileLocation )
        {
            var engine = new FileHelperAsyncEngine<SaleRecord>();
            engine.BeginReadFile( fileLocation );
            return engine;
        }

        public void WriteRecords( IEnumerable<SaleRecord> records )
        {
            string dir = Path.Combine( Path.GetTempPath(), "UltraMapper.CSV.Benchmarks" );
            Directory.CreateDirectory( dir );

            string fileLocation = Path.Combine( dir,
                $"1m Sales Records.output.{nameof( SingleCharFileHelpersSalesTest )}.csv" );

            var engine = new DelimitedFileEngine<SaleRecord>();
            engine.WriteFile( fileLocation, records );
        }
    }
}
