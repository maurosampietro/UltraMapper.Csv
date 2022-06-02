using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UltraMapper.Conventions;
using UltraMapper.Csv.Config;
using UltraMapper.Csv.Config.FieldOptions;
using UltraMapper.Csv.FileFormats;
using UltraMapper.Csv.Footer;
using UltraMapper.Csv.Footer.FooterReaders;
using UltraMapper.Csv.Header;
using UltraMapper.Csv.LineReaders;
using UltraMapper.Csv.LineSplitters;
using UltraMapper.Csv.UltraMapper.Extensions.Write;
using UltraMapper.MappingExpressionBuilders;

namespace UltraMapper.Csv
{
    public class CsvParser<TRecord> : DataFileParser<TRecord, ICsvParserConfiguration>,
        ICsvParser<TRecord>, IHeaderSupport, IFooterSupport
        where TRecord : class, new()
    {
        public FieldOptionsProvider<TRecord, CsvReadOptionsAttribute> FieldConfig { get; }

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
        {
            var sourceMemberProvider = Mapper.Config.Conventions.OfType<DefaultConvention>().Single().SourceMemberProvider;
            this.FieldConfig = new FieldOptionsProvider<TRecord, CsvReadOptionsAttribute>( sourceMemberProvider );
        }
    }
}
