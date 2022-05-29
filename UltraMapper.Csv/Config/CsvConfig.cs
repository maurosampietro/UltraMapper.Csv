using System.Globalization;
using UltraMapper.Csv.Config.DataFileParserConfig;

namespace UltraMapper.Csv.Config
{
    public class CsvConfig : DataFileParserConfiguration, ICsvParserConfiguration
    {
        public string Delimiter { get; set; } =
           CultureInfo.CurrentCulture.TextInfo.ListSeparator;

        public bool HasDelimiterInQuotes { get; set; } = false;
    }
}
