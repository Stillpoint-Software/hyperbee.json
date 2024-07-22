using System.Text.Json;
using Hyperbee.Json.Descriptors.Element.Functions;
using Hyperbee.Json.Filters;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element;

public class ElementTypeDescriptor : ITypeDescriptor<JsonElement>
{
    private ElementValueAccessor _accessor;
    private FilterRuntime<JsonElement> _runtime;
    private ValueTypeComparer<JsonElement> _comparer;

    public FunctionRegistry Functions { get; } = new();

    public IValueAccessor<JsonElement> Accessor =>
        _accessor ??= new ElementValueAccessor();

    public IFilterRuntime<JsonElement> FilterRuntime =>
        _runtime ??= new FilterRuntime<JsonElement>();

    public IValueTypeComparer Comparer =>
        _comparer ??= new ValueTypeComparer<JsonElement>( Accessor );

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
