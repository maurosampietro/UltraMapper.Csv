namespace UltraMapper.Csv.LineSplitters
{
    public interface ILineSplitter
    {
        string[] Split( string line );
    }
}
