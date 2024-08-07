﻿using System.Reflection;
using System.Text.Json;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Path.Filters.Parser;
using Hyperbee.Json.Path.Filters.Values;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class ValueElementFunction() : ExtensionFunction( ValueMethod, CompareConstraint.MustCompare )
{
    public const string Name = "value";
    private static readonly MethodInfo ValueMethod = GetMethod<ValueElementFunction>( nameof( Value ) );

    public static IValueType Value( IValueType argument )
    {
        if ( argument is not NodeList<JsonElement> nodes )
            throw new NotSupportedException( $"Function `{Name}` does not support kind {argument.ValueKind}" );

        var node = nodes.OneOrDefault();

        return node.ValueKind switch
        {
            JsonValueKind.Number when node.TryGetInt32( out var value ) => Scalar.Value( value ),
            JsonValueKind.Number => Scalar.Value( node.GetSingle() ),
            JsonValueKind.String => Scalar.Value( node.GetString() ),
            JsonValueKind.Object => Scalar.Value( node.EnumerateObject().Any() ),
            JsonValueKind.Array => Scalar.Value( node.GetArrayLength() != 0 ),
            JsonValueKind.True => Scalar.True,
            JsonValueKind.False => Scalar.False,
            _ => Scalar.Nothing
        };
    }
}
