using UltraMapper.Csv.LineReaders;

namespace UltraMapper.Csv
{
    public interface IHeaderReader
    {
        bool IsConsumingOriginalStream { get; }
        string GetHeader( ILineReader lineReader );
    }
}
