using System;
using System.Collections.Generic;
using UltraMapper.Conventions;

namespace UltraMapper.Csv.Config.FieldOptions
{
    public static class FieldConfiguration
    {
        private class Key
        {
            public readonly Type TFieldConfig;
            public readonly Type TRecord;

            public Key( Type tfieldconig, Type trecord )
            {
                this.TFieldConfig = tfieldconig;
                this.TRecord = trecord;
            }

            public override int GetHashCode()
            {
                return TFieldConfig.GetHashCode() ^ TRecord.GetHashCode();
            }

            public override bool Equals( object obj )
            {
                if( obj is Key otherKey )
                {
                    return TFieldConfig == otherKey.TFieldConfig &&
                        TRecord == otherKey.TRecord;
                }

                return base.Equals( obj );
            }
        }

        private static readonly Dictionary<Key, object> _fieldOptions = new Dictionary<Key, object>();

        public static FieldOptionsProvider<TFieldConfig>
            Register<TFieldConfig, TRecord>( IMemberProvider memberProvider )
            where TFieldConfig : Attribute, IFieldConfig, new()
        {
            var key = new Key( typeof( TFieldConfig ), typeof( TRecord ) );

            if( !_fieldOptions.TryGetValue( key, out object value ) )
            {
                value = new FieldOptionsProvider<TFieldConfig>( memberProvider, typeof( TRecord ) );
                _fieldOptions.Add( key, value );
            }

            return (FieldOptionsProvider<TFieldConfig>)value;
        }

        public static FieldOptionsProvider<TFieldConfig> Get<TFieldConfig>( Type type )
            where TFieldConfig : Attribute, IFieldConfig, new()
        {
            var key = new Key( typeof( TFieldConfig ), type );

            return _fieldOptions.TryGetValue( key, out object value )
                ? (FieldOptionsProvider<TFieldConfig>)value : null;
        }
    }
}
