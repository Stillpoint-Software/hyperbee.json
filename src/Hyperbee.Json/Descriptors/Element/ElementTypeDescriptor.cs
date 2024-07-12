using System.Text.Json;
using Hyperbee.Json.Descriptors.Element.Functions;
using Hyperbee.Json.Descriptors.Types;
using Hyperbee.Json.Filters;

namespace Hyperbee.Json.Descriptors.Element;

public class ElementTypeDescriptor : ITypeDescriptor<JsonElement>
{
    private FilterEvaluator<JsonElement> _evaluator;
    private ElementValueAccessor _accessor;
    private NodeTypeComparer<JsonElement> _comparer;

    public FunctionRegistry Functions { get; } = new();

    public IValueAccessor<JsonElement> Accessor =>
        _accessor ??= new ElementValueAccessor();

    public IFilterEvaluator<JsonElement> FilterEvaluator =>
        _evaluator ??= new FilterEvaluator<JsonElement>( this );

    public INodeTypeComparer Comparer =>
        _comparer ??= new NodeTypeComparer<JsonElement>( Accessor );

    public bool CanUsePointer => true;

    public ElementTypeDescriptor()
    {
        Functions.Register( CountElementFunction.Name, () => new CountElementFunction() );
        Functions.Register( LengthElementFunction.Name, () => new LengthElementFunction() );
        Functions.Register( MatchElementFunction.Name, () => new MatchElementFunction() );
        Functions.Register( SearchElementFunction.Name, () => new SearchElementFunction() );
        Functions.Register( ValueElementFunction.Name, () => new ValueElementFunction() );
    }
}
