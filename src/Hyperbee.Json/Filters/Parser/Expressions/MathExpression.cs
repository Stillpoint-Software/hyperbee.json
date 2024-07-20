using System.Linq.Expressions;
using System.Reflection;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Filters.Parser.Expressions;

public static class MathExpression<TNode>
{
    private static readonly ITypeDescriptor<TNode> Descriptor = JsonTypeDescriptorRegistry.GetDescriptor<TNode>();

    // Expressions

    public static Expression Add( Expression left, Expression right ) => Expression.Call( AddMethod, left, right );
    public static Expression Subtract( Expression left, Expression right ) => Expression.Call( SubtractMethod, left, right );
    public static Expression Modulus( Expression left, Expression right ) => Expression.Call( ModulusMethod, left, right );
    public static Expression Multiply( Expression left, Expression right ) => Expression.Call( MultiplyMethod, left, right );
    public static Expression Divide( Expression left, Expression right ) => Expression.Call( DivideMethod, left, right );

    // MethodInfo

    private const BindingFlags BindingAttr = BindingFlags.Static | BindingFlags.NonPublic;

    private static readonly MethodInfo AddMethod = typeof( MathExpression<TNode> ).GetMethod( nameof( Add ), BindingAttr );
    private static readonly MethodInfo SubtractMethod = typeof( MathExpression<TNode> ).GetMethod( nameof( Subtract ), BindingAttr );
    private static readonly MethodInfo ModulusMethod = typeof( MathExpression<TNode> ).GetMethod( nameof( Modulus ), BindingAttr );
    private static readonly MethodInfo MultiplyMethod = typeof( MathExpression<TNode> ).GetMethod( nameof( Multiply ), BindingAttr );
    private static readonly MethodInfo DivideMethod = typeof( MathExpression<TNode> ).GetMethod( nameof( Divide ), BindingAttr );

    // Methods

    private static IValueType Add( IValueType left, IValueType right )
    {
        if ( !TryGetNumber( left, out var leftValue ) || !TryGetNumber( right, out var rightValue ) )
            return Scalar.Nothing; //BF: should we be throwing NotSupportedException?

        return leftValue is int leftInt && rightValue is int rightInt
            ? Scalar.Value( leftInt + rightInt )
            : Scalar.Value( (float) leftValue + (float) rightValue );
    }

    private static IValueType Subtract( IValueType left, IValueType right )
    {
        if ( !TryGetNumber( left, out var leftValue ) || !TryGetNumber( right, out var rightValue ) )
            return Scalar.Nothing; //BF: should we be throwing NotSupportedException?

        return leftValue is int leftInt && rightValue is int rightInt
            ? Scalar.Value( leftInt - rightInt )
            : Scalar.Value( (float) leftValue - (float) rightValue );
    }

    private static IValueType Modulus( IValueType left, IValueType right )
    {
        if ( !TryGetNumber( left, out var leftValue ) || !TryGetNumber( right, out var rightValue ) )
            return Scalar.Nothing; //BF: should we be throwing NotSupportedException?

        return leftValue is int leftInt && rightValue is int rightInt
            ? Scalar.Value( leftInt % rightInt )
            : Scalar.Value( (float) leftValue % (float) rightValue );
    }

    private static IValueType Multiply( IValueType left, IValueType right )
    {
        if ( !TryGetNumber( left, out var leftValue ) || !TryGetNumber( right, out var rightValue ) )
            return Scalar.Nothing; //BF: should we be throwing NotSupportedException?

        return leftValue is int leftInt && rightValue is int rightInt
            ? Scalar.Value( leftInt * rightInt )
            : Scalar.Value( (float) leftValue * (float) rightValue );
    }

    private static IValueType Divide( IValueType left, IValueType right )
    {
        if ( !TryGetNumber( left, out var leftValue ) || !TryGetNumber( right, out var rightValue ) )
            return Scalar.Nothing; //BF: should we be throwing NotSupportedException?

        // dividing two int values may produce a fractional result
        var floatValue = Convert.ToSingle( leftValue ) / Convert.ToSingle( rightValue );

        // back to int if possible
        if ( leftValue is int && rightValue is int && TryConvertToInt( floatValue, out var intValue ) )
            return Scalar.Value( intValue );

        return Scalar.Value( floatValue );

        // Helper to convert float to int if the fractional part is within a tolerance

        static bool TryConvertToInt( float value, out int result, float tolerance = 1e-6f )
        {
            // Calculate the difference between the float and the nearest integer
            float fractionalPart = Math.Abs( value - (float) Math.Round( value ) );

            // Check if the fractional part is within the tolerance
            if ( fractionalPart < tolerance )
            {
                // If within the tolerance, assign the rounded value to result and return true
                result = (int) Math.Round( value );
                return true;
            }

            // If not within the tolerance, assign default value to result and return false
            result = default;
            return false;
        }
    }

    // Helpers

    private static bool TryGetNumber( IValueType valueType, out IConvertible value )
    {
        if ( valueType is ScalarValue<int> intValue )
        {
            value = intValue.Value;
            return true;
        }

        if ( valueType is ScalarValue<float> floatValue )
        {
            value = floatValue.Value;
            return true;
        }

        if ( valueType is NodeList<TNode> nodes )
        {
            var node = nodes.OneOrDefault();

            if ( node != null && Descriptor.Accessor.TryGetValueFromNode( node, out var nodeValue ) )
            {
                if ( nodeValue is float || nodeValue is int )
                {
                    value = nodeValue;
                    return true;
                }
            }
        }

        value = default;
        return false;
    }
}
