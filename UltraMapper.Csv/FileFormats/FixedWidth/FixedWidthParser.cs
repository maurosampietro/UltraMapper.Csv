using System.IO;
using System.Linq;
using UltraMapper.Conventions;
using UltraMapper.Csv.Config.DataFileParserConfig;
using UltraMapper.Csv.Config.FieldOptions;
using UltraMapper.Csv.FileFormats;
using UltraMapper.Csv.Footer.FooterReaders;
using UltraMapper.Csv.LineReaders;
using UltraMapper.Csv.LineSplitters;
using UltraMapper.Csv.UltraMapper.Extensions.Read.Csv;
using UltraMapper.Csv.UltraMapper.Extensions.Read.FixedWidth;
using UltraMapper.MappingExpressionBuilders;

namespace UltraMapper.Csv
{
    public class FixedWidthParser<TRecord> : DataFileParser<TRecord, IDataFileParserConfiguration, FixedWidthRecordReadObject>
        where TRecord : class, new()
    {
        public FieldOptionsProvider<FixedWidthFieldReadOptionsAttribute> FieldConfig { get; }

        static FixedWidthParser()
        {
            Mapper.Config.Mappers.AddBefore<ReferenceMapper>( new FixedWidthRecordToObjectMapper( Mapper.Config ) );
        }

        internal FixedWidthParser( TextReader reader,
            ILineSplitter lineSplitter, ILineReader lineReader,
            IHeaderReader headerReader = null,
            IFooterReader footerReader = null,
            IDataFileParserConfiguration options = null )
        : base( reader, lineSplitter, lineReader, headerReader, footerReader, options )
        {
            var sourceMemberProvider = Mapper.Config.Conventions.OfType<DefaultConvention>().Single().SourceMemberProvider;
            this.FieldConfig = new FieldOptionsProvider<FixedWidthFieldReadOptionsAttribute>( sourceMemberProvider, typeof( TRecord ) );
        }
    }
}
