using BenchmarkDotNet.Attributes;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using UltraMapper.Csv;
using UltraMapper.Csv.FileFormats.Delimited;
using UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.SalesExample;
using UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.SalesExample.MultiCharCsvDelimiter;

namespace UltraMapper.DataFileParsers.Benchmarks
{
    [SimpleJob( BenchmarkDotNet.Jobs.RuntimeMoniker.Net60 )]
    [SimpleJob( BenchmarkDotNet.Jobs.RuntimeMoniker.Net50 )]
    [SimpleJob( BenchmarkDotNet.Jobs.RuntimeMoniker.Net47 )]
    public class SalesBenchmarkReadMultichar
    {
        private string _inputFile;

        private string DownloadSalesFile()
        {
            //string salesMcdZipLink = "https://www.googleapis.com/drive/v3/files/12gD70KlhhbnTe7iGie6MGNmESJEl15CG?alt=media&key=AIzaSyBHXYDBnLwVEhNdKp0mCU0XDoR8VmpPowM";

            string dir = Path.Combine( Path.GetTempPath(), "UltraMapper.CSV.Benchmarks" );
            Directory.CreateDirectory( dir );

            string salesCsvFile = Path.Combine( dir, "5m Sales Records.csv" );
            string salesZipFile = Path.Combine( dir, "5m Sales Records.zip" );
            string salesFileLink = "https://www.googleapis.com/drive/v3/files/1WCO6OqCVMW4ugViGgrkcXr_mIkjFouIt?alt=media&key=AIzaSyBHXYDBnLwVEhNdKp0mCU0XDoR8VmpPowM";

            if( !File.Exists( salesCsvFile ) )
            {
                Console.WriteLine( "Downloading test data... (170MB, will take a while)" );

                using( var client = new WebClient() )
                    client.DownloadFile( new Uri( salesFileLink ), salesZipFile );

                Console.WriteLine( "Download completed." );
                Console.WriteLine( "Unzipping data..." );
                ZipFile.ExtractToDirectory( salesZipFile, dir );
                Console.WriteLine( "Unzip completed." );
                File.Delete( salesZipFile );
            }

            return salesCsvFile;
        }

        public SalesBenchmarkReadMultichar()
        {
            string salesCsvFile = DownloadSalesFile();

            string dir = Path.Combine( Path.GetTempPath(), "UltraMapper.CSV.Benchmarks" );
            string salesCsvFileMulti = Path.Combine( dir, "5m Sales Records.MulticharDelimiter.csv" );

            var reader = CsvParser<SaleRecord>.GetInstance( salesCsvFile, cfg => cfg.Delimiter = "," );
            var records = reader.GetRecords();

            using( var ws = new StreamWriter( salesCsvFileMulti ) )
            {
                var writer = new CsvWriter<SaleRecord>( ws, "~DELIMITER~" );
                writer.WriteHeader();
                writer.WriteRecords( records );
            }

            _inputFile = salesCsvFileMulti;
        }

        [Benchmark]
        public void BaseLine()
        {
            var c = new MultiCharBaselineSalesTest().ReadRecords( _inputFile ).Count();
            Console.WriteLine(c);
        }

        //[Benchmark]
        //public void FileHelper()
        //{
        //    new MultiCharFileHelpersSalesTest().ReadRecords( _inputFile ).Count();
        //}

        [Benchmark]
        public void CsvHelper()
        {
            var c = new MultiCharCsvHelperSalesTest().ReadRecords( _inputFile ).Count();
            Console.WriteLine( c );
        }

        [Benchmark]
        public void UltraMapper()
        {
            var c = new MultiCharUltraMapperSalesTest().ReadRecords( _inputFile ).Count();
            Console.WriteLine( c );
        }
    }
}
