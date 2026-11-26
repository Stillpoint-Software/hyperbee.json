using System.Runtime.CompilerServices;

namespace Hyperbee.Json.Shared;

/// <summary>
/// Guard helpers that forward to the framework throw helpers where they exist.
/// </summary>
/// <remarks>
/// C# cannot add static members to an existing type, so no polyfill package can supply
/// <c>ArgumentNullException.ThrowIfNull</c> on netstandard2.1. This forwards to the real
/// framework method on net10.0 and net11.0 and throws directly under NETSTANDARD, so the
/// exception type and <c>ParamName</c> are identical on every target.
/// </remarks>
internal static class ThrowIf
{
    public static void Null( object value, [CallerArgumentExpression( nameof( value ) )] string paramName = null )
    {
#if NETSTANDARD
        if ( value is null )
            throw new ArgumentNullException( paramName );
#else
        ArgumentNullException.ThrowIfNull( value, paramName );
#endif
    }
}
