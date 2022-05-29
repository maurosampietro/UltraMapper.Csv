using UltraMapper.Csv.Config.DataFileParserConfig;

namespace UltraMapper.Csv.Config
{
    public interface ICsvParserConfiguration : IDataFileParserConfiguration
    {
        string Delimiter { get; }
        bool HasDelimiterInQuotes { get; }
    }
}
