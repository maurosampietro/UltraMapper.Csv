using System;
using System.Collections.Generic;
using System.IO;
using UltraMapper.Conventions;
using UltraMapper.Csv.UltraMapper.Extensions.Write;
using UltraMapper.Internals;

namespace UltraMapper.Csv.FileFormats
{
    public abstract class DataFileWriter<TRecord, TWriteObject>
        where TWriteObject : IRecordWriteAdapter, new()
    {
        protected readonly TextWriter _writer;
        protected readonly TWriteObject _writingObject;
        private UltraMapperDelegate _mapFunction;

        public DataFileWriter( TextWriter writer )
        {
            _writer = writer;
            _writingObject = new TWriteObject();
        }

        public static Mapper Mapper = new Mapper( cfg =>
        {
            cfg.IsReferenceTrackingEnabled = false;
            cfg.ReferenceBehavior = ReferenceBehaviors.CREATE_NEW_INSTANCE;

            cfg.Conventions.GetOrAdd<DefaultConvention>( rule =>
            {
                rule.SourceMemberProvider.IgnoreFields = true;
                rule.SourceMemberProvider.IgnoreMethods = true;
                rule.SourceMemberProvider.IgnoreNonPublicMembers = true;

                rule.TargetMemberProvider.IgnoreFields = true;
                rule.TargetMemberProvider.IgnoreMethods = true;
                rule.TargetMemberProvider.IgnoreNonPublicMembers = true;
            } );
        } );

        //public void WriteFooter( string text ) { }

        public void WriteRecord( TRecord record )
        {
            this.WriteRecords( new[] { record } );
        }

        public void WriteRecords( IEnumerable<TRecord> records )
        {
            if( _mapFunction == null )
                _mapFunction = Mapper.Config[ typeof( TRecord ), typeof( TWriteObject ) ].MappingFunc;

            foreach( var record in records )
            {
                _writingObject.RecordBuilder.Clear();
                _mapFunction( null, record, _writingObject );
                _writer.WriteLine( _writingObject.RecordBuilder.ToString() );
            }
        }
    }
}
