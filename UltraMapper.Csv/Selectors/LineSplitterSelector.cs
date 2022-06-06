using UltraMapper.Csv.Config;
using UltraMapper.Csv.LineSplitters;

namespace UltraMapper.Csv
{
    public static class LineSplitterSelector
    {
        public static ILineSplitter GetLineSplitter( CsvConfig config )
        {
            if( config.HasDelimiterInQuotes )
                return new CsvRfc4180DelimitedLineSplitter( config.Delimiter );

            return new DelimiterSplitter( config.Delimiter );
        }
    }
}
