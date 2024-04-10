using System.Text;
using System.Text.Json;
using Hyperbee.Json.Tokenizer;

namespace Hyperbee.Json.Extensions;

public static class JsonHelper
{
    // comparison

    public static bool Compare( string strA, string strB, JsonDocumentOptions options = default )
    {
        if ( strA == null && strB == null )
            return true;

        if ( strA == null || strB == null )
            return false;

        var comparer = new JsonElementEqualityComparer( options.MaxDepth );

        using var docA = JsonDocument.Parse( strA, options );
        using var docB = JsonDocument.Parse( strB, options );

        return comparer.Equals( docA.RootElement, docB.RootElement );
    }

    public static bool Compare( JsonElement elmA, string strB, JsonDocumentOptions options = default )
    {
        if ( strB == null )
            return false;

        var comparer = new JsonElementEqualityComparer( options.MaxDepth );
        using var docB = JsonDocument.Parse( strB, options );

        return comparer.Equals( elmA, docB.RootElement );
    }

    public static bool Compare( JsonElement elmA, JsonElement elmB, JsonDocumentOptions options = default )
    {
        var comparer = new JsonElementEqualityComparer( options.MaxDepth );
        return comparer.Equals( elmA, elmB );
    }

    // conversion

    public static ReadOnlySpan<char> NormalizePath( ReadOnlySpan<char> path )
    {
        var tokens = JsonPathQueryTokenizer.TokenizeNoCache( path );

        var builder = new StringBuilder();

        foreach ( var token in tokens )
        {
            builder.Append( '[' );

            foreach ( var selector in token.Selectors )
            {
                switch ( selector.SelectorKind )
                {
                    case SelectorKind.Root:
                        builder.Append( "'$'" );
                        break;
                    case SelectorKind.Dot:
                    case SelectorKind.Name:
                        builder.Append( $"'{selector.Value}'" );
                        break;
                    case SelectorKind.Wildcard:
                        builder.Append( '*' );
                        break;
                    case SelectorKind.Descendant:
                        builder.Append( ".." );
                        break;
                    case SelectorKind.Slice:
                    case SelectorKind.Filter:
                    case SelectorKind.Index:
                        builder.Append( selector.Value );
                        break;

                    case SelectorKind.Undefined:
                    case SelectorKind.UnspecifiedSingular:
                    case SelectorKind.UnspecifiedGroup:
                    default:
                        throw new NotSupportedException( $"Unsupported {nameof( SelectorKind )}." );
                }
            }

            builder.Append( ']' );
        }

        return builder.ToString();
    }
}
