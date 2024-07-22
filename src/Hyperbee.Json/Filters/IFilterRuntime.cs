
namespace Hyperbee.Json.Filters;

public interface IFilterRuntime<in TNode>
{
    public bool Evaluate( string filter, TNode current, TNode root );
}
