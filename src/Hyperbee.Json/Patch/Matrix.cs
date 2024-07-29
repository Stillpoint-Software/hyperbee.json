using System.Buffers;

namespace Hyperbee.Json.Patch;

public ref struct Matrix
{
    // ReSharper disable FieldCanBeMadeReadOnly.Local

    // dispose will reset values, so don't use readonly
    private Span<byte> _stackAllocated;
    private int[] _pooledArray;
    private int _rows;
    private int _cols;

    // ReSharper restore FieldCanBeMadeReadOnly.Local

    public Matrix( Span<byte> arrayBuffer, int rows, int columns )
    {
        var totalElements = rows * columns;

        if ( totalElements > 32 )
            throw new ArgumentException( $"{nameof( rows )}.Length + {nameof( columns )}.Length exceeds the stack allocation limit of 32." );

        if ( arrayBuffer.Length != totalElements )
            throw new ArgumentException( $"Length of {nameof( columns )} does not match the {nameof( rows )}.Length + {nameof( columns )}.Length." );

        _stackAllocated = arrayBuffer;
        _pooledArray = null;

        _rows = rows;
        _cols = columns;
    }

    public Matrix( int rows, int columns )
    {
        _rows = rows;
        _cols = columns;

        _stackAllocated = [];
        _pooledArray = ArrayPool<int>.Shared.Rent( rows * columns );
    }

    public int this[int row, int column]
    {
        readonly get
        {
            ThrowIfArgumentOutOfBounds( row, column );

            return _pooledArray != null
                ? _pooledArray[row * _cols + column]
                : _stackAllocated[row * _cols + column];
        }
        set
        {
            ThrowIfArgumentOutOfBounds( row, column );

            switch ( _pooledArray )
            {
                case null:
                    _stackAllocated[row * _cols + column] = (byte) value;
                    break;
                default:
                    _pooledArray[row * _cols + column] = value;
                    break;
            }
        }
    }

    private readonly void ThrowIfArgumentOutOfBounds( int row, int column )
    {
        if ( row < 0 || row >= _rows )
            throw new ArgumentOutOfRangeException( nameof( row ), $"Index {nameof( row )} is out of bounds." );

        if ( column < 0 || column >= _cols )
            throw new ArgumentOutOfRangeException( nameof( column ), $"Index {nameof( column )} is out of bounds." );
    }

    public void Dispose()
    {
        var pooledArray = _pooledArray;
        this = default; // for safety, to avoid using pooled array if this instance is erroneously appended to again

        if ( pooledArray != null )
            ArrayPool<int>.Shared.Return( pooledArray );
    }
}
