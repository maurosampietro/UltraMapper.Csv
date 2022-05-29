using System.IO;
using System.Linq;
using System.Text;
using UltraMapper.Csv.Footer.FooterReaders;
using UltraMapper.Csv.Internals;
using UltraMapper.Csv.LineReaders;

namespace UltraMapper.Csv
{
    public class FileFooterReader : IFooterReader
    {
        private readonly string _filePath;

        public bool IsConsumingOriginalStream => false;

        public FileFooterReader( string filePath )
        {
            _filePath = filePath;
        }

        public string GetFooter( ILineReader lineReader = null )
        {
            var encoding = this.GetTextFileEncoding();
            return ReadFileBackwards.GetLines( _filePath, encoding ).FirstOrDefault();
        }

        private Encoding GetTextFileEncoding()
        {
            return File.OpenText( _filePath ).CurrentEncoding;
        }
    }
}
