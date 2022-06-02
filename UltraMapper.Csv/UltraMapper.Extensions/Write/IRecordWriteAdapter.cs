using System.Text;

namespace UltraMapper.Csv.UltraMapper.Extensions.Write
{
    public interface IRecordWriteAdapter
    {
        StringBuilder RecordBuilder { get; }
    }
}