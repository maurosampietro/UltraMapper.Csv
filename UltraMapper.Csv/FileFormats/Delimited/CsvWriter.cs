using System;
using System.IO;
using System.Linq;
using UltraMapper.Conventions;
using UltraMapper.Csv.Config.FieldOptions;
using UltraMapper.Csv.UltraMapper.Extensions.Write;
using UltraMapper.MappingExpressionBuilders;

namespace UltraMapper.Csv.FileFormats.Delimited
{
    public class CsvWriter<TRecord> : DataFileWriter<TRecord,CsvRecordWriteObject>
    {
        public FieldOptionsProvider<TRecord, CsvWriteOptionsAttribute> FieldConfig { get; }
        public string Delimiter { get; }

        public CsvWriter( TextWriter writer, string delimiter ) : base( writer )
        {
            this.Delimiter = delimiter;

            var targetMemberProvider = Mapper.Config.Conventions.OfType<DefaultConvention>().Single().TargetMemberProvider;
            this.FieldConfig = new FieldOptionsProvider<TRecord, CsvWriteOptionsAttribute>( targetMemberProvider );

            Mapper.Config.Mappers.AddBefore<ReferenceMapper>( new ObjectToCsvRecord( Mapper.Config ) );
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
