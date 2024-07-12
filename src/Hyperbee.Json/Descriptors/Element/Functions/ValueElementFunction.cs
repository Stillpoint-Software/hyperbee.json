﻿using System.Reflection;
using System.Text.Json;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class ValueElementFunction() : FilterExtensionFunction( ValueMethodInfo, FilterExtensionInfo.MustCompare )
{
    public const string Name = "value";
    private static readonly MethodInfo ValueMethodInfo = GetMethod<ValueElementFunction>( nameof( Value ) );

    public static INodeType Value( INodeType arg )
    {
        if ( arg.Kind != NodeTypeKind.NodeList )
            throw new NotSupportedException( $"Function {Name} does not support kind {arg.Kind}" );

        var nodeArray = ((NodesType<JsonElement>) arg).ToArray();

        if ( nodeArray.Length != 1 )
            return Constants.Nothing;

        var node = nodeArray.FirstOrDefault();

        return node.ValueKind switch
        {
            JsonValueKind.Number => new ValueType<float>( node.GetSingle() ),
            JsonValueKind.String => new ValueType<string>( node.GetString() ),
            JsonValueKind.Object or JsonValueKind.Array => new ValueType<bool>( IsNotEmpty( node ) ),
            JsonValueKind.True => Constants.True,
            JsonValueKind.False or JsonValueKind.Null or JsonValueKind.Undefined => Constants.False,
            _ => Constants.False
        };

        static bool IsNotEmpty( JsonElement node )
        {
            return node.ValueKind switch
            {
                JsonValueKind.Array => node.EnumerateArray().Any(),
                JsonValueKind.Object => node.EnumerateObject().Any(),
                _ => false
            };
        }
    }
}
