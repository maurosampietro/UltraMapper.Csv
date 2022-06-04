using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UltraMapper.Csv;
using UltraMapper.Csv.Factories;

namespace UltraMapper.DataFileParsers.Benchmarks.PerformanceTests.GpsDataExample.SingleCharCsvDelimiter
{
    public class UltraMapperGpsDataTest : ICsvBenchmark<GpsDataRecord>
    {
        public IEnumerable<GpsDataRecord> ReadRecords( string fileLocation )
        {
            var csvReader = CsvParser<GpsDataRecord>.GetInstance( fileLocation, cfg =>
            {
                cfg.HasHeader = true;
                cfg.HasFooter = true;
            } );

            return csvReader.GetRecords();
        }

        public void WriteRecords( IEnumerable<GpsDataRecord> records )
        {
            string fileLocation = Path.Combine(
              Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ),
             "Resources", $"dataset.{nameof( UltraMapperGpsDataTest )}.csv" );

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
