using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.SalesExample.MultiCharCsvDelimiter
{
    public class MultiCharCsvHelperSalesTest : ICsvBenchmark<SaleRecordMCD>
    {
        public IEnumerable<SaleRecordMCD> ReadRecords( string fileLocation )
        {
            var reader = new StreamReader( fileLocation );

            var config = new CsvConfiguration( CultureInfo.InvariantCulture )
            {
                Delimiter = "~DELIMITER~",
                HasHeaderRecord = true,
                LeaveOpen = true,
                HeaderValidated = null,
                MissingFieldFound = null
            };

            var csvReader = new CsvReader( reader, config );

            return csvReader.GetRecords<SaleRecordMCD>();
        }

        public void WriteRecords( IEnumerable<SaleRecordMCD> records )
        {
            string fileLocation = Path.Combine(
                 Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ),
                "Resources", $"1m Sales Records.output.{nameof( MultiCharCsvHelperSalesTest )}.csv" );

            using( var writer = new StreamWriter( fileLocation ) )
            {
                var engine = new CsvWriter( writer, CultureInfo.InvariantCulture );
                engine.WriteRecords( records );
            }
        }
    }
}
