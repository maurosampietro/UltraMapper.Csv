using System.Linq.Expressions;
using System.Reflection;

namespace UltraMapper.Csv.UltraMapper.Extensions.PreprocessOptions
{
    public interface IPreProcessOption
    {
        bool CanExecute( PropertyInfo targetMember, CsvReadOptionsAttribute options );
        Expression Execute( PropertyInfo targetMember, CsvReadOptionsAttribute options, Expression source );
    }
}
