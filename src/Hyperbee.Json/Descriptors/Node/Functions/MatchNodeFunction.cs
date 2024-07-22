using System.Reflection;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class MatchNodeFunction() : ExtensionFunction( MatchMethod, CompareConstraint.MustNotCompare )
{
    public const string Name = "match";
    private static readonly MethodInfo MatchMethod = GetMethod<MatchNodeFunction>( nameof( Match ) );

    public static ScalarValue<bool> Match( IValueType argValue, IValueType argPattern )
    {
        if ( !argValue.TryGetValue<string>( out var value ) || value == null )
            return false;

        if ( !argPattern.TryGetValue<string>( out var pattern ) || pattern == null )
            return false;

        var regex = new Regex( $"^{IRegexp.ConvertToIRegexp( pattern )}$" );
        return regex.IsMatch( value );
    }
}

