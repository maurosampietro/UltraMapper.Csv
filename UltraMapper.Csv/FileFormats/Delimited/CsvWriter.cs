using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UltraMapper.Conventions;
using UltraMapper.Csv.Config.FieldOptions;
using UltraMapper.Csv.UltraMapper.Extensions.Write;
using UltraMapper.MappingExpressionBuilders;

namespace UltraMapper.Csv.FileFormats.Delimited
{
    public class CsvWriter<TRecord> : DataFileWriter<TRecord, CsvRecordWriteObject>
    {
        public FieldOptionsProvider<CsvWriteOptionsAttribute> FieldConfig { get; }
        public string Delimiter { get; }

        static CsvWriter()
        {
            Mapper.Config.Mappers.AddBefore<ReferenceMapper>( new ObjectToCsvRecordMapper( Mapper.Config ) );
        }

        public CsvWriter( TextWriter writer, string delimiter ) : base( writer )
        {
            this.Delimiter = delimiter;

            var targetMemberProvider = Mapper.Config.Conventions.OfType<DefaultConvention>().Single().TargetMemberProvider;
            this.FieldConfig = new FieldOptionsProvider<CsvWriteOptionsAttribute>( targetMemberProvider, typeof( TRecord ) );

            FieldConfiguration.Register<TRecord>( this.FieldConfig );
        }

        public void WriteHeader()
        {
            var fieldNames = this.FieldConfig.Fields
                .Where( f => !f.IsIgnored )
                .OrderBy( f => f.Order )
                .Select( f => f.Name );

            _writer.WriteLine( String.Join( this.Delimiter, fieldNames ) );
        }
    }
}
