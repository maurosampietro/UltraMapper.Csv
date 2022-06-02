using System;
using System.IO;
using System.Linq;
using UltraMapper.Conventions;
using UltraMapper.Csv.Config.FieldOptions;
using UltraMapper.Csv.UltraMapper.Extensions.Write;
using UltraMapper.MappingExpressionBuilders;
using static UltraMapper.Csv.FixedWidthFieldWriteOptionsAttribute;

namespace UltraMapper.Csv.FileFormats.FixedWidth
{
    public class FixedWidthWriter<TRecord> : DataFileWriter<TRecord, FixedWidthRecordWriteObject>
    {
        public FieldOptionsProvider<TRecord, FixedWidthFieldWriteOptionsAttribute> FieldConfig { get; }

        public FixedWidthWriter( TextWriter writer ) : base( writer )
        {
            var targetMemberProvider = Mapper.Config.Conventions.OfType<DefaultConvention>().Single().TargetMemberProvider;
            this.FieldConfig = new FieldOptionsProvider<TRecord, FixedWidthFieldWriteOptionsAttribute>( targetMemberProvider );

            Mapper.Config.Mappers.AddBefore<ReferenceMapper>( new ObjectToFixedWidthRecord( Mapper.Config ) );
        }

        public void WriteHeader()
        {
            var fieldNames = this.FieldConfig.Fields
                .Where( f => !f.IsIgnored )
                .OrderBy( f => f.Order )
                .Select( f => f.Name.Pad( f.PadSide, f.FieldLength, f.PadChar ) );

            _writer.WriteLine( String.Join( String.Empty, fieldNames ) );
        }
    }

    internal static class StringExtensions
    {
        internal static string Pad( this string text, PadSides padSide, int totalWidth, char paddingChar )
        {
            if( padSide == PadSides.LEFT )
                return text.PadLeft( totalWidth, paddingChar );

            return text.PadRight( totalWidth, paddingChar );
        }
    }
}
