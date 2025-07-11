﻿using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Path.Filters.Values;

namespace Hyperbee.Json.Path.Filters.Parser.Expressions;

internal class JsonExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, out CompareConstraint compareConstraint, ITypeDescriptor<TNode> descriptor )
    {
        compareConstraint = CompareConstraint.None;

        if ( !TryParseNode( descriptor.NodeActions, state.Item, out var node ) )
        {
            expression = null;
            return false;
        }

        expression = Expression.Constant( new NodeList<TNode>( [node], isNormalized: true ) );
        return true;
    }

    private static bool TryParseNode<TNode>( INodeActions<TNode> actions, ReadOnlySpan<char> item, out TNode node )
    {
        var maxLength = Encoding.UTF8.GetMaxByteCount( item.Length );
        Span<byte> bytes = maxLength <= 256 ? stackalloc byte[maxLength] : new byte[maxLength];

        var length = Encoding.UTF8.GetBytes( item, bytes );

        // the jsonpath rfc supports single quotes, but the json parser does not
        ConvertToDoubleQuotes( ref bytes, length );

        var reader = new Utf8JsonReader( bytes[..length] );

        if ( actions.TryParse( ref reader, out node ) )
            return true;

        node = default;
        return false;
    }

    private static void ConvertToDoubleQuotes( ref Span<byte> buffer, int length )
    {
        var insideString = false;

        for ( var i = 0; i < length; i++ )
        {
            if ( buffer[i] == (byte) '\"' )
            {
                insideString = !insideString;
            }
            else if ( !insideString && buffer[i] == (byte) '\'' && (i == 0 || buffer[i - 1] != '\\') )
            {
                buffer[i] = (byte) '\"';
            }
        }
    }
}
