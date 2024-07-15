using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class SearchElementFunction() : ExtensionFunction( SearchMethod, ExtensionInfo.MustNotCompare )
{
    public const string Name = "search";
    private static readonly MethodInfo SearchMethod = GetMethod<SearchElementFunction>( nameof( Search ) );

    public static ScalarValue<bool> Search( IValueType input, IValueType pattern )
    {
        if ( !input.TryGetValue<string>( out var value ) || value == null )
            return false;

        if ( !pattern.TryGetValue<string>( out var patternValue ) || patternValue == null )
            return false;

        var regex = new Regex( IRegexp.ConvertToIRegexp( patternValue ) );
        return regex.IsMatch( value );
    }
}
