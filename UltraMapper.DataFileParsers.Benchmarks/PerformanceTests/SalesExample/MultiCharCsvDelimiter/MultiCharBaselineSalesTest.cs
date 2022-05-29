using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UltraMapper.DataFileParsers.Benchmarks;

namespace UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.SalesExample.MultiCharCsvDelimiter
{
    public class MultiCharBaselineSalesTest : ICsvBenchmark<SaleRecordMCD>
    {
        public IEnumerable<SaleRecordMCD> ReadRecords( string fileLocation )
        {
            //this is our performance reference since we cannot be faster than this

            using( var reader = new StreamReader( fileLocation ) )
            {
                var line = reader.ReadLine();
                while( !reader.EndOfStream )
                {
                    line = reader.ReadLine();
                    var splitValues = line.Split( new[] { "~DELIMITER~" }, StringSplitOptions.None );

                    yield return new SaleRecordMCD()
                    {
                        Region = splitValues[ 0 ],
                        Country = splitValues[ 1 ],
                        ItemType = splitValues[ 2 ],
                        SalesChannel = splitValues[ 3 ],
                        OrderPriority = splitValues[ 4 ],
                        OrderDate = splitValues[ 5 ],
                        OrderID = splitValues[ 6 ],
                        ShipDate = splitValues[ 7 ],
                        UnitsSold = splitValues[ 8 ],
                        UnitPrice = splitValues[ 9 ],
                        UnitCost = splitValues[ 10 ],
                        TotalRevenue = splitValues[ 11 ],
                        TotalCost = splitValues[ 12 ],
                        TotalProfit = splitValues[ 13 ],
                    };
                }
            }
        }

        public void WriteRecords( IEnumerable<SaleRecordMCD> records )
        {
            string fileLocation = Path.Combine(
              Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ),
             "Resources", $"dataset.{nameof(MultiCharBaselineSalesTest)}.csv" );

            //using( var writer = new StreamWriter( fileLocation ) )
            //{
            //    var sb = new StringBuilder();

            //    foreach( var item in records )
            //    {
            //        sb.Append( item.anzsic06 ).Append( ',' );
            //        sb.Append( item.Area ).Append( ',' );
            //        sb.Append( item.year ).Append( ',' );
            //        sb.Append( item.geo_count ).Append( ',' );
            //        sb.Append( item.ec_count );

            //        writer.WriteLine( sb.ToString() );
            //        sb.Clear();
            //    }
            //}
        }
    }
}
