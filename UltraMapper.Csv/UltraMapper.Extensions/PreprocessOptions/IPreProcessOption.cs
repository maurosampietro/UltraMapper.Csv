using System.Linq.Expressions;
using System.Reflection;
using UltraMapper.MappingExpressionBuilders;

namespace UltraMapper.Csv.UltraMapper.Extensions.PreprocessOptions
{
    public interface IPreProcessOption
    {
        bool CanExecute( Mapper mapper, ReferenceMapperContext context, PropertyInfo targetMember, CsvFieldOptionsAttribute options );
        Expression Execute( Mapper mapper, ReferenceMapperContext context, PropertyInfo targetMember, CsvFieldOptionsAttribute options, Expression source );
    }
}
