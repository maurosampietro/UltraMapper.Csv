using System;

namespace UltraMapper.Csv.Tests
{
    internal class BioStat
    {
        [CsvReadOptions( TrimWhitespaces = true, Unquote = true )]
        public string Name { get; set; }

        [CsvReadOptions( TrimWhitespaces = true, Unquote = true )]
        public char Sex { get; set; }

        [CsvReadOptions( TrimWhitespaces = true, Unquote = true )] 
        public int Age { get; set; }

        [CsvReadOptions( TrimWhitespaces = true, Unquote = true )] 
        public double Height { get; set; }

        [CsvReadOptions( TrimWhitespaces = true, Unquote = true )] 
        public double Weight { get; set; }
    }
}