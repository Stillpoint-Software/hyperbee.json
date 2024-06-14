
namespace Hyperbee.Json.Filters;

public interface IFilterEvaluator<in TNode>
{
    public object Evaluate( string filter, TNode current, TNode root );
}
