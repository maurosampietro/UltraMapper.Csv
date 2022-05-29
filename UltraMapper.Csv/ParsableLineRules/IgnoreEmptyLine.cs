using System;

namespace UltraMapper.Csv.ParsableLineRules
{
    public class IgnoreEmptyLine : IParsableLineRule
    {
        public bool IsLineParsable( string line )
        {
            return !String.IsNullOrWhiteSpace( line );
        }
    }
}
