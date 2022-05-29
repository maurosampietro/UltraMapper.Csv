using System;
using System.IO;
using UltraMapper.Csv.Config.DataFileParserConfig;
using UltraMapper.Csv.Header.HeaderReaders;
using UltraMapper.Csv.LineReaders;
using UltraMapper.Csv.ParsableLineRules;

namespace UltraMapper.Csv.Factories
{
    public class FixedWidthFactory
    {
        public static FixedWidthParser<T> GetInstance<T>( string content, DataFileParserConfiguration config )
            where T : class, new()
        {
            var reader = new StringReader( content );
            return GetInstance<T>( reader, config );
        }

        public static FixedWidthParser<T> GetInstance<T>( string content, Action<DataFileParserConfiguration> configSetup )
            where T : class, new()
        {
            var config = new DataFileParserConfiguration();
            configSetup.Invoke( config );

            return GetInstance<T>( content, config );
        }

        public static FixedWidthParser<T> GetInstance<T>( Uri filePath, DataFileParserConfiguration config )
            where T : class, new()
        {
            //We are gonna open a StreamReader on a file so we are responsible of disposing it
            config.DisposeReader = true;

            var lineSplitter = new FixedWidthLineSplitter<T>();
            var lineReader = GetLineReader( config );

            var reader = new StreamReader( filePath.LocalPath, config.Encoding );

            var headerReader = new FileHeaderReader( filePath.LocalPath );
            var footerReader = new FileFooterReader( filePath.LocalPath );

            return new FixedWidthParser<T>( reader, lineSplitter, lineReader, headerReader, footerReader, config );
        }

        public static FixedWidthParser<T> GetInstance<T>( Uri filePath, Action<DataFileParserConfiguration> configSetup )
            where T : class, new()
        {
            var config = new DataFileParserConfiguration();
            configSetup.Invoke( config );

            return GetInstance<T>( filePath, config );
        }

        public static FixedWidthParser<T> GetInstance<T>( TextReader reader, DataFileParserConfiguration config )
            where T : class, new()
        {
            var lineSplitter = new FixedWidthLineSplitter<T>();
            var lineReader = GetLineReader( config );
            var headerReader = new DefaultHeaderReader( reader );
            var footerReader = new DefaultFooterReader( reader );

            return new FixedWidthParser<T>( reader, lineSplitter, lineReader, headerReader, footerReader, config );
        }

        public static FixedWidthParser<T> GetInstance<T>( TextReader reader, Action<DataFileParserConfiguration> configSetup )
            where T : class, new()
        {
            var config = new DataFileParserConfiguration();
            configSetup.Invoke( config );

            return GetInstance<T>( reader, config );
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
