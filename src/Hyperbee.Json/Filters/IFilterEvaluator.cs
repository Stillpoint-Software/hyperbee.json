
namespace Hyperbee.Json.Filters;

public interface IFilterEvaluator<in TNode>
{
    public bool Evaluate( string filter, TNode current, TNode root );
}
