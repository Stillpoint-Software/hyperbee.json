﻿using System.Text.Json;
using Hyperbee.Json.Descriptors.Element.Functions;

namespace Hyperbee.Json.Descriptors.Element;

public class ElementTypeDescriptor : ITypeDescriptor<JsonElement>
{
    public IValueAccessor<JsonElement> Accessor => new ElementValueAccessor();
    public FunctionRegistry Functions { get; } = new();

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
