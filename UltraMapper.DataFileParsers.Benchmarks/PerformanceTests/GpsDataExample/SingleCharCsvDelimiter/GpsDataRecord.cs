using FileHelpers;

namespace UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.GpsDataExample.SingleCharCsvDelimiter
{
    [DelimitedRecord( "," ), IgnoreFirst( 1 )]
    public class GpsDataRecord
    {
        public string anzsic06 { get; set; }
        public string Area { get; set; }
        public int year { get; set; }
        public int geo_count { get; set; }
        public int ec_count { get; set; }
    }
}
