using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UltraMapper.Csv.Factories;
using UltraMapper.DataFileParsers.Benchmarks;
using UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.SalesExample;

namespace UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.SalesExample.MultiCharCsvDelimiter
{
    public class MultiCharUltraMapperSalesTest : ICsvBenchmark<SaleRecordMCD>
    {
        public IEnumerable<SaleRecordMCD> ReadRecords( string fileLocation )
        {
            var csvReader = CsvParserFactory.GetInstance<SaleRecordMCD>( new Uri( fileLocation ), cfg =>
            {
                cfg.Delimiter = "~DELIMITER~";
                cfg.HasNewLinesInQuotes = true;
                cfg.HasDelimiterInQuotes = true;
                cfg.IgnoreCommentedLines = true;
                cfg.IgnoreEmptyLines = true;
                cfg.HasHeader = true;
                cfg.HasFooter = true;
            } );

            return csvReader.GetRecords();
        }

        public void WriteRecords( IEnumerable<SaleRecordMCD> records )
        {
            string fileLocation = Path.Combine(
              Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ),
             "Resources", $"dataset.{nameof(MultiCharUltraMapperSalesTest)}.csv" );

            //var csvWriter = new CsvWriter( fileLocation )
            //{
            //    //HasHeader = false
            //};

            //csvWriter.Write( records, false );
        }
    }

    //public class CsvFileParserWithRecordChecks : ICsvBenchmark<GpsDataRecord>
    //{
    //    public string Name => "UltraMapper record checks";
    //    public int ExecutionOrder => 2;

    //    public IEnumerable<GpsDataRecord> ReadRecords()
    //    {
    //        string fileLocation = Path.Combine(
    //              Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ),
    //              "Resources", "dataset.csv" );

    //        var csvReader = new CsvReader( new Uri( fileLocation ), isRfc4180Compliant: true );
    //        csvReader.HasHeader = true;
    //        csvReader.HasFooter = false;

    //        return csvReader.GetRecords<GpsDataRecord>().ToList();
    //    }

    //    public void WriteRecords( IEnumerable<GpsDataRecord> records )
    //    {
    //        string fileLocation = Path.Combine(
    //          Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ),
    //         "Resources", $"dataset.{Name}.csv" );

    //        //var csvWriter = new CsvWriter( fileLocation )
    //        //{
    //        //    //HasHeader = false
    //        //};

    //        //csvWriter.Write( records, false );
    //    }
    //}

    //public class UltraMapperRfc4180Test : ICsvBenchmark<GpsDataRecord>
    //{
    //    public string Name => "UltraMapper Rfc4180";
    //    public int ExecutionOrder => 2;

    //    public IEnumerable<GpsDataRecord> ReadRecords()
    //    {
    //        string fileLocation = Path.Combine(
    //              Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ),
    //              "Resources", "dataset.csv" );

    //        var csvReader = CsvParser.GetReader( fileLocation, isRfc4180Compliant: true );
    //        csvReader.HasHeader = true;
    //        csvReader.HasFooter = false;

    //        return csvReader.GetRecords<GpsDataRecord>().ToList();
    //    }

    //    public void WriteRecords( IEnumerable<GpsDataRecord> records )
    //    {
    //        string fileLocation = Path.Combine(
    //          Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ),
    //         "Resources", $"dataset.{Name}.csv" );

    //        var csvWriter = new CsvWriter( fileLocation )
    //        {
    //            //HasHeader = false
    //        };

    //        csvWriter.Write( records, false );
    //    }
    //}
}
