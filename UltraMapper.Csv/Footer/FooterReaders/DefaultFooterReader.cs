using System.IO;
using UltraMapper.Csv.Footer.FooterReaders;
using UltraMapper.Csv.LineReaders;

namespace UltraMapper.Csv
{
    public class DefaultFooterReader : IFooterReader
    {
        private readonly TextReader _reader;

        public bool IsConsumingOriginalStream => true;

        public DefaultFooterReader( TextReader reader )
        {
            _reader = reader;
        }

        public string GetFooter( ILineReader lineReader )
        {
            string lastLine = null;
            string line = lineReader.ReadLine( _reader );

            while( line != null )
            {
                lastLine = line;
                line = lineReader.ReadLine( _reader );
            }

            return lastLine;
        }
    }
}
