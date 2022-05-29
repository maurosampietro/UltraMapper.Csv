using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UltraMapper.Conventions;
using UltraMapper.Internals;
using UltraMapper.MappingExpressionBuilders;

namespace UltraMapper.Csv.UltraMapper.Extensions.Write
{
    internal class ObjectToFixedWidthRecord : ReferenceMapper
    {
        private readonly SourceMemberProvider _sourceMemberProvider = new SourceMemberProvider()
        {
            IgnoreFields = true,
            IgnoreMethods = true,
            IgnoreNonPublicMembers = true,
        };

        public ObjectToFixedWidthRecord( Configuration mappingConfiguration )
            : base( mappingConfiguration ) { }

        public override bool CanHandle( Type source, Type target )
        {
            return !source.IsEnumerable() && target == typeof( CsvWritingString );
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

        readonly Expression<Action<CsvWritingString, string, bool>> _appendText = ( sb, text, addDelimiter ) => AppendText( sb, text, addDelimiter );
        private static void AppendText( CsvWritingString sb, string text, bool addDelimiter )
        {
            sb.CsvRecordString.Append( text );

            if( addDelimiter )
                sb.CsvRecordString.Append( sb.Delimiter );
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
                        Expression.Constant( i != targetMembers.Length - 1, typeof( bool ) ) );
                }
            }
        }

        protected MemberInfo[] SelectSourceMembers( Type sourceType )
        {
            return _sourceMemberProvider.GetMembers( sourceType )
                .Select( ( m, index ) => new
                {
                    Member = m,
                    Options = m.GetCustomAttribute<FixedWidthFieldWriteOptionsAttribute>() ??
                            new FixedWidthFieldWriteOptionsAttribute() {/*Order = index*/ }
                } )
                .Where( m => !m.Options.IsIgnored )
                .OrderBy( info => info.Options.Order )
                .Select( m => m.Member )
                .ToArray();
        }
    }
}
