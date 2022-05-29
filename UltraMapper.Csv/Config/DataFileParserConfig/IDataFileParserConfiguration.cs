using System.Globalization;

namespace UltraMapper.Csv.Config.DataFileParserConfig
{
    public interface IDataFileParserConfiguration
    {
        CultureInfo Culture { get; }

        bool HasHeader { get; }
        bool HasFooter { get; }

        bool HasNewLinesInQuotes { get; }
        bool IgnoreCommentedLines { get; }
        bool IgnoreEmptyLines { get; }

        string CommentMarker { get; }
        bool DisposeReader { get; }
    }
}
