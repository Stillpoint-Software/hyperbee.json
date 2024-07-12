﻿using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class LengthNodeFunction() : FilterExtensionFunction( LengthMethodInfo, FilterExtensionInfo.MustCompare | FilterExtensionInfo.ExpectNormalized )
{
    public const string Name = "length";
    private static readonly MethodInfo LengthMethodInfo = GetMethod<LengthNodeFunction>( nameof( Length ) );

    public static INodeType Length( INodeType input )
    {
        return input switch
        {
            NodesType<JsonNode> nodes => LengthImpl( nodes.FirstOrDefault() ),
            ValueType<string> valueString => new ValueType<float>( valueString.Value.Length ),
            Null or Nothing => input,
            _ => ValueType.Nothing
        };
    }

    public static INodeType LengthImpl( object value )
    {
        return value switch
        {
            string str => new ValueType<float>( str.Length ),
            Array array => new ValueType<float>( array.Length ),
            System.Collections.ICollection collection => new ValueType<float>( collection.Count ),
            System.Collections.IEnumerable enumerable => new ValueType<float>( enumerable.Cast<object>().Count() ),
            JsonNode node => node.GetValueKind() switch
            {
                JsonValueKind.String => new ValueType<float>( node.GetValue<string>()?.Length ?? 0 ),
                JsonValueKind.Array => new ValueType<float>( node.AsArray().Count ),
                JsonValueKind.Object => new ValueType<float>( node.AsObject().Count ),
                _ => ValueType.Nothing
            },
            _ => ValueType.Nothing
        };
    }

}
