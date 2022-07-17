using System;
using System.Linq.Expressions;
using System.Reflection;
using UltraMapper.MappingExpressionBuilders;

namespace UltraMapper.Csv.UltraMapper.Extensions.PreprocessOptions
{
    public class TrimCharPreProess : IPreProcessOption
    {
        private static readonly MethodInfo _trimMethod =
            typeof( string ).GetMethod( nameof( String.Trim ), new Type[] { typeof( char[] ) } );

        public bool CanExecute( Mapper mapper, ReferenceMapperContext context, PropertyInfo targetMember, CsvFieldOptionsAttribute options )
        {
            return options != null && options.TrimChar != '\0';
        }

        public Expression Execute( Mapper mapper, ReferenceMapperContext context, PropertyInfo targetMember, CsvFieldOptionsAttribute options, Expression source )
        {
            return Expression.Call( source, _trimMethod,
                Expression.Constant( new[] { options.TrimChar } ) );
        }
    }
}
