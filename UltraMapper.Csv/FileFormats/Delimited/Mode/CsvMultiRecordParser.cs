using System;
using System.IO;
using UltraMapper.Csv.Config;
using UltraMapper.Csv.FileFormats;
using UltraMapper.Csv.FileFormats.Delimited.Mode;
using UltraMapper.Csv.Footer.FooterReaders;
using UltraMapper.Csv.LineReaders;
using UltraMapper.Csv.LineSplitters;
using UltraMapper.Csv.UltraMapper.Extensions.Read;

namespace UltraMapper.Csv
{
    /// <summary>
    /// Handle heterogeneous records.
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    public class CsvMultiRecordParser : DataFileParser<object, ICsvParserConfiguration>, ICsvParser<object>
    {
        private Type _recordType;

        public IMultiRecordSelector Selector { get; set; }

        internal CsvMultiRecordParser( TextReader reader,
            ILineSplitter lineSplitter, ILineReader lineReader,
            IHeaderReader headerReader, IFooterReader footerReader,
            IMultiRecordSelector selector, CsvConfig config )
           : base( reader, lineSplitter, lineReader, headerReader, footerReader, config )
        {
            this.Selector = selector ?? throw new ArgumentNullException( nameof( selector ) );
            this.Configuration = new CsvReadonlyConfig( config );
        }

        protected override object MapLine( string line )
        {
            var recordType = this.Selector.SelectType( line );
            if( _recordType != recordType )
            {
                _recordType = recordType;
                _mapFunction = Mapper.Config[ typeof( DataRecord ), recordType ].MappingFunc;
            }

            _dataRecord.Data = _lineSplitter.Split( line );
            return Mapper.Map( _dataRecord, targetType: recordType );
        }
    }
}
