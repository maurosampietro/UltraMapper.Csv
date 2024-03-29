﻿using System.Collections.Generic;
using System.IO;
using UltraMapper.Csv;
using UltraMapper.Csv.FileFormats.Delimited;

namespace UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.SalesExample.SingleCharCsvDelimiter
{
    public class SingleCharUltraMapperSalesTest : ICsvBenchmark<SaleRecord>
    {
        public IEnumerable<SaleRecord> ReadRecords( string fileLocation )
        {
            var csvReader = CsvParser<SaleRecord>.GetInstance( fileLocation, cfg =>
            {
                cfg.Delimiter = ",";
                cfg.HasHeader = true;
                cfg.HasFooter = false;
                cfg.HasNewLinesInQuotes = false;
                cfg.HasDelimiterInQuotes = false;
                cfg.IgnoreCommentedLines = false;
                cfg.IgnoreEmptyLines = false;
            } );

            return csvReader.GetRecords();
        }

        public void WriteRecords( IEnumerable<SaleRecord> records )
        {
            string dir = Path.Combine( Path.GetTempPath(), "UltraMapper.CSV.Benchmarks" );
            Directory.CreateDirectory( dir );

            string fileLocation = Path.Combine( dir,
                $"1m Sales Records.output.{nameof( SingleCharUltraMapperSalesTest )}.csv" );

            using( var writer = new StreamWriter( fileLocation ) )
            {
                var csvWriter = new CsvWriter<SaleRecord>( writer, "~" )
                {
                    //HasHeader = false
                };

                csvWriter.WriteRecords( records );
            }
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
