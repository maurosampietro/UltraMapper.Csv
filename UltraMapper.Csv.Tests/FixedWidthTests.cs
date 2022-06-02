using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using UltraMapper.Csv.Factories;
using UltraMapper.Csv.FileFormats;
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
            [FixedWidthFieldWriteOptions( FieldLength = 20, PadChar = '~', PadSide = FixedWidthFieldWriteOptionsAttribute.PadSides.RIGHT )]
            public string Name { get; set; }

            [FixedWidthFieldReadOptions( FieldLength = 10, TrimChar = '~' )]
            [FixedWidthFieldWriteOptions( FieldLength = 10, PadChar = '~', PadSide = FixedWidthFieldWriteOptionsAttribute.PadSides.RIGHT )]
            public string State { get; set; }

            [FixedWidthFieldReadOptions( FieldLength = 12, TrimChar = '~' )]
            [FixedWidthFieldWriteOptions( FieldLength = 12, PadChar = '~', PadSide = FixedWidthFieldWriteOptionsAttribute.PadSides.RIGHT )]
            public string Telephone { get; set; }
        }

        [TestMethod]
        public void ReadingLengthsSet()
        {
            string fileLocation = Resources.GetFileLocation( "FixedWidth.dat" );

            var reader = FixedWidthFactory.GetInstance<FixedWidthRecordWithLengths>( new Uri( fileLocation ), cfg =>
            {
                cfg.HasHeader = true;
                cfg.HasFooter = true;
            } );

            reader.FieldConfig.Configure( "Name", o => { o.FieldLength = 30; } );

            var footer = reader.GetFooter();
            var records = reader.GetRecords().ToList();

            Assert.IsTrue( records.Count == 3 );
            Assert.IsTrue( footer == "EndOfFile" );
        }

        [TestMethod]
        public void ReadingLengthNoSet()
        {
            string fileLocation = Resources.GetFileLocation( "FixedWidth.dat" );

            var reader = FixedWidthFactory.GetInstance<FixedWidthRecordNoLengths>(
                new Uri( fileLocation ), cfg =>
                {
                    cfg.HasHeader = true;
                    cfg.HasFooter = true;
                } );

            Assert.ThrowsException<Exception>( () =>
                reader.GetRecords().ToList() );
        }

        [TestMethod]
        public void Writing()
        {
            string writeFileLocation = Resources.GetFileLocation( "FixedWidthExample.writingtest.dat" );

            string readFileLocation = Resources.GetFileLocation( "FixedWidth.dat" );

            var reader = FixedWidthFactory.GetInstance<FixedWidthRecordWithLengths>(
                new Uri( readFileLocation ), cfg =>
                {
                    cfg.HasHeader = true;
                    cfg.HasFooter = true;
                } );

            var records = reader.GetRecords().ToList();

            using( var writer = new StreamWriter( writeFileLocation ) )
            {
                var csvWriter = new FixedWidthWriter<FixedWidthRecordWithLengths>( writer );
                csvWriter.WriteHeader();
                csvWriter.WriteRecords( records );
                writer.Write( "EndOfFile" );
            }

            string rawInputFile = File.ReadAllText( readFileLocation );
            string rawOutputFile = File.ReadAllText( writeFileLocation );

            //manca l'header e il footer
            Assert.IsTrue( rawInputFile == rawOutputFile );
        }
    }
}
