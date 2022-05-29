using System.Globalization;

namespace UltraMapper.Csv.Config.DataFileParserConfig
{
    public class DataFileReadOnlyConfiguration : IDataFileParserConfiguration
    {
#if NET4_0_OR_GREATER
        public CultureInfo CultureInfo { get; init; }

        public bool HasHeader { get; init; }
        public bool HasFooter { get; init; }

        public bool HasNewLinesInQuotes { get; init; }
        public bool IgnoreCommentedLines { get; init; }
        public bool IgnoreEmptyLines { get; init; }

        public string CommentMarker { get; init; }
#else
        public CultureInfo Culture { get; private set; }

        public bool HasHeader { get; private set; }
        public bool HasFooter { get; private set; }

        public bool HasNewLinesInQuotes { get; private set; }
        public bool IgnoreCommentedLines { get; private set; }
        public bool IgnoreEmptyLines { get; private set; }

        public string CommentMarker { get; private set; }

        public bool DisposeReader { get; private set; }
#endif

        public DataFileReadOnlyConfiguration( IDataFileParserConfiguration config )
        {
            this.Culture = config.Culture;

            this.HasHeader = config.HasHeader;
            this.HasFooter = config.HasFooter;

            this.HasNewLinesInQuotes = config.HasNewLinesInQuotes;
            this.IgnoreCommentedLines = config.IgnoreCommentedLines;
            this.IgnoreEmptyLines = config.IgnoreEmptyLines;

            this.CommentMarker = config.CommentMarker;
            this.DisposeReader = config.DisposeReader;
        }
    }
}
