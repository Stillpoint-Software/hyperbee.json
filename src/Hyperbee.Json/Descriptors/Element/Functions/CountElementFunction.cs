﻿using System.Reflection;
using System.Text.Json;
using Hyperbee.Json.Path.Filters.Parser;
using Hyperbee.Json.Path.Filters.Values;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class CountElementFunction() : ExtensionFunction( CountMethod, CompareConstraint.MustCompare )
{
    public const string Name = "count";
    private static readonly MethodInfo CountMethod = GetMethod<CountElementFunction>( nameof( Count ) );

    public static ScalarValue<int> Count( IValueType argument )
    {
        if ( argument.ValueKind != ValueKind.NodeList )
            throw new NotSupportedException( $"Function `{Name}` must be a node list." );

        var nodes = (NodeList<JsonElement>) argument;

        if ( nodes.IsNormalized && !nodes.Any() )
            return 1;

        return nodes.Count();
    }
}
