using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using UltraMapper.Conventions;
using UltraMapper.Csv.Config.DataFileParserConfig;
using UltraMapper.Csv.Footer;
using UltraMapper.Csv.Footer.FooterReaders;
using UltraMapper.Csv.Header;
using UltraMapper.Csv.Header.HeaderReaders;
using UltraMapper.Csv.LineReaders;
using UltraMapper.Csv.LineSplitters;
using UltraMapper.Csv.UltraMapper.Extensions.Read;
using UltraMapper.Csv.UltraMapper.Extensions.Read.Csv;
using UltraMapper.Csv.UltraMapper.Extensions.Write;
using UltraMapper.MappingExpressionBuilders;

namespace UltraMapper.Csv.FileFormats
{

    public abstract class DataFileParser<TRecord, TConfiguration> : IHeaderSupport, IFooterSupport
        where TRecord : class, new()
        where TConfiguration : IDataFileParserConfiguration
    {
        protected readonly DataRecord _dataRecord = new DataRecord();
        protected readonly TextReader _reader;

        protected Action<ReferenceTracker, object, object> _mapFunction;

        protected readonly IHeaderReader _headerReader;
        protected readonly IFooterReader _footerReader;
        protected readonly ILineReader _lineReader;
        protected readonly ILineSplitter _lineSplitter;

        protected string _firstLine;
        protected string _lastLine;

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

            cfg.Mappers.AddBefore<ReferenceMapper>( new IMappingExpressionBuilder[]
            {
                new DataRecordMapper( cfg )
            } );
        } );

        public TConfiguration Configuration { get; protected set; }

        public DataFileParser( TextReader reader,
            ILineSplitter lineSplitter,
            ILineReader lineReader,
            IHeaderReader headerReader,
            IFooterReader footerReader,
            TConfiguration options )
        {
            _reader = reader ?? throw new ArgumentNullException( nameof( reader ) );
            _lineSplitter = lineSplitter ?? throw new ArgumentNullException( nameof( lineSplitter ) );
            _lineReader = lineReader ?? throw new ArgumentNullException( nameof( lineReader ) );
            _headerReader = headerReader ?? new DefaultHeaderReader( reader );
            _footerReader = footerReader ?? new DefaultFooterReader( reader );

            this.Configuration = options;

            //this configuration depends on instance properties
            Mapper.Config.MapTypes<string, double>( str => ConvertStringToDouble( str ) );
            Mapper.Config.MapTypes<string, bool>( str => ConvertStringToBoolean( str ) );
        }

        /// <summary>
        /// Gets the raw unsplit header
        /// </summary>
        public string GetHeader()
        {
            if( !this.Configuration.HasHeader )
                throw new Exception( "This parser is configured not to have a header" );

            if( this.Configuration.HasHeader && _headerReader == null )
                throw new Exception( $"A header is expected but no '{nameof( IHeaderReader )}' has been provided" );

            if( _firstLine == null )
            {
                _firstLine = _headerReader.GetHeader( _lineReader );

                if( _firstLine == null )
                    throw new Exception( "The end of the stream has been reached before the header could be read" );
            }

            return _firstLine;
        }

        /// <summary>
        /// Gets the raw unsplit footer
        /// </summary>
        public string GetFooter()
        {
            if( !this.Configuration.HasFooter )
                throw new Exception( "This parser is configured not to have a footer" );

            if( this.Configuration.HasFooter && _footerReader == null )
                throw new Exception( $"A footer is expected but no '{nameof( IFooterReader )}' has been provided" );

            if( _lastLine == null )
            {
                _lastLine = _footerReader.GetFooter( _lineReader );

                if( _lastLine == null )
                    throw new Exception( "The end of the stream has been reached before the footer could be read" );
            }

            return _lastLine;
        }

        /// <summary>
        /// Returns an enumerable collection of records.
        /// Each record is mapped on a <typeparamref name="TRecord"/> instance.
        /// </summary>
        /// <typeparam name="TRecord"></typeparam>
        public virtual IEnumerable<TRecord> GetRecords()
        {
            _mapFunction = Mapper.Config[ typeof( DataRecord ), typeof( TRecord ) ].MappingFunc;

            if( _lastLine != null && _footerReader.IsConsumingOriginalStream )
                throw new Exception( "Cannot read the stream after the footer has been read. The end of the stream has been reached." );

            if( this.Configuration.HasHeader )
            {
                this.GetHeader();

                if( !_headerReader.IsConsumingOriginalStream )
                    _lineReader.ReadLine( _reader );
            }

            if( this.Configuration.HasFooter ) //avoid this check inside the loop to improve performance
            {
                var line1 = _lineReader.ReadLine( _reader );
                var line2 = _lineReader.ReadLine( _reader );

                while( line1 != null )
                {
                    if( line2 == null )
                    {
                        //make the footer available later when calling GetFooter method
                        _lastLine = line1;
                        yield break;
                    }
                    else
                    {
                        yield return this.MapLine( line1 );

                        line1 = line2;
                        line2 = _lineReader.ReadLine( _reader );
                    }
                }
            }
            else
            {
                var line = _lineReader.ReadLine( _reader );
                while( line != null )
                {
                    yield return this.MapLine( line );
                    line = _lineReader.ReadLine( _reader );
                }
            }
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        protected virtual TRecord MapLine( string line )
        {
            try
            {
                _dataRecord.Data = _lineSplitter.Split( line );

                //faster version
                var target = new TRecord();
                _mapFunction( null, _dataRecord, target );
                return target;

                //do not user slower versions:
                //return Mapper.Map<TRecord>( _dataRecord );
                //return (TRecord)Mapper.Map( _dataRecord, targetType: typeof( TRecord ) );
            }
            catch( Exception ex )
            {
                throw new Exception( $"Error mapping '{line}' to type {typeof( TRecord )}", ex );
            }
        }

        private double ConvertStringToDouble( string str )
        {
            if( String.IsNullOrWhiteSpace( str ) )
                return 0.0;

            return Double.Parse( str, NumberStyles.Any, Configuration.Culture );
        }

        private bool ConvertStringToBoolean( string str )
        {
            if( str == "1" ) return true;
            if( str == "0" ) return false;

            return Boolean.Parse( str );
        }

        ~DataFileParser()
        {
            if( this.Configuration.DisposeReader )
                _reader?.Dispose();
        }
    }

    public class DataFileWriter
    {
        private readonly TextWriter _writer;

        public DataFileWriter( TextWriter writer )
        {
            _writer = writer;
        }

        public void WriteRecords<TRecord>( IEnumerable<TRecord> records )
        {
            var mapper = new Mapper( cfg =>
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

                cfg.Mappers.AddBefore<ReferenceMapper>( new ObjectToCsvRecord( cfg ) );
            } );

            var ws = new CsvWritingString();

            foreach( var record in records )
            {
                mapper.Map( record, ws );
                _writer.WriteLine( ws.CsvRecordString.ToString() );
                ws.CsvRecordString.Clear();
            }
        }
    }
}
