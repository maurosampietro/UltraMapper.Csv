using System;
using System.Linq.Expressions;
using System.Reflection;
using UltraMapper.MappingExpressionBuilders;

namespace UltraMapper.Csv.UltraMapper.Extensions.PreprocessOptions
{
    public class TrimWhiteSpacesPreProess : IPreProcessOption
    {
        private readonly static MethodInfo _trimMethod =
            typeof( string ).GetMethod( nameof( String.Trim ), new Type[] { } );

        public bool CanExecute( Mapper mapper, ReferenceMapperContext context, PropertyInfo targetMember, CsvFieldOptionsAttribute options )
        {
            return options?.TrimWhitespaces == true;
        }

        public Expression Execute( Mapper mapper, ReferenceMapperContext context, PropertyInfo targetMember, CsvFieldOptionsAttribute options, Expression source )
        {
            return Expression.Call( source, _trimMethod );
        }
    }
}
