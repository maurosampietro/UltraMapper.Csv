using System;
using System.Linq.Expressions;
using System.Reflection;
using UltraMapper.Csv.Internals;

namespace UltraMapper.Csv.UltraMapper.Extensions.PreprocessOptions
{
    public class UnescapePreprocess : IPreProcessOption
    {
        private static readonly Expression<Func<string, string>> _unescapeQuotesExp =
            str => str.UnescapeQuotes( '"' );

        public bool CanExecute( PropertyInfo targetMember, CsvReadOptionsAttribute options )
        {
            return options?.Unquote == true;
        }

        public Expression Execute( PropertyInfo targetMember, CsvReadOptionsAttribute options, Expression source )
        {
            return Expression.Invoke( _unescapeQuotesExp, source );
        }
    }
}
