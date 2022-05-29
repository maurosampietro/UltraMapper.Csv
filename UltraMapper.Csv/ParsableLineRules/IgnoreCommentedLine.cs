namespace UltraMapper.Csv.ParsableLineRules
{
    public class IgnoreCommentedLine : IParsableLineRule
    {
        public readonly string CommentMarker;

        public IgnoreCommentedLine( string commentMarker )
        {
            this.CommentMarker = commentMarker;
        }

        public bool IsLineParsable( string line )
        {
            return !line.StartsWith( CommentMarker );
        }
    }
}
