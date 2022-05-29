using UltraMapper.Csv.LineReaders;

namespace UltraMapper.Csv.Footer.FooterReaders
{
    public interface IFooterReader
    {
        bool IsConsumingOriginalStream { get; }
        string GetFooter( ILineReader lineReader );
    }
}
