using FileHelpers;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.GpsDataExample.SingleCharCsvDelimiter
{
    public class FileHelpersGpsDataTest : ICsvBenchmark<GpsDataRecord>
    {
        public IEnumerable<GpsDataRecord> ReadRecords( string fileLocation )
        {
            var engine = new FileHelperEngine<GpsDataRecord>();
            return engine.ReadFile( fileLocation );
        }

        public void WriteRecords( IEnumerable<GpsDataRecord> records )
        {
            string fileLocation = Path.Combine(
                 Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ),
                "Resources", $"dataset.{nameof( FileHelpersGpsDataTest )}.csv" );

            var engine = new FileHelperEngine<GpsDataRecord>();
            engine.WriteFile( fileLocation, records );
        }
    }
}
