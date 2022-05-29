using System;
using System.IO;
using UltraMapper.Csv.Config;
using UltraMapper.Csv.FileFormats.Delimited.Mode;
using UltraMapper.Csv.Header.HeaderReaders;
using UltraMapper.Csv.LineReaders;
using UltraMapper.Csv.LineSplitters;
using UltraMapper.Csv.ParsableLineRules;

namespace UltraMapper.Csv.Factories
{
    public class CsvParserFactory
    {
        #region CSV
        public static CsvParser<T> GetInstance<T>( string content, CsvConfig config ) where T : class, new()
        {
            var reader = new StringReader( content );
            return GetInstance<T>( reader, config );
        }

        public static CsvParser<T> GetInstance<T>( string content, Action<CsvConfig> configSetup ) where T : class, new()
        {
            var config = new CsvConfig();
            configSetup.Invoke( config );

            return GetInstance<T>( content, config );
        }

        public static CsvParser<T> GetInstance<T>( Uri filePath, CsvConfig config ) where T : class, new()
        {
            //We are gonna open a StreamReader on a file so we are responsible of disposing it
            config.DisposeReader = true;

            var lineSplitter = GetLineSplitter( config );
            var lineReader = GetLineReader( config );

            var reader = new StreamReader( filePath.LocalPath, config.Encoding );

            var headerReader = new FileHeaderReader( filePath.LocalPath );
            var footerReader = new FileFooterReader( filePath.LocalPath );

            return new CsvParser<T>( reader, lineSplitter, lineReader, headerReader, footerReader, config );
        }

        public static CsvParser<T> GetInstance<T>( Uri filePath, Action<CsvConfig> configSetup ) where T : class, new()
        {
            var config = new CsvConfig();
            configSetup.Invoke( config );

            return GetInstance<T>( filePath, config );
        }

        public static CsvParser<T> GetInstance<T>( TextReader reader, CsvConfig config ) where T : class, new()
        {
            var lineSplitter = GetLineSplitter( config );
            var lineReader = GetLineReader( config );
            var headerReader = new DefaultHeaderReader( reader );
            var footerReader = new DefaultFooterReader( reader );

            return new CsvParser<T>( reader, lineSplitter, lineReader, headerReader, footerReader, config );
        }

        public static CsvParser<T> GetInstance<T>( TextReader reader, Action<CsvConfig> configSetup ) where T : class, new()
        {
            var config = new CsvConfig();
            configSetup.Invoke( config );

            return GetInstance<T>( reader, config );
        } 
        #endregion

        #region MultiRecord CSV

        public static CsvMultiRecordParser GetMultiRecordInstance( string content, IMultiRecordSelector selector, CsvConfig config )
        {
            var reader = new StringReader( content );
            return GetMultiRecordInstance( reader, selector, config );
        }

        public static CsvMultiRecordParser GetMultiRecordInstance( string content, IMultiRecordSelector selector, Action<CsvConfig> configSetup )
        {
            var config = new CsvConfig();
            configSetup.Invoke( config );

            return GetMultiRecordInstance( content, selector, config );
        }

        public static CsvMultiRecordParser GetMultiRecordInstance( string content, Func<string, Type> selector, Action<CsvConfig> configSetup )
        {
            var config = new CsvConfig();
            configSetup.Invoke( config );

            var relaySelector = new MultiRecordRelaySelector( selector );
            return GetMultiRecordInstance( content, relaySelector, config );
        }

        public static CsvMultiRecordParser GetMultiRecordInstance( string content, Func<string, Type> selector, CsvConfig config )
        {
            var reader = new StringReader( content );
            var relaySelector = new MultiRecordRelaySelector( selector );

            return GetMultiRecordInstance( reader, relaySelector, config );
        }


        public static CsvMultiRecordParser GetMultiRecordInstance( Uri filePath, IMultiRecordSelector selector, CsvConfig config )
        {
            //We are gonna open a StreamReader on a file so we are responsible of disposing it
            config.DisposeReader = true;

            var lineSplitter = GetLineSplitter( config );
            var lineReader = GetLineReader( config );

            var reader = new StreamReader( filePath.LocalPath, config.Encoding );

            var headerReader = new FileHeaderReader( filePath.LocalPath );
            var footerReader = new FileFooterReader( filePath.LocalPath );

            return new CsvMultiRecordParser( reader, lineSplitter, lineReader, headerReader, footerReader, selector, config );
        }

        public static CsvMultiRecordParser GetMultiRecordInstance( Uri filePath, IMultiRecordSelector selector, Action<CsvConfig> configSetup )
        {
            var config = new CsvConfig();
            configSetup.Invoke( config );

            return GetMultiRecordInstance( filePath, selector, config );
        }

        public static CsvMultiRecordParser GetMultiRecordInstance( Uri filePath, Func<string, Type> selector, CsvConfig config )
        {
            var relaySelector = new MultiRecordRelaySelector( selector );
            return GetMultiRecordInstance( filePath, relaySelector, config );
        }

        public static CsvMultiRecordParser GetMultiRecordInstance( Uri filePath, Func<string, Type> selector, Action<CsvConfig> configSetup )
        {
            var config = new CsvConfig();
            configSetup.Invoke( config );

            var relaySelector = new MultiRecordRelaySelector( selector );

            return GetMultiRecordInstance( filePath, relaySelector, config );
        }


        public static CsvMultiRecordParser GetMultiRecordInstance( TextReader reader, IMultiRecordSelector selector, CsvConfig config )
        {
            var lineSplitter = GetLineSplitter( config );
            var lineReader = GetLineReader( config );

            var headerReader = new DefaultHeaderReader( reader );
            var footerReader = new DefaultFooterReader( reader );

            return new CsvMultiRecordParser( reader, lineSplitter, lineReader, headerReader, footerReader, selector, config );
        }

        public static CsvMultiRecordParser GetMultiRecordInstance( TextReader reader, IMultiRecordSelector selector, Action<CsvConfig> configSetup )
        {
            var config = new CsvConfig();
            configSetup.Invoke( config );

            return GetMultiRecordInstance( reader, selector, config );
        }

        public static CsvMultiRecordParser GetMultiRecordInstance( TextReader reader, Func<string, Type> selector, CsvConfig config )
        {
            var relaySelector = new MultiRecordRelaySelector( selector );
            return GetMultiRecordInstance( reader, relaySelector, config );
        }

        public static CsvMultiRecordParser GetMultiRecordInstance( TextReader reader, Func<string, Type> selector, Action<CsvConfig> configSetup )
        {
            var config = new CsvConfig();
            configSetup.Invoke( config );

            var relaySelector = new MultiRecordRelaySelector( selector );

            return GetMultiRecordInstance( reader, relaySelector, config );
        }

        #endregion

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
