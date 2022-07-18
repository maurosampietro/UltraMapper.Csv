using System.IO;
using UltraMapper.Csv.LineReaders;

namespace UltraMapper.Csv
{
    /// <summary>
    /// Reads the first line by opening a new stream on the file.
    /// </summary>
    public class FileHeaderReader : IHeaderReader
    {
        private readonly string _filePath;

        public bool IsConsumingOriginalStream => false;

        public FileHeaderReader( string filePath )
        {
            _filePath = filePath;
        }

        public string GetHeader( ILineReader lineReader )
        {
            using( var streamReader = File.OpenText( _filePath ) )
                return lineReader.ReadLine( streamReader );
        }
    }
}