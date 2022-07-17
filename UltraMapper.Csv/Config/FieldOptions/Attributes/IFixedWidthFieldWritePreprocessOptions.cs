using UltraMapper.Csv.Config.FieldOptions;

namespace UltraMapper.Csv
{
    public interface IFixedWidthFieldWriteOptionsAttribute
    {
        PadSides HeaderPadSide { get; set; }
        char PadChar { get; set; }
        PadSides PadSide { get; set; }
    }
}