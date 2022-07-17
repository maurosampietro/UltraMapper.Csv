using System;
using System.Linq.Expressions;
using System.Reflection;
using UltraMapper.Csv.Config.FieldOptions;
using UltraMapper.MappingExpressionBuilders;

namespace UltraMapper.Csv.UltraMapper.Extensions.PreprocessOptions
{
    public class DateFormatPreprocess : IPreProcessOption
    {
        private static readonly Expression<Func<string, string, DateTime>> _parseDateTimeFormatExp =
            ( str, format ) => DateTime.ParseExact( str, format, null );

        public bool CanExecute( Mapper mapper, ReferenceMapperContext context, PropertyInfo targetMember, CsvFieldOptionsAttribute options )
        {
            return targetMember.PropertyType == typeof( DateTime ) &&
                !String.IsNullOrWhiteSpace( options.Format );
        }

        public Expression Execute( Mapper mapper, ReferenceMapperContext context, PropertyInfo targetMember, CsvFieldOptionsAttribute options, Expression source )
        {
            var memberOptions = FieldConfiguration.Get<CsvFieldOptionsAttribute>( targetMember.DeclaringType )
                .FieldOptions[ targetMember ];

            return Expression.Invoke( _parseDateTimeFormatExp,
                source, Expression.Constant( memberOptions.Format ) );
        }
    }
}
