﻿using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Path.Filters.Parser;
using Hyperbee.Json.Path.Filters.Values;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class ValueNodeFunction() : ExtensionFunction( ValueMethod, CompareConstraint.MustCompare )
{
    public const string Name = "value";
    private static readonly MethodInfo ValueMethod = GetMethod<ValueNodeFunction>( nameof( Value ) );

    public static IValueType Value( IValueType argument )
    {
        if ( argument is not NodeList<JsonNode> nodes )
            throw new NotSupportedException( $"Function `{Name}` does not support kind {argument.ValueKind}" );

        var node = nodes.OneOrDefault();

        return node?.GetValueKind() switch
        {
            JsonValueKind.Number when node.AsValue().TryGetValue<int>( out var value ) => Scalar.Value( value ),
            JsonValueKind.Number => Scalar.Value( node.GetValue<float>() ),
            JsonValueKind.String => Scalar.Value( node.GetValue<string>() ),
            JsonValueKind.Object => Scalar.Value( node.AsObject().Count != 0 ),
            JsonValueKind.Array => Scalar.Value( node.AsArray().Count != 0 ),
            JsonValueKind.True => Scalar.True,
            JsonValueKind.False => Scalar.False,
            _ => Scalar.Nothing
        };
    }
}
