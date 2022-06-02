namespace UltraMapper.Csv.UltraMapper.Extensions.Read.Csv
{
    //Encapsulating data in this ad-hoc class is a neat way 
    //to inform the mapper to use a specific UltraMapper extension
    //to perform its operations.
    //
    //Only one instance of this class will be created per parser instance.
    //This one instance will be reused over and over again to pass data to the ExpressionBuilder.
    public class CsvRecordReadObject : IRecordReadAdapter
    {
        public string[] Data { get; set; }
    }
}
