using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UltraMapper.Csv.Config.FieldOptions;
using UltraMapper.Csv.Internals;
using UltraMapper.Csv.UltraMapper.Extensions.Write.FixedWidth;
using UltraMapper.Internals;
using UltraMapper.MappingExpressionBuilders;

namespace UltraMapper.Csv.UltraMapper.Extensions.Write
{
    internal class ObjectToFixedWidthRecordMapper : ReferenceMapper
    {
        public ObjectToFixedWidthRecordMapper( Configuration mappingConfiguration )
            : base( mappingConfiguration ) { }

        public override bool CanHandle( Mapping mapping )
        {
            var source = mapping.Source.EntryType;
            var target = mapping.Target.EntryType;

            return !source.IsEnumerable() && target == typeof( FixedWidthRecordWriteObject );
        }

        public override LambdaExpression GetMappingExpression( Mapping mapping )
        {
            var source = mapping.Source.EntryType;
            var target = mapping.Target.EntryType;

            var context = this.GetMapperContext( mapping );
            var sourceMembers = this.SelectSourceMembers( source ).OfType<PropertyInfo>().ToArray();

            var expressions = GetTargetStrings( sourceMembers, context );
            var expression = Expression.Block( expressions );

            var delegateType = typeof( Action<,,> ).MakeGenericType(
                 context.ReferenceTracker.Type, context.SourceInstance.Type,
                 context.TargetInstance.Type );

            return Expression.Lambda( delegateType, expression,
                context.ReferenceTracker, context.SourceInstance, context.TargetInstance );
        }

        readonly Expression<Action<FixedWidthRecordWriteObject, string, FixedWidthFieldWriteOptionsAttribute>> _appendText =
            ( sb, text, options ) => AppendText( sb, text, options );

        private static void AppendText( FixedWidthRecordWriteObject sb, string text, FixedWidthFieldWriteOptionsAttribute options )
        {
            text = text.Pad( options.PadSide, options.FieldLength, options.PadChar );
            sb.RecordBuilder.Append( text );
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

                    var memberOptions = FieldConfiguration.Get<FixedWidthFieldWriteOptionsAttribute>( context.SourceInstance.Type ).FieldOptions[ item ];

                    yield return Expression.Invoke( _appendText, context.TargetInstance,
                        Expression.Invoke( toStringExp, memberAccess ),
                        Expression.Constant( memberOptions ) );
                }
            }
        }

        protected MemberInfo[] SelectSourceMembers( Type sourceType )
        {
            return FieldConfiguration.Get<FixedWidthFieldWriteOptionsAttribute>( sourceType ).FieldOptions
                .Select( kvp => new { Member = kvp.Key, Options = kvp.Value } )
                .Where( m => !m.Options.IsIgnored )
                .OrderBy( info => info.Options.Order )
                .Select( m => m.Member )
                .ToArray();
        }
    }
}
