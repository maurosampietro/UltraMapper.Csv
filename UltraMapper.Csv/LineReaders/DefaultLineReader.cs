using System.IO;

namespace UltraMapper.Csv.LineReaders
{
    /// <summary>
    /// Returns a line from the reader.
    /// A line ends when a newline-char is found.
    /// </summary>
    public class DefaultLineReader : ILineReader
    {
        public string ReadLine( TextReader reader )
        {
            return reader.ReadLine();
        }
    }
}
