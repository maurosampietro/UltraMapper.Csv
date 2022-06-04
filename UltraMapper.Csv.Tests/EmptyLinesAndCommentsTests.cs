using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using UltraMapper.Csv.Factories;

namespace UltraMapper.Csv.Tests
{
    [TestClass]
    public class RecordChecks
    {
        [TestMethod]
        public void ReadingFromFile()
        {
            string fileLocation = Resources.GetFileLocation( "CommentsAndEmptyLines.csv" );

            var csvReader = CsvParser<BioStat>.GetInstance( fileLocation, cfg =>
            {
                cfg.Delimiter = ",";
                cfg.HasHeader = true;
                cfg.HasFooter = false;
                cfg.IgnoreCommentedLines = true;
                cfg.IgnoreEmptyLines = true;
            } );

            var records = csvReader.GetRecords().ToList();
            Assert.IsTrue( records.Count == 4 );
        }
    }
}