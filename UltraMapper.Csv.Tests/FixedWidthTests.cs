using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using UltraMapper.Csv.Config.FieldOptions;
using UltraMapper.Csv.FileFormats.FixedWidth;

namespace UltraMapper.Csv.Tests
{
    [TestClass]
    public class FixedWidthTests
    {
        private class FixedWidthRecordNoLengths
        {
            public string Name { get; set; }
            public string State { get; set; }
            public string Telephone { get; set; }
        }

        private class FixedWidthRecordWithLengths
        {
            [FixedWidthFieldReadOptions( FieldLength = 20, TrimChar = '~' )]
            [FixedWidthFieldWriteOptions( FieldLength = 20, PadChar = '~', PadSide = PadSides.LEFT, HeaderPadSide = PadSides.RIGHT )]
            public string Name { get; set; }

            [FixedWidthFieldReadOptions( FieldLength = 10, TrimChar = '~' )]
            [FixedWidthFieldWriteOptions( FieldLength = 10, PadChar = '~', PadSide = PadSides.CENTER, HeaderPadSide = PadSides.RIGHT )]
            public string State { get; set; }

            [FixedWidthFieldReadOptions( FieldLength = 12, TrimChar = '~' )]
            [FixedWidthFieldWriteOptions( FieldLength = 12, PadChar = '~', PadSide = PadSides.RIGHT, HeaderPadSide = PadSides.RIGHT )]
            public string Telephone { get; set; }
        }

        [TestMethod]
        public void ReadingLengthsSet()
        {
            string fileLocation = Resources.GetFileLocation( "FixedWidth.dat" );

            var reader = FixedWidthParser<FixedWidthRecordWithLengths>.GetInstance( fileLocation, cfg =>
            {
                cfg.HasHeader = true;
                cfg.HasFooter = true;
            } );

            var footer = reader.GetFooter();
            var records = reader.GetRecords().ToList();

            Assert.IsTrue( records.Count == 3 );

            Assert.IsTrue( records[ 0 ].Name == "John Smith" );
            Assert.IsTrue( records[ 0 ].State == "WA" );
            Assert.IsTrue( records[ 0 ].Telephone == "418-Y11-4111" );

            Assert.IsTrue( records[ 1 ].Name == "Mary Hartford" );
            Assert.IsTrue( records[ 1 ].State == "CA" );
            Assert.IsTrue( records[ 1 ].Telephone == "319-Z19-4341" );

            Assert.IsTrue( records[ 2 ].Name == "Evan Nolan" );
            Assert.IsTrue( records[ 2 ].State == "IL" );
            Assert.IsTrue( records[ 2 ].Telephone == "219-532-5301" );

            Assert.IsTrue( footer == "EndOfFile" );
        }

        [TestMethod]
        public void ReadingLengthNoSet()
        {
            string fileLocation = Resources.GetFileLocation( "FixedWidth.dat" );

            var reader = FixedWidthParser<FixedWidthRecordNoLengths>.GetInstance( fileLocation, cfg =>
            {
                cfg.HasHeader = true;
                cfg.HasFooter = true;
            } );

            //Assert.ThrowsException<Exception>( () =>
            reader.GetRecords().ToList();
            //);
        }

        [TestMethod]
        public void Writing()
        {
            string readFileLocation = Resources.GetFileLocation( "FixedWidth.dat" );

            var reader = FixedWidthParser<FixedWidthRecordWithLengths>.GetInstance( readFileLocation, cfg =>
            {
                cfg.HasHeader = true;
                cfg.HasFooter = true;
            } );

            var records = reader.GetRecords().ToList();

            string writeFileLocation = Resources.GetFileLocation( "FixedWidthExample.writingtest.dat" );
            using( var writer = new StreamWriter( writeFileLocation ) )
            {
                var csvWriter = new FixedWidthWriter<FixedWidthRecordWithLengths>( writer );
                csvWriter.WriteHeader();
                csvWriter.WriteRecords( records );
                writer.Write( "EndOfFile" );
            }

            string rawInputFile = File.ReadAllText( readFileLocation );
            string rawOutputFile = File.ReadAllText( writeFileLocation );

            Assert.IsTrue( rawInputFile == rawOutputFile );
        }
    }
}
