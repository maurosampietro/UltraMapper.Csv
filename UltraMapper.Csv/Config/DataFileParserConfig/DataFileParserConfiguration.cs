using System.Globalization;
using System.Text;

namespace UltraMapper.Csv.Config.DataFileParserConfig
{
    public class DataFileParserConfiguration : IDataFileParserConfiguration
    {
        public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

        public bool HasNewLinesInQuotes { get; set; } = false;
        public bool IgnoreCommentedLines { get; set; } = false;
        public bool IgnoreEmptyLines { get; set; } = false;

        public string CommentMarker { get; set; } = "#";

        public bool HasHeader { get; set; } = false;
        public bool HasFooter { get; set; } = false;

        public Encoding Encoding { get; set; } = Encoding.UTF8;
        public bool DisposeReader { get; internal set; } = false;
    }
}
