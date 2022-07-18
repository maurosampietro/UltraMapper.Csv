namespace UltraMapper.Csv.Tests
{
    internal class BioStat
    {
        [CsvFieldOptions( TrimWhitespaces = true, Unquote = true )]
        public string Name { get; set; }

        [CsvFieldOptions( TrimWhitespaces = true, Unquote = true )]
        public char Sex { get; set; }

        [CsvFieldOptions( TrimWhitespaces = true, Unquote = true )]
        public int Age { get; set; }

        [CsvFieldOptions( TrimWhitespaces = true, Unquote = true )]
        public double Height { get; set; }

        [CsvFieldOptions( TrimWhitespaces = true, Unquote = true )]
        public double Weight { get; set; }
    }
}