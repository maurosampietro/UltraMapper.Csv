using System;
using UltraMapper.Csv.Config.FieldOptions;

namespace UltraMapper.Csv
{
    public sealed class FixedWidthFieldWriteOptionsAttribute : Attribute, IFieldConfig
    {
        public enum PadSides { LEFT, RIGHT }

        public bool IsIgnored { get; set; }
        public bool IsRequired { get; set; }
        public int Order { get; set; } = -1;
        public string Name { get; set; }

        public char PadChar { get; set; } = '\0';
        public PadSides PadSide { get; set; }

        public int FieldLength { get; set; }
    }
}
