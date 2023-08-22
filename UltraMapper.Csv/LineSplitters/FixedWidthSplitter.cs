using System;
using System.Linq;
using System.Reflection;
using UltraMapper.Csv.Internals;
using UltraMapper.Csv.LineSplitters;

namespace UltraMapper.Csv
{
    public class FixedWidthLineSplitter : ILineSplitter
    {
        private readonly int[] _fieldWidths;

        public FixedWidthLineSplitter( int[] fieldWidths )
        {
            if( fieldWidths == null || fieldWidths.Length == 0 )
                throw new ArgumentException( "Field widths not provided or empty" );

            _fieldWidths = fieldWidths;
        }

        public string[] Split( string line )
        {
            return line.Split( _fieldWidths ).ToArray();
        }
    }

    public class FixedWidthLineSplitter<TRecord> : FixedWidthLineSplitter
    {
        public FixedWidthLineSplitter()
            : base( GetFieldWidths( typeof( TRecord ) ) ) { }

        private static int[] GetFieldWidths( Type type )
        {
            var properties = type.GetProperties(
                    BindingFlags.Public | BindingFlags.Instance )
                .Select( p => p.GetCustomAttribute<FixedWidthFieldReadOptionsAttribute>() )
                .Where( p => p != null );

            return properties.OrderBy( p => p.Order )
                .Where( p => p.FieldLength > -1 )
                .Select( p => p.FieldLength )
                .ToArray();
        }
    }
}