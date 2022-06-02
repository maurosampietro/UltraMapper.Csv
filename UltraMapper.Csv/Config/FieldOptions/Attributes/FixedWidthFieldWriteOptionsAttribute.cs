using System;
using UltraMapper.Csv.Config.FieldOptions;

namespace UltraMapper.Csv
{
    public sealed class FixedWidthFieldWriteOptionsAttribute : Attribute, IFieldConfig
    {
        public bool IsIgnored { get; set; }
        public bool IsRequired { get; set; }
        public int Order { get; set; } = -1;
        public string Name { get; set; }
        public int FieldLength { get; set; } = -1;
        public char PadChar { get; set; } = '\0'; //non va bene come default '\0' perchè termina la stringa
        public PadSides PadSide { get; set; }
        public PadSides HeaderPadSide { get; set; }
    }
}
