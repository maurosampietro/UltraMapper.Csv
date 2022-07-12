﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UltraMapper.Conventions;
using UltraMapper.Csv.Config.FieldOptions;
using UltraMapper.Csv.Internals;
using UltraMapper.Csv.UltraMapper.Extensions.PreprocessOptions;
using UltraMapper.Internals;
using UltraMapper.MappingExpressionBuilders;

namespace UltraMapper.Csv.UltraMapper.Extensions.Read.Csv
{
    internal class CsvRecordToObjectMapper : ReferenceMapper
    {
        private static readonly List<IPreProcessOption> _preprocessOptions;

        static CsvRecordToObjectMapper()
        {
            var temp = Assembly.GetExecutingAssembly().GetTypes()
                .Where( t => t.ImplementsInterface( typeof( IPreProcessOption ) ) ).ToList();

            _preprocessOptions = temp
            .Select( t => (IPreProcessOption)InstanceFactory.CreateObject( t ) ).ToList();
        }

        public CsvRecordToObjectMapper( Configuration mappingConfiguration )
            : base( mappingConfiguration ) { }

        public override bool CanHandle( Type source, Type target )
        {
            return source == typeof( CsvRecordReadObject );
        }

        public override LambdaExpression GetMappingExpression( Type source, Type target, IMappingOptions options )
        {
            var context = this.GetMapperContext( source, target, options );
            var targetMembers = this.SelectTargetMembers( target )
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

        private readonly Expression<Func<string, string, string, string, string>> _getErrorExp =
            ( error, fieldErrorValue, memberName, memberType ) => String.Format( error, fieldErrorValue, memberName, memberType );

        protected IEnumerable<Expression> GetAssignments( PropertyInfo[] targets, Expression dataArray, ReferenceMapperContext context )
        {
            string errorMsg = "Value '{0}' not assignable to param '{1}' of type {2}";

            for( int i = 0; i < targets.Length; i++ )
            {
                var targetMember = targets[ i ];
                var inOptions = targetMember.GetCustomAttribute<CsvReadOptionsAttribute>();


                var arrayAccess = (Expression)Expression.ArrayAccess( dataArray, Expression.Constant( i ) );

                foreach( var item in _preprocessOptions )
                {
                    if( item.CanExecute( targetMember, inOptions ) )
                        arrayAccess = item.Execute( targetMember, inOptions, arrayAccess );
                }

                var mappingExpression = MapperConfiguration[ typeof( string ),
                    targetMember.PropertyType ].MappingExpression;

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

                    var convertExp = _mapper.Config[ typeof( string ), targetMember.PropertyType ].MappingExpression;

                    assignment = Expression.IfThenElse
                    (
                        Expression.IsTrue( stringNullOrWhiteSpaceExp ),

                        Expression.Assign( Expression.Property( context.TargetInstance, targetMember ),
                            Expression.Invoke( convertExp, Expression.Constant( inOptions.FillInValue.ToString() ) ) ),

                        Expression.Assign( Expression.Property(
                            context.TargetInstance, targetMember ), expression )
                    );
                }

                var exceptionParam = Expression.Parameter( typeof( Exception ), "exception" );
                var ctor = typeof( ArgumentException )
                    .GetConstructor( new Type[] { typeof( string ), typeof( Exception ) } );

                var getErrorMsg = Expression.Invoke
                (
                    _getErrorExp,
                    Expression.Constant( errorMsg ),
                    arrayAccess,
                    Expression.Constant( targetMember.Name ),
                    Expression.Constant( targetMember.PropertyType.Name )
                );

                yield return Expression.TryCatch
                (
                    Expression.Block( typeof( void ), assignment ),

                    Expression.Catch( exceptionParam, Expression.Throw
                    (
                        Expression.New( ctor, getErrorMsg, exceptionParam ),
                        typeof( void )
                    ) )
                );
            }
        }

        protected IEnumerable<MemberInfo> SelectTargetMembers( Type targetType )
        {
            var targetMemberProvider = _mapper.Config.Conventions
              .OfType<DefaultConvention>().Single().SourceMemberProvider;

            return targetMemberProvider.GetMembers( targetType )
                .Select( ( m, index ) => new
                {
                    Member = m,
                    Options = m.GetCustomAttribute<CsvReadOptionsAttribute>() ??
                        new CsvReadOptionsAttribute() {/*Order = index*/ }
                } )
                .Where( m => !m.Options.IsIgnored )
                .OrderByDescending( info => info.Options.IsRequired )
                .ThenBy( info => info.Options.Order )
                .Select( m => m.Member );
        }
    }
}
