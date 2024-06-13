using System.Diagnostics;

namespace Hyperbee.Json.Tokenizer; 

[DebuggerDisplay( "{Value}, SelectorKind = {SelectorKind}" )]
internal record SelectorDescriptor
{
    public SelectorKind SelectorKind { get; init; }
    public string Value { get; init; }
}

[DebuggerTypeProxy( typeof( SegmentDebugView ) )]
[DebuggerDisplay( "First = ({Selectors[0]}), Singular = {Singular}, Count = {Selectors.Length}" )]
internal class Segment
{
    internal static readonly Segment Terminal = new( null, null, SelectorKind.Undefined ); // marks end of segments

    public bool IsEmpty => Next == null;

    public bool Singular { get; }

    public Segment Next { get; set; }
    public SelectorDescriptor[] Selectors { get; init; }

    public Segment( Segment next, string selector, SelectorKind kind )
    {
        Next = next; //BF: should we get smarter here and set Selectors = [] for terminal
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

    public Segment Push( string selector, SelectorKind kind ) => new( this, selector, kind ); //BF: Insert(), AddHead()

    public Segment Pop( out Segment segment )   //BF: Next()
    {
        segment = this;
        return Next;
    }

    private bool IsSingular()
    {
        if ( Selectors.Length != 1 )
            return false;

        var selectorKind = Selectors[0].SelectorKind;

        return selectorKind == SelectorKind.UnspecifiedSingular || 
               selectorKind == SelectorKind.Dot ||
               selectorKind == SelectorKind.Index ||
               selectorKind == SelectorKind.Name ||
               selectorKind == SelectorKind.Root;
    }

    internal class SegmentDebugView( Segment instance )
    {
        [DebuggerBrowsable( DebuggerBrowsableState.RootHidden )]
        public SelectorDescriptor[] Selectors => instance.Selectors;

        [DebuggerBrowsable( DebuggerBrowsableState.Collapsed )]
        public Segment Next => instance.Next;
    }
}
