namespace Hyperbee.Json.Memory;
// copied here from Hyperbee.Core to prevent additional assembly dependency

/*
   var splitter = new SpanSplitter( span, ',', SpanSplitOptions.RemoveEmptyEntries );
   while ( splitter.TryMoveNext(out var slice) )
   {
       // ...
   }
*/

internal ref struct SpanSplitter<T> where T : IEquatable<T>
{
    private readonly bool _removeEmptyEntries;
    private readonly bool _forward;

    private readonly T _separator;
    private readonly ReadOnlySpan<T> _separators;
    private readonly bool _any;

    private ReadOnlySpan<T> _span;
    private bool _isFinal;

    internal SpanSplitter( ReadOnlySpan<T> span, T separator )
        : this( span, separator, default, false, default )
    {
    }

    internal SpanSplitter( ReadOnlySpan<T> span, T separator, SpanSplitOptions options )
        : this( span, separator, default, false, options )
    {
    }

    internal SpanSplitter( ReadOnlySpan<T> span, ReadOnlySpan<T> separators )
        : this( span, default, separators, true, default )
    {
    }

    internal SpanSplitter( ReadOnlySpan<T> span, ReadOnlySpan<T> separators, SpanSplitOptions options )
        : this( span, default, separators, true, options )
    {
    }

    private SpanSplitter( ReadOnlySpan<T> span, T separator, ReadOnlySpan<T> separators, bool any, SpanSplitOptions options )
    {
        _span = span;
        _separator = separator;
        _separators = separators;
        _any = any;
        _isFinal = false;

        _removeEmptyEntries = options.HasFlag( SpanSplitOptions.RemoveEmptyEntries );
        _forward = !options.HasFlag( SpanSplitOptions.Reverse );
    }

    public bool TryMoveNext( out ReadOnlySpan<T> result )
    {
        if ( _isFinal )
        {
            result = default;
            return false;
        }

        ReadOnlySpan<T> current;
        do
        {
            var index = _forward
                ? _any
                    ? _span.IndexOfAny( _separators )
                    : _span.IndexOf( _separator )
                : _any
                    ? _span.LastIndexOfAny( _separators )
                    : _span.LastIndexOf( _separator );

            if ( index < 0 )
            {
                _isFinal = true;
                current = _span;
                break;
            }

            if ( _forward )
            {
                current = _span[..index];
                _span = _span[(index + 1)..];
            }
            else
            {
                current = _span[(index + 1)..];
                _span = _span[..index];
            }

        } while ( _removeEmptyEntries && current.IsEmpty );

        result = current;
        return !_removeEmptyEntries || !current.IsEmpty;
    }
}
