using System;
using System.IO;
using UltraMapper.Csv.Config;
using UltraMapper.Csv.FileFormats;
using UltraMapper.Csv.FileFormats.Delimited.Mode;
using UltraMapper.Csv.Footer.FooterReaders;
using UltraMapper.Csv.Header.HeaderReaders;
using UltraMapper.Csv.LineReaders;
using UltraMapper.Csv.LineSplitters;
using UltraMapper.Csv.ParsableLineRules;
using UltraMapper.Csv.UltraMapper.Extensions.Read.Csv;

namespace UltraMapper.Csv
{
    /// <summary>
    /// Handle heterogeneous records.
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    public class CsvMultiRecordParser : DataFileParser<object, ICsvParserConfiguration, CsvRecordReadObject>, ICsvParser<object>
    {
        private Type _recordType = null;

        public IMultiRecordSelector Selector { get; set; }

        internal CsvMultiRecordParser( TextReader reader,
            ILineSplitter lineSplitter, ILineReader lineReader,
            IHeaderReader headerReader, IFooterReader footerReader,
            IMultiRecordSelector selector, CsvConfig config )
           : base( reader, lineSplitter, lineReader, headerReader, footerReader, config )
        {
            this.Selector = selector ?? throw new ArgumentNullException( nameof( selector ) );
            this.Configuration = new CsvReadonlyConfig( config );
        }

        protected override object MapLine( string line )
        {
            var recordType = this.Selector.SelectType( line );
            if( _recordType != recordType )
            {
                _recordType = recordType;
                _mapFunction = Mapper.Config[ typeof( CsvRecordReadObject ), recordType ].MappingFunc;
            }

            _dataRecord.Data = _lineSplitter.Split( line );
            return Mapper.Map( _dataRecord, targetType: recordType );
        }

        public static CsvMultiRecordParser GetInstance( string filePath, IMultiRecordSelector selector, CsvConfig config )
        {
            //We are gonna open a StreamReader on a file so we are responsible of disposing it
            config.DisposeReader = true;

            var lineSplitter = GetLineSplitter( config );
            var lineReader = GetLineReader( config );

            var reader = new StreamReader( filePath, config.Encoding );

            var headerReader = new FileHeaderReader( filePath );
            var footerReader = new FileFooterReader( filePath );

            return new CsvMultiRecordParser( reader, lineSplitter, lineReader, headerReader, footerReader, selector, config );
        }

        public static CsvMultiRecordParser GetInstance( string filePath, IMultiRecordSelector selector, Action<CsvConfig> configSetup )
        {
            var config = new CsvConfig();
            configSetup.Invoke( config );

            return GetInstance( filePath, selector, config );
        }

        public static CsvMultiRecordParser GetInstance( string filePath, Func<string, Type> selector, CsvConfig config )
        {
            var relaySelector = new MultiRecordRelaySelector( selector );
            return GetInstance( filePath, relaySelector, config );
        }

        public static CsvMultiRecordParser GetInstance( string filePath, Func<string, Type> selector, Action<CsvConfig> configSetup )
        {
            var config = new CsvConfig();
            configSetup.Invoke( config );

            var relaySelector = new MultiRecordRelaySelector( selector );

            return GetInstance( filePath, relaySelector, config );
        }

        public static CsvMultiRecordParser GetInstance( TextReader reader, IMultiRecordSelector selector, CsvConfig config )
        {
            var lineSplitter = GetLineSplitter( config );
            var lineReader = GetLineReader( config );

            var headerReader = new DefaultHeaderReader( reader );
            var footerReader = new DefaultFooterReader( reader );

            return new CsvMultiRecordParser( reader, lineSplitter, lineReader, headerReader, footerReader, selector, config );
        }

        public static CsvMultiRecordParser GetInstance( TextReader reader, IMultiRecordSelector selector, Action<CsvConfig> configSetup )
        {
            var config = new CsvConfig();
            configSetup.Invoke( config );

            return GetInstance( reader, selector, config );
        }

        public static CsvMultiRecordParser GetInstance( TextReader reader, Func<string, Type> selector, CsvConfig config )
        {
            var relaySelector = new MultiRecordRelaySelector( selector );
            return GetInstance( reader, relaySelector, config );
        }

        public static CsvMultiRecordParser GetInstance( TextReader reader, Func<string, Type> selector, Action<CsvConfig> configSetup )
        {
            var config = new CsvConfig();
            configSetup.Invoke( config );

            var relaySelector = new MultiRecordRelaySelector( selector );

            return GetInstance( reader, relaySelector, config );
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
