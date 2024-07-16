using System.Reflection;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class MatchElementFunction() : ExtensionFunction( MatchMethod, ExtensionInfo.MustNotCompare )
{
    public const string Name = "match";
    private static readonly MethodInfo MatchMethod = GetMethod<MatchElementFunction>( nameof( Match ) );

    public static ScalarValue<bool> Match( IValueType input, IValueType pattern )
    {
        if ( !input.TryGetValue<string>( out var value ) || value == null )
            return false;

        if ( !pattern.TryGetValue<string>( out var patternValue ) || patternValue == null )
            return false;

        var regex = new Regex( $"^{IRegexp.ConvertToIRegexp( patternValue )}$" );
        return regex.IsMatch( value );
    }
}
