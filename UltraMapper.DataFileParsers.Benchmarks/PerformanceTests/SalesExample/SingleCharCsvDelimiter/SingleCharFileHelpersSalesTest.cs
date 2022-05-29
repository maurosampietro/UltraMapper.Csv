using FileHelpers;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.SalesExample.SingleCharCsvDelimiter
{
    public class SingleCharFileHelpersSalesTest : ICsvBenchmark<SaleRecord>
    {
        public string Name => "FileHelpers";
        public int ExecutionOrder => 3;

        public IEnumerable<SaleRecord> ReadRecords( string fileLocation )
        {
            var engine = new FileHelperAsyncEngine<SaleRecord>();
            engine.BeginReadFile( fileLocation );
            return engine;
        }

        public void WriteRecords( IEnumerable<SaleRecord> records )
        {
            string fileLocation = Path.Combine(
                 Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ),
                "Resources", $"1m Sales Records.{Name}.csv" );

            var engine = new DelimitedFileEngine<SaleRecord>();
            engine.WriteFile( fileLocation, records );
        }
    }
}
