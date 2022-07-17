using System;
using System.Linq.Expressions;
using System.Reflection;
using UltraMapper.Csv.Internals;
using UltraMapper.MappingExpressionBuilders;

namespace UltraMapper.Csv.UltraMapper.Extensions.PreprocessOptions
{
    public class UnescapePreprocess : IPreProcessOption
    {
        private static readonly Expression<Func<string, string>> _unescapeQuotesExp =
            str => str.UnescapeQuotes( '"' );

        public bool CanExecute( Mapper mapper, ReferenceMapperContext context, PropertyInfo targetMember, CsvFieldOptionsAttribute options )
        {
            return options?.Unquote == true;
        }

        public Expression Execute( Mapper mapper, ReferenceMapperContext context, PropertyInfo targetMember, CsvFieldOptionsAttribute options, Expression source )
        {
            return Expression.Invoke( _unescapeQuotesExp, source );
        }
    }
}
