
namespace Hyperbee.Json.Evaluators;

public delegate object JsonPathEvaluator<in TType>( string filter, TType current, TType root );

public interface IJsonPathFilterEvaluator<in TType>
{
    public object Evaluator( string filter, TType current, TType root );
}
