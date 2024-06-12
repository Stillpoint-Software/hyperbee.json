using System.Text;
using Hyperbee.Json.Tokenizer;

namespace Hyperbee.Json.Extensions;

public static class JsonHelper
{
    // conversion

    public static ReadOnlySpan<char> NormalizePath( ReadOnlySpan<char> path )
    {
        var tokens = JsonPathQueryTokenizer.TokenizeNoCache( path );

        // TODO: FIX!!!
        return "".AsSpan();
        //
        // var builder = new StringBuilder();
        //
        // foreach ( var token in tokens )
        // {
        //     builder.Append( '[' );
        //
        //     foreach ( var selector in token.Selectors )
        //     {
        //         switch ( selector.SelectorKind )
        //         {
        //             case SelectorKind.Root:
        //                 builder.Append( "'$'" );
        //                 break;
        //             case SelectorKind.Dot:
        //             case SelectorKind.Name:
        //                 builder.Append( $"'{selector.Value}'" );
        //                 break;
        //             case SelectorKind.Wildcard:
        //                 builder.Append( '*' );
        //                 break;
        //             case SelectorKind.Descendant:
        //                 builder.Append( ".." );
        //                 break;
        //             case SelectorKind.Slice:
        //             case SelectorKind.Filter:
        //             case SelectorKind.Index:
        //                 builder.Append( selector.Value );
        //                 break;
        //
        //             case SelectorKind.Undefined:
        //             case SelectorKind.UnspecifiedSingular:
        //             case SelectorKind.UnspecifiedGroup:
        //             default:
        //                 throw new NotSupportedException( $"Unsupported {nameof( SelectorKind )}." );
        //         }
        //     }
        //
        //     builder.Append( ']' );
        // }
        //
        // return builder.ToString();
    }
}
