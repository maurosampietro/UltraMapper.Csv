namespace UltraMapper.Csv
{
    public interface IPreprocessOptions
    {
        string FillInValue { get; set; }
        string Format { get; set; }
        char TrimChar { get; set; }
        bool TrimWhitespaces { get; set; }
        bool UnescapeQuotes { get; set; }
        bool Unquote { get; set; }
    }
}