using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UltraMapper.Conventions;
using UltraMapper.Csv.Config.FieldOptions.Attributes;
using UltraMapper.Internals;
using UltraMapper.MappingExpressionBuilders;

namespace UltraMapper.Csv.UltraMapper.Extensions.Write
{
    internal class ObjectToCsvRecordMapper : ReferenceMapper
    {
        public ObjectToCsvRecordMapper( Configuration mappingConfiguration )
            : base( mappingConfiguration ) { }

        public override bool CanHandle( Type source, Type target )
        {
            return !source.IsEnumerable() && target == typeof( CsvRecordWriteObject );
        }

        public override LambdaExpression GetMappingExpression( Type source, Type target, IMappingOptions options )
        {
            var context = this.GetMapperContext( source, target, options );
            var sourceMembers = this.SelectSourceMembers( source ).OfType<PropertyInfo>().ToArray();

            var expressions = GetTargetStrings( sourceMembers, context );
            var expression = Expression.Block( expressions );

            var delegateType = typeof( Action<,,> ).MakeGenericType(
                 context.ReferenceTracker.Type, context.SourceInstance.Type,
                 context.TargetInstance.Type );

            return Expression.Lambda( delegateType, expression,
                context.ReferenceTracker, context.SourceInstance, context.TargetInstance );
        }

        readonly Expression<Action<CsvRecordWriteObject, string, bool>> _appendText = ( sb, text, addDelimiter ) => AppendText( sb, text, addDelimiter );
        private static void AppendText( CsvRecordWriteObject sb, string text, bool addDelimiter )
        {
            sb.RecordBuilder.Append( text );

            if( addDelimiter )
                sb.RecordBuilder.Append( sb.Delimiter );
        }

        private IEnumerable<Expression> GetTargetStrings( PropertyInfo[] targetMembers,
            ReferenceMapperContext context )
        {
            for( int i = 0; i < targetMembers.Length; i++ )
            {
                var item = targetMembers[ i ];

                if( item.PropertyType.IsBuiltIn( true ) )
                {
                    var memberAccess = Expression.Property( context.SourceInstance, item );
                    LambdaExpression toStringExp = MapperConfiguration[ item.PropertyType, typeof( string ) ].MappingExpression;

                    yield return Expression.Invoke( _appendText, context.TargetInstance,
                        Expression.Invoke( toStringExp, memberAccess ), 
                        Expression.Constant( i != targetMembers.Length - 1, typeof(bool) ) );
                }
            }
        }

        protected MemberInfo[] SelectSourceMembers( Type sourceType )
        {
            var sourceMemberProvider = _mapper.Config.Conventions
              .OfType<DefaultConvention>().Single().SourceMemberProvider;

            return sourceMemberProvider.GetMembers( sourceType )
                .Select( ( m, index ) => new
                {
                    Member = m,
                    Options = m.GetCustomAttribute<CsvWriteOptionsAttribute>() ??
                            new CsvWriteOptionsAttribute() {/*Order = index*/ }
                } )
                .Where( m => !m.Options.IsIgnored )
                .OrderBy( info => info.Options.Order )
                .Select( m => m.Member )
                .ToArray();
        }
    }
}
