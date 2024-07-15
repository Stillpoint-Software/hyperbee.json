using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Filters.Values;

public struct ScalarValue<T>( T value ) : IValueType where T : IConvertible
{
    public readonly ValueKind ValueKind => ValueKind.Scalar;
    public IValueTypeComparer Comparer { get; set; } = null;

    public T Value { get; } = value;

    // Implicit conversion operators for bool, string, float, and int

    public static implicit operator ScalarValue<T>( bool value ) => new( (T) (IConvertible) value );
    public static implicit operator ScalarValue<T>( string value ) => new( (T) (IConvertible) value );
    public static implicit operator ScalarValue<T>( int value ) => new( (T) (IConvertible) value );
    public static implicit operator ScalarValue<T>( float value ) => new( (T) (IConvertible) value );
}

public static class Scalar
{
    public static ScalarValue<T> Value<T>( T value ) where T : IConvertible => new( value );

    public static ScalarValue<bool> True { get; } = new( true );
    public static ScalarValue<bool> False { get; } = new( false );

    public static Null Null { get; } = new();
    public static Nothing Nothing { get; } = new();
}
