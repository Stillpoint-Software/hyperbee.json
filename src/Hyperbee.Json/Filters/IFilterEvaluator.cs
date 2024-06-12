
namespace Hyperbee.Json.Filters;

public interface IFilterEvaluator<in TType>
{
    public object Evaluate( string filter, TType current, TType root );
}
