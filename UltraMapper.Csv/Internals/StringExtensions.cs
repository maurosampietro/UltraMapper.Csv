using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltraMapper.Csv.Config.FieldOptions;

namespace UltraMapper.Csv.Internals
{
    internal static class StringExtensions
    {
        public static Dictionary<char, List<int>> IndexesOf( this string str, char[] chars )
        {
            var charIndexes = new List<List<int>>( chars.Length );
            for( int i = 0; i < chars.Length; i++ )
                charIndexes.Add( new List<int>() );

            for( int i = 0; i < str.Length; i++ )
            {
                for( int k = 0; k < chars.Length; k++ )
                {
                    if( str[ i ] == chars[ k ] )
                    {
                        charIndexes[ k ].Add( i );
                        break;
                    }
                }
            }

            var dict = new Dictionary<char, List<int>>( chars.Length );
            for( int i = 0; i < chars.Length; i++ )
                dict.Add( chars[ i ], charIndexes[ i ] );

            return dict;
        }

        public static IEnumerable<int> IndexesOf( this string str, string value )
        {
            for( int index = 0; ; index += value.Length )
            {
                index = str.IndexOf( value, index );
                if( index == -1 )
                    yield break;

                yield return index;
            }
        }

        public static IEnumerable<string> Split( this string str, int[] widths )
        {
            if( widths == null || widths.Length == 0 )
                yield return str;

            for( int i = 0, w = 0; w < widths.Length; w++ )
            {
                yield return str.Substring( i, widths[ w ] );
                i += widths[ w ];
            }
        }

#if NET5_0_OR_GREATER
        public static IEnumerable<string> SplitIfNotQuoted( this string str, char delimiter )
        {
            var strSpan = str.AsSpan();
            var chunks = new List<string>();
            
            int startIndex = 0;
            bool quote = false;

            for( int i = 0; i < str.Length; i++ )
            {
                if( !quote && str[ i ] == delimiter )
                {
                    chunks.Add( strSpan[ startIndex..i ].ToString() );
                    startIndex = i + 1;
                }
                else if( str[ i ] == '"' )
                {
                    quote = !quote;
                }
            }

            if( startIndex < str.Length )
                chunks.Add( strSpan[ startIndex..str.Length ].ToString() );

            return chunks;
        }
#else

        public static IEnumerable<string> SplitIfNotQuoted( this string str, char delimiter )
        {
            //Stick with Substring. Anything involing StringBuilder is much slower.

            bool quote = false;
            int startIndex = 0;

            for( int i = 0; i < str.Length; i++ )
            {
                if( !quote && str[ i ] == delimiter )
                {
                    yield return str.Substring( startIndex, i - startIndex );
                    startIndex = i + 1;
                }
                else
                {
                    if( str[ i ] == '"' )
                        quote = !quote;
                }
            }

            //We return empty strings if a field is empty, even if it's the last field.
            //We don't remove empty entries.
            if( (startIndex != str.Length) ||
                (startIndex == str.Length && str[ str.Length - 1 ] == delimiter) )
            {
                yield return str.Substring( startIndex );
            }
        }

#endif

        public static IEnumerable<string> SplitIfNotQuoted( this string str, string delimiter )
        {
            var quotesIndexes = str.IndexesOf( "\"" ).ToList();

            ////Allow escaped quotes
            //for( int i = quotesIndexes.Count - 1; i >= 0; i-- )
            //{
            //    int quoteIndex = quotesIndexes[ i ];
            //    if( quoteIndex > 0 && str[ quoteIndex - 1 ] == '\\' )
            //        quotesIndexes.RemoveAt( i );
            //}

            //At least 2 quotes are needed to determine a range
            //within which delimiters are to be ignored
            if( quotesIndexes.Count <= 1 )
                return str.Split( new[] { delimiter }, StringSplitOptions.None );

            var delimiterIndexes = str.IndexesOf( delimiter ).ToList();
            return SplitIfNotQuotedInternal( str, delimiter, quotesIndexes, delimiterIndexes );
        }

        private static IEnumerable<string> SplitIfNotQuotedInternal( string str, string delimiter,
            List<int> quotesIndexes, List<int> delimiterIndexes )
        {
            int usefulIndexes = quotesIndexes.Count;
            //An odd number of indexes: ignore the last non-balanced quote
            if( (quotesIndexes.Count & 1) == 1 )
                usefulIndexes--;

            int lastIndex = 0;
            int currentQuote = 0;
            int quoteStartIndex = quotesIndexes[ currentQuote ];
            int quoteEndIndex = quotesIndexes[ ++currentQuote ];

            foreach( var index in delimiterIndexes )
            {
                if( index > quoteEndIndex )
                {
                    if( currentQuote < usefulIndexes - 1 )
                    {
                        quoteStartIndex = quotesIndexes[ ++currentQuote ];
                        quoteEndIndex = quotesIndexes[ ++currentQuote ];
                    }
                }

                if( index >= quoteStartIndex && index <= quoteEndIndex )
                    continue;

                yield return str.Substring( lastIndex, index - lastIndex );
                lastIndex = index + delimiter.Length;
            }

            yield return str.Substring( lastIndex );
        }

        /// <summary>
        /// Removes the first leading and the last trailing
        /// quotation-char occurence in a string which starts
        /// with a leading quotation-char and 
        /// ends with a trailing quotation-char
        /// </summary>
        /// <param name="str"></param>
        /// <param name="quoteChar"></param>
        /// <returns></returns>
        public static string Unquote( this string str, char quoteChar = '"' )
        {
            if( str == null )
                return str;

            if( str.Length < 2 ) return str;

            if( str[ 0 ] == quoteChar && str[ str.Length - 1 ] == quoteChar )
                return str.Substring( 1, str.Length - 2 );

            return str;
        }

        public static string UnescapeQuotes( this string str, char quoteChar = '"' )
        {
            string pair = new string( quoteChar, 2 );
            string single = new string( quoteChar, 1 );

            return str.Replace( pair, single );
        }

        internal static string Pad( this string str, PadSides padSide, int totalWidth, char paddingChar )
        {
            switch( padSide )
            {
                case PadSides.LEFT: return str.PadLeft( totalWidth, paddingChar );
                case PadSides.RIGHT: return str.PadRight( totalWidth, paddingChar );
                case PadSides.CENTER: return str.PadCenter( totalWidth, paddingChar );
            }

            throw new ArgumentException( $"Unsupported value '{padSide}' used for parameter '{nameof( padSide )}'" );
        }

        internal static string PadCenter( this string str, int totalWidth, char paddingChar )
        {
            int padding = totalWidth - str.Length;
            int padLeft = (padding / 2) + str.Length;

            return str.PadLeft( padLeft, paddingChar )
                .PadRight( totalWidth, paddingChar );
        }
    }
}
