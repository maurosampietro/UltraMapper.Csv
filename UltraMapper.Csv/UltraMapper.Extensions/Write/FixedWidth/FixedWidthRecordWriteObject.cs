using System.Text;

namespace UltraMapper.Csv.UltraMapper.Extensions.Write.FixedWidth
{
    //Encapsulating data in this ad-hoc class is a neat way 
    //to inform the mapper to use a specific UltraMapper extension
    //to perform its operations.
    //
    //Only one instance of this class will be created per parser instance.
    //This one instance will be reused over and over again to pass data to the ExpressionBuilder.
    public class FixedWidthRecordWriteObject : IRecordWriteAdapter
    {
        public StringBuilder RecordBuilder { get; } = new StringBuilder();
    }
}
