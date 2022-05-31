using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using UltraMapper.DataFileParsers.Benchmarks;

namespace UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.SalesExample.MultiCharCsvDelimiter
{
    public class MultiCharCsvHelperSalesTest : ICsvBenchmark<SaleRecordMCD>
    {
        public IEnumerable<SaleRecordMCD> ReadRecords( string fileLocation )
        {
            var reader = new StreamReader( fileLocation );
            var csvReader = new CsvReader( reader, CultureInfo.InvariantCulture, leaveOpen: true );

            return csvReader.GetRecords<SaleRecordMCD>();
        }

        public void WriteRecords( IEnumerable<SaleRecordMCD> records )
        {
            string fileLocation = Path.Combine(
                 Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ),
                "Resources", $"1m Sales Records.output.{nameof( MultiCharCsvHelperSalesTest )}.csv" );

            using( var writer = new StreamWriter( fileLocation ) )
            {
                var engine = new CsvWriter(writer, CultureInfo.InvariantCulture);
                engine.WriteRecords( records );
            }
        }
    }
}
