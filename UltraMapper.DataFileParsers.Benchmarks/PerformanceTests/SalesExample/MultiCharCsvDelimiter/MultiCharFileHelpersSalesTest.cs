using FileHelpers;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.SalesExample.MultiCharCsvDelimiter
{
    public class MultiCharFileHelpersSalesTest : ICsvBenchmark<SaleRecordMCD>
    {
        public IEnumerable<SaleRecordMCD> ReadRecords( string fileLocation )
        {
            var engine = new FileHelperAsyncEngine<SaleRecordMCD>();
            engine.BeginReadFile( fileLocation );
            return engine;
        }

        public void WriteRecords( IEnumerable<SaleRecordMCD> records )
        {
            string fileLocation = Path.Combine(
                 Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ),
                "Resources", $"1m Sales Records.{nameof( MultiCharFileHelpersSalesTest )}.csv" );

            var engine = new DelimitedFileEngine<SaleRecordMCD>();
            engine.WriteFile( fileLocation, records );
        }
    }
}
