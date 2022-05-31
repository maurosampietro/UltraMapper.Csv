using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UltraMapper.Conventions;
using UltraMapper.Internals;

namespace UltraMapper.Csv.Config.FieldOptions
{
    public class FieldOptionsProvider<TRecord, TFieldConfig>
        where TFieldConfig : Attribute, new()
    {
        private readonly IMemberProvider _memberProvider;
        private readonly Dictionary<MemberInfo, TFieldConfig> _fieldOptions;

        public FieldOptionsProvider( IMemberProvider memberProvider )
        {
            _memberProvider = memberProvider;
            _fieldOptions = memberProvider.GetMembers( typeof( TRecord ) )
                  .ToDictionary( m => m, v => v.GetCustomAttribute<TFieldConfig>() ?? new TFieldConfig() );
        }

        public void Configure( PropertyInfo pi, Action<TFieldConfig> fieldConfig )
        {
            if( !_fieldOptions.TryGetValue( pi, out var fieldConfiguration ) )
                throw new ArgumentException( $"'{pi.Name}' is a no a member of the type '{typeof( TRecord ).Name}'" );
            
            fieldConfig?.Invoke( fieldConfiguration );
        }

        public void Configure( string propertyName, Action<TFieldConfig> fieldConfig )
        {
            var pi = _fieldOptions.FirstOrDefault( m => m.Key.Name == propertyName ).Key;
            if( pi == null )
                throw new ArgumentException( $"'{propertyName}' is a no a member of the type '{typeof( TRecord ).Name}'" );

            Configure( (PropertyInfo)pi, fieldConfig );
        }

        public void Configure( Expression<Func<TRecord, object>> selectProperty, Action<TFieldConfig> fieldConfig )
        {
            var member = selectProperty.GetMemberAccessPath().Last();
            Configure( (PropertyInfo)member, fieldConfig );
        }
    }
}
