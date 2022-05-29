﻿using System.IO;
using UltraMapper.Csv.Config.DataFileParserConfig;
using UltraMapper.Csv.FileFormats;
using UltraMapper.Csv.Footer.FooterReaders;
using UltraMapper.Csv.LineReaders;
using UltraMapper.Csv.LineSplitters;

namespace UltraMapper.Csv
{
    public class FixedWidthParser<TRecord> : DataFileParser<TRecord, IDataFileParserConfiguration>
        where TRecord : class, new()
    {
        internal FixedWidthParser( TextReader reader,
            ILineSplitter lineSplitter, ILineReader lineReader,
            IHeaderReader headerReader = null,
            IFooterReader footerReader = null,
            IDataFileParserConfiguration options = null )
        : base( reader, lineSplitter, lineReader, headerReader, footerReader, options )
        {
        }
    }
}
