using System;

namespace UltraMapper.Csv.FileFormats.Delimited.Mode
{
    public class MultiRecordRelaySelector : IMultiRecordSelector
    {
        private readonly Func<string, Type> _typeSelector;

        public MultiRecordRelaySelector( Func<string, Type> typeSelector )
        {
            _typeSelector = typeSelector;
        }

        public Type SelectType( string record ) => _typeSelector( record );
    }
}
