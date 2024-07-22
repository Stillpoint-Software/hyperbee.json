using System.Reflection;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class SearchElementFunction() : ExtensionFunction( SearchMethod, CompareConstraint.MustNotCompare )
{
    public const string Name = "search";
    private static readonly MethodInfo SearchMethod = GetMethod<SearchElementFunction>( nameof( Search ) );

    public static ScalarValue<bool> Search( IValueType argValue, IValueType argPattern )
    {
        if ( !argValue.TryGetValue<string>( out var value ) || value == null )
            return false;

        if ( !argPattern.TryGetValue<string>( out var pattern ) || pattern == null )
            return false;

        var regex = new Regex( IRegexp.ConvertToIRegexp( pattern ) );
        return regex.IsMatch( value );
    }
}
