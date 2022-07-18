using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.SalesExample.SingleCharCsvDelimiter
{
    public class SingleCharCsvHelperSalesTest : ICsvBenchmark<SaleRecord>
    {
        public IEnumerable<SaleRecord> ReadRecords( string fileLocation )
        {
            var reader = new StreamReader( fileLocation );
            var csvReader = new CsvReader( reader, CultureInfo.InvariantCulture, leaveOpen: true );

            return csvReader.GetRecords<SaleRecord>();
        }

        public void WriteRecords( IEnumerable<SaleRecord> records )
        {
            string dir = Path.Combine( Path.GetTempPath(), "UltraMapper.CSV.Benchmarks" );
            Directory.CreateDirectory( dir );

            string fileLocation = Path.Combine( dir,
                $"1m Sales Records.output.{nameof( SingleCharCsvHelperSalesTest )}.csv" );

            using( var writer = new StreamWriter( fileLocation ) )
            {
                var engine = new CsvWriter( writer, CultureInfo.InvariantCulture );
                engine.WriteRecords( records );
            }
        }
    }
}
