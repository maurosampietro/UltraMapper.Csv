using System;
using UltraMapper.Csv.Config.FieldOptions;

namespace UltraMapper.Csv
{
    public sealed class FixedWidthFieldReadOptionsAttribute : Attribute, IFieldConfig, IFixedWidthFieldPreprocessOptions
    {
        public bool IsIgnored { get; set; }
        public bool IsRequired { get; set; }
        public int Order { get; set; } = -1;
        public string Name { get; set; }
        public int FieldLength { get; set; } = -1;

        public char TrimChar { get; set; } = '\0';
    }
}
