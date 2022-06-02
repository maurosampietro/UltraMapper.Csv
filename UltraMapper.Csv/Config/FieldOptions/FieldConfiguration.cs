using System;
using System.Collections.Generic;

namespace UltraMapper.Csv.Config.FieldOptions
{
    public static class FieldConfiguration
    {
        private static readonly Dictionary<Type, object> _fieldOptions = new Dictionary<Type, object>();

        public static void Register<TRecord>( object instance )
        {
            _fieldOptions.Add( typeof( TRecord ), instance );
        }

        public static FieldOptionsProvider<TFieldConfig> Get<TFieldConfig>( Type type )
            where TFieldConfig : Attribute, IFieldConfig, new()
        {
            return _fieldOptions.TryGetValue( type, out object value )
                ? (FieldOptionsProvider<TFieldConfig>)value : null;
        }
    }
}
