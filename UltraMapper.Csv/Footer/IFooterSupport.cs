namespace UltraMapper.Csv.Footer
{
    public interface IFooterSupport
    {
        //bool HasFooter { get; set; }
        string GetFooter();
    }

    public interface IFooterSupport<TFooter> where TFooter : class, new()
    {
        TFooter GetFooter();
    }
}
