namespace UltraMapper.Csv.Header
{
    public interface IHeaderSupport
    {
        //bool HasHeader { get; set; }
        string GetHeader();
    }

    public interface IHeaderSupport<THeader> where THeader : class, new()
    {
        THeader GetHeader();
    }
}
