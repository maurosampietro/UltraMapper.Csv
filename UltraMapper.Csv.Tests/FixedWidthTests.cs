using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using UltraMapper.Csv.Factories;

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
            public string Name { get; set; }

            [FixedWidthFieldReadOptions( FieldLength = 10, TrimChar = '~' )]
            public string State { get; set; }

            [FixedWidthFieldReadOptions( FieldLength = 12, TrimChar = '~' )]
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

            reader.FieldConfig.Reading.Configure( "Name", o => { o.FieldLength = 30; } );

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

        //[TestMethod]
        //public void Writing()
        //{
        //    string fileLocation = Resources.GetFileLocation( "FixedWidthExample.writingtest.dat" );

        //    var csvReader = new CsvParser( new Uri( fileLocation ) )
        //    {
        //        HasHeader = true,
        //        HasFooter = true
        //    };

        //    var records = csvReader.GetRecords<FixedWidthRecordWithLengths>().ToList();

        //    var csvWriter = new CsvWriter( fileLocation + ".ultramapper" );
        //    csvWriter.Write( records );
        //}
    }
}
