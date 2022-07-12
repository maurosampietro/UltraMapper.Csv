using System;
using System.Linq.Expressions;
using System.Reflection;
using UltraMapper.Csv.Internals;

namespace UltraMapper.Csv.UltraMapper.Extensions.PreprocessOptions
{
    public class UnquotePreprocess : IPreProcessOption
    {
        public static Expression<Func<string, string>> _unquote =
            str => str.Trim().Unquote( '"' );

        public bool CanExecute( PropertyInfo targetMember, CsvReadOptionsAttribute options )
        {
            return options?.Unquote == true;
        }

        public Expression Execute( PropertyInfo targetMember, CsvReadOptionsAttribute options, Expression source )
        {
            return Expression.Invoke( _unquote, source );
        }
    }
}
