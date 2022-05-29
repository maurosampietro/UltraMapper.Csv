//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Text;
//using UltraMapper.Csv.Config;
//using UltraMapper.Csv.LineReaders;
//using UltraMapper.Csv.LineSplitters;
//using UltraMapper.Csv.ParsableLineRules;

//namespace UltraMapper.Csv.Factories
//{
//    internal class CsvParserElements
//    {
//        public IHeaderReader HeaderReader { get; set; }
//        public IFooterReader FooterReader { get; set; }
//        public ILineReader LineReader { get; set; }
//        public ILineSplitter LineSplitter { get; set; }

//    }

//    public class ParserFactory
//    {
//        private static ConcurrentDictionary<CsvConfig, ICsvParserElements> _cacheT
//            = new ConcurrentDictionary<CsvConfig, ICsvParserElement>();

//        public static ICsvParser<T> GetInstance<T>( CsvConfig config ) where T : class, new()
//        {
//            if( !_cacheT.TryGetValue( config, out object cachedParser ) )
//            {
//                var lineSplitter = GetLineSplitter( config );
//                var lineReader = GetLineReader( config );

//                var newParser = new CsvParserElements( lineSplitter, lineReader, config );
//                _cacheT.TryAdd( config, newParser );
//                return newParser;
//            }

//            return (ICsvParser<T>)cachedParser;
//        }

//        private static ILineReader GetLineReader( CsvConfig config )
//        {
//            var parsableLineRule = GetParsableLineRule( config );

//            ILineReader lineReader = new DefaultLineReader();
//            if( config.HasNewLinesInQuotes )
//                lineReader = new CsvRfc4180LineReader();

//            if( parsableLineRule != null )
//                lineReader = new SkipNonParsableLineReader( lineReader, parsableLineRule );

//            return lineReader;

//            //questo viene risolto quanto ho in carico il file
//            //if( this.Configuration.HasNewLinesInQuotes )
//            //{
//            //    _reader = new ExStreamReader( _originalReader );
//            //}
//            //else
//            //    _reader = _originalReader;
//        }

//        protected static IParsableLineRule GetParsableLineRule( CsvConfig config )
//        {
//            if( config.IgnoreCommentedLines && config.IgnoreEmptyLines )
//                return new IgnoreEmptyAndCommentedLine( config.CommentMarker );

//            if( config.IgnoreCommentedLines )
//                return new IgnoreCommentedLine( config.CommentMarker );

//            if( config.IgnoreEmptyLines )
//                return new IgnoreEmptyLine();

//            return null;
//        }


//        private static ILineSplitter GetLineSplitter( CsvConfig config )
//        {
//            if( config.HasDelimiterInQuotes )
//                return new CsvRfc4180DelimitedLineSplitter( config.Delimiter );

//            return new DelimiterSplitter( config.Delimiter );
//        }
//    }


//    /*
//        1. CSV generico per qualsiasi T (prende in carico parametri configurazione generali ma non per la lettura del file che cambierà di volta in volta)
//            - GetRecords() avrà come parametro di input il file e le opzioni per leggerlo

//        2 CSV specifico per un file - il costruttore prenderà in carico anche la configuraione di lettura del file
    
//    */
//}
