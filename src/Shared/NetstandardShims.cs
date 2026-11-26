#if NETSTANDARD
// Everything netstandard2.1 needs from the compiler that newer targets have in box.
// Hand-written rather than taken from the Polyfill package: Polyfill compiles all of
// its internal polyfills into the consuming assembly and the compiler does not strip
// unused internal types, which measures in the hundreds of kilobytes against a few
// thousand bytes for these shims. Only a handful of things are actually used, and all
// of them are small and stable.

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Marker the compiler requires to emit an <c>init</c> accessor.
    /// </summary>
    /// <remarks>
    /// The compiler only needs this type to exist by name, so an empty internal class
    /// is the whole contract. Added in .NET 5.
    /// </remarks>
    internal static class IsExternalInit;

    /// <summary>
    /// Lets a parameter default to the caller's source text for another argument.
    /// </summary>
    /// <remarks>
    /// The compiler reads the parameter name and fills the argument in at the call
    /// site, so, as with <see cref="IsExternalInit"/>, only the shape matters.
    /// Added in .NET 6.
    /// </remarks>
    [AttributeUsage( AttributeTargets.Parameter, AllowMultiple = false, Inherited = false )]
    internal sealed class CallerArgumentExpressionAttribute( string parameterName ) : Attribute
    {
        public string ParameterName { get; } = parameterName;
    }
}

namespace System
{
    /// <summary>
    /// Span members that newer targets have in box.
    /// </summary>
    /// <remarks>
    /// <see cref="MemoryExtensions"/> gained <c>Contains</c> after netstandard2.1 was
    /// cut. Compiled only for netstandard, so on targets that have the framework
    /// extension there is never a second candidate. <c>IndexOf</c> is the vectorized
    /// primitive the framework <c>Contains</c> is itself built on.
    /// </remarks>
    internal static class SpanShimExtensions
    {
        public static bool Contains<T>( this Span<T> span, T value ) where T : IEquatable<T>
            => span.IndexOf( value ) >= 0;

        public static bool Contains<T>( this ReadOnlySpan<T> span, T value ) where T : IEquatable<T>
            => span.IndexOf( value ) >= 0;
    }
}
#endif
