using System;

namespace UltraMapper.Csv.Config.FieldOptions.Attributes
{
    public class CsvWriteOptionsAttribute : Attribute, IFieldConfig
    {
        public string Name { get; set; }
        public bool IsIgnored { get; set; }
        public int Order { get; set; } = -1;
    }
}