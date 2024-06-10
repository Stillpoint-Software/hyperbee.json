
using System.Diagnostics;
using System.Runtime.CompilerServices;

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
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        get
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
