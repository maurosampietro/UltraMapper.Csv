using System;
using System.Linq.Expressions;
using System.Reflection;
using UltraMapper.MappingExpressionBuilders;

namespace UltraMapper.Csv.UltraMapper.Extensions.PreprocessOptions
{
    internal class FillInPreProcess : IPreProcessOption
    {
        private static readonly MethodInfo _isNullOrWhiteSpace = 
            typeof( string ).GetMethod( nameof( String.IsNullOrWhiteSpace ) );

        public bool CanExecute( Mapper mapper, ReferenceMapperContext context, PropertyInfo targetMember, CsvFieldOptionsAttribute options )
        {
            return options?.FillInValue != null;
        }

        public Expression Execute( Mapper mapper, ReferenceMapperContext context, PropertyInfo targetMember, CsvFieldOptionsAttribute options, Expression source )
        {
            var stringNullOrWhiteSpaceExp = Expression.Call( null, _isNullOrWhiteSpace, source );

            var convertExp = mapper.Config[ typeof( string ), targetMember.PropertyType ].MappingExpression;

            return Expression.IfThenElse
            (
                Expression.IsTrue( stringNullOrWhiteSpaceExp ),

                Expression.Assign( Expression.Property( context.TargetInstance, targetMember ),
                    Expression.Invoke( convertExp, Expression.Constant( options.FillInValue.ToString() ) ) ),

                Expression.Assign( Expression.Property(
                    context.TargetInstance, targetMember ), source )
            );
        }
    }
}