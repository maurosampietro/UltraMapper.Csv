using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UltraMapper.DataFileParsers.Benchmarks;

namespace UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.SalesExample.MultiCharCsvDelimiter
{
    public class MultiCharCsvHelperSalesTest : BaseTest, ICsvBenchmark<SaleRecordMCD>
    {
        protected override string FileName => "5m Sales Records.mcd.csv";

        public IEnumerable<SaleRecordMCD> ReadRecords( string fileLocation )
        {
            var reader = new StreamReader( fileLocation );
            var csvReader = new CsvReader( reader, CultureInfo.InvariantCulture, leaveOpen: true );

            return csvReader.GetRecords<SaleRecordMCD>();
        }

        public void WriteRecords( IEnumerable<SaleRecordMCD> records )
        {
            //string fileLocation = $"{this.FileLocation}.{Name}";

            //var engine = new DelimitedFileEngine<SaleRecordMCD>();
            //engine.WriteFile( fileLocation, records );
        }
    }
}
