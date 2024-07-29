using System.Diagnostics;

namespace Hyperbee.Json.Query;

[DebuggerDisplay( "{Value}, SelectorKind = {SelectorKind}" )]
public record SelectorDescriptor
{
    public SelectorKind SelectorKind { get; internal set; }
    public string Value { get; init; }

    public void Deconstruct( out string value, out SelectorKind selectorKind )
    {
        value = Value;
        selectorKind = SelectorKind;
    }
}

[DebuggerTypeProxy( typeof( SegmentDebugView ) )]
[DebuggerDisplay( "First = ({Selectors?[0]}), IsSingular = {IsSingular}, Count = {Selectors?.Length}" )]
public class JsonSegment
{
    internal static readonly JsonSegment Final = new(); // special end node

    public bool IsFinal => Next == null;

    public bool IsSingular { get; } // singular is true when the selector resolves to one and only one element

    public JsonSegment Next { get; set; }
    public SelectorDescriptor[] Selectors { get; init; }

    private JsonSegment() { }

    public JsonSegment( JsonSegment next, string selector, SelectorKind kind )
    {
        Next = next;
        Selectors =
        [
            new SelectorDescriptor { SelectorKind = kind, Value = selector }
        ];
        IsSingular = InitIsSingular();
    }

    public JsonSegment( SelectorDescriptor[] selectors )
    {
        Selectors = selectors;
        IsSingular = InitIsSingular();
    }

    public JsonSegment Prepend( string selector, SelectorKind kind )
    {
        return new JsonSegment( this, selector, kind );
    }

    public IEnumerable<JsonSegment> AsEnumerable()
    {
        var current = this;

        while ( current != Final )
        {
            yield return current;

            current = current.Next;
        }
    }

    public bool IsNormalized
    {
        get
        {
            var current = this;

            while ( current != Final )
            {
                if ( !current.IsSingular )
                    return false;

                current = current.Next;
            }

            return true;
        }
    }

    private bool InitIsSingular()
    {
        // singular is one selector that is not a group

        if ( Selectors.Length != 1 )
            return false;

        return (Selectors[0].SelectorKind & SelectorKind.Singular) == SelectorKind.Singular;
    }

    public JsonSegment Last()
    {
        return AsEnumerable().Last();
    }

    public void Deconstruct( out bool singular, out SelectorDescriptor[] selectors )
    {
        singular = IsSingular;
        selectors = Selectors;
    }

    internal static JsonQuery LinkSegments( ReadOnlySpan<char> query, IList<JsonSegment> segments )
    {
        if ( segments == null || segments.Count == 0 )
            return new JsonQuery( query.ToString(), Final, false );

        // link the segments

        for ( var index = 0; index < segments.Count; index++ )
        {
            var segment = segments[index];

            segment.Next = index == segments.Count - 1
                ? Final
                : segments[index + 1];
        }

        var rootSegment = segments.First(); // first segment is the root
        var normalized = rootSegment.IsNormalized;

        return new JsonQuery( query.ToString(), rootSegment, normalized );
    }

    internal class SegmentDebugView( JsonSegment instance )
    {
        [DebuggerBrowsable( DebuggerBrowsableState.RootHidden )]
        public SelectorDescriptor[] Selectors => instance.Selectors;

        [DebuggerBrowsable( DebuggerBrowsableState.Collapsed )]
        public JsonSegment Next => instance.Next;
    }
}
