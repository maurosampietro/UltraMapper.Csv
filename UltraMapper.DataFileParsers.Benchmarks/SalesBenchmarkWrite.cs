//using BenchmarkDotNet.Attributes;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.IO.Compression;
//using System.Linq;
//using System.Net;
//using UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.SalesExample;
//using UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.SalesExample.SingleCharCsvDelimiter;

//namespace UltraMapper.DataFileParsers.Benchmarks
//{
//    //[SimpleJob( BenchmarkDotNet.Jobs.RuntimeMoniker.Net70 )]
//    //[SimpleJob( BenchmarkDotNet.Jobs.RuntimeMoniker.Net60 )]
//    //[SimpleJob( BenchmarkDotNet.Jobs.RuntimeMoniker.Net50 )]
//    //[SimpleJob( BenchmarkDotNet.Jobs.RuntimeMoniker.Net48 )]
//    public class SalesBenchmarkWrite
//    {
//        private string _inputFile;
//        private List<SaleRecord> _sales;

//        private string DownloadSalesFile()
//        {
//            //string salesMcdZipLink = "https://www.googleapis.com/drive/v3/files/12gD70KlhhbnTe7iGie6MGNmESJEl15CG?alt=media&key=AIzaSyBHXYDBnLwVEhNdKp0mCU0XDoR8VmpPowM";

//            string dir = Path.Combine( Path.GetTempPath(), "UltraMapper.CSV.Benchmarks" );
//            Directory.CreateDirectory( dir );

//            string salesCsvFile = Path.Combine( dir, "5m Sales Records.csv" );
//            string salesZipFile = Path.Combine( dir, "5m Sales Records.zip" );
//            string salesFileLink = "https://www.googleapis.com/drive/v3/files/1WCO6OqCVMW4ugViGgrkcXr_mIkjFouIt?alt=media&key=AIzaSyBHXYDBnLwVEhNdKp0mCU0XDoR8VmpPowM";

//            if( !File.Exists( salesCsvFile ) )
//            {
//                Console.WriteLine( "Downloading test data... (170MB, will take a while)" );

//                using( var client = new WebClient() )
//                    client.DownloadFile( new Uri( salesFileLink ), salesZipFile );

//                Console.WriteLine( "Download completed." );
//                Console.WriteLine( "Unzipping data..." );
//                ZipFile.ExtractToDirectory( salesZipFile, dir );
//                Console.WriteLine( "Unzip completed." );
//                File.Delete( salesZipFile );
//            }

//            return salesCsvFile;
//        }

//        public SalesBenchmarkWrite()
//        {
//            _inputFile = DownloadSalesFile();
//            _sales = new SingleCharUltraMapperSalesTest().ReadRecords( _inputFile ).ToList();
//        }

//        [Benchmark]
//        public void FileHelper()
//        {
//            new SingleCharFileHelpersSalesTest().WriteRecords( _sales );
//        }

//        [Benchmark]
//        public void CsvHelper()
//        {
//            new SingleCharCsvHelperSalesTest().WriteRecords( _sales );
//        }

//        [Benchmark]
//        public void UltraMapper()
//        {
//            new SingleCharUltraMapperSalesTest().WriteRecords( _sales );
//        }
//    }
//}
