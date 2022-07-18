using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.SalesExample.SingleCharCsvDelimiter
{
    public class SingleCharBaselineSalesTest : ICsvBenchmark<SaleRecord>
    {
        public string Name => "Baseline (ad-hoc code)";
        public int ExecutionOrder => 1;

        public IEnumerable<SaleRecord> ReadRecords( string fileLocation )
        {
            //this is our performance reference since we cannot be faster than this
            using( var reader = new StreamReader( fileLocation ) )
            {
                var line = reader.ReadLine();
                while( !reader.EndOfStream )
                {
                    line = reader.ReadLine();
                    var splitValues = line.Split( ',' );

                    yield return new SaleRecord()
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

        public void WriteRecords( IEnumerable<SaleRecord> records )
        {
            string dir = Path.Combine( Path.GetTempPath(), "UltraMapper.CSV.Benchmarks" );
            Directory.CreateDirectory( dir );

            string fileLocation = Path.Combine( dir,
                $"1m Sales Records.output.{nameof( SingleCharBaselineSalesTest )}.csv" );

            using( var writer = new StreamWriter( fileLocation ) )
            {
                var sb = new StringBuilder();

                foreach( var item in records )
                {
                    sb.Append( item.Region ).Append( ',' );
                    sb.Append( item.Country ).Append( ',' );
                    sb.Append( item.ItemType ).Append( ',' );
                    sb.Append( item.SalesChannel ).Append( ',' );
                    sb.Append( item.OrderPriority ).Append( ',' );
                    sb.Append( item.OrderDate ).Append( ',' );
                    sb.Append( item.OrderID ).Append( ',' );
                    sb.Append( item.ShipDate ).Append( ',' );
                    sb.Append( item.UnitsSold ).Append( ',' );
                    sb.Append( item.UnitPrice ).Append( ',' );
                    sb.Append( item.UnitCost ).Append( ',' );
                    sb.Append( item.TotalRevenue ).Append( ',' );
                    sb.Append( item.TotalCost ).Append( ',' );
                    sb.Append( item.TotalProfit );

                    writer.WriteLine( sb.ToString() );
                    sb.Clear();
                }
            }
        }
    }
}
