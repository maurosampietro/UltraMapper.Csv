using System;
using System.IO;
using System.Linq;
using UltraMapper.Conventions;
using UltraMapper.Csv.Config.DataFileParserConfig;
using UltraMapper.Csv.Config.FieldOptions;
using UltraMapper.Csv.FileFormats;
using UltraMapper.Csv.Footer.FooterReaders;
using UltraMapper.Csv.Header.HeaderReaders;
using UltraMapper.Csv.LineReaders;
using UltraMapper.Csv.LineSplitters;
using UltraMapper.Csv.ParsableLineRules;
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
            this.FieldConfig = FieldConfiguration.Register<FixedWidthFieldReadOptionsAttribute, TRecord>( sourceMemberProvider );
        }

        public static FixedWidthParser<TRecord> GetInstance( string filePath, DataFileParserConfiguration config )
        {
            //We are gonna open a StreamReader on a file so we are responsible of disposing it
            config.DisposeReader = true;

            var lineSplitter = new FixedWidthLineSplitter<TRecord>();
            var lineReader = GetLineReader( config );

            var reader = new StreamReader( filePath, config.Encoding );

            var headerReader = new FileHeaderReader( filePath );
            var footerReader = new FileFooterReader( filePath );

            return new FixedWidthParser<TRecord>( reader, lineSplitter, lineReader, headerReader, footerReader, config );
        }

        public static FixedWidthParser<TRecord> GetInstance( string filePath, Action<DataFileParserConfiguration> configSetup )
        {
            var config = new DataFileParserConfiguration();
            configSetup.Invoke( config );

            return GetInstance( filePath, config );
        }

        public static FixedWidthParser<TRecord> GetInstance( TextReader reader, DataFileParserConfiguration config )
        {
            var lineSplitter = new FixedWidthLineSplitter<TRecord>();
            var lineReader = GetLineReader( config );
            var headerReader = new DefaultHeaderReader( reader );
            var footerReader = new DefaultFooterReader( reader );

            return new FixedWidthParser<TRecord>( reader, lineSplitter, lineReader, headerReader, footerReader, config );
        }

        public static FixedWidthParser<TRecord> GetInstance( TextReader reader, Action<DataFileParserConfiguration> configSetup )
        {
            var config = new DataFileParserConfiguration();
            configSetup.Invoke( config );

            return GetInstance( reader, config );
        }

        private static ILineReader GetLineReader( DataFileParserConfiguration config )
        {
            var parsableLineRule = GetParsableLineRule( config );

            ILineReader lineReader = new DefaultLineReader();
            if( config.HasNewLinesInQuotes )
                lineReader = new CsvRfc4180LineReader();

            if( parsableLineRule != null )
                lineReader = new SkipNonParsableLineReader( lineReader, parsableLineRule );

            return lineReader;
        }

        protected static IParsableLineRule GetParsableLineRule( DataFileParserConfiguration config )
        {
            if( config.IgnoreCommentedLines && config.IgnoreEmptyLines )
                return new IgnoreEmptyAndCommentedLine( config.CommentMarker );

            if( config.IgnoreCommentedLines )
                return new IgnoreCommentedLine( config.CommentMarker );

            if( config.IgnoreEmptyLines )
                return new IgnoreEmptyLine();

            return null;
        }
    }
}
