using UltraMapper.Csv.Config.DataFileParserConfig;

namespace UltraMapper.Csv.Config
{
    public class CsvReadonlyConfig : DataFileReadOnlyConfiguration, ICsvParserConfiguration
    {
        public string Delimiter { get; }
        public bool HasDelimiterInQuotes { get; }

        public CsvReadonlyConfig( ICsvParserConfiguration configuration )
            : base( configuration )
        {
            this.Delimiter = configuration.Delimiter;
            this.HasDelimiterInQuotes = configuration.HasDelimiterInQuotes;
        }
    }
}
