using System;

namespace UltraMapper.Csv
{
    public sealed class FixedWidthFieldReadOptionsAttribute : Attribute
    {
        public bool IsIgnored { get; set; }
        public bool IsRequired { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }

        public char TrimChar { get; set; } = '\0';
        public int FieldLength { get; set; }
    }
}
