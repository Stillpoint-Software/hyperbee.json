using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class JsonExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, ref ExpressionInfo exprInfo, ITypeDescriptor<TNode> descriptor )
    {
        if ( !TryParseNode( descriptor.Accessor, state.Item, out var node ) )
        {
            expression = null;
            return false;
        }

        expression = Expression.Constant( new NodeList<TNode>( [node], isNormalized: true ) );
        exprInfo.Kind = ExpressionKind.Json;
        return true;
    }

    private static bool TryParseNode<TNode>( IValueAccessor<TNode> accessor, ReadOnlySpan<char> item, out TNode node )
    {
        var maxLength = Encoding.UTF8.GetMaxByteCount( item.Length );
        Span<byte> bytes = maxLength <= 256 ? stackalloc byte[maxLength] : new byte[maxLength];

        var length = Encoding.UTF8.GetBytes( item, bytes );

        // the jsonpath rfc supports single quotes, but the json parser does not
        ConvertToDoubleQuotes( ref bytes, length );

        var reader = new Utf8JsonReader( bytes[..length] );

        if ( accessor.TryParseNode( ref reader, out node ) )
            return true;

        node = default;
        return false;

        // Helper to replace single quotes with double quotes

        static void ConvertToDoubleQuotes( ref Span<byte> buffer, int length )
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
}
