using UltraMapper.Csv.Config.DataFileParserConfig;
using UltraMapper.Csv.ParsableLineRules;

namespace UltraMapper.Csv
{
    public static class ParsableLineRuleSelector
    {
        public static IParsableLineRule GetParsableLineRule( DataFileParserConfiguration config )
        {
            if( config.IgnoreCommentedLines && config.IgnoreEmptyLines )
                return new IgnoreEmptyAndCommentedLine( config.CommentMarker );

            if( config.IgnoreCommentedLines )
                return new IgnoreCommentedLine( config.CommentMarker );

            if( config.IgnoreEmptyLines )
                return new IgnoreEmptyLine();

            return null;
        }
    }
}
