using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using UltraMapper.Csv.Internals;

namespace UltraMapper.Csv.Tests
{
    [TestClass]
    public class FastFooterRead
    {
        [TestMethod]
        public void EmptyFile()
        {
            string fileLocation = Resources.GetFileLocation( "EmptyFile.csv" );

            var lines = ReadFileBackwards.GetLines( fileLocation, Encoding.UTF8 );
            Assert.IsTrue( lines.Count() == 0 );
        }

        [TestMethod]
        public void OnlyNewlines()
        {
            string fileLocation = Resources.GetFileLocation( "NewLinesOnly.csv" );

            var lines = ReadFileBackwards.GetLines( fileLocation, Encoding.UTF8 );
            Assert.IsTrue( lines.Count() == 0 );
        }
    }
}