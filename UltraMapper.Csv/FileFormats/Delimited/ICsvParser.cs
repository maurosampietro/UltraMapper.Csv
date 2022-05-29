using System.Collections.Generic;
using UltraMapper.Csv.Config;

namespace UltraMapper.Csv
{
    public interface ICsvParser<TRecord>
        where TRecord : class, new()
    {
        ICsvParserConfiguration Configuration { get; }
        IEnumerable<TRecord> GetRecords();
    }
}
