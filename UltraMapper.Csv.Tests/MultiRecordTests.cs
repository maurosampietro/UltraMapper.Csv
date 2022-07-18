using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using UltraMapper.Csv.Benchmarks.SalesExample;


namespace UltraMapper.Csv.Tests
{
    [TestClass]
    public class MultiRecordTests
    {
        [TestMethod]
        public void MultiRecord()
        {
            var csvInput = @"
BioStatsAlex,      M,   41,       74,      170
Sales,Sub-Saharan Africa,South Africa,Fruits,Offline,M,7/27/2012,443368995,7/28/2012,1593,9.33,6.92,14862.69,11023.56,3839.13
BioStatsBert,      M,   42,       68,      166
Sales,Middle East and North Africa,Morocco,Clothes,Online,M,9/14/2013,667593514,10/19/2013,4611,109.28,35.84,503890.08,165258.24,338631.84
";

            Type recordTypeSelector( string line )
            {
                if( line.StartsWith( "BioStats", StringComparison.InvariantCultureIgnoreCase ) )
                    return typeof( BioStat );

                if( line.StartsWith( "Sales", StringComparison.InvariantCultureIgnoreCase ) )
                    return typeof( SaleRecord );

                throw new NotSupportedException();
            }

            var csvReader = CsvMultiRecordParser.GetInstance( new StringReader( csvInput ), recordTypeSelector, cfg =>
            {
                cfg.Delimiter = ",";
                cfg.IgnoreEmptyLines = true;
            } );

            var records = csvReader.GetRecords().ToList();

            Assert.IsTrue( records.Count == 4 );
            Assert.IsTrue( records.OfType<BioStat>().Count() == 2 );
            Assert.IsTrue( records.OfType<SaleRecord>().Count() == 2 );
        }
    }
}