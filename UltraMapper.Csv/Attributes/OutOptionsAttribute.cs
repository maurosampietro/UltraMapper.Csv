using System;

namespace UltraMapper.Csv.UltraMapper.Extensions.Write
{
    internal class OutOptionsAttribute : Attribute
    {
        public bool IsIgnored { get; internal set; }
        public object Order { get; internal set; }
    }
}