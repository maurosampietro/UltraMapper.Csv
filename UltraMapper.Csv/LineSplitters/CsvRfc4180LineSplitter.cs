using System.Linq;
using UltraMapper.Csv.Internals;

namespace UltraMapper.Csv.LineSplitters
{
    public class CsvRfc4180DelimitedLineSplitter : ILineSplitter
    {
        private readonly string _delimiter;

        public CsvRfc4180DelimitedLineSplitter( string delimiter )
        {
            _delimiter = delimiter;
        }

        public string[] Split( string line )
        {
            if( _delimiter.Length == 1 )
                return line.SplitIfNotQuoted( _delimiter[ 0 ] ).ToArray();

            return line.SplitIfNotQuoted( _delimiter ).ToArray();
        }
    }
}
