using UltraMapper.Csv.Config.DataFileParserConfig;
using UltraMapper.Csv.LineReaders;

namespace UltraMapper.Csv
{
    public static class LineReaderSelector
    {
        public static ILineReader GetLineReader( DataFileParserConfiguration config )
        {
            var parsableLineRule = ParsableLineRuleSelector.GetParsableLineRule( config );

            ILineReader lineReader = new DefaultLineReader();
            if( config.HasNewLinesInQuotes )
                lineReader = new CsvRfc4180LineReader();

            if( parsableLineRule != null )
                lineReader = new SkipNonParsableLineReader( lineReader, parsableLineRule );

            return lineReader;
        }
    }
}
