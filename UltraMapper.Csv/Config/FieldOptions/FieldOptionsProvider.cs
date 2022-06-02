using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UltraMapper.Conventions;
using UltraMapper.Internals;

namespace UltraMapper.Csv.Config.FieldOptions
{
    public class FieldOptionsProvider<TFieldConfig>
        where TFieldConfig : Attribute, IFieldConfig, new()
    {
        private readonly Type _recordType;
        private readonly Dictionary<MemberInfo, TFieldConfig> _fieldOptions;
        public IReadOnlyDictionary<MemberInfo, TFieldConfig> FieldOptions => _fieldOptions;

        public IEnumerable<TFieldConfig> Fields => _fieldOptions.Values;

        public FieldOptionsProvider( IMemberProvider memberProvider, Type recordType )
        {
            _recordType = recordType;
            _fieldOptions = memberProvider.GetMembers( recordType )
                  .ToDictionary( m => m, v => v.GetCustomAttribute<TFieldConfig>() ?? new TFieldConfig() );

            foreach( var item in _fieldOptions )
            {
                if( String.IsNullOrWhiteSpace( item.Value.Name ) )
                    item.Value.Name = item.Key.Name; //member name
            }
        }

        public void Configure( string propertyName, Action<TFieldConfig> fieldConfig )
        {
            var fieldOptions = _fieldOptions.FirstOrDefault( m => m.Key.Name == propertyName ).Value;
            if( fieldOptions == null )
                throw new ArgumentException( $"'{propertyName}' is a not a property of the type '{_recordType.Name}'" );

            fieldConfig?.Invoke( fieldOptions );
        }

        public void Configure( PropertyInfo pi, Action<TFieldConfig> fieldConfig )
        {
            Configure( pi.Name, fieldConfig );
        }

        public void Configure<TRecord, TPropertyType>( Expression<Func<TRecord, TPropertyType>> selectProperty, Action<TFieldConfig> fieldConfig )
        {
            var member = selectProperty.GetMemberAccessPath().Last();
            if( !(member is PropertyInfo pi) )
                throw new ArgumentException( $"'{nameof( selectProperty )}' must select a property of the type '{_recordType.Name}'" );

            Configure( pi, fieldConfig );
        }
    }
}
