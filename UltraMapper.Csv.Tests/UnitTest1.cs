using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using UltraMapper.Csv.Factories;
using UltraMapper.Csv.Tests.Biostats;

namespace UltraMapper.Csv.Tests
{
    [TestClass]
    public class TitanicTests
    {
        [TestMethod]
        public void MyTestMethod()
        {
            string fileLocation = Resources.GetFileLocation( "Titanic.csv" );

            var csvReader = CsvParser<Titanic>.GetInstance( fileLocation, cfg =>
            {
                cfg.Delimiter = ",";
                cfg.HasHeader = true;
                cfg.HasFooter = false;
                cfg.HasDelimiterInQuotes = true;
            } );

            var records = csvReader.GetRecords().ToList();
        }
    }

    /// <summary>
    /// Example csv data found at https://people.sc.fsu.edu/~jburkardt/data/csv/csv.html
    /// </summary>
    [TestClass]
    public partial class BioStats
    {
        [TestMethod]
        public void ReadingFromEmptyString()
        {
            var csvInput = new StringReader( String.Empty );

            var csvReader = CsvParser<BioStat>.GetInstance( csvInput, cfg =>
            {
                cfg.HasHeader = true;
                cfg.HasFooter = true;
            } );

            Assert.ThrowsException<Exception>( () =>
            {
                var header = csvReader.GetHeader();
            } );

            Assert.ThrowsException<Exception>( () =>
            {
                var records = csvReader.GetRecords().ToList();
            } );

            Assert.ThrowsException<Exception>( () =>
            {
                var footer = csvReader.GetFooter();
            } );
        }

        //[TestMethod]
        //public void Writing()
        //{
        //    string fileLocation = Resources.GetFileLocation( "biostats.csv" );

        //    var csvReader = new CsvParser( fileLocation )
        //    {
        //        HasHeader = true,
        //        HasFooter = true
        //    };

        //    var records = csvReader.GetRecords<BioStat>().ToList();

        //    var csvWriter = new CsvWriter( fileLocation + ".ultramapper" );
        //    csvWriter.Write( records );
        //}

        [TestMethod]
        public void ReadingFromEmptyFile()
        {
            string fileLocation = Resources.GetFileLocation( "EmptyFile.csv" );

            var csvReader = CsvParser<BioStat>.GetInstance( fileLocation, cfg =>
            {
                cfg.HasHeader = true;
                cfg.HasFooter = true;
            } );

            Assert.ThrowsException<Exception>( () => csvReader.GetHeader() );
            Assert.ThrowsException<Exception>( () => csvReader.GetRecords().ToList() );
            Assert.ThrowsException<Exception>( () => csvReader.GetFooter() );
        }

        [TestMethod]
        public void ReadingFromEmptyFileNoHeader()
        {
            string fileLocation = Resources.GetFileLocation( "EmptyFile.csv" );

            var csvReader = CsvParser<BioStat>.GetInstance( fileLocation, cfg =>
            {
                cfg.Delimiter = ",";
                cfg.HasHeader = false;
                cfg.HasFooter = true;
            } );

            Assert.IsTrue( csvReader.GetRecords().ToList().Count == 0 );
            Assert.ThrowsException<Exception>( () => csvReader.GetFooter() );
        }

        [TestMethod]
        public void ReadingFromFile()
        {
            string fileLocation = Resources.GetFileLocation( "biostats.csv" );

            var csvReader = CsvParser<BioStat>.GetInstance( fileLocation, cfg =>
            {
                cfg.Delimiter = ",";
                cfg.HasHeader = true;
            } );

            var header = csvReader.GetHeader();
            var records = csvReader.GetRecords().ToList();

            Assert.IsTrue( records.Count == 18 );
        }

        [TestMethod]
        public void ReadingFromString()
        {
            var csvInput = @"Name,     Sex, Age, Height (in), Weight (lbs)
    Alex,       M,   41,       74,      170
    Bert,       M,   42,       68,      166
    Carl,       M,   32,       70,      155
    Dave,       M,   39,       72,      167
    Elly,       F,   30,       66,      124
    Fran,       F,   33,       66,      115
    Gwen,       F,   26,       64,      121
    Hank,       M,   30,       71,      158
    Ivan,       M,   53,       72,      175
    Jake,       M,   32,       69,      143
    Kate,       F,   47,       69,      139
    Luke,       M,   34,       72,      163
    Myra,       F,   23,       62,       98
    Neil,       M,   36,       75,      160
    Omar,       M,   38,       70,      145
    Page,       F,   31,       67,      135
    Quin,       M,   29,       71,      176
    Ruth,       F,   28,       65,      131
    EndOfFile";

            var csvReader = CsvParser<BioStat>.GetInstance( new StringReader( csvInput ), cfg =>
            {
                cfg.Delimiter = ",";
                cfg.HasHeader = true;
                cfg.HasFooter = true;
            } );

            var header = csvReader.GetHeader();
            var records = csvReader.GetRecords().ToList();
            var footer = csvReader.GetFooter(); //footer MUST be read after the header and records if working with streams

            Assert.IsTrue( records.Count == 18 );
            Assert.IsTrue( footer.Trim() == "EndOfFile" );
        }
    }
}