using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Hyperbee.Json.Query;

[DebuggerDisplay( "{Value}, SelectorKind = {SelectorKind}" )]
public record SelectorDescriptor
{
    public SelectorKind SelectorKind { get; internal set; }
    public string Value { get; init; }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public void Deconstruct( out string value, out SelectorKind selectorKind )
    {
        value = Value;
        selectorKind = SelectorKind;
    }
}

[DebuggerTypeProxy( typeof( SegmentDebugView ) )]
[DebuggerDisplay( "First = {Selectors.Length == 0 ? null : Selectors[0].Value}, IsSingular = {IsSingular}, Count = {Selectors.Length}" )]
public class JsonSegment : IEnumerable<JsonSegment>
{
    internal static readonly JsonSegment Final = new(); // special end node

    private readonly SelectorDescriptor[] _selectors;

    public bool IsFinal
    {
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        get => Next == null;
    }

    // singular is true when the selector resolves to one and only one element
    public bool IsSingular
    {
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        get;
    }

    public JsonSegment Next
    {
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        get;

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        set;
    }

    public SelectorDescriptor[] Selectors
    {
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        get => _selectors ?? [];
    }

    private JsonSegment()
    {
        IsSingular = false;
    }

    public JsonSegment( JsonSegment next, string selector, SelectorKind kind )
    {
        Next = next;
        _selectors = [new SelectorDescriptor { SelectorKind = kind, Value = selector }];
        IsSingular = SetIsSingular();
    }

    public JsonSegment( SelectorDescriptor[] selectors )
    {
        _selectors = selectors;

        IsSingular = SetIsSingular();
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public JsonSegment Prepend( string selector, SelectorKind kind )
    {
        return new JsonSegment( this, selector, kind );
    }

    public bool IsNormalized
    {
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
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

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private bool SetIsSingular()
    {
        // the segment is singular, when there is only one selector
        // and it is SelectorKind.Singular

        if ( Selectors.Length != 1 )
            return false;

        return (Selectors[0].SelectorKind & SelectorKind.Singular) == SelectorKind.Singular;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
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

    public IEnumerator<JsonSegment> GetEnumerator()
    {
        var current = this;

        while ( current != Final )
        {
            yield return current;

            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    internal class SegmentDebugView( JsonSegment instance )
    {
        [DebuggerBrowsable( DebuggerBrowsableState.RootHidden )]
        public SelectorDescriptor[] Selectors => instance.Selectors;

        [DebuggerBrowsable( DebuggerBrowsableState.Collapsed )]
        public JsonSegment Next => instance.Next;
    }
}
