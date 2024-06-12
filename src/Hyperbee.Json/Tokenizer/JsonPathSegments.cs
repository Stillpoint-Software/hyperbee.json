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
internal record Segment
{
    public static Segment TerminalSegment = new( null, null, SelectorKind.Undefined );

    public bool IsEmpty => Next == null;

    public string FirstSelector => Selectors[0].Value;

    public bool Singular { get; }

    public Segment Next { get; set; }
    public SelectorDescriptor[] Selectors { get; init; }

    public Segment( Segment next, string selector, SelectorKind kind )
    {
        Next = next;
        Selectors =
        [
            new SelectorDescriptor { SelectorKind = kind, Value = selector }
        ];
        Singular = IsSingular();
    }

    public Segment( SelectorDescriptor[] selectors )
    {
        Selectors = selectors;
        Singular = IsSingular();
    }

    public Segment Push( string selector, SelectorKind kind ) => new( this, selector, kind );

    public Segment Pop( out Segment segment )
    {
        segment = this;
        return Next;
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

    internal class JsonPathSegmentDebugView( Segment instance )
    {
        [DebuggerBrowsable( DebuggerBrowsableState.RootHidden )]
        public SelectorDescriptor[] Selectors => instance.Selectors;
    }
}
