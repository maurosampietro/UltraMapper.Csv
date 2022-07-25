using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UltraMapper.Conventions;
using UltraMapper.Csv.UltraMapper.Extensions.PreprocessOptions;
using UltraMapper.Internals;
using UltraMapper.MappingExpressionBuilders;

namespace UltraMapper.Csv.UltraMapper.Extensions.Read.Csv
{
    internal class CsvRecordToObjectMapper : ReferenceMapper
    {
        private static readonly List<IPreProcessOption> _preprocessOptions = new List<IPreProcessOption>()
        {
            new UnquotePreprocess(),
            new UnescapePreprocess(),
            new TrimWhiteSpacesPreProess(),
            new TrimCharPreProess(),
            //new FillInPreProcess(),
            new DateFormatPreprocess()
        };

        public override bool CanHandle( Mapping mapping )
        {
            var source = mapping.Source.EntryType;
            return source == typeof( CsvRecordReadObject );
        }

        public override LambdaExpression GetMappingExpression( Mapping mapping )
        {
            var target = mapping.Target.EntryType;

            var context = this.GetMapperContext( mapping );
            var targetMembers = this.SelectTargetMembers( context, target )
                .OfType<PropertyInfo>().ToArray();

            var dataArray = Expression.Property( context.SourceInstance, nameof( CsvRecordReadObject.Data ) );
            var assignments = this.GetAssignments( targetMembers, dataArray, context ).ToList();
            var expression = assignments.Count > 0 ? Expression.Block( assignments )
                : (Expression)Expression.Empty();

            //By default the preprocess method is searched, even if the PreprocessAttribute is not applied.
            //You can opt-out by applying the attribute and set CustomProcess = false
            var preprocessOptions = target.GetCustomAttribute<CustomPreprocessAttribute>() ?? new CustomPreprocessAttribute();
            if( preprocessOptions?.IsEnabled == true ) //true is the default value
            {
                var method = target.GetMethod( preprocessOptions?.MethodName, new Type[] { typeof( string[] ) } );
                if( method != null )
                    expression = Expression.Block( expression, Expression.Call( context.TargetInstance, method, dataArray ) );
            }

            var delegateType = typeof( Action<,,> ).MakeGenericType(
                 context.ReferenceTracker.Type, context.SourceInstance.Type,
                 context.TargetInstance.Type );

            return Expression.Lambda( delegateType, expression,
                context.ReferenceTracker, context.SourceInstance, context.TargetInstance );
        }

        protected IEnumerable<Expression> GetAssignments( PropertyInfo[] targets, Expression dataArray, ReferenceMapperContext context )
        {
            var typeMapping = context.MapperConfiguration[ typeof( string[] ), context.TargetInstance.Type ];

            for( int i = 0; i < targets.Length; i++ )
            {
                var targetMember = targets[ i ];

                var mappingSource = new MappingSource( typeof( string ) );
                var mappingTarget = new MappingTarget( targetMember );

                var memberMapping = typeMapping.AddMemberToMemberMapping( mappingSource, mappingTarget );
                var mappingExpression = memberMapping.MappingExpression;

                var arrayAccess = (Expression)Expression.ArrayAccess( dataArray, Expression.Constant( i ) );

                var inOptions = targetMember.GetCustomAttribute<CsvFieldOptionsAttribute>();
                foreach( var item in _preprocessOptions )
                {
                    if( item.CanExecute( context.MapperInstance, context, targetMember, inOptions ) )
                        arrayAccess = item.Execute( context.MapperInstance, context, targetMember, inOptions, arrayAccess );
                }

                var expression = mappingExpression.Body;

                string paramNameToReplace = mappingExpression.Parameters
                    .First( p => p.Type != typeof( ReferenceTracker ) ).Name;

                expression = expression.ReplaceParameter(
                     arrayAccess, paramNameToReplace );

                var assignment = (Expression)Expression.Assign( Expression.Property(
                     context.TargetInstance, targetMember ), expression );

                if( inOptions?.FillInValue != null )
                {
                    var stringNullOrWhiteSpaceMethod = typeof( string ).GetMethod( nameof( String.IsNullOrWhiteSpace ) );
                    var stringNullOrWhiteSpaceExp = Expression.Call( null, stringNullOrWhiteSpaceMethod, arrayAccess );

                    var convertExp = context.MapperConfiguration[ typeof( string ), targetMember.PropertyType ].MappingExpression;

                    assignment = Expression.IfThenElse
                    (
                        Expression.IsTrue( stringNullOrWhiteSpaceExp ),

                        Expression.Assign( Expression.Property( context.TargetInstance, targetMember ),
                            Expression.Invoke( convertExp, Expression.Constant( inOptions.FillInValue.ToString() ) ) ),

                        Expression.Assign( Expression.Property(
                            context.TargetInstance, targetMember ), expression )
                    );
                }

                yield return assignment;
            }
        }

        protected IEnumerable<MemberInfo> SelectTargetMembers( ReferenceMapperContext context, Type targetType )
        {
            var targetMemberProvider = context.MapperInstance.Config.Conventions
                .OfType<DefaultConvention>().Single().SourceMemberProvider;

            return targetMemberProvider.GetMembers( targetType )
                .Select( ( m, index ) => new
                {
                    Member = m,
                    Options = m.GetCustomAttribute<CsvFieldOptionsAttribute>() ??
                        new CsvFieldOptionsAttribute() {/*Order = index*/ }
                } )
                .Where( m => !m.Options.IsIgnored )
                .OrderByDescending( info => info.Options.IsRequired )
                .ThenBy( info => info.Options.Order )
                .Select( m => m.Member );
        }
    }
}
