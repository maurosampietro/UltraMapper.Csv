using System.IO;
using UltraMapper.Csv.ParsableLineRules;

namespace UltraMapper.Csv.LineReaders
{
    /// <summary>
    /// Wraps another ILineReader and provides lines skip functionalities
    /// </summary>
    public class SkipNonParsableLineReader : ILineReader
    {
        public readonly ILineReader LineReader;
        public readonly IParsableLineRule ParsableLineRule;

        public SkipNonParsableLineReader( ILineReader lineReader, IParsableLineRule parsableLineRule )
        {
            this.LineReader = lineReader;
            this.ParsableLineRule = parsableLineRule;
        }

        public string ReadLine( TextReader reader )
        {
            var line = this.LineReader.ReadLine( reader );
            while( line != null && !this.ParsableLineRule.IsLineParsable( line ) )
                line = this.LineReader.ReadLine( reader );

            return line;
        }
    }
}
