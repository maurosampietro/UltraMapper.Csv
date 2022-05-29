using System;

namespace UltraMapper.Csv.ParsableLineRules
{
    public class IgnoreEmptyAndCommentedLine : IParsableLineRule
    {
        public readonly string CommentMarker;

        public IgnoreEmptyAndCommentedLine( string commentMarker )
        {
            this.CommentMarker = commentMarker;
        }

        public bool IsLineParsable( string line )
        {
            return !String.IsNullOrWhiteSpace( line ) &&
                !line.StartsWith( CommentMarker );
        }
    }
}
