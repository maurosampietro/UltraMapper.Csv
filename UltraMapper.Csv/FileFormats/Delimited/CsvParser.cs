﻿using System;
using System.IO;
using System.Linq;
using UltraMapper.Conventions;
using UltraMapper.Csv.Config;
using UltraMapper.Csv.Config.FieldOptions;
using UltraMapper.Csv.FileFormats;
using UltraMapper.Csv.Footer;
using UltraMapper.Csv.Footer.FooterReaders;
using UltraMapper.Csv.Header;
using UltraMapper.Csv.Header.HeaderReaders;
using UltraMapper.Csv.LineReaders;
using UltraMapper.Csv.LineSplitters;
using UltraMapper.Csv.ParsableLineRules;
using UltraMapper.Csv.UltraMapper.Extensions.Read.Csv;
using UltraMapper.MappingExpressionBuilders;

namespace UltraMapper.Csv
{
    public class CsvParser<TRecord> : DataFileParser<TRecord, ICsvParserConfiguration, CsvRecordReadObject>,
        ICsvParser<TRecord>, IHeaderSupport, IFooterSupport
        where TRecord : class, new()
    {
        public FieldOptionsProvider<CsvReadOptionsAttribute> FieldConfig { get; }

        static CsvParser()
        {
            Mapper.Config.Mappers.AddBefore<ReferenceMapper>( new CsvRecordToObjectMapper( Mapper.Config ) );
        }

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
            this.FieldConfig = new FieldOptionsProvider<CsvReadOptionsAttribute>( sourceMemberProvider, typeof( TRecord ) );
        }

        public static CsvParser<TRecord> GetInstance( string filePath, CsvConfig config )
        {
            //We are gonna open a StreamReader on a file so we are responsible of disposing it
            config.DisposeReader = true;

            var lineSplitter = GetLineSplitter( config );
            var lineReader = GetLineReader( config );

            var reader = new StreamReader( filePath, config.Encoding );

            var headerReader = new FileHeaderReader( filePath );
            var footerReader = new FileFooterReader( filePath );

            return new CsvParser<TRecord>( reader, lineSplitter, lineReader, headerReader, footerReader, config );
        }

        public static CsvParser<TRecord> GetInstance( string filePath, Action<CsvConfig> configSetup )
        {
            var config = new CsvConfig();
            configSetup.Invoke( config );

            return GetInstance( filePath, config );
        }

        public static CsvParser<TRecord> GetInstance( TextReader reader, CsvConfig config )
        {
            var lineSplitter = GetLineSplitter( config );
            var lineReader = GetLineReader( config );
            var headerReader = new DefaultHeaderReader( reader );
            var footerReader = new DefaultFooterReader( reader );

            return new CsvParser<TRecord>( reader, lineSplitter, lineReader, headerReader, footerReader, config );
        }

        public static CsvParser<TRecord> GetInstance( TextReader reader, Action<CsvConfig> configSetup )
        {
            var config = new CsvConfig();
            configSetup.Invoke( config );

            return GetInstance( reader, config );
        }

        private static ILineReader GetLineReader( CsvConfig config )
        {
            var parsableLineRule = GetParsableLineRule( config );

            ILineReader lineReader = new DefaultLineReader();
            if( config.HasNewLinesInQuotes )
                lineReader = new CsvRfc4180LineReader();

            if( parsableLineRule != null )
                lineReader = new SkipNonParsableLineReader( lineReader, parsableLineRule );

            return lineReader;
        }

        protected static IParsableLineRule GetParsableLineRule( CsvConfig config )
        {
            if( config.IgnoreCommentedLines && config.IgnoreEmptyLines )
                return new IgnoreEmptyAndCommentedLine( config.CommentMarker );

            if( config.IgnoreCommentedLines )
                return new IgnoreCommentedLine( config.CommentMarker );

            if( config.IgnoreEmptyLines )
                return new IgnoreEmptyLine();

            return null;
        }

        private static ILineSplitter GetLineSplitter( CsvConfig config )
        {
            if( config.HasDelimiterInQuotes )
                return new CsvRfc4180DelimitedLineSplitter( config.Delimiter );

            return new DelimiterSplitter( config.Delimiter );
        }
    }
}
