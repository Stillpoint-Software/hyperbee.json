using System.Buffers;
using System.Runtime.CompilerServices;

namespace Hyperbee.Json.Patch;

public ref struct Matrix<T>
{
    private readonly Span<T> _span;
    private readonly T[] _pooledArray;
    private readonly int _rows;
    private readonly int _columns;

    internal Matrix( Span<T> span, int rows, int columns )
    {
        if ( span.Length != rows * columns )
            throw new ArgumentException( "Invalid span length." );

        _span = span;
        _rows = rows;
        _pooledArray = null;
        _columns = columns;
    }

    public Matrix( int rows, int columns )
    {
        _columns = columns;
        _rows = rows;

        var length = rows * columns;

        _pooledArray = ArrayPool<T>.Shared.Rent( length );
        _span = _pooledArray.AsSpan( 0, length );
    }

    public static int StackSize( int rows, int columns ) => rows * columns * Unsafe.SizeOf<T>();

    public readonly T this[int row, int col]
    {
        get => _span[row * _columns + col];
        set => _span[row * _columns + col] = value;
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public void Dispose()
    {
        var pooledArray = _pooledArray;
        this = default; // for safety, to avoid using pooled array if this instance is erroneously appended to again

        if ( pooledArray != null )
            ArrayPool<T>.Shared.Return( pooledArray );
    }
}
