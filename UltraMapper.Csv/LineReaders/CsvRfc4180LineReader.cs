using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UltraMapper.Csv.LineReaders
{
    /// <summary>
    /// Returns a line from the reader.
    /// New lines are allowed within double quotes.
    /// A line ends when a newline-char is found outside double quotes.
    /// </summary>
    public class CsvRfc4180LineReader : ILineReader
    {
        /*
         * This LineReader needs to keep track of the reading status
         * across multiple calls to the ReadLine method to work correctly.
         * 
         * Since a single instance of this class can be used on multiple readers,
         * we need to keep track of each reader's status.
         */

        private const char CARRIAGE_RETURN = '\r';
        private const char LINE_FEED = '\n';
        private const char QUOTE_CHAR = '"';

        private readonly StringBuilder _lineBuilder = new StringBuilder();
        private readonly Dictionary<TextReader, ReadingStatus> _readStatus =
            new Dictionary<TextReader, ReadingStatus>();

        private class ReadingStatus
        {
            public char[] buffer = new char[ 8192 ];
            public int bufPos = 0;
            public int bufReadBytes = -1;
        }

        public string ReadLine( TextReader reader )
        {
            if( !_readStatus.TryGetValue( reader, out ReadingStatus status ) )
                _readStatus.Add( reader, status = new ReadingStatus() );

            _lineBuilder.Clear();
            bool quote = false;

            while( true )
            {
                if( status.bufPos > status.bufReadBytes - 1 )
                {
                    status.bufReadBytes = reader.ReadBlock( status.buffer, 0, status.buffer.Length );
                    status.bufPos = 0;
                }

                if( status.bufReadBytes <= 0 )
                {
                    _readStatus.Remove( reader );

                    if( _lineBuilder.Length > 0 )
                        return _lineBuilder.ToString();

                    return null;
                }

                for( ; status.bufPos < status.bufReadBytes; status.bufPos++ )
                {
                    if( status.buffer[ status.bufPos ] == CARRIAGE_RETURN )
                    {
                        if( quote )
                            _lineBuilder.Append( CARRIAGE_RETURN );

                        if( status.bufPos + 1 < status.bufReadBytes && status.buffer[ status.bufPos + 1 ] == LINE_FEED )
                        {
                            if( quote )
                                _lineBuilder.Append( LINE_FEED );

                            status.bufPos++;
                        }

                        if( !quote )
                        {
                            status.bufPos++;
                            return _lineBuilder.ToString();
                        }
                    }
                    else if( status.buffer[ status.bufPos ] == LINE_FEED )
                    {
                        if( quote )
                            _lineBuilder.Append( LINE_FEED );
                        else
                        {
                            status.bufPos++;
                            return _lineBuilder.ToString();
                        }
                    }
                    else
                    {
                        _lineBuilder.Append( status.buffer[ status.bufPos ] );
                        if( status.buffer[ status.bufPos ] == QUOTE_CHAR )
                            quote = !quote;
                    }
                }
            }
        }
    }
}
