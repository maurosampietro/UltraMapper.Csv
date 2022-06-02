using System;
using UltraMapper.Csv.Config.FieldOptions;

namespace UltraMapper.Csv.UltraMapper.Extensions.Write
{
    public class CsvWriteOptionsAttribute : Attribute, IFieldConfig
    {
        public string Name { get; set; }
        public bool IsIgnored { get; set; }
        public int Order { get; set; } = -1;
    }
}