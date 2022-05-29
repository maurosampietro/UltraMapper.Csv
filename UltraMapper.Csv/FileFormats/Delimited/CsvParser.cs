using System;
using System.Globalization;
using System.IO;
using System.Text;
using UltraMapper.Csv.Config;
using UltraMapper.Csv.FileFormats;
using UltraMapper.Csv.Footer;
using UltraMapper.Csv.Footer.FooterReaders;
using UltraMapper.Csv.Header;
using UltraMapper.Csv.LineReaders;
using UltraMapper.Csv.LineSplitters;

namespace UltraMapper.Csv
{
    public class CsvParser<TRecord> : DataFileParser<TRecord, ICsvParserConfiguration>,
        ICsvParser<TRecord>, IHeaderSupport, IFooterSupport
        where TRecord : class, new()
    {
        private readonly bool _disposeReader = false;

        internal CsvParser( TextReader reader, string delimiter,
            ILineSplitter lineSplitter,
            ILineReader lineReader,
            IHeaderReader headerReader = null,
            IFooterReader footerReader = null )
            : this( reader, lineSplitter, lineReader, headerReader,
                  footerReader, new CsvConfig() { Delimiter = delimiter } )
        { }

        internal CsvParser( TextReader reader,
            ILineSplitter lineSplitter, ILineReader lineReader,
            IHeaderReader headerReader = null, IFooterReader footerReader = null,
            CsvConfig config = null )
            : base( reader, lineSplitter, lineReader, headerReader, footerReader, new CsvReadonlyConfig( config ) )
        { }

        ~CsvParser()
        {
            if( _disposeReader )
                _reader?.Dispose();
        }
    }
}
