using System;

namespace UltraMapper.Csv.UltraMapper.Extensions.Write
{
    public class CsvWriteOptionsAttribute : Attribute
    {
        public bool IsIgnored { get; internal set; }
        public int Order { get; internal set; } = -1;
    }
}