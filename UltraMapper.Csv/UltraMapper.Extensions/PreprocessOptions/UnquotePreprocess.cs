using System;
using System.Linq.Expressions;
using System.Reflection;
using UltraMapper.Csv.Internals;
using UltraMapper.MappingExpressionBuilders;

namespace UltraMapper.Csv.UltraMapper.Extensions.PreprocessOptions
{
    public class UnquotePreprocess : IPreProcessOption
    {
        public static Expression<Func<string, string>> _unquote =
            str => str.Trim().Unquote( '"' );

        public bool CanExecute( Mapper mapper, ReferenceMapperContext context, PropertyInfo targetMember, CsvFieldOptionsAttribute options )
        {
            return options?.Unquote == true;
        }

        public Expression Execute( Mapper mapper, ReferenceMapperContext context, PropertyInfo targetMember, CsvFieldOptionsAttribute options, Expression source )
        {
            return Expression.Invoke( _unquote, source );
        }
    }
}
