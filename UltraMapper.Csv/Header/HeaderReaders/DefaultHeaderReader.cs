using System.IO;
using UltraMapper.Csv.LineReaders;

namespace UltraMapper.Csv.Header.HeaderReaders
{
    /// <summary>
    /// Reads a line without not knowing if it is the first line
    /// </summary>
    public class DefaultHeaderReader : IHeaderReader
    {
        private readonly TextReader _reader;

        public bool IsConsumingOriginalStream => true;

        public DefaultHeaderReader( TextReader reader )
        {
            _reader = reader;
        }

        public string GetHeader( ILineReader lineReader )
        {
            return lineReader.ReadLine( _reader );
        }
    }
}
