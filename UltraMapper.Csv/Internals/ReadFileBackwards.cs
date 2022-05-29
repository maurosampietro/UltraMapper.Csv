using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UltraMapper.Csv.Internals
{
    internal static class ReadFileBackwards
    {
        /// <summary>
        /// The file is actually read from the end.
        /// The first line read is the last line in the file.
        /// This is not a File.GetLines().Reverse().
        /// </summary>
        public static IEnumerable<string> GetLines( string filePath, Encoding encoding )
        {
            using( var reader = File.OpenRead( filePath ) )
            {
                long lineEndPosition = reader.Length;
                long lineStartPosition = -1;

                bool endPosFound = false;
                bool startPosFound = false;

                for( long i = reader.Length - 1; i > 0; i-- )
                {
                    reader.Seek( i, SeekOrigin.Begin );

                    int buffer = reader.ReadByte();
                    if( buffer == 10 || buffer == 13 )
                    {
                        if( endPosFound && !startPosFound )
                        {
                            lineStartPosition = i + 1;
                            startPosFound = true;
                        }

                        continue;
                    }

                    if( !endPosFound )
                    {
                        lineEndPosition = i + 1;
                        endPosFound = true;
                    }

                    if( endPosFound && startPosFound )
                    {
                        byte[] lineBytes = new byte[ (int)lineEndPosition - lineStartPosition ];

                        reader.Seek( lineStartPosition, SeekOrigin.Begin );
                        reader.Read( lineBytes, 0, (int)lineEndPosition - (int)lineStartPosition );

                        string line = encoding.GetString( lineBytes );
                        yield return line;

                        reader.Seek( lineStartPosition, SeekOrigin.Begin );

                        endPosFound = false;
                        startPosFound = false;
                    }
                }
            }
        }
   }
}
