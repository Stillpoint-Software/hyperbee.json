
using System.Diagnostics;

namespace Hyperbee.Json.Tokenizer;

[DebuggerDisplay( "SelectorKind = {SelectorKind}, Value = {Value}" )]
internal record SelectorDescriptor
{
    public SelectorKind SelectorKind { get; init; }
    public string Value { get; init; }
}

[DebuggerTypeProxy( typeof( JsonPathTokenDebugView ) )]
[DebuggerDisplay( "Singular = {Singular}, SelectorCount = {Selectors.Length}" )]
internal record JsonPathToken
{
    public SelectorDescriptor[] Selectors { get; init; }

    public string FirstSelector => Selectors[0].Value;

    public bool Singular
    {
        get
        {
            if ( Selectors.Length != 1 )
                return false;

            return Selectors[0].SelectorKind == SelectorKind.UnspecifiedSingular || // prioritize runtime value
                   Selectors[0].SelectorKind == SelectorKind.Dot ||
                   Selectors[0].SelectorKind == SelectorKind.Index ||
                   Selectors[0].SelectorKind == SelectorKind.Name ||
                   Selectors[0].SelectorKind == SelectorKind.Root;
        }
    }

    public JsonPathToken( string selector, SelectorKind kind )
    {
        Selectors =
        [
            new SelectorDescriptor { SelectorKind = kind, Value = selector }
        ];
    }

    public JsonPathToken( SelectorDescriptor[] selectors )
    {
        Selectors = selectors;
    }

    public void Deconstruct( out bool singular, out SelectorDescriptor[] selectors )
    {
        singular = Singular;
        selectors = Selectors;
    }

    internal class JsonPathTokenDebugView( JsonPathToken instance )
    {
        [DebuggerBrowsable( DebuggerBrowsableState.RootHidden )]
        public SelectorDescriptor[] Selectors => instance.Selectors;
    }
}
