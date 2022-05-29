using System;

namespace UltraMapper.Csv.FileFormats.Delimited.Mode
{
    public interface IMultiRecordSelector
    {
        Type SelectType( string record );
    }
}
