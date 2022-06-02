using System;
using System.IO;
using System.Linq;
using UltraMapper.Conventions;
using UltraMapper.Csv.Config.FieldOptions;
using UltraMapper.Csv.Internals;
using UltraMapper.Csv.UltraMapper.Extensions.Write;
using UltraMapper.MappingExpressionBuilders;
using static UltraMapper.Csv.FixedWidthFieldWriteOptionsAttribute;

namespace UltraMapper.Csv.FileFormats.FixedWidth
{
    public class FixedWidthWriter<TRecord> : DataFileWriter<TRecord, FixedWidthRecordWriteObject>
        where TRecord : class
    {
        public FieldOptionsProvider<FixedWidthFieldWriteOptionsAttribute> FieldConfig { get; }

        static FixedWidthWriter()
        {
            Mapper.Config.Mappers.AddBefore<ReferenceMapper>( new ObjectToFixedWidthRecordMapper( Mapper.Config ) );
        }

        public FixedWidthWriter( TextWriter writer ) : base( writer )
        {
            var targetMemberProvider = Mapper.Config.Conventions.OfType<DefaultConvention>().Single().TargetMemberProvider;
            this.FieldConfig = new FieldOptionsProvider<FixedWidthFieldWriteOptionsAttribute>( targetMemberProvider, typeof( TRecord ) );

            FieldConfiguration.Register<TRecord>( FieldConfig );
        }

        public void WriteHeader()
        {
            var fieldNames = this.FieldConfig.Fields
                .Where( f => !f.IsIgnored )
                .OrderBy( f => f.Order )
                .Select( f => f.Name.Pad( f.HeaderPadSide, f.FieldLength, f.PadChar ) );

            _writer.WriteLine( String.Join( String.Empty, fieldNames ) );
        }
    }
}
