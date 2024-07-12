using System.Linq.Expressions;
using System.Reflection;
using Hyperbee.Json.Descriptors.Types;

namespace Hyperbee.Json.Filters.Parser;

public static class NodeTypeExpression<TNode>
{
    // TODO: Add, Add, Or, and Not operators

    private static readonly MethodInfo AreEqualMethodInfo = typeof( NodeTypeExpression<TNode> ).GetMethod( nameof( AreEqual ) );
    private static readonly MethodInfo AreNotEqualMethodInfo = typeof( NodeTypeExpression<TNode> ).GetMethod( nameof( AreNotEqual ) );
    private static readonly MethodInfo IsLessThanMethodInfo = typeof( NodeTypeExpression<TNode> ).GetMethod( nameof( IsLessThan ) );
    private static readonly MethodInfo IsLessThanOrEqualMethodInfo = typeof( NodeTypeExpression<TNode> ).GetMethod( nameof( IsLessThanOrEqual ) );
    private static readonly MethodInfo IsGreaterThanMethodInfo = typeof( NodeTypeExpression<TNode> ).GetMethod( nameof( IsGreaterThan ) );
    private static readonly MethodInfo IsGreaterThanOrEqualMethodInfo = typeof( NodeTypeExpression<TNode> ).GetMethod( nameof( IsGreaterThanOrEqual ) );

    public static Expression Equal( Expression left, Expression right ) => Expression.Call( AreEqualMethodInfo, left, right );
    public static Expression NotEqual( Expression left, Expression right ) => Expression.Call( AreNotEqualMethodInfo, left, right );
    public static Expression LessThan( Expression left, Expression right ) => Expression.Call( IsLessThanMethodInfo, left, right );
    public static Expression LessThanOrEqual( Expression left, Expression right ) => Expression.Call( IsLessThanOrEqualMethodInfo, left, right );
    public static Expression GreaterThan( Expression left, Expression right ) => Expression.Call( IsGreaterThanMethodInfo, left, right );
    public static Expression GreaterThanOrEqual( Expression left, Expression right ) => Expression.Call( IsGreaterThanOrEqualMethodInfo, left, right );


    public static bool AreEqual( INodeType left, INodeType right ) => left.Comparer.Compare( left, right, Operator.Equals ) == 0;
    public static bool AreNotEqual( INodeType left, INodeType right ) => left.Comparer.Compare( left, right, Operator.NotEquals ) != 0;
    public static bool IsLessThan( INodeType left, INodeType right ) => left.Comparer.Compare( left, right, Operator.LessThan ) < 0;
    public static bool IsLessThanOrEqual( INodeType left, INodeType right ) => left.Comparer.Compare( left, right, Operator.LessThanOrEqual ) <= 0;
    public static bool IsGreaterThan( INodeType left, INodeType right ) => left.Comparer.Compare( left, right, Operator.GreaterThan ) > 0;
    public static bool IsGreaterThanOrEqual( INodeType left, INodeType right ) => left.Comparer.Compare( left, right, Operator.GreaterThanOrEqual ) >= 0;
}
