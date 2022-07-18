using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Linq;

namespace UltraMapper.Csv.Tests
{
    [TestClass]
    public class Rfc4180CompliantTest
    {
        [CustomPreprocess( IsEnabled = true )]
        private class Auto
        {
            public int Year { get; set; }
            public string Make { get; set; }

            [CsvFieldOptions( Unquote = true, UnescapeQuotes = true )]
            public string Model { get; set; }

            [CsvFieldOptions( Unquote = true )]
            public string Description { get; set; }
            public double Price { get; set; }

            public void Preprocess( string[] data )
            {

            }
        }

        [TestMethod]
        public void Rfc4180Compliant()
        {
            string fileLocation = Resources.GetFileLocation( "RFC4180Standard.csv" );
            var csvReader = CsvParser<Auto>.GetInstance( fileLocation, cfg =>
            {
                cfg.Culture = CultureInfo.GetCultureInfo( "en-Us" );
                cfg.Delimiter = ",";
                cfg.HasNewLinesInQuotes = true;
                cfg.HasDelimiterInQuotes = true;
                cfg.HasHeader = true;
                cfg.HasFooter = false;
            } );

            var header = csvReader.GetHeader();
            var records = csvReader.GetRecords().ToList();

            Assert.IsTrue( records.Count == 4 );
        }
    }
}