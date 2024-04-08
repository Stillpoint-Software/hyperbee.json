using System.Text.Json;
using System.Text.Json.Nodes;

namespace Hyperbee.Json;

public readonly struct JsonPathElement
{
    public JsonElement Value { get; }

    public string Path { get; }

    public ReadOnlySpan<char> Name => GetName( Path );

    public JsonPathElement( JsonElement value, string path )
    {
        Value = value;
        Path = path;
    }

    public static implicit operator JsonElement( JsonPathElement pathElement ) => pathElement.Value;

    private static ReadOnlySpan<char> GetName( ReadOnlySpan<char> path )
    {
        var index = path.LastIndexOf( '\'' );

        var count = 0;
        while ( --index > 0 )
        {
            if ( path[index] == '\'' )
            {
                if ( index == 0 || path[index - 1] != '\\' ) // make sure this isn't escaped \'
                    return path.Slice( index + 1, count );
            }

            count++;
        }

        return [];
    }
}