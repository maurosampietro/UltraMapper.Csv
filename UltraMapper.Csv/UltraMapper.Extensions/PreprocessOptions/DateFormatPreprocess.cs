using System;
using System.Linq.Expressions;
using System.Reflection;
using UltraMapper.Csv.Config.FieldOptions;

namespace UltraMapper.Csv.UltraMapper.Extensions.PreprocessOptions
{
    public class DateFormatPreprocess : IPreProcessOption
    {
        private static readonly Expression<Func<string, string, DateTime>> _parseDateTimeFormatExp =
            ( str, format ) => DateTime.ParseExact( str, format, null );

        public bool CanExecute( PropertyInfo targetMember, CsvReadOptionsAttribute options )
        {
            return targetMember.PropertyType == typeof( DateTime ) &&
                !String.IsNullOrWhiteSpace( options.Format );
        }

        public Expression Execute( PropertyInfo targetMember, CsvReadOptionsAttribute options, Expression source )
        {
            var memberOptions = FieldConfiguration.Get<CsvReadOptionsAttribute>( targetMember.DeclaringType )
                .FieldOptions[ targetMember ];

            return Expression.Invoke( _parseDateTimeFormatExp,
                source, Expression.Constant( memberOptions.Format ) );
        }
    }
}
