
using System.Diagnostics;

namespace Hyperbee.Json.Tokenizer;

[DebuggerDisplay( "SelectorKind = {SelectorKind}, Value = {Value}" )]
internal record SelectorDescriptor
{
    public SelectorKind SelectorKind { get; init; }
    public string Value { get; init; }
}

[DebuggerTypeProxy( typeof( JsonPathSegmentDebugView ) )]
[DebuggerDisplay( "Singular = {Singular}, SelectorCount = {Selectors.Length}" )]
internal record JsonPathSegments
{
    public static JsonPathSegments DescendSegments = new( "..", SelectorKind.UnspecifiedGroup );

    public SelectorDescriptor[] Selectors { get; init; }

    // TODO: Check if we can set in ctor
    public string FirstSelector => Selectors[0].Value;

    public bool Singular { get; }

    public JsonPathSegments( string selector, SelectorKind kind )
    {
        Selectors =
        [
            new SelectorDescriptor { SelectorKind = kind, Value = selector }
        ];

        Singular = IsSingular();
    }

    public JsonPathSegments( SelectorDescriptor[] selectors )
    {
        Selectors = selectors;
        Singular = IsSingular();
    }

    private bool IsSingular()
    {
        if ( Selectors.Length != 1 )
            return false;

        var selectorKind = Selectors[0].SelectorKind;

        return selectorKind == SelectorKind.UnspecifiedSingular || // prioritize runtime value
               selectorKind == SelectorKind.Dot ||
               selectorKind == SelectorKind.Index ||
               selectorKind == SelectorKind.Name ||
               selectorKind == SelectorKind.Root;
    }

    public void Deconstruct( out bool singular, out SelectorDescriptor[] selectors )
    {
        singular = Singular;
        selectors = Selectors;
    }

    internal class JsonPathSegmentDebugView( JsonPathSegments instance )
    {
        [DebuggerBrowsable( DebuggerBrowsableState.RootHidden )]
        public SelectorDescriptor[] Selectors => instance.Selectors;
    }
}
