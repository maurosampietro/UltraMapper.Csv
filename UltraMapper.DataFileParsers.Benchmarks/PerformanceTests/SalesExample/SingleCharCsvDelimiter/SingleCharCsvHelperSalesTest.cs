using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UltraMapper.DataFileParsers.Benchmarks;

namespace UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.SalesExample.SingleCharCsvDelimiter
{

    public class SingleCharCsvHelperSalesTest : BaseTest, ICsvBenchmark<SaleRecord>
    {
        public string Name => "CsvHelper";
        public int ExecutionOrder => 4;

        protected override string FileName => "5m Sales Records.csv";

        public IEnumerable<SaleRecord> ReadRecords( string fileLocation )
        {
            var reader = new StreamReader( fileLocation );
            var csvReader = new CsvReader( reader, CultureInfo.InvariantCulture, leaveOpen: true );

            return csvReader.GetRecords<SaleRecord>();
        }

        public void WriteRecords( IEnumerable<SaleRecord> records )
        {
            //string fileLocation = $"{this.FileLocation}.{Name}";

            //var engine = new DelimitedFileEngine<SaleRecord>();
            //engine.WriteFile( fileLocation, records );
        }
    }
}
