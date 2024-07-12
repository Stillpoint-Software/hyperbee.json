using System.Linq.Expressions;
using System.Reflection;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Filters.Parser;

public static class NodeTypeExpression<TNode>
{
    private static readonly MethodInfo AreEqualMethodInfo = typeof( NodeTypeExpression<TNode> ).GetMethod( nameof( AreEqual ) );
    private static readonly MethodInfo AreNotEqualMethodInfo = typeof( NodeTypeExpression<TNode> ).GetMethod( nameof( AreNotEqual ) );
    private static readonly MethodInfo IsLessThanMethodInfo = typeof( NodeTypeExpression<TNode> ).GetMethod( nameof( IsLessThan ) );
    private static readonly MethodInfo IsLessThanOrEqualMethodInfo = typeof( NodeTypeExpression<TNode> ).GetMethod( nameof( IsLessThanOrEqual ) );
    private static readonly MethodInfo IsGreaterThanMethodInfo = typeof( NodeTypeExpression<TNode> ).GetMethod( nameof( IsGreaterThan ) );
    private static readonly MethodInfo IsGreaterThanOrEqualMethodInfo = typeof( NodeTypeExpression<TNode> ).GetMethod( nameof( IsGreaterThanOrEqual ) );

    private static readonly MethodInfo AndAlsoMethodInfo = typeof( NodeTypeExpression<TNode> ).GetMethod( nameof( AndAlso ) );
    private static readonly MethodInfo OrElseMethodInfo = typeof( NodeTypeExpression<TNode> ).GetMethod( nameof( OrElse ) );
    private static readonly MethodInfo NotMethodInfo = typeof( NodeTypeExpression<TNode> ).GetMethod( nameof( NotBoolean ) );

    public static Expression Equal( Expression left, Expression right ) => Expression.Call( AreEqualMethodInfo, left, right );
    public static Expression NotEqual( Expression left, Expression right ) => Expression.Call( AreNotEqualMethodInfo, left, right );
    public static Expression LessThan( Expression left, Expression right ) => Expression.Call( IsLessThanMethodInfo, left, right );
    public static Expression LessThanOrEqual( Expression left, Expression right ) => Expression.Call( IsLessThanOrEqualMethodInfo, left, right );
    public static Expression GreaterThan( Expression left, Expression right ) => Expression.Call( IsGreaterThanMethodInfo, left, right );
    public static Expression GreaterThanOrEqual( Expression left, Expression right ) => Expression.Call( IsGreaterThanOrEqualMethodInfo, left, right );

    // Binary operators
    public static Expression And( Expression left, Expression right ) => Expression.Call( AndAlsoMethodInfo, left, right );
    public static Expression Or( Expression left, Expression right ) => Expression.Call( OrElseMethodInfo, left, right );
    public static Expression Not( Expression expression ) => Expression.Call( NotMethodInfo, expression );

    public static bool AreEqual( INodeType left, INodeType right ) => left.Comparer.Compare( left, right, Operator.Equals ) == 0;
    public static bool AreNotEqual( INodeType left, INodeType right ) => left.Comparer.Compare( left, right, Operator.NotEquals ) != 0;
    public static bool IsLessThan( INodeType left, INodeType right ) => left.Comparer.Compare( left, right, Operator.LessThan ) < 0;
    public static bool IsLessThanOrEqual( INodeType left, INodeType right ) => left.Comparer.Compare( left, right, Operator.LessThanOrEqual ) <= 0;
    public static bool IsGreaterThan( INodeType left, INodeType right ) => left.Comparer.Compare( left, right, Operator.GreaterThan ) > 0;
    public static bool IsGreaterThanOrEqual( INodeType left, INodeType right ) => left.Comparer.Compare( left, right, Operator.GreaterThanOrEqual ) >= 0;

    public static bool AndAlso( INodeType left, INodeType right )
    {
        if ( left is ValueType<bool> leftBoolValue && right is ValueType<bool> rightBoolValue )
            return leftBoolValue.Value && rightBoolValue.Value;

        return left.Comparer.Exists( left ) &&
               right.Comparer.Exists( right );
    }

    public static bool OrElse( INodeType left, INodeType right )
    {
        if ( left is ValueType<bool> leftBoolValue && right is ValueType<bool> rightBoolValue )
            return leftBoolValue.Value || rightBoolValue.Value;

        return left.Comparer.Exists( left ) ||
               right.Comparer.Exists( right );
    }

    public static bool NotBoolean( INodeType value )
    {
        if ( value is ValueType<bool> { Value: false } )
            return true;

        return !value.Comparer.Exists( value );
    }
}
