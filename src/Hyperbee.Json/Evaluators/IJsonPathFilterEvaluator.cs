
namespace Hyperbee.Json.Evaluators;

public interface IJsonPathFilterEvaluator<in TType>
{
    public object Evaluate( string filter, TType current, TType root );
}
