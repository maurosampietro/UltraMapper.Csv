using System;
using System.Linq.Expressions;
using System.Reflection;

namespace UltraMapper.Csv.UltraMapper.Extensions.PreprocessOptions
{
    public class TrimWhiteSpacesPreProess : IPreProcessOption
    {
        private readonly static MethodInfo _trimMethod =
            typeof( string ).GetMethod( nameof( String.Trim ), new Type[] { } );

        public bool CanExecute( PropertyInfo targetMember, CsvReadOptionsAttribute options )
        {
            return options?.TrimWhitespaces == true;
        }

        public Expression Execute( PropertyInfo targetMember, CsvReadOptionsAttribute options, Expression source )
        {
            return Expression.Call( source, _trimMethod );
        }
    }
}
