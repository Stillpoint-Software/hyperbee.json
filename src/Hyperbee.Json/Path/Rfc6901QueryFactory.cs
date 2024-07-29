using Hyperbee.Json.Pointer;

namespace Hyperbee.Json.Path;

internal static class Rfc6901QueryFactory
{
    internal static JsonPathQuery Parse( ReadOnlySpan<char> query, JsonQueryParserOptions options )
    {
        // https://www.rfc-editor.org/rfc/rfc6901.html

        options = options.HasFlag( JsonQueryParserOptions.Rfc6902 )
            ? JsonQueryParserOptions.Rfc6902
            : JsonQueryParserOptions.Rfc6901;

        var convertOptions = options == JsonQueryParserOptions.Rfc6902
            ? JsonPointerConvertOptions.Rfc6902
            : JsonPointerConvertOptions.Default;

        var jsonpath = JsonPathPointerConverter.ConvertJsonPointerToJsonPath( query.ToString(), convertOptions );
        return Rfc9535QueryFactory.Parse( jsonpath.AsSpan(), options );
    }
}
