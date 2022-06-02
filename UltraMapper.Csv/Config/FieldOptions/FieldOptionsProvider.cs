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
        where TFieldConfig : Attribute, IFieldConfig, new()
    {
        private readonly Dictionary<MemberInfo, TFieldConfig> _fieldOptions;
        
        public IEnumerable<TFieldConfig> Fields => _fieldOptions.Values;

        public FieldOptionsProvider( IMemberProvider memberProvider )
        {
            _fieldOptions = memberProvider.GetMembers( typeof( TRecord ) )
                  .ToDictionary( m => m, v => v.GetCustomAttribute<TFieldConfig>() ?? new TFieldConfig() );

            foreach( var item in _fieldOptions )
            {
                if( String.IsNullOrWhiteSpace( item.Value.Name ) )
                    item.Value.Name = item.Key.Name; //member name
            }
        }

        public void Configure( PropertyInfo pi, Action<TFieldConfig> fieldConfig )
        {
            if( !_fieldOptions.TryGetValue( pi, out var fieldConfiguration ) )
                throw new ArgumentException( $"'{pi.Name}' is a not a property of the type '{typeof( TRecord ).Name}'" );

            fieldConfig?.Invoke( fieldConfiguration );
        }

        public void Configure( string propertyName, Action<TFieldConfig> fieldConfig )
        {
            var pi = _fieldOptions.FirstOrDefault( m => m.Key.Name == propertyName ).Key;
            if( pi == null )
                throw new ArgumentException( $"'{propertyName}' is a not a property of the type '{typeof( TRecord ).Name}'" );

            Configure( (PropertyInfo)pi, fieldConfig );
        }

        public void Configure<TPropertyType>( Expression<Func<TRecord, TPropertyType>> selectProperty, Action<TFieldConfig> fieldConfig )
        {
            var member = selectProperty.GetMemberAccessPath().Last();
            if( member is PropertyInfo pi )
                Configure( pi, fieldConfig );
            else
                throw new ArgumentException( $"'{member.Name}' is not a property, of the type '{typeof( TRecord ).Name}'" );
        }
    }
}
