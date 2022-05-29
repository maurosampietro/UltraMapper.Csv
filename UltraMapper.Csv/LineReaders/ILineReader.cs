using System.IO;

namespace UltraMapper.Csv.LineReaders
{
    public interface ILineReader
    {
        string ReadLine( TextReader reader );
    }
}