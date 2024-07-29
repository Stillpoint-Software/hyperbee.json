using System.Buffers;
using System.Runtime.CompilerServices;

namespace Hyperbee.Json.Core;

internal ref struct ValueStringBuilder // use in a try finally with an explicit Dispose
{
    private char[] _arrayPoolBuffer;
    private Span<char> _chars;
    private int _pos;

    public ValueStringBuilder( int initialCapacity )
    {
        _arrayPoolBuffer = ArrayPool<char>.Shared.Rent( initialCapacity );
        _chars = _arrayPoolBuffer;
        _pos = 0;
    }

    public ValueStringBuilder( Span<char> initialBuffer )
    {
        _arrayPoolBuffer = null;
        _chars = initialBuffer;
        _pos = 0;
    }

    public readonly bool IsEmpty => _pos == 0;
    public readonly int Length => _pos;

    public void Append( char value )
    {
        if ( _pos >= _chars.Length )
            Grow();

        _chars[_pos++] = value;
    }

    public void Append( ReadOnlySpan<char> value )
    {
        if ( _pos + value.Length > _chars.Length )
            Grow( value.Length );

        value.CopyTo( _chars[_pos..] );
        _pos += value.Length;
    }

    public void Clear() => _pos = 0;

    public readonly ReadOnlySpan<char> AsSpan() => _chars[.._pos];
    public readonly string AsString() => _chars[.._pos].ToString();

    private void Grow( int additionalCapacity = 0 )
    {
        var newCapacity = Math.Max( _chars.Length * 2, _chars.Length + additionalCapacity );
        var newArray = ArrayPool<char>.Shared.Rent( newCapacity );
        _chars.CopyTo( newArray );

        if ( _arrayPoolBuffer != null )
            ArrayPool<char>.Shared.Return( _arrayPoolBuffer );

        _arrayPoolBuffer = newArray;
        _chars = newArray;
    }

    public override string ToString() // disposes
    {
        var value = AsString();
        Dispose();
        return value;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public void Dispose()
    {
        var arrayPoolBuffer = _arrayPoolBuffer;
        this = default; // for safety, to avoid using pooled array if this instance is erroneously appended to again

        if ( arrayPoolBuffer != null )
            ArrayPool<char>.Shared.Return( arrayPoolBuffer );

        _arrayPoolBuffer = null;
        _chars = default;
        _pos = 0;
    }
}
