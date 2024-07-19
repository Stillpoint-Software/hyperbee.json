using System;
using System.Diagnostics;

namespace Hyperbee.Json.Filters.Values;

[DebuggerDisplay( "{ValueKind}, Value = {Value}" )]
public readonly struct ScalarValue<TType>( TType value ) : IValueType where TType : IConvertible
{
    public ValueKind ValueKind => ValueKind.Scalar;

    public TType Value { get; } = value;

    public static implicit operator ScalarValue<TType>( bool value ) => new( (TType) (IConvertible) value );
    public static implicit operator ScalarValue<TType>( string value ) => new( (TType) (IConvertible) value );
    public static implicit operator ScalarValue<TType>( int value ) => new( (TType) (IConvertible) value );
    public static implicit operator ScalarValue<TType>( float value ) => new( (TType) (IConvertible) value );
}

public static class Scalar
{
    public static ScalarValue<T> Value<T>( T value ) where T : IConvertible => new( value );

    public static ScalarValue<bool> True { get; } = new( true );
    public static ScalarValue<bool> False { get; } = new( false );

    public static Null Null { get; } = new();
    public static Nothing Nothing { get; } = new();
}
