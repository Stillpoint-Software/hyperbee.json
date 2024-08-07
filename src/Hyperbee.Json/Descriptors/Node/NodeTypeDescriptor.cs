﻿using System.Text.Json.Nodes;
using Hyperbee.Json.Descriptors.Node.Functions;

namespace Hyperbee.Json.Descriptors.Node;

public class NodeTypeDescriptor : ITypeDescriptor<JsonNode>
{
    public IValueAccessor<JsonNode> ValueAccessor => new NodeValueAccessor();
    public INodeActions<JsonNode> NodeActions => new NodeActions();
    public FunctionRegistry Functions { get; } = new();

    public NodeTypeDescriptor()
    {
        Functions.Register( CountNodeFunction.Name, () => new CountNodeFunction() );
        Functions.Register( LengthNodeFunction.Name, () => new LengthNodeFunction() );
        Functions.Register( MatchNodeFunction.Name, () => new MatchNodeFunction() );
        Functions.Register( SearchNodeFunction.Name, () => new SearchNodeFunction() );
        Functions.Register( ValueNodeFunction.Name, () => new ValueNodeFunction() );
    }
}
