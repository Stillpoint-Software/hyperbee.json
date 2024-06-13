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
    internal static readonly Segment Terminal = new();

    public bool IsEmpty => Next == null;

    public bool Singular { get; }

    public Segment Next { get; set; }
    public SelectorDescriptor[] Selectors { get; init; }

    private Segment() { }

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

    public Segment Insert( string selector, SelectorKind kind ) => new( this, selector, kind );

    public Segment MoveNext( out Segment previous )
    {
        previous = this;
        return Next;
    }

    public IEnumerable<Segment> AsEnumerable()
    {
        var current = this;

        while ( current != Terminal )
        {
            yield return current;

            current = current.Next;
        }
    }

    public void Deconstruct( out bool singular, out SelectorDescriptor[] selectors )
    {
        singular = Singular;
        selectors = Selectors;
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
