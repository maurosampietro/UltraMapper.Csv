using System;
using UltraMapper.Csv.Config.FieldOptions;

namespace UltraMapper.Csv
{
    public sealed class FixedWidthFieldReadOptionsAttribute : Attribute, IFieldConfig
    {
        public bool IsIgnored { get; set; }
        public bool IsRequired { get; set; }
        public int Order { get; set; } = -1;
        public string Name { get; set; }

        public char TrimChar { get; set; } = '\0';
        public int FieldLength { get; set; }
    }
}
