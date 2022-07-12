using System;
using System.Linq.Expressions;
using System.Reflection;

namespace UltraMapper.Csv.UltraMapper.Extensions.PreprocessOptions
{
    public class TrimCharPreProess : IPreProcessOption
    {
        private static readonly MethodInfo _trimMethod =
            typeof( string ).GetMethod( nameof( String.Trim ), new Type[] { typeof( char[] ) } );

        public bool CanExecute( PropertyInfo targetMember, CsvReadOptionsAttribute options )
        {
            return options != null && options.TrimChar != '\0';
        }

        public Expression Execute( PropertyInfo targetMember, CsvReadOptionsAttribute options, Expression source )
        {
            return Expression.Call( source, _trimMethod,
                Expression.Constant( new[] { options.TrimChar } ) );
        }
    }
}
