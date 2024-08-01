using System.Diagnostics;

namespace Hyperbee.Json.Path.Filters.Values;

[DebuggerDisplay( "{ValueKind}, Value = {Value}" )]
public readonly struct ScalarValue<TType> : IValueType where TType : IConvertible
{
    public ValueKind ValueKind { get; }

    public TType Value { get; }

    public ScalarValue( TType value )
    {
        ValueKind = ValueKind.Scalar;
        Value = value;
    }

    private ScalarValue( Nothing _ )
    {
        ValueKind = ValueKind.Nothing;
        Value = default;
    }

    private ScalarValue( Null _ )
    {
        ValueKind = ValueKind.Null;
        Value = default;
    }

    public static implicit operator ScalarValue<TType>( bool value ) => new( (TType) (IConvertible) value );
    public static implicit operator ScalarValue<TType>( string value ) => new( (TType) (IConvertible) value );
    public static implicit operator ScalarValue<TType>( int value ) => new( (TType) (IConvertible) value );
    public static implicit operator ScalarValue<TType>( float value ) => new( (TType) (IConvertible) value );

    public static implicit operator ScalarValue<TType>( Nothing nothing ) => new( nothing );
    public static implicit operator ScalarValue<TType>( Null nul ) => new( nul );
}


public static class Scalar
{
    public static ScalarValue<T> Value<T>( T value ) where T : IConvertible => new( value );

    public static ScalarValue<bool> True { get; } = new( true );
    public static ScalarValue<bool> False { get; } = new( false );

    public static Null Null { get; } = new();
    public static Nothing Nothing { get; } = new();
}
