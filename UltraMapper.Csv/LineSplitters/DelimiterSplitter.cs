using System;

namespace UltraMapper.Csv.LineSplitters
{
    public class DelimiterSplitter : ILineSplitter
    {
        private readonly string _delimiter;

        public DelimiterSplitter( string delimiter )
        {
            _delimiter = delimiter;
        }

        public string[] Split( string line )
        {
            if( _delimiter.Length == 1 )
                return line.Split( _delimiter[ 0 ] );

            return line.Split( new string[] { _delimiter }, StringSplitOptions.None );
        }
    }
}
