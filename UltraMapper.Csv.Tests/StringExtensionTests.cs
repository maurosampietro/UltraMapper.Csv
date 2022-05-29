using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using UltraMapper.Csv.Internals;

namespace UltraMapper.Csv.Tests
{
    [TestClass]
    public class StringExtensionTests
    {
        [TestMethod]
        public void StringSplitIfNotQuoted()
        {
            string str = @"a,b,""c,d"",e,f";
            var result = str.SplitIfNotQuoted( "," ).ToArray();

            Assert.IsTrue( result[ 0 ] == "a" );
            Assert.IsTrue( result[ 1 ] == "b" );
            Assert.IsTrue( result[ 2 ] == "\"c,d\"" );
            Assert.IsTrue( result[ 3 ] == "e" );
            Assert.IsTrue( result[ 4 ] == "f" );
        }

        [TestMethod]
        public void StringSplitIfNotQuoted2()
        {
            string str = "a,b,\"c,d\",e,f,\"g,h,i";
            var result = str.SplitIfNotQuoted( "," ).ToArray();
        }

        [TestMethod]
        public void StringSplitFixedLengths()
        {
            string str = "AlexM4174170";
            var result = str.Split( new int[] { 4, 1, 2, 2, 3 } ).ToArray();

            Assert.IsTrue( result[ 0 ] == "Alex" );
            Assert.IsTrue( result[ 1 ] == "M" );
            Assert.IsTrue( result[ 2 ] == "41" );
            Assert.IsTrue( result[ 3 ] == "74" );
            Assert.IsTrue( result[ 4 ] == "170" );
        }
    }
}