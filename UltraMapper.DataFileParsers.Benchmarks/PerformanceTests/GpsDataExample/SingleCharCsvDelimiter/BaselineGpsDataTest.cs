using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UltraMapper.DataFileParsers.Benchmarks;

namespace UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.GpsDataExample.SingleCharCsvDelimiter
{
    public class BaselineGpsDataTest : ICsvBenchmark<GpsDataRecord>
    {
        public IEnumerable<GpsDataRecord> ReadRecords( string fileLocation )
        {
            //this is our performance reference since we cannot be faster than this

            using( var reader = new StreamReader( fileLocation ) )
            {
                var line = reader.ReadLine();
                while( !reader.EndOfStream )
                {
                    line = reader.ReadLine();
                    var splitValues = line.Split( ',' );

                    yield return new GpsDataRecord()
                    {
                        anzsic06 = splitValues[ 0 ],
                        Area = splitValues[ 1 ],
                        year = Convert.ToInt32( splitValues[ 2 ] ),
                        geo_count = Convert.ToInt32( splitValues[ 3 ] ),
                        ec_count = Convert.ToInt32( splitValues[ 4 ] )
                    };
                }
            }
        }

        public void WriteRecords( IEnumerable<GpsDataRecord> records )
        {
            string fileLocation = Path.Combine(
              Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ),
             "Resources", $"dataset.{nameof(BaselineGpsDataTest)}.csv" );

            using( var writer = new StreamWriter( fileLocation ) )
            {
                var sb = new StringBuilder();

                foreach( var item in records )
                {
                    sb.Append( item.anzsic06 ).Append( ',' );
                    sb.Append( item.Area ).Append( ',' );
                    sb.Append( item.year ).Append( ',' );
                    sb.Append( item.geo_count ).Append( ',' );
                    sb.Append( item.ec_count );

                    writer.WriteLine( sb.ToString() );
                    sb.Clear();
                }
            }
        }
    }
}
