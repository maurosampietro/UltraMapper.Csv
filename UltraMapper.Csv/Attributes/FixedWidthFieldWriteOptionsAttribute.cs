using System;

namespace UltraMapper.Csv
{
    public sealed class FixedWidthFieldWriteOptionsAttribute : Attribute
    {
        public enum PadSides { LEFT, RIGHT }

        public bool IsIgnored { get; set; }
        public bool IsRequired { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }

        public char Pad { get; set; } = '\0';
        public PadSides PadSide { get; set; }

        public int FieldLength { get; set; }
    }
}
