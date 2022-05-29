using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltraMapper.Csv.Tests
{

    [TestClass]
    public class SplitterTests
    {
        [TestMethod]
        public void KeepEmptyEntries()
        {
            //7 items
            string record = @"a,b,c,,,f,g";

            var splitter = new LineSplitters.CsvRfc4180DelimitedLineSplitter( "," );
            var splitData = splitter.Split( record ).ToArray();

            Assert.IsTrue( splitData.Length == 7 );

            Assert.IsTrue( splitData[0] == "a" );
            Assert.IsTrue( splitData[1] == "b" );
            Assert.IsTrue( splitData[2] == "c" );
            Assert.IsTrue( splitData[3] == "" );
            Assert.IsTrue( splitData[4] == "" );
            Assert.IsTrue( splitData[5] == "f" );
            Assert.IsTrue( splitData[6] == "g" );
        }

        [TestMethod]
        public void KeepEmptyEntryAtTheEnd()
        {
            //7 items, 4 empty
            string record = "a,b,c,,,f,";

            var splitter = new LineSplitters.CsvRfc4180DelimitedLineSplitter( "," );
            var splitData = splitter.Split( record ).ToArray();

            Assert.IsTrue( splitData.Length == 7 );
            Assert.IsTrue( splitData[ 0 ] == "a" );
            Assert.IsTrue( splitData[ 1 ] == "b" );
            Assert.IsTrue( splitData[ 2 ] == "c" );
            Assert.IsTrue( splitData[ 3 ] == "" );
            Assert.IsTrue( splitData[ 4 ] == "" );
            Assert.IsTrue( splitData[ 5 ] == "f" );
            Assert.IsTrue( splitData[ 6 ] == "" );
        }
    }
}
