using System;
using System.Collections.Generic;

namespace UltraMapper.Csv.Config.FieldOptions
{
    /// <summary>
    /// NON VA BENE QUEL COSO QUA!!!
    /// </summary>
    public static class FieldConfiguration
    {
        private static readonly Dictionary<Type, object> _fieldOptions = new Dictionary<Type, object>();

        public static void Register<TRecord>( object instance )
        {
            //noooooo!!!! USARE CON GET OR ADD CON FACTORY (PER USARE SEMPRE LA STESSA ALMENO SE NO NON VIENE AGGIUNTA E BASTA)
            if( !_fieldOptions.ContainsKey( typeof( TRecord ) ) )
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
